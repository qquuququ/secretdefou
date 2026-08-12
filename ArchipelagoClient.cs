using System;
using System.Collections.Generic;
using UnityEngine;

public class ArchipelagoClient
{
    private static ArchipelagoClient _instance;
    private bool _isConnected = false;
    private string _playerSlot = "";
    private string _serverUrl = "";

    // Mapping: Location Subnautica -> Item ID AP
    private Dictionary<string, long> _locationIdMap = new Dictionary<string, long>();
    // Mapping: Item ID AP -> Item Name
    private Dictionary<long, string> _itemNameMap = new Dictionary<long, string>();
    // Locations déjà collectées
    private HashSet<long> _checkedLocations = new HashSet<long>();

    public static ArchipelagoClient Instance
    {
        get
        {
            if (_instance == null)
                _instance = new ArchipelagoClient();
            return _instance;
        }
    }

    public bool IsConnected => _isConnected;
    public string PlayerSlot => _playerSlot;

    /// <summary>
    /// Initialise la connexion à Archipelago
    /// </summary>
    public void Connect(string serverUrl, string slotName, string password = "")
    {
        try
        {
            _serverUrl = serverUrl;
            _playerSlot = slotName;

            // TODO: Implémenter la vraie connexion AP
            // Pour l'instant, simuler la connexion
            _isConnected = true;

            Debug.Log($"✅ Connected to Archipelago: {slotName}@{serverUrl}");
            LoadLocationMappings();
        }
        catch (Exception ex)
        {
            Debug.LogError($"❌ Failed to connect to Archipelago: {ex.Message}");
            _isConnected = false;
        }
    }

    /// <summary>
    /// Charge les mappings location<->item depuis ArchipelagoLocationMapping
    /// </summary>
    private void LoadLocationMappings()
    {
        _locationIdMap.Clear();
        _itemNameMap.Clear();

        // ✅ Charger depuis ArchipelagoLocationMapping.LocationIdMap
        foreach (var kvp in ArchipelagoLocationMapping.LocationIdMap)
        {
            string subnauticaName = kvp.Key;
            long locationId = kvp.Value;

            _locationIdMap[subnauticaName] = locationId;
            _itemNameMap[locationId] = subnauticaName;

            Debug.Log($"📍 Mapped: {subnauticaName} -> ID:{locationId}");
        }

        Debug.Log($"✅ Loaded {_locationIdMap.Count} location mappings from ArchipelagoLocationMapping");
    }

    /// <summary>
    /// Envoie une location comme collectée à Archipelago
    /// </summary>
    public void CompleteLocation(string locationName)
    {
        if (!_isConnected)
        {
            Debug.LogWarning($"⚠️ Not connected to Archipelago, ignoring: {locationName}");
            return;
        }

        if (!_locationIdMap.TryGetValue(locationName, out long locationId))
        {
            Debug.LogWarning($"⚠️ Location not found in AP mapping: {locationName}");
            return;
        }

        if (_checkedLocations.Contains(locationId))
        {
            Debug.Log($"ℹ️ Location already checked: {locationName}");
            return;
        }

        _checkedLocations.Add(locationId);

        // TODO: Envoyer à AP via leur client API
        Debug.Log($"📤 Sent to Archipelago: {locationName} (ID: {locationId})");
    }

    /// <summary>
    /// Récupère le nom de l'item AP pour une location
    /// </summary>
    public string GetItemForLocation(string locationName)
    {
        if (!_isConnected || !_locationIdMap.TryGetValue(locationName, out long itemId))
            return null;

        return _itemNameMap.TryGetValue(itemId, out string itemName) ? itemName : null;
    }

    /// <summary>
    /// Vérifie si une location a été collectée
    /// </summary>
    public bool IsLocationChecked(string locationName)
    {
        if (!_isConnected)
            return false;

        return _locationIdMap.TryGetValue(locationName, out long locationId) &&
               _checkedLocations.Contains(locationId);
    }

    public void Disconnect()
    {
        _isConnected = false;
        _locationIdMap.Clear();
        _itemNameMap.Clear();
        _checkedLocations.Clear();
        Debug.Log("🔌 Disconnected from Archipelago");
    }
}
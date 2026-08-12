using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ArchipelagoLocationDatabase
{
    private static List<ArchipelagoLocation> allLocations = new List<ArchipelagoLocation>();
    private static HashSet<string> locationIds = new HashSet<string>();

    // Ajouter une nouvelle location découverte
    public static void AddLocation(ArchipelagoLocation location)
    {
        if (location == null || string.IsNullOrEmpty(location.LocationId))
            return;

        if (!locationIds.Contains(location.LocationId))
        {
            allLocations.Add(location);
            locationIds.Add(location.LocationId);
            ErrorMessage.AddMessage($"[AP] Location added: {location.DisplayName}");
        }
    }

    // Récupérer toutes les locations
    public static List<ArchipelagoLocation> GetAllLocations()
    {
        return new List<ArchipelagoLocation>(allLocations);
    }

    // Récupérer une location spécifique
    public static ArchipelagoLocation GetLocation(string locationId)
    {
        return allLocations.FirstOrDefault(l => l.LocationId == locationId);
    }

    // Marquer une location comme collectée
    public static void MarkAsCollected(string locationId)
    {
        var location = GetLocation(locationId);
        if (location != null)
        {
            location.Collected = true;
        }
    }

    // Compter les collectées
    public static int GetCollectedCount()
    {
        return allLocations.Count(l => l.Collected);
    }

    // Total de locations
    public static int GetTotalCount()
    {
        return allLocations.Count;
    }

    // Vider la base
    public static void Clear()
    {
        allLocations.Clear();
        locationIds.Clear();
    }

    // Imprimer le rapport
    public static string GetStatusReport()
    {
        int total = GetTotalCount();
        int collected = GetCollectedCount();
        int fragments = allLocations.Count(l => l.IsFragment);
        int pdas = total - fragments;

        return $"[ARCHIPELAGO] Status Report\n" +
               $"Total Locations: {total}\n" +
               $"Collected: {collected}/{total}\n" +
               $"Fragments: {fragments}\n" +
               $"PDAs/Databoxes: {pdas}\n" +
               $"Progress: {(total > 0 ? ((float)collected / total * 100).ToString("F1") : "0")}%";
    }
}

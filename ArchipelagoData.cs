using System;
using System.Collections.Generic;
using BepInEx;
using Newtonsoft.Json;
using UnityEngine;

namespace Archipelago
{
    public static class ArchipelagoData
    {
        public static T ReadJSON<T>(string filename)
        {
            T result;
            try
            {
                string path = Paths.PluginPath + "/Archipelago/" + filename + ".json";
                string text = System.IO.File.ReadAllText(path);
                result = JsonConvert.DeserializeObject<T>(text);
            }
            catch (Exception ex)
            {
                Debug.LogError("Could not read " + filename + ".json\n" + ex.ToString());
                result = default(T);
            }
            return result;
        }

        public static void Initialize(Dictionary<string, object> slotData)
        {
            if (ArchipelagoData.Initialized)
            {
                return;
            }

            try
            {
                // Charger les items
                foreach (KeyValuePair<long, string> kvp in ArchipelagoData.ReadJSON<Dictionary<long, string>>("items"))
                {
                    if (Enum.TryParse<TechType>(kvp.Value, out TechType techType))
                    {
                        ArchipelagoData.ItemCodeToTechType[kvp.Key] = techType;
                    }
                }

                // Charger les groupes d'items
                ArchipelagoData.GroupItems = ArchipelagoData.ReadJSON<Dictionary<long, List<long>>>("group_items");

                // Charger les locations
                foreach (KeyValuePair<long, Dictionary<string, float>> kvp in ArchipelagoData.ReadJSON<Dictionary<long, Dictionary<string, float>>>("locations"))
                {
                    APState.Location location = new APState.Location();
                    location.ID = kvp.Key;
                    location.Position = new Vector3(kvp.Value["x"], kvp.Value["y"], kvp.Value["z"]);
                    ArchipelagoData.Locations.Add(location.ID, location);
                }

                // Charger les types d'items
                foreach (KeyValuePair<string, List<long>> kvp in ArchipelagoData.ReadJSON<Dictionary<string, List<long>>>("item_types"))
                {
                    int typeNum = int.Parse(kvp.Key);
                    ArchipelagoItemType itemTypeEnum = (ArchipelagoItemType)typeNum;

                    Debug.Log($"Loading item type '{itemTypeEnum}' with {kvp.Value.Count} items");

                    foreach (long itemCode in kvp.Value)
                    {
                        ArchipelagoData.ItemCodeToItemType[itemCode] = itemTypeEnum.ToString();
                    }
                }

                // Charger l'encyclopédie
                ArchipelagoData.Encyclopedia = ArchipelagoData.ReadJSON<Dictionary<string, long>>("encyclopedia");

                // Charger la logique
                ArchipelagoData.LogicDict = ArchipelagoData.ReadJSON<Dictionary<TechType, List<long>>>("logic");

                Debug.Log("ItemCodeToItemType " + JsonConvert.SerializeObject(ArchipelagoData.ItemCodeToItemType));

                ArchipelagoData.Initialized = true;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error initializing ArchipelagoData: " + ex.ToString());
                ArchipelagoData.Initialized = false;
            }
        }

        public static bool Initialized;
        public static Dictionary<string, long> Encyclopedia;
        public static Dictionary<TechType, List<long>> LogicDict;
        public static Dictionary<long, TechType> ItemCodeToTechType = new Dictionary<long, TechType>();
        public static Dictionary<long, APState.Location> Locations = new Dictionary<long, APState.Location>();
        public static Dictionary<long, List<long>> GroupItems = new Dictionary<long, List<long>>();
        public static Dictionary<long, string> ItemCodeToItemType = new Dictionary<long, string>();
    }
}

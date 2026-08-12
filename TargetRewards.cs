using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Archipelago
{
    public static class TargetRewards
    {
        private static Dictionary<string, string> scanRewards = new Dictionary<string, string>();
        private static Dictionary<string, string> pickupRewards = new Dictionary<string, string>();
        private static Dictionary<int, string> milestoneRewards = new Dictionary<int, string>();
        public static Dictionary<int, string> MilestoneRewards => milestoneRewards;
        private static List<string> creatures = new List<string>();

        private static bool isInitialized = false;

        /// <summary>
        /// Charge toutes les récompenses depuis le SlotData du serveur Archipelago
        /// </summary>
        public static void LoadFromSlotData(Dictionary<string, object> slotData)
        {
            try
            {
                // Charger ScanRewards
                if (slotData.TryGetValue("scan_rewards", out object scanRewardsObj))
                {
                    var jObject = scanRewardsObj as JObject;
                    if (jObject != null)
                    {
                        scanRewards.Clear();
                        foreach (var kvp in jObject)
                        {
                            scanRewards[kvp.Key] = kvp.Value.ToString();
                        }
                    }
                }

                // Charger PickupRewards
                if (slotData.TryGetValue("pickup_rewards", out object pickupRewardsObj))
                {
                    var jObject = pickupRewardsObj as JObject;
                    if (jObject != null)
                    {
                        pickupRewards.Clear();
                        foreach (var kvp in jObject)
                        {
                            pickupRewards[kvp.Key] = kvp.Value.ToString();
                        }
                    }
                }

                // Charger MilestoneRewards
                if (slotData.TryGetValue("milestone_rewards", out object milestoneRewardsObj))
                {
                    var jObject = milestoneRewardsObj as JObject;
                    if (jObject != null)
                    {
                        milestoneRewards.Clear();
                        foreach (var kvp in jObject)
                        {
                            if (int.TryParse(kvp.Key, out int milestoneNum))
                            {
                                milestoneRewards[milestoneNum] = kvp.Value.ToString();
                            }
                        }
                    }
                }

                // Charger Creatures
                if (slotData.TryGetValue("creatures", out object creaturesObj))
                {
                    var jArray = creaturesObj as JArray;
                    if (jArray != null)
                    {
                        creatures.Clear();
                        foreach (var creature in jArray)
                        {
                            creatures.Add(creature.ToString());
                        }
                    }
                }

                isInitialized = true;
                Logging.Log("TargetRewards loaded successfully from SlotData", false, true, false);
            }
            catch (Exception e)
            {
                Logging.LogError("Error loading TargetRewards from SlotData: " + e.Message, true, true);
                isInitialized = false;
            }
        }

        /// <summary>
        /// Récupère la récompense pour scanner une créature
        /// </summary>
        public static string GetScanReward(string creatureName)
        {
            if (!isInitialized)
            {
                Logging.Log("TargetRewards not initialized yet", false, true, false);
                return null;
            }

            if (scanRewards.TryGetValue(creatureName, out string reward))
                return reward;

            Logging.Log($"No scan reward found for creature: {creatureName}", false, true, false);
            return null;
        }

        /// <summary>
        /// Récupère la récompense pour trouver un databox/PDA
        /// </summary>
        public static string GetPickupReward(string locationName)
        {
            if (!isInitialized)
            {
                Logging.Log("TargetRewards not initialized yet", false, true, false);
                return null;
            }

            if (pickupRewards.TryGetValue(locationName, out string reward))
                return reward;

            Logging.Log($"No pickup reward found for location: {locationName}", false, true, false);
            return null;
        }

        /// <summary>
        /// Récupère la récompense pour un milestone de scans
        /// </summary>
        public static string GetMilestoneReward(int scanCount)
        {
            if (!isInitialized)
            {
                Logging.Log("TargetRewards not initialized yet", false, true, false);
                return null;
            }

            if (milestoneRewards.TryGetValue(scanCount, out string reward))
                return reward;

            return null; // Pas d'erreur, c'est normal qu'il n'y ait pas de milestone à chaque scan
        }

        /// <summary>
        /// Vérifie si une créature est dans la liste
        /// </summary>
        public static bool IsCreature(string name)
        {
            return creatures.Contains(name);
        }

        /// <summary>
        /// Retourne toutes les créatures
        /// </summary>
        public static List<string> GetAllCreatures()
        {
            return new List<string>(creatures);
        }

        /// <summary>
        /// Vérifie si les récompenses sont chargées
        /// </summary>
        public static bool IsInitialized => isInitialized;
    }
}
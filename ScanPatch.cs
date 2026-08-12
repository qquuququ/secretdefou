using Archipelago;
using HarmonyLib;
using Newtonsoft.Json;
using Story;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[HarmonyPatch(typeof(PDAScanner), nameof(PDAScanner.Initialize))]
public class PDAScannerInitPatch
{
    static void Postfix(PDAData pdaData)
    {
        Debug.Log("🔧 PDAScannerInitPatch: Initializing custom fragments...");
        AddCustomFragment(TechType.ExosuitFragment, TechType.Exosuit, 3, "exosuit_fragment");
        AddCustomFragment(TechType.SeaTruckFragment, TechType.SeaTruck, 3, "seatruck_fragment");
    }

    static void AddCustomFragment(TechType fragmentType, TechType blueprintType, int totalFragments, string encyclopediaKey)
    {
        var entryData = new PDAScanner.EntryData()
        {
            key = fragmentType,
            locked = false,
            totalFragments = totalFragments,
            destroyAfterScan = false,
            encyclopedia = encyclopediaKey,
            blueprint = blueprintType,
            scanTime = 2f,
            unlockStoryGoal = false,
            isFragment = true
        };

        var mappingField = typeof(PDAScanner).GetField("mapping",
            BindingFlags.NonPublic | BindingFlags.Static);

        if (mappingField == null)
        {
            Debug.LogError("❌ Cannot find 'mapping' field in PDAScanner!");
            return;
        }

        var mapping = (Dictionary<TechType, PDAScanner.EntryData>)mappingField.GetValue(null);

        if (mapping == null)
        {
            Debug.LogError("❌ 'mapping' is null!");
            return;
        }

        if (!mapping.ContainsKey(fragmentType))
        {
            mapping.Add(fragmentType, entryData);
            Debug.Log($"✅ Fragment ajouté: {fragmentType}");
        }
        else
        {
            Debug.Log($"⚠️ Fragment déjà existe: {fragmentType}");
        }
    }
}

[HarmonyPatch(typeof(PDAScanner), nameof(PDAScanner.Scan))]
public class ScanPatch
{
    static float lastRewardTime = 0f;
    static Dictionary<string, string> locationRewards = new Dictionary<string, string>();
    static Dictionary<string, string> customNames = new Dictionary<string, string>();
    static Dictionary<string, long> locationNameToId = new Dictionary<string, long>();
    static bool rewardsLoaded = false;

    static void LoadRewards()
    {
        if (rewardsLoaded) return;

        try
        {
            string rewardsPath = System.IO.Path.Combine(
                Application.persistentDataPath,
                "rewards.json"
            );

            if (System.IO.File.Exists(rewardsPath))
            {
                string json = System.IO.File.ReadAllText(rewardsPath);

                // ✅ Parser directement sans dynamic - utilise Dictionary<string, object>
                var rewardsData = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

                if (rewardsData != null)
                {
                    // Parse location_rewards
                    if (rewardsData.ContainsKey("location_rewards"))
                    {
                        var locRewards = JsonConvert.DeserializeObject<Dictionary<string, string>>(
                            rewardsData["location_rewards"].ToString()
                        );

                        foreach (var kvp in locRewards)
                        {
                            locationRewards[kvp.Key] = kvp.Value;
                        }
                        Debug.Log($"✅ Loaded {locationRewards.Count} location rewards");
                    }

                    // Parse custom_names
                    if (rewardsData.ContainsKey("custom_names"))
                    {
                        var custNames = JsonConvert.DeserializeObject<Dictionary<string, string>>(
                            rewardsData["custom_names"].ToString()
                        );

                        foreach (var kvp in custNames)
                        {
                            customNames[kvp.Key] = kvp.Value;
                        }
                        Debug.Log($"✅ Loaded {customNames.Count} custom names");
                    }

                    // Parse location_ids
                    if (rewardsData.ContainsKey("location_ids"))
                    {
                        var locIds = JsonConvert.DeserializeObject<Dictionary<string, long>>(
                            rewardsData["location_ids"].ToString()
                        );

                        foreach (var kvp in locIds)
                        {
                            locationNameToId[kvp.Key] = kvp.Value;
                        }
                        Debug.Log($"✅ Loaded {locationNameToId.Count} location ID mappings");
                    }
                }

                rewardsLoaded = true;
            }
            else
            {
                Debug.LogWarning($"⚠️ rewards.json not found at {rewardsPath}");
                rewardsLoaded = true;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"❌ Error loading rewards.json: {e.Message}");
            rewardsLoaded = true;
        }
    }

    static void Prefix(out TechType __state)
    {
        __state = TechType.None;

        if (PDAScanner.scanTarget.Equals(default(PDAScanner.ScanTarget)))
            return;

        __state = PDAScanner.scanTarget.techType;
    }

    static void Postfix(PDAScanner.Result __result, TechType __state)
    {
        if (__result != PDAScanner.Result.Researched &&
            __result != PDAScanner.Result.Done)
            return;

        if (__state == TechType.None)
            return;

        Debug.Log($"🎯 SCAN RESULT: {__state} -> {__result}");

        // ========== LOG CREATURE DEPTH ==========
        Vector3 playerPos = Player.main.transform.position;
        float depth = -playerPos.y;
        string logEntry = $"{__state},{depth},{playerPos}";

        Debug.Log($"📊 CREATURE SCAN: {logEntry}");
        System.IO.File.AppendAllText(
            System.IO.Path.Combine(Application.persistentDataPath, "creature_depths.log"),
            logEntry + "\n"
        );

        // Anti spam
        if (Time.time - lastRewardTime < 0.5f)
            return;

        lastRewardTime = Time.time;

        // ============================================
        // CHARGER LES RÉCOMPENSES (une seule fois)
        // ============================================
        LoadRewards();

        // ============================================
        // MODE ARCHIPELAGO
        // ============================================
        if (APState.Authenticated)
        {
            // ✅ Convertir TechType en string
            string locationName = __state.ToString(); // "ArcticPeeper", "BruteShark", etc.

            // Récupérer la récompense aléatoire
            if (locationRewards.TryGetValue(locationName, out string rewardTechName))
            {
                if (Enum.TryParse<TechType>(rewardTechName, out TechType reward))
                {
                    string displayName = GetDisplayName(rewardTechName);
                    Debug.Log($"🌐 Archipelago Reward: {displayName} ({rewardTechName})");
                    ErrorMessage.AddMessage($"🌐 Archipelago: {displayName}");

                    // ✅ Récupérer le location ID du mapping
                    long locationId = -1L;

                    if (locationNameToId.TryGetValue(locationName, out long id))
                    {
                        locationId = id;
                    }

                    if (locationId != -1L)
                    {
                        APState.SendLocID(locationId);
                        Debug.Log($"✅ Location sent to Archipelago: ID={locationId}, Name={locationName}");
                    }
                    else
                    {
                        Debug.LogWarning($"⚠️ Could not find location ID for: {locationName}");
                    }

                    // Donner la récompense
                    GiveReward(reward);
                    return;
                }
            }
            else
            {
                Debug.LogWarning($"⚠️ No random reward found for: {locationName}");
            }
        }

        // ============================================
        // MODE VANILLA (fallback)
        // ============================================

        // Vérifier les paliers Precursor EN PREMIER
        TechType milestoneReward = MilestoneRewards.CheckMilestoneReward();
        if (milestoneReward != TechType.None)
        {
            Debug.Log($"🏆 MILESTONE REWARD: {milestoneReward}");
            ErrorMessage.AddMessage($"🏆 MILESTONE: {GetDisplayName(milestoneReward.ToString())}");
            GiveReward(milestoneReward);
            return;
        }

        Debug.Log($"❌ No reward for: {__state}");
    }

    static string GetDisplayName(string techName)
    {
        return customNames.TryGetValue(techName, out string displayName)
            ? displayName
            : techName;
    }

    public static void GiveReward(TechType techType)
    {
        Debug.Log($"📦 GiveReward called for: {techType}");

        if (RewardDB.DirectItems.Contains(techType))
        {
            Debug.Log($"✅ DirectItem: {techType}");
            CraftData.AddToInventory(techType, 1, false, true);
            ErrorMessage.AddMessage("Item: " + RewardDB.GetDisplayName(techType));
            return;
        }

        if (RewardDB.Blueprints.Contains(techType))
        {
            Debug.Log($"✅ Blueprint: {techType}");
            KnownTech.Add(techType, true, true);
            ErrorMessage.AddMessage("Blueprint unlocked: " + RewardDB.GetDisplayName(techType));
            return;
        }

        if (RewardDB.Fragments.Contains(techType))
        {
            Debug.Log($"🔍 Checking fragment: {techType}");
            var entryData = PDAScanner.GetEntryData(techType);

            if (entryData != null)
            {
                Debug.Log($"✅ Fragment found in PDAScanner: {techType}");
                PDAScanner.Entry entry;
                bool isNew = !PDAScanner.GetPartialEntryByKey(techType, out entry);

                if (isNew)
                {
                    Debug.Log($"📝 New fragment entry for: {techType}");
                    PDAScanner.AddByUnlockable(techType, 1);
                    PDAScanner.GetPartialEntryByKey(techType, out entry);

                    ErrorMessage.AddMessage(
                        $"Fragment +1: {RewardDB.GetDisplayName(techType)} (1/{entryData.totalFragments})"
                    );
                }
                else
                {
                    Debug.Log($"📈 Incrementing fragment: {techType} -> {entry.unlocked + 1}");
                    entry.unlocked++;

                    if (entry.unlocked >= entryData.totalFragments)
                    {
                        Debug.Log($"🎉 Fragment complete: {techType}");
                        PDAScanner.RemoveAllEntriesWhichUnlocks(techType);
                        PDAScanner.CompleteAllEntriesWhichUnlocks(techType);

                        KnownTech.Add(entryData.blueprint, true, true);

                        ErrorMessage.AddMessage(
                            $"Blueprint unlocked: {RewardDB.GetDisplayName(entryData.blueprint)}"
                        );
                    }
                    else
                    {
                        ErrorMessage.AddMessage(
                            $"Fragment +1: {RewardDB.GetDisplayName(techType)} ({entry.unlocked}/{entryData.totalFragments})"
                        );
                    }
                }
            }
            else
            {
                Debug.LogWarning($"⚠️ Fragment NOT found in PDAScanner: {techType} - Adding as item");
                CraftData.AddToInventory(techType, 1, false, true);
                ErrorMessage.AddMessage("Fragment Item: " + RewardDB.GetDisplayName(techType));
            }

            return;
        }

        Debug.LogError($"❌ {techType} not in any reward database!");
    }
}

[HarmonyPatch(typeof(StoryGoalManager), nameof(StoryGoalManager.OnGoalComplete))]
public class StoryGoalManagerPatch
{
    static bool Prefix(StoryGoalManager __instance, string key)
    {
        Debug.Log($"[StoryGoalManagerPatch] OnGoalComplete called for: {key}");

        // Bloquer tous les goals liés aux milestones Precursor
        if (key.Contains("PrecursorScan_Level_") ||
            key.StartsWith("PrecursorScanBounty_") ||
            (key.Contains("Precursor") && key.Contains("Level")))
        {
            Debug.Log($"🚫 BLOCKED milestone goal: {key}");
            return false;
        }

        return true;
    }
}

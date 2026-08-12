using Archipelago;
using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;

[HarmonyPatch(typeof(BlueprintHandTarget), "UnlockBlueprint")]
internal class BlueprintHandTarget_Patch
{
    private static HashSet<string> processedDataboxes = new HashSet<string>();

    [HarmonyPrefix]
    public static void BlockVanillaReward(BlueprintHandTarget __instance)
    {
        PrefabIdentifier prefabId = __instance.gameObject.GetComponent<PrefabIdentifier>();
        string classId = prefabId != null ? prefabId.ClassId : "";

        if (string.IsNullOrEmpty(classId))
            return;

        Vector3 pos = __instance.gameObject.transform.position;
        string fallbackName = $"DataBox at ({pos.x:.0f}, {pos.y:.0f}, {pos.z:.0f})";
        string displayName = RewardDB.GetDataboxPDAName(classId, fallbackName);

        // ✅ Utiliser la nouvelle API TargetRewards
        string rewardName = TargetRewards.GetPickupReward(displayName);
        if (rewardName != null)
        {
            Debug.Log($"🛑 Blocking vanilla reward for: {displayName}");
            __instance.unlockTechType = TechType.None;
        }
    }

    [HarmonyPatch(typeof(BlueprintHandTarget), "TryToAddToKnownTech")]
    [HarmonyPrefix]
    public static bool BlockTryToAddKnownTech(BlueprintHandTarget __instance, ref bool __result)
    {
        if (__instance.unlockTechType == TechType.None)
        {
            __result = true;
            Debug.Log("🛑 Blocking Titanium bonus");
            return false;
        }
        return true;
    }

    [HarmonyPostfix]
    public static void ReplaceReward(BlueprintHandTarget __instance)
    {
        PrefabIdentifier prefabId = __instance.gameObject.GetComponent<PrefabIdentifier>();
        string classId = prefabId != null ? prefabId.ClassId : "";

        if (string.IsNullOrEmpty(classId))
        {
            Logging.Log("⚠️ DataBox sans ClassID!", false, true);
            return;
        }

        Vector3 pos = __instance.gameObject.transform.position;
        string uniqueKey = $"{classId}_{pos.x:F0}_{pos.y:F0}_{pos.z:F0}";
        string fallbackName = $"DataBox at ({pos.x:.0f}, {pos.y:.0f}, {pos.z:.0f})";
        string displayName = RewardDB.GetDataboxPDAName(classId, fallbackName);

        Debug.Log($"📦 DATABOX - ClassID: {classId} | Pos: ({pos.x:.0f}, {pos.y:.0f}, {pos.z:.0f})");

        ExportLocationToFile(displayName, classId, pos);

        if (processedDataboxes.Contains(uniqueKey))
            return;

        processedDataboxes.Add(uniqueKey);

        // ============================================
        // MODE ARCHIPELAGO
        // ============================================
        if (APState.Authenticated)
        {
            Debug.Log($"🌐 Checking Archipelago for location: {displayName}");

            // ✅ NOUVEAU: Envoyer directement l'ID
            long? locationId = ArchipelagoLocationMapping.GetLocationId(displayName);
            if (locationId.HasValue)
            {
                Debug.Log($"✅ Sending location ID {locationId} to Archipelago");
                APState.SendLocID(locationId.Value);
            }
            else
            {
                Debug.LogWarning($"⚠️ Location not mapped: {displayName}");
            }
            return;
        }

        // ============================================
        // MODE VANILLA
        // ============================================
        string rewardName = TargetRewards.GetPickupReward(displayName);
        if (rewardName != null && Enum.TryParse<TechType>(rewardName, true, out TechType reward))
        {
            Debug.Log($"✅ CUSTOM REWARD FOUND: {reward}");
            GiveCustomReward(reward);
        }
        else
        {
            Debug.Log($"❌ No custom reward for: {displayName}");
        }
    }

    private static void ExportLocationToFile(string displayName, string classId, Vector3 pos)
    {
        try
        {
            string appDataPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            string filePath = System.IO.Path.Combine(
                appDataPath,
                "..\\LocalLow\\Unknown Worlds\\Subnautica Below Zero\\ArchipelagoExport\\Locations.txt"
            );

            string directory = System.IO.Path.GetDirectoryName(filePath);
            if (!System.IO.Directory.Exists(directory))
                System.IO.Directory.CreateDirectory(directory);

            string positionEntry = $"'{displayName}': {{'x': {pos.x.ToString(System.Globalization.CultureInfo.InvariantCulture)}, 'y': {pos.y.ToString(System.Globalization.CultureInfo.InvariantCulture)}, 'z': {pos.z.ToString(System.Globalization.CultureInfo.InvariantCulture)}}}";

            System.IO.File.AppendAllText(filePath, positionEntry + System.Environment.NewLine);
            Debug.Log($"✅ Position exportée: {filePath}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"❌ Erreur export location: {ex.Message}");
        }
    }

    private static void GiveCustomReward(TechType techType)
    {
        Debug.Log($"📦 GiveCustomReward called for: {techType}");

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
                    Debug.Log($"📈 Incrementing fragment: {techType}");
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
                Debug.LogError($"⚠️ Fragment NOT found in PDAScanner: {techType}");
                CraftData.AddToInventory(techType, 1, false, true);
                ErrorMessage.AddMessage("Fragment Item: " + RewardDB.GetDisplayName(techType));
            }

            return;
        }

        Debug.LogError($"❌ {techType} not in any reward database!");
    }
}
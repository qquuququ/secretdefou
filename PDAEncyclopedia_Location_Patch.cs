using Archipelago;
using HarmonyLib;
using System;
using System.IO;
using UnityEngine;

[HarmonyPatch(typeof(PDAEncyclopedia), nameof(PDAEncyclopedia.Add))]
[HarmonyPatch(new[] { typeof(string), typeof(bool), typeof(bool) })]
internal class PDAEncyclopedia_Location_Patch
{
    private static System.Collections.Generic.HashSet<string> trackedPDAs =
        new System.Collections.Generic.HashSet<string>();

    private static System.Collections.Generic.HashSet<string> FakePDADisplayNames =
        new System.Collections.Generic.HashSet<string>()
    {
        "PDA: ControlRoom",
        "PDA: MapRoom",
        "PDA: Moonpool",
        "PDA: MoonpoolExpension",
    };

    [HarmonyPrefix]
    public static bool TrackPDA(string key)
    {
        if (string.IsNullOrEmpty(key)) return true;

        string classId = GenerateStableClassId(key);
        string displayName = RewardDB.GetDataboxPDAName(classId, $"PDA: {key}");

        if (displayName.Contains("Databox") || displayName.Contains("databox") || IsFakePDA(displayName))
        {
            Debug.Log($"⏭️ Ignoring: {displayName}");
            return false;
        }

        if (trackedPDAs.Contains(key))
            return true;

        trackedPDAs.Add(key);

        try
        {
            ExportPDALocation(displayName);

            // ============================================
            // MODE ARCHIPELAGO
            // ============================================
            if (APState.Authenticated)
            {
                long? locationId = ArchipelagoLocationMapping.GetLocationId(displayName);
                if (locationId.HasValue)
                {
                    Debug.Log($"✅ Sending PDA location ID {locationId} to Archipelago");
                    APState.SendLocID(locationId.Value);
                }
                else
                {
                    Debug.LogWarning($"⚠️ PDA location not mapped: {displayName}");
                }
                return true;
            }

            // ============================================
            // MODE VANILLA
            // ============================================
            string rewardName = TargetRewards.GetPickupReward(displayName);
            if (rewardName != null && Enum.TryParse<TechType>(rewardName, true, out TechType reward))
            {
                Debug.Log($"✅ REWARD FOUND: {reward}");
                ErrorMessage.AddMessage($"[AP] 🎯 TARGET TROUVÉE: {displayName}");
                GiveReward(reward);
            }
            else
            {
                Debug.Log($"❌ No reward for: {displayName}");
                ErrorMessage.AddMessage($"[AP] PDA trouvé: {displayName}");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"[AP] Erreur PDA: {ex.Message}\n{ex.StackTrace}");
        }

        return true;
    }

    private static void ExportPDALocation(string displayName)
    {
        try
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Vector3 pos = player != null ? player.transform.position : Vector3.zero;

            string appDataPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            string filePath = Path.Combine(
                appDataPath,
                "..\\LocalLow\\Unknown Worlds\\Subnautica Below Zero\\ArchipelagoExport\\Locations.txt"
            );

            string directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            string pdaEntry = $"'{displayName}': {{'x': {pos.x.ToString(System.Globalization.CultureInfo.InvariantCulture)}, 'y': {pos.y.ToString(System.Globalization.CultureInfo.InvariantCulture)}, 'z': {pos.z.ToString(System.Globalization.CultureInfo.InvariantCulture)}}}";
            File.AppendAllText(filePath, pdaEntry + System.Environment.NewLine);
            Debug.Log($"✅ PDA exporté: {displayName} à ({pos.x:F2}, {pos.y:F2}, {pos.z:F2})");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"❌ Erreur export PDA: {ex.Message}");
        }
    }

    private static bool IsFakePDA(string displayName)
    {
        return FakePDADisplayNames.Contains(displayName);
    }

    private static string GenerateStableClassId(string input)
    {
        using (var md5 = System.Security.Cryptography.MD5.Create())
        {
            byte[] hash = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
            return new Guid(hash).ToString();
        }
    }

    private static void GiveReward(TechType techType)
    {
        Debug.Log($"📦 PDA GiveReward called for: {techType}");

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
                    ErrorMessage.AddMessage($"Fragment +1: {RewardDB.GetDisplayName(techType)} (1/{entryData.totalFragments})");
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
                        ErrorMessage.AddMessage($"Blueprint unlocked: {RewardDB.GetDisplayName(entryData.blueprint)}");
                    }
                    else
                    {
                        ErrorMessage.AddMessage($"Fragment +1: {RewardDB.GetDisplayName(techType)} ({entry.unlocked}/{entryData.totalFragments})");
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
using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FakePDARemover
{
    public static class FakePDAContext
    {
        public static bool Active;
    }

    public static class FakePDAIDs
    {
        public static readonly HashSet<string> DisplayNames = new HashSet<string>()
        {
            "Omega Lab - Lilypads Islands - Greenhouse Databox",
            "Koppa Mining Site - Entrance Databox",
            "Purple Vents Small Debris Wreck - Databox",
            "Delta Station - Outside Databox",
            "PDA: ControlRoom",
            "PDA: MapRoom",
            "PDA: Moonpool",
            "PDA: MoonpoolExpansion"
        };

        public static bool IsFake(string displayName)
        {
            return !string.IsNullOrEmpty(displayName) && DisplayNames.Contains(displayName);
        }
    }

    [HarmonyPatch(typeof(PDAEncyclopedia), nameof(PDAEncyclopedia.Add))]
    public class PDAEncyclopedia_Add_Patch
    {
        static void Prefix(string key)
        {
            string classId = GenerateStableClassId(key);
            string displayName = RewardDB.GetDataboxPDAName(classId, $"PDA: {key}");

            FakePDAContext.Active = FakePDAIDs.IsFake(displayName);

            if (FakePDAContext.Active)
            {
                Debug.Log($"[FakePDARemover] Fake PDA detected: {displayName}");
            }
        }

        static void Postfix()
        {
            FakePDAContext.Active = false;
        }

        private static string GenerateStableClassId(string input)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] hash = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
                return new Guid(hash).ToString();
            }
        }
    }

    [HarmonyPatch(typeof(KnownTech), nameof(KnownTech.Add))]
    public class KnownTech_Add_Patch
    {
        static bool Prefix(TechType techType)
        {
            if (!FakePDAContext.Active)
                return true;

            Debug.Log($"[FakePDARemover] Blocked unlock from Fake PDA: {techType}");

            return false;
        }
    }

    // ✅ NOUVEAU: Harmoniser avec BlueprintHandTarget
    [HarmonyPatch(typeof(BlueprintHandTarget), "UnlockBlueprint")]
    public class BlueprintHandTarget_UnlockBlueprint_Patch
    {
        static void Prefix(BlueprintHandTarget __instance)
        {
            PrefabIdentifier prefabId = __instance.gameObject.GetComponent<PrefabIdentifier>();
            string classId = prefabId != null ? prefabId.ClassId : "";

            if (string.IsNullOrEmpty(classId))
                return;

            Vector3 pos = __instance.gameObject.transform.position;
            string fallbackName = $"DataBox at ({pos.x:.0f}, {pos.y:.0f}, {pos.z:.0f})";
            string displayName = RewardDB.GetDataboxPDAName(classId, fallbackName);

            // Bloquer les fake databoxes
            if (FakePDAIDs.IsFake(displayName))
            {
                Debug.Log($"[FakePDARemover] Blocking fake databox: {displayName}");
                __instance.unlockTechType = TechType.None;
            }
        }
    }

    [HarmonyPatch(typeof(BlueprintHandTarget), "TryToAddToKnownTech")]
    public class BlueprintHandTarget_TryToAddKnownTech_Patch
    {
        static bool Prefix(BlueprintHandTarget __instance, ref bool __result)
        {
            // Si TechType.None, bloquer le titanium bonus
            if (__instance.unlockTechType == TechType.None)
            {
                __result = true;
                Debug.Log("[FakePDARemover] Blocking Titanium bonus from fake databox");
                return false;
            }
            return true;
        }
    }

    public class Main : MonoBehaviour
    {
        public static void Patch()
        {
            Harmony harmony = new Harmony("com.yourname.fakepdaremover");
            harmony.PatchAll();

            Debug.Log("[FakePDARemover] Loaded.");
        }
    }
}
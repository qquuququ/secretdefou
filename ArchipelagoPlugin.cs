using BepInEx;
using HarmonyLib;
using System;
using System.Reflection;
using System.Threading;
using UnityEngine;
using Story;

namespace Archipelago
{
    [BepInPlugin("com.archipelago.subnautica", "Archipelago", "1.0.0")]
    public class ArchipelagoPlugin : BaseUnityPlugin
    {
        public static bool Zero = true;
        public static Type SubnauticaEscapePod = null;

        public void Awake()
        {
            var harmony = new Harmony("com.archipelago.subnautica");

            // ========== INITIALISATION ==========
            Logger.LogInfo("✅ Archipelago initialization...");

            // ========== TRACKER THREAD ==========
            ThreadStart start = new ThreadStart(TrackerThread.DoWork);
            APState.TrackerProcessing = new Thread(start)
            {
                IsBackground = true
            };
            APState.TrackerProcessing.Start();
            Logger.LogInfo("✅ Tracker thread started");

            // ========== ADD UI COMPONENT ==========
            ArchipelagoUI uiComponent = gameObject.AddComponent<ArchipelagoUI>();
            Logger.LogInfo("✅ ArchipelagoUI component added");

            // S'assurer que le GameObject du plugin est actif
            gameObject.SetActive(true);
            Logger.LogInfo("✅ Plugin GameObject is active");

            // ========== HARMONY PATCHES ==========
            try
            {
                harmony.PatchAll(Assembly.GetExecutingAssembly());
                Logger.LogInfo("✅ Harmony patches loaded!");
            }
            catch (Exception ex)
            {
                Logger.LogWarning($"⚠️ Harmony patch warning: {ex.Message}");
                Logger.LogInfo("✅ Plugin continues despite patch warning");
            }

            string versionStr = $"Archipelago v{APState.AP_VERSION[0]}.{APState.AP_VERSION[1]}.{APState.AP_VERSION[2]} for Below Zero - Ready to connect!";
            Logger.LogInfo(versionStr);
        }

        // ========== GOAL COMPLETION PATCH ==========
        [HarmonyPatch(typeof(StoryGoalManager))]
        [HarmonyPatch("OnGoalComplete")]
        internal class StoryGoalManager_OnGoalComplete_Patch
        {
            [HarmonyPrefix]
            public static void OnGoalComplete(string key)
            {
                if (key == "AlAn_WarpAway" && APState.Authenticated)
                {
                    Logging.Log("🎉 Goal completed: Leave with Al-An!", false, true, false);
                    APState.send_completion();
                }
            }
        }
    }
}
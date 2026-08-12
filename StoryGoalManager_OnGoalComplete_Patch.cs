using HarmonyLib;
using Story;
using System;

namespace Archipelago
{
    [HarmonyPatch(typeof(StoryGoalManager))]
    [HarmonyPatch("OnGoalComplete")]
    internal class StoryGoalManager_OnGoalComplete_Patch
    {
        [HarmonyPrefix]
        public static void OnGoalComplete(string key)
        {
            // Vérifie si c'est le goal "AlAn_WarpAway"
            if (key == "AlAn_WarpAway" && APState.Authenticated)
            {
                Logging.Log("🎉 Goal completed: Leave with Al-An!", false, true, false);
                APState.send_completion();
            }
        }
    }
}
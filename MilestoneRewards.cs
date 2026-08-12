using Archipelago;
using HarmonyLib;
using Story;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class MilestoneRewards
{
    public static Dictionary<int, List<string>> MilestoneThresholds = new Dictionary<int, List<string>>()
    {
        { 1, new List<string>() { "Scan 1 Architect Artifact" } },
        { 2, new List<string>() { "Scan 2 Architect Artifacts" } },
        { 3, new List<string>() { "Scan 3 Architect Artifacts" } },
        { 4, new List<string>() { "Scan 4 Architect Artifacts" } },
        { 5, new List<string>() { "Scan 5 Architect Artifacts" } },
        { 6, new List<string>() { "Scan 6 Architect Artifacts" } },
        { 9, new List<string>() { "Scan 9 Architect Artifacts" } },
        { 10, new List<string>() { "Scan 10 Architect Artifacts" } },
    };

    private static HashSet<int> claimedMilestones = new HashSet<int>();

    public static void Initialize()
    {
        claimedMilestones.Clear();
        Debug.Log("[MilestoneRewards] Initialized");
    }

    public static TechType CheckMilestoneReward()
    {
        if (StoryGoalManager.main == null)
            return TechType.None;

        int totalPrecursorScans = StoryGoalManager.main.precursorScanCount;

        var sortedThresholds = new List<int>(MilestoneThresholds.Keys);
        sortedThresholds.Sort();

        foreach (var milestone in sortedThresholds)
        {
            if (totalPrecursorScans >= milestone && !claimedMilestones.Contains(milestone))
            {
                claimedMilestones.Add(milestone);

                List<string> descriptions = MilestoneThresholds[milestone];
                string description = descriptions[UnityEngine.Random.Range(0, descriptions.Count)]; // ✅ Fixed ambiguity

                TechType techReward = TechType.None;
                if (TargetRewards.MilestoneRewards.TryGetValue(milestone, out string rewardStr)) // ✅ Need property in TargetRewards
                {
                    Enum.TryParse<TechType>(rewardStr, out techReward);
                }

                Debug.Log($"🎉 MILESTONE REACHED: {totalPrecursorScans} Precursor scans! Milestone: {milestone} | Description: {description} | Tech Reward: {techReward}");
                return techReward;
            }
        }

        return TechType.None;
    }
}
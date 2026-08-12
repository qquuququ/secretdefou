using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;

[HarmonyPatch(typeof(PDAScanner), nameof(PDAScanner.Scan))]
public class ArchipelagoScanPatch
{
    static TechType lastTech;
    static string lastUID;
    static float lastRewardTime = 0f;

    static void Prefix()
    {
        if (PDAScanner.scanTarget.Equals(default(PDAScanner.ScanTarget)))
            return;

        lastTech = PDAScanner.scanTarget.techType;
        lastUID = PDAScanner.scanTarget.uid;
    }

    static void Postfix(PDAScanner.Result __result)
    {
        if (__result != PDAScanner.Result.Researched &&
            __result != PDAScanner.Result.Done)
            return;
        if (lastTech == TechType.None)
            return;

        if (Time.time - lastRewardTime < 0.5f)
            return;

        string locationId = $"{lastTech}_{lastUID}";

        var location = new ArchipelagoLocation
        {
            LocationId = locationId,
            TechType = lastTech.ToString(),
            UID = lastUID,
            Position = PDAScanner.scanTarget.gameObject?.transform.position != null
                ? new float[] {
                    PDAScanner.scanTarget.gameObject.transform.position.x,
                    PDAScanner.scanTarget.gameObject.transform.position.y,
                    PDAScanner.scanTarget.gameObject.transform.position.z
                  }
                : new float[] { 0, 0, 0 },
            Collected = true
        };

        ArchipelagoLocationDatabase.AddLocation(location);
        LocationExporter.ExportToText();
    }
}
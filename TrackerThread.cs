using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Archipelago
{
    // Token: 0x02000026 RID: 38
    public class TrackerThread
    {
        // Token: 0x06000067 RID: 103 RVA: 0x00004848 File Offset: 0x00002A48
        public static bool InLogic(long locID)
        {
            foreach (KeyValuePair<TechType, List<long>> keyValuePair in ArchipelagoData.LogicDict)
            {
                bool flag;
                try
                {
                    flag = KnownTech.Contains(keyValuePair.Key);
                }
                catch (NullReferenceException)
                {
                    flag = false;
                }
                if (!flag && keyValuePair.Value.Contains(locID))
                {
                    return false;
                }
            }
            return -(TrackerThread.LogicVehicleDepth + TrackerThread.LogicSwimDepth) < ArchipelagoData.Locations[locID].Position.y;
        }

        // Token: 0x06000068 RID: 104 RVA: 0x000048F0 File Offset: 0x00002AF0
        public static void PrimeDepthSystem()
        {
            TrackerThread.DepthString = APState.SwimRule;
            if (TrackerThread.DepthString.Length == 0)
            {
                TrackerThread.DepthString = "items_hard";
            }
            string[] array = TrackerThread.DepthString.Split(new char[]
            {
                '_'
            });
            TrackerThread.ItemsRelevant = (array.Length > 1);
            string last = array.GetLast<string>();
            if (last == "easy")
            {
                TrackerThread.BaseDepth = 200f;
                return;
            }
            if (last == "normal")
            {
                TrackerThread.BaseDepth = 400f;
                return;
            }
            if (!(last == "hard"))
            {
                return;
            }
            TrackerThread.BaseDepth = 600f;
        }

        // Token: 0x06000069 RID: 105 RVA: 0x0000498C File Offset: 0x00002B8C
        public static void UpdateLogicDepth()
        {
            float num = TrackerThread.BaseDepth;
            bool flag;
            try
            {
                flag = KnownTech.Contains(TechType.Workbench);
            }
            catch (NullReferenceException)
            {
                return;
            }
            if (KnownTech.Contains(TechType.Seaglide))
            {
                num += 200f;
                if (flag && KnownTech.Contains(TechType.HighCapacityTank))
                {
                    num += 150f;
                }
            }
            else if (flag && KnownTech.Contains(TechType.UltraGlideFins))
            {
                num += 50f;
                if (KnownTech.Contains(TechType.HighCapacityTank))
                {
                    num += 100f;
                }
                else if (KnownTech.Contains(TechType.PlasteelTank))
                {
                    num += 25f;
                }
            }
            else if (flag && KnownTech.Contains(TechType.HighCapacityTank))
            {
                num += 100f;
            }
            else if (flag && KnownTech.Contains(TechType.PlasteelTank))
            {
                num += 25f;
            }
            TrackerThread.LogicSwimDepth = num;
        }

        // Token: 0x0600006A RID: 106 RVA: 0x00004A6C File Offset: 0x00002C6C
        public static void UpdateVehicleDepth()
        {
            float num = 0f;
            string logicVehicle = "Vehicle";
            bool flag;
            try
            {
                flag = KnownTech.Contains(TechType.Constructor);
            }
            catch (Exception)
            {
                return;
            }
            if (!flag)
            {
                TrackerThread.LogicVehicleDepth = 0f;
                TrackerThread.LogicVehicle = logicVehicle;
                return;
            }
            bool flag2 = KnownTech.Contains(TechType.Workbench);
            bool flag3 = KnownTech.Contains(TechType.BaseUpgradeConsole) && KnownTech.Contains(TechType.BaseMoonpool);
            float num2 = num;
            if (KnownTech.Contains(TechType.SeaTruck))
            {
                num = Math.Max(num, 150f);
                if (flag3 && KnownTech.Contains(TechType.SeaTruckUpgradeHull1))
                {
                    num = Math.Max(num, 300f);
                    if (flag2 && KnownTech.Contains(TechType.SeaTruckUpgradeHull2))
                    {
                        num = Math.Max(num, 650f);
                        if (KnownTech.Contains(TechType.SeaTruckUpgradeHull3))
                        {
                            num = Math.Max(num, 1000f);
                        }
                    }
                }
                if (Math.Abs(num2 - num) > 1f)
                {
                    logicVehicle = "Seamoth";
                }
            }
            num2 = num;
            if (KnownTech.Contains(TechType.Exosuit))
            {
                num = Math.Max(num, 400f);
                if (flag3 && KnownTech.Contains(TechType.ExoHullModule1))
                {
                    num = Math.Max(num, 700f);
                    if (flag2 && KnownTech.Contains(TechType.ExoHullModule2))
                    {
                        num = Math.Max(num, 1100f);
                    }
                }
                if (Math.Abs(num2 - num) > 1f)
                {
                    logicVehicle = "Prawn Suit";
                }
            }
            TrackerThread.LogicVehicle = logicVehicle;
            TrackerThread.LogicVehicleDepth = num;
        }

        // Token: 0x0600006B RID: 107 RVA: 0x00004C54 File Offset: 0x00002E54
        public static void DoWork()
        {
            long num = 33999L;
            long val = 7L;
            for (; ; )
            {
                if (APState.SwimRule != TrackerThread.DepthString)
                {
                    TrackerThread.PrimeDepthSystem();
                }
                if (TrackerThread.ItemsRelevant)
                {
                    TrackerThread.UpdateLogicDepth();
                }
                else
                {
                    TrackerThread.LogicSwimDepth = TrackerThread.BaseDepth;
                }
                TrackerThread.UpdateVehicleDepth();
                long num2 = 0L;
                if (APState.state == APState.State.InGame && APState.Session != null && Player.main != null)
                {
                    Vector3 position = Player.main.gameObject.transform.position;
                    float num3 = 100000f;
                    long num4 = -1L;
                    foreach (long num5 in APState.Session.Locations.AllMissingLocations)
                    {
                        if (num5 < num)
                        {
                            num2 += 1L;
                            if (APState.TrackedMode != TrackerMode.Logical || TrackerThread.InLogic(num5))
                            {
                                float num6 = Vector3.Distance(position, ArchipelagoData.Locations[num5].Position);
                                if (num6 < num3)
                                {
                                    num3 = num6;
                                    num4 = num5;
                                }
                            }
                        }
                    }
                    APState.TrackedLocationsCount = num2;
                    APState.TrackedDistance = num3;
                    APState.TrackedLocation = num4;
                    if (num4 != -1L)
                    {
                        APState.TrackedLocationName = APState.Session.Locations.GetLocationNameFromId(APState.TrackedLocation, null);
                        Vector3 from = ArchipelagoData.Locations[num4].Position - Player.main.gameObject.transform.position;
                        from.Normalize();
                        APState.TrackedAngle = Vector3.Angle(from, Player.main.viewModelCamera.transform.forward);
                    }
                }
                else
                {
                    APState.TrackedLocationsCount = 0L;
                    APState.TrackedLocation = -1L;
                }
                if (APState.Session != null)
                {
                    List<long> list = new List<long>();
                    foreach (long num7 in APState.Session.Locations.AllMissingLocations)
                    {
                        if (num7 > num)
                        {
                            list.Add(num7);
                        }
                    }
                    APState.TrackedFishCount = (long)list.Count;
                    if (APState.TrackedFishCount != 0L)
                    {
                        list.Sort();
                        List<string> list2 = new List<string>();
                        int num8 = 0;
                        while ((long)num8 < Math.Min(APState.TrackedFishCount, val))
                        {
                            list2.Add(APState.Session.Locations.GetLocationNameFromId(list[num8], null).Replace(" Scan", ""));
                            num8++;
                        }
                        APState.TrackedFish = string.Join(", ", list2);
                    }
                    else
                    {
                        APState.TrackedFish = "";
                    }
                }
                else
                {
                    APState.TrackedFishCount = 0L;
                }
                Thread.Sleep(150);
            }
        }

        // Token: 0x0400003B RID: 59
        public static string DepthString = "";

        // Token: 0x0400003C RID: 60
        public static bool ItemsRelevant = true;

        // Token: 0x0400003D RID: 61
        public static float BaseDepth = 600f;

        // Token: 0x0400003E RID: 62
        public static float LogicSwimDepth = TrackerThread.BaseDepth;

        // Token: 0x0400003F RID: 63
        public static float LogicVehicleDepth = 0f;

        // Token: 0x04000040 RID: 64
        public static string LogicVehicle = "Vehicle";
    }
}

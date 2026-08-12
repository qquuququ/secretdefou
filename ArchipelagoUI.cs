using System;
using System.Reflection;
using Archipelago.MultiClient.Net.Packets;
using HarmonyLib;
using UnityEngine;

namespace Archipelago
{
    public class ArchipelagoUI : MonoBehaviour
    {
        private void OnGUI()
        {
            Logging.TryUpdateLog();

            Debug.Log($"[ArchipelagoUI] State: {APState.state}, Session: {APState.Session != null}, Authenticated: {APState.Authenticated}");
            string str = string.Concat(new string[]
            {
                "Archipelago v",
                APState.AP_VERSION[0].ToString(),
                ".",
                APState.AP_VERSION[1].ToString(),
                ".",
                APState.AP_VERSION[2].ToString()
            });

            // ========== STATUS LINE ==========
            if (APState.Session != null)
            {
                if (APState.Authenticated)
                {
                    GUI.Label(new Rect(16f, 16f, 300f, 20f), str + " Status: Connected");
                }
                else
                {
                    GUI.Label(new Rect(16f, 16f, 300f, 20f), str + " Status: Authentication failed");
                }
            }
            else
            {
                GUI.Label(new Rect(16f, 16f, 300f, 20f), str + " Status: Not Connected");
            }

            // ========== CONNECTION FORM (Menu) ==========
            if ((APState.Session == null || !APState.Authenticated) && APState.state == APState.State.Menu)
            {
                GUI.Label(new Rect(16f, 36f, 150f, 20f), "Host: ");
                GUI.Label(new Rect(16f, 56f, 150f, 20f), "Slot Name: ");
                GUI.Label(new Rect(16f, 76f, 150f, 20f), "Password: ");

                bool flag = Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return;
                APState.ServerConnectInfo.host_name = GUI.TextField(new Rect(174f, 36f, 150f, 20f), APState.ServerConnectInfo.host_name);
                APState.ServerConnectInfo.slot_name = GUI.TextField(new Rect(174f, 56f, 150f, 20f), APState.ServerConnectInfo.slot_name);
                APState.ServerConnectInfo.password = GUI.TextField(new Rect(174f, 76f, 150f, 20f), APState.ServerConnectInfo.password);

                if (flag && Event.current.type == EventType.KeyDown)
                {
                    flag = false;
                }

                if ((GUI.Button(new Rect(16f, 96f, 100f, 20f), "Connect") || flag) && APState.ServerConnectInfo.Valid)
                {
                    APState.Connect();
                    return;
                }
            }
            // ========== IN-GAME DISPLAY ==========
            else if (APState.state == APState.State.InGame && APState.Session != null && Player.main != null)
            {
                if (APState.TrackedLocation != -1L && APState.TrackedMode != TrackerMode.Disabled)
                {
                    string text = "Locations left: " + APState.TrackedLocationsCount.ToString();
                    if (APState.TrackedLocation != -1L)
                    {
                        text = string.Concat(new string[]
                        {
                            text,
                            ". Closest is ",
                            ((long)APState.TrackedDistance).ToString(),
                            " m (",
                            ((int)APState.TrackedAngle).ToString(),
                            "°) away"
                        });
                        text = text + ", named " + APState.TrackedLocationName;
                    }
                    GUI.Label(new Rect(16f, 36f, 1000f, 20f), text);
                }
                if (APState.TrackedFishCount > 0L && APState.TrackedMode != TrackerMode.Disabled)
                {
                    GUI.Label(new Rect(16f, 56f, 1000f, 22f), "Fish left: " + APState.TrackedFishCount.ToString() + ". Such as: " + APState.TrackedFish);
                }
                if (this.PlayerNearStart())
                {
                    GUI.Label(new Rect(16f, 76f, 1000f, 22f), "Goal: " + APState.Goal);
                    if (APState.SwimRule.Length == 0)
                    {
                        GUI.Label(new Rect(16f, 96f, 1000f, 22f), "No Swim Rule sent by Server. Assuming items_hard. Current Logical Depth: " + (TrackerThread.LogicSwimDepth + TrackerThread.LogicVehicleDepth).ToString());
                    }
                    else
                    {
                        GUI.Label(new Rect(16f, 96f, 1000f, 22f), string.Concat(new string[]
                        {
                            "Swim Rule: ",
                            APState.SwimRule,
                            " Current Logical Depth: ",
                            (TrackerThread.LogicSwimDepth + TrackerThread.LogicVehicleDepth).ToString(),
                            " = ",
                            TrackerThread.LogicSwimDepth.ToString(),
                            " (Swim) + ",
                            TrackerThread.LogicVehicleDepth.ToString(),
                            " (",
                            TrackerThread.LogicVehicle,
                            ")"
                        }));
                    }
                }
                if (!APState.TrackerProcessing.IsAlive)
                {
                    GUI.Label(new Rect(16f, 116f, 1000f, 22f), "Error: Tracker Thread died. Tracker will not update.");
                }
            }
        }

        public bool PlayerNearStart()
        {
            if (ArchipelagoPlugin.Zero)
            {
                return true;
            }
            FieldInfo field = ArchipelagoPlugin.SubnauticaEscapePod.GetField("main");
            object obj = (field != null) ? field.GetValue(ArchipelagoPlugin.SubnauticaEscapePod) : null;
            if (obj == null)
            {
                return false;
            }
            PropertyInfo property = ArchipelagoPlugin.SubnauticaEscapePod.GetProperty("transform");
            Transform transform = ((property != null) ? property.GetValue(obj) : null) as Transform;
            return transform != null && (transform.position - Player.main.transform.position).magnitude < 10f;
        }

        [HarmonyPatch(typeof(ConsoleInput))]
        [HarmonyPatch("Validate")]
        internal class ConsoleHook
        {
            [HarmonyPrefix]
            private static bool AllowExclamationPoint(string text, int pos, char ch, ref char __result)
            {
                if (ch == '!')
                {
                    __result = ch;
                    return false;
                }
                return true;
            }
        }
    }
}
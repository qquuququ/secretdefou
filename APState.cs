using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.MessageLog.Messages;
using Archipelago.MultiClient.Net.Models;
using Archipelago.MultiClient.Net.Packets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Archipelago
{
    public static class APState
    {
        public static bool Connect()
        {
            if (APState.Authenticated)
            {
                return true;
            }
            if (APState.ServerConnectInfo.host_name == null || APState.ServerConnectInfo.host_name.Length == 0)
            {
                return false;
            }
            APState.Session = ArchipelagoSessionFactory.CreateSession(APState.ServerConnectInfo.host_name, 38281);

            // Subscribe to events
            APState.Session.MessageLog.OnMessageReceived += APState.Session_MessageReceived;
            APState.Session.Socket.ErrorReceived += APState.Session_ErrorReceived;
            APState.Session.Socket.SocketClosed += APState.Session_SocketClosed;

            HashSet<TechType> hashSet = new HashSet<TechType>();
            LoginResult loginResult = APState.Session.TryConnectAndLogin("Subnautica: Below Zero", APState.ServerConnectInfo.slot_name, ItemsHandlingFlags.AllItems, new Version(APState.AP_VERSION[0], APState.AP_VERSION[1], APState.AP_VERSION[2]), null, "", APState.ServerConnectInfo.password, true);
            LoginSuccessful loginSuccessful = loginResult as LoginSuccessful;
            if (loginSuccessful != null)
            {
                UserStoragePC userStoragePC = PlatformUtils.main.GetServices().GetUserStorage() as UserStoragePC;
                object obj = null;
                if (userStoragePC != null)
                {
                    FieldInfo field = userStoragePC.GetType().GetField("savePath", BindingFlags.Instance | BindingFlags.NonPublic);
                    obj = ((field != null) ? field.GetValue(userStoragePC) : null);
                }

                if (obj != null)
                {
                    APState.ServerConnectInfo.GetAsLastConnect().WriteToFile(obj.ToString() + "/archipelago_last_connection.json");
                }
                else
                {
                    Logging.LogError("Could not write most recent connect info to file.", true, true);
                }

                APState.Authenticated = true;
                APState.state = APState.State.InGame;

                ArchipelagoData.Initialize(loginSuccessful.SlotData);
                TargetRewards.LoadFromSlotData(loginSuccessful.SlotData);
                MilestoneRewards.Initialize();

                // 📦 Log des items au démarrage
                Debug.Log($"✅ Connected! SlotData keys: {string.Join(", ", loginSuccessful.SlotData.Keys)}");
                Debug.Log($"📦 AllItemsReceived at start: {APState.Session.Items.AllItemsReceived.Count}");
                foreach (var item in APState.Session.Items.AllItemsReceived)
                {
                    Debug.Log($"   - Item ID: {item.ItemId}");
                }

                // Force la synchronisation au démarrage
                APState.Resync();

                // Read slot data
                object obj3;
                if (loginSuccessful.SlotData.TryGetValue("swim_rule", out obj3))
                {
                    APState.SwimRule = (string)obj3;
                }
                object value4;
                if (loginSuccessful.SlotData.TryGetValue("free_samples", out value4))
                {
                    APState.FreeSamples = (Convert.ToInt32(value4) > 0);
                }
                object value5;
                if (loginSuccessful.SlotData.TryGetValue("empty_tanks", out value5))
                {
                    APState.EmptyTanks = (Convert.ToInt32(value5) > 0);
                }

                APState.Goal = (string)loginSuccessful.SlotData["goal"];
                APState.GoalMapping.TryGetValue(APState.Goal, out APState.GoalEvent);

                JArray jarray = loginSuccessful.SlotData["vanilla_tech"] as JArray;
                if (jarray != null)
                {
                    foreach (JToken jtoken in jarray)
                    {
                        hashSet.Add((TechType)Enum.Parse(typeof(TechType), jtoken.ToString()));
                    }
                }

                Logging.Log("SlotData: " + JsonConvert.SerializeObject(loginSuccessful.SlotData), false, true, false);
                APState.ServerConnectInfo.death_link = (Convert.ToInt32(loginSuccessful.SlotData["death_link"]) > 0);
                APState.set_deathlink();
            }
            else
            {
                LoginFailure loginFailure = loginResult as LoginFailure;
                if (loginFailure != null)
                {
                    APState.Authenticated = false;
                    Logging.LogError("Connection Error: " + string.Join("\n", loginFailure.Errors), true, true);
                    APState.Session = null;
                }
            }

            APState.TechFragmentsToDestroy = new HashSet<TechType>(APState.tech_fragments);
            APState.TechFragmentsToDestroy.ExceptWith(hashSet);
            Logging.LogDebug("Preventing scanning of: " + string.Join<TechType>(", ", APState.TechFragmentsToDestroy));
            Logging.LogDebug("Allowing scanning of: " + string.Join<TechType>(", ", hashSet));
            return loginResult.Successful;
        }

        private static void Session_SocketClosed(string reason)
        {
            Logging.LogError("Connection to Archipelago lost: " + reason, true, true);
            APState.Disconnect();
        }

        private static void Session_MessageReceived(LogMessage message)
        {
            if (!APState.Silent)
            {
                APState.message_queue.Add(message.ToString());
            }
        }

        private static void Session_ErrorReceived(Exception e, string message)
        {
            Logging.LogError(message, true, true);
            if (e != null)
            {
                Logging.LogError(e.ToString(), true, true);
            }
            APState.Disconnect();
        }

        public static void Disconnect()
        {
            APState.Authenticated = false;
            APState.state = APState.State.Menu;
            if (APState.Session != null && APState.Session.Socket != null && APState.Session.Socket.Connected)
            {
                Task.Run(delegate ()
                {
                    APState.Session.Socket.DisconnectAsync();
                }).Wait();
            }
            APState.Session = null;
        }

        public static void DeathLinkReceived(DeathLink deathLink)
        {
            if (!Player.main.liveMixin)
            {
                return;
            }
            Logging.LogDebug("Received DeathLink");
            APState.DeathLinkKilling = true;
            Player.main.liveMixin.Kill(DamageType.Normal);
            APState.message_queue.Add(deathLink.Cause);
        }

        public static void SendLocID(long id)
        {
            Debug.Log($"🔴 SendLocID CALLED with id={id}");

            if (APState.ServerConnectInfo.@checked.Add(id))
            {
                Debug.Log($"✅ Location {id} added to checked set");

                try
                {
                    Debug.Log($"📤 Sending location {id} to Archipelago...");

                    var locationsToCheck = APState.ServerConnectInfo.@checked
                        .Except(APState.Session.Locations.AllLocationsChecked)
                        .ToArray<long>();

                    Debug.Log($"📊 Locations to check: {locationsToCheck.Length}");

                    APState.Session.Locations.CompleteLocationChecksAsync(locationsToCheck).Wait();

                    Debug.Log($"✅ Location {id} sent successfully to Archipelago");
                    Debug.Log($"📦 AllItemsReceived count: {APState.Session.Items.AllItemsReceived.Count}");

                    // Force resync après envoi de location
                    APState.Resync();
                }
                catch (Exception ex)
                {
                    Debug.LogError($"❌ Failed to send location {id}: {ex.Message}\n{ex.StackTrace}");
                    return;
                }
            }
            else
            {
                Debug.Log($"⚠️ Location {id} already checked before");
            }
        }

        public static void Resync()
        {
            Logging.LogDebug("Running Item resync with " + APState.Session.Items.AllItemsReceived.Count.ToString() + " items.");
            Debug.Log($"🔄 Resync called - AllItemsReceived: {APState.Session.Items.AllItemsReceived.Count}");

            HashSet<long> hashSet = new HashSet<long>();
            for (int i = 0; i < APState.Session.Items.AllItemsReceived.Count; i++)
            {
                ItemInfo itemInfo = APState.Session.Items.AllItemsReceived[i];
                long itemId = itemInfo.ItemId;
                long index = i;

                Debug.Log($"   [{i}] ItemId: {itemId}");

                if (!ArchipelagoData.ItemCodeToItemType.ContainsKey(itemId))
                {
                    Debug.LogWarning($"   ❌ ItemId {itemId} NOT in ItemCodeToItemType!");
                    continue;
                }

                string itemType = ArchipelagoData.ItemCodeToItemType[itemId];
                Debug.Log($"   ✅ ItemId {itemId} is type: {itemType}");

                if (itemType == ArchipelagoItemType.Resource.ToString() || !hashSet.Contains(itemId))
                {
                    APState.Unlock(itemId, index);
                    hashSet.Add(itemId);
                }
            }
        }

        public static void Unlock(long apItemID, long index)
        {
            Debug.Log($"🔓 Unlock called with apItemID={apItemID}, index={index}");

            // ✅ Vérifier que l'item existe dans les dictionnaires
            if (!ArchipelagoData.ItemCodeToItemType.ContainsKey(apItemID))
            {
                Debug.LogError($"❌ Item {apItemID} not found in ItemCodeToItemType!");
                Debug.Log($"   Available keys: {string.Join(", ", ArchipelagoData.ItemCodeToItemType.Keys.Take(10))}...");
                return;
            }

            List<long> list;
            if (ArchipelagoData.GroupItems.TryGetValue(apItemID, out list))
            {
                Debug.Log($"👥 Item {apItemID} is a group with {list.Count} items");
                foreach (long apItemID2 in list)
                {
                    APState.Unlock(apItemID2, index);
                }
                return;
            }

            TechType techType = TechType.None;
            ArchipelagoData.ItemCodeToTechType.TryGetValue(apItemID, out techType);

            string itemType;
            if (!ArchipelagoData.ItemCodeToItemType.TryGetValue(apItemID, out itemType))
            {
                Debug.LogError($"❌ Item type not found for {apItemID}");
                return;
            }

            // Parse l'enum
            if (Enum.TryParse<ArchipelagoItemType>(itemType, out ArchipelagoItemType archType))
            {
                switch (archType)
                {
                    case ArchipelagoItemType.Resource:
                        Debug.Log($"📦 Processing resource item {apItemID}");

                        HashSet<long> hashSet;
                        if (!APState.ServerConnectInfo.resources_granted.TryGetValue(apItemID, out hashSet))
                        {
                            hashSet = new HashSet<long>();
                            APState.ServerConnectInfo.resources_granted[apItemID] = hashSet;
                        }
                        if (hashSet.Contains(index))
                        {
                            Debug.Log($"⚠️ Resource {apItemID} already granted at index {index}");
                            return;
                        }
                        hashSet.Add(index);
                        for (int i = 0; i < hashSet.Count; i++)
                        {
                            Inventory.main.StartCoroutine(APState.PickUp(techType));
                        }
                        break;

                    case ArchipelagoItemType.Technology:
                        Debug.Log($"🔬 Processing tech item {apItemID} -> {techType}");

                        if (techType == TechType.None || KnownTech.Contains(techType))
                        {
                            Debug.Log($"⚠️ Tech {techType} already known or None");
                            return;
                        }

                        if (PDAScanner.IsFragment(techType))
                        {
                            Debug.Log($"🔍 Processing fragment: {techType}");

                            PDAScanner.EntryData entryData = PDAScanner.GetEntryData(techType);
                            PDAScanner.Entry entry;
                            if (!PDAScanner.GetPartialEntryByKey(techType, out entry))
                            {
                                entry = (PDAScanner.Entry)typeof(PDAScanner).GetMethod("Add", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[]
                                {
                                    typeof(TechType),
                                    typeof(int)
                                }, null).Invoke(null, new object[]
                                {
                                    techType,
                                    0
                                });
                            }
                            if (entry != null)
                            {
                                int num = APState.Session.Items.AllItemsReceived.Count((ItemInfo networkItem) => networkItem.ItemId == apItemID);
                                if (num == entry.unlocked)
                                {
                                    return;
                                }
                                entry.unlocked = num;
                                if (entry.unlocked < entryData.totalFragments)
                                {
                                    int totalFragments = entryData.totalFragments;
                                    if (totalFragments > 1)
                                    {
                                        float arg = (float)Mathf.RoundToInt((float)entry.unlocked / (float)totalFragments * 100f);
                                        ErrorMessage.AddError(Language.main.GetFormat<string, float, int, int>("ScannerInstanceScanned", Language.main.Get(entry.techType.AsString(false)), arg, entry.unlocked, totalFragments));
                                    }
                                    typeof(PDAScanner).GetMethod("NotifyProgress", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[]
                                    {
                                        typeof(PDAScanner.Entry)
                                    }, null).Invoke(null, new object[]
                                    {
                                        entry
                                    });
                                    return;
                                }
                                List<PDAScanner.Entry> list2 = (List<PDAScanner.Entry>)typeof(PDAScanner).GetField("partial", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
                                HashSet<TechType> hashSet2 = (HashSet<TechType>)typeof(PDAScanner).GetField("complete", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
                                list2.Remove(entry);
                                hashSet2.Add(entry.techType);
                                typeof(PDAScanner).GetMethod("NotifyRemove", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[]
                                {
                                    typeof(PDAScanner.Entry)
                                }, null).Invoke(null, new object[]
                                {
                                    entry
                                });
                                typeof(PDAScanner).GetMethod("Unlock", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[]
                                {
                                    typeof(PDAScanner.EntryData),
                                    typeof(bool),
                                    typeof(bool),
                                    typeof(bool)
                                }, null).Invoke(null, new object[]
                                {
                                    entryData,
                                    true,
                                    false,
                                    true
                                });
                                if (APState.FreeSamples)
                                {
                                    APState.GiveItem(entryData.blueprint);
                                    return;
                                }
                            }
                        }
                        else
                        {
                            Debug.Log($"🔧 Unlocking blueprint: {techType}");

                            try
                            {
                                KnownTech.Add(techType, true, true);
                                Debug.Log($"✅ Blueprint {techType} unlocked successfully");
                            }
                            catch (System.Exception ex)
                            {
                                Debug.LogError($"❌ Failed to unlock blueprint {techType}: {ex.Message}");
                            }

                            if (APState.FreeSamples)
                            {
                                APState.GiveItem(techType);
                            }
                        }
                        break;

                    case ArchipelagoItemType.Group:
                        Debug.Log($"👥 Processing group item {apItemID}");
                        // Les groupes sont gérés ci-dessus dans le check GroupItems
                        break;
                }
            }
            else
            {
                Debug.LogError($"❌ Could not parse item type '{itemType}' to ArchipelagoItemType");
            }
        }

        private static IEnumerator PickUp(TechType techType)
        {
            yield return new WaitForSeconds(0.1f);

            TaskResult<GameObject> instResult = new TaskResult<GameObject>();
            yield return CraftData.InstantiateFromPrefabAsync(techType, instResult, false);

            GameObject gameObject = instResult.Get();
            if (gameObject != null)
            {
                Pickupable pickupable = gameObject.GetComponent<Pickupable>();
                if (pickupable != null)
                {
                    Inventory.main.Pickup(pickupable, true);
                }
            }
        }

        private static IEnumerator GiveItemAsync(TechType techType, bool giveLinked = false, bool filterCategory = true)
        {
            yield return new WaitForSeconds(0.1f);

            TaskResult<GameObject> instResult = new TaskResult<GameObject>();
            yield return CraftData.InstantiateFromPrefabAsync(techType, instResult, false);

            GameObject gameObject = instResult.Get();
            if (gameObject != null)
            {
                Pickupable pickupable = gameObject.GetComponent<Pickupable>();
                if (pickupable != null)
                {
                    Inventory.main.Pickup(pickupable, true);
                }
                else
                {
                    gameObject.transform.position = Player.main.transform.position + Player.main.transform.forward * 2f;
                }
            }
        }

        public static void GiveItem(TechType techType)
        {
            Inventory.main.StartCoroutine(APState.GiveItemAsync(techType, true, true));
        }

        public static void set_deathlink()
        {
            if (APState.DeathLinkService == null)
            {
                APState.DeathLinkService = APState.Session.CreateDeathLinkService();
                APState.DeathLinkService.OnDeathLinkReceived += APState.DeathLinkReceived;
            }
            if (APState.ServerConnectInfo.death_link)
            {
                APState.DeathLinkService.EnableDeathLink();
                return;
            }
            APState.DeathLinkService.DisableDeathLink();
        }

        public static void send_completion()
        {
            StatusUpdatePacket statusUpdatePacket = new StatusUpdatePacket();
            statusUpdatePacket.Status = ArchipelagoClientState.ClientGoal;
            APState.Session.Socket.SendPacket(statusUpdatePacket);
        }

        public static Dictionary<string, string> GoalMapping = new Dictionary<string, string>
        {
            {
                "launch",
                "AlAn_WarpAway"
            }
        };

        public static int[] AP_VERSION = new int[]
        {
            0,
            6,
            3
        };

        public static APConnectInfo ServerConnectInfo = new APConnectInfo();

        public static DeathLinkService DeathLinkService = null;

        public static bool DeathLinkKilling = false;

        public static Dictionary<string, int> archipelago_indexes = new Dictionary<string, int>();

        public static float unlock_dequeue_timeout = 0f;

        public static List<string> message_queue = new List<string>();

        public static float message_dequeue_timeout = 0f;

        public static APState.State state = APState.State.Menu;

        public static bool Authenticated;

        public static string Goal = "launch";

        public static string GoalEvent = "";

        public static string SwimRule = "";

        public static bool EmptyTanks = true;

        public static bool FreeSamples;

        public static bool Silent = false;

        public static Thread TrackerProcessing;

        public static long TrackedLocationsCount = 0L;

        public static long TrackedFishCount = 0L;

        public static string TrackedFish = "";

        public static long TrackedLocation = -1L;

        public static string TrackedLocationName;

        public static float TrackedDistance;

        public static float TrackedAngle;

        public static ArchipelagoSession Session;

        public static ArchipelagoUI ArchipelagoUI = null;

        public static HashSet<TechType> tech_fragments = new HashSet<TechType>
        {
            TechType.SeaglideFragment,
            TechType.PropulsionCannonFragment,
            TechType.LaserCutterFragment,
            TechType.ExosuitFragment,
            TechType.SeaTruckFragment,
            TechType.BuilderFragment,
            TechType.ExosuitDrillArmFragment,
            TechType.ExosuitGrapplingArmFragment,
            TechType.ExosuitPropulsionArmFragment,
            TechType.ExosuitTorpedoArmFragment,
            TechType.SeaTruckDockingModuleFragment,
            TechType.SeaTruckStorageModuleFragment,
            TechType.SeaTruckFabricatorModuleFragment,
            TechType.SeaTruckAquariumModuleFragment,
            TechType.SeaTruckSleeperModuleFragment,
            TechType.SeaTruckUpgradeHorsePowerFragment,
            TechType.SeaTruckUpgradeAfterburnerFragment,
            TechType.NuclearReactorFragment,
            TechType.ThermalPlantFragment,
            TechType.RadioTowerPPUFragment,
            TechType.RadioTowerTOMFragment,
            TechType.MetalDetectorFragment,
            TechType.HydraulicFluidFragment,
            TechType.ColdSuitFragment,
            TechType.HighCapacityTankFragment,
            TechType.ReinforcedDiveSuitFragment,
            TechType.GravSphereFragment,
            TechType.SpyPenguinFragment,
            TechType.LEDLightFragment
        };

        public static TrackerMode TrackedMode = TrackerMode.Logical;

        public static HashSet<TechType> TechFragmentsToDestroy = new HashSet<TechType>();

        public struct Location
        {
            public long ID;
            public Vector3 Position;
        }

        public enum State
        {
            Menu,
            InGame
        }
    }
}
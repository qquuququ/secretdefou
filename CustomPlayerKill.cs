using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using HarmonyLib;

namespace Archipelago
{
    // Token: 0x0200001A RID: 26
    [HarmonyPatch(typeof(Player), "OnKill")]
    internal class CustomPlayerKill
    {
        // Token: 0x0600004D RID: 77 RVA: 0x00004186 File Offset: 0x00002386
        [HarmonyPostfix]
        public static void PlayerDeath(DamageType damageType)
        {
            if (!APState.DeathLinkKilling && APState.ServerConnectInfo.death_link)
            {
                APState.DeathLinkService.SendDeathLink(new DeathLink(APState.ServerConnectInfo.slot_name, null));
            }
            APState.DeathLinkKilling = false;
        }
    }
}
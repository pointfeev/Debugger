using HarmonyLib;
using TaleWorlds.CampaignSystem;

namespace Debugger
{
    [HarmonyPatch(typeof(TroopUpgradeTracker))]
    public static class PatchTroopUpgradeTracker
    {
        [HarmonyPrefix]
        [HarmonyPatch("CalculateReadyToUpgradeSafe")]
        public static bool CalculateReadyToUpgradeSafe(ref TroopRosterElement el, PartyBase owner, ref int __result)
        {
            if (el.Character is null || owner is null)
            {
                OutputUtils.DoOutput($"Debugger prevented crashes from an invalid CalculateReadyToUpgradeSafe call.", true);
                __result = 0;
                return false;
            }
            return true;
        }
    }
}
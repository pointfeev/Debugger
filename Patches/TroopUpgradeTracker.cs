using HarmonyLib;
using System;
using System.Reflection;
using TaleWorlds.CampaignSystem;

namespace Debugger
{
    [HarmonyPatch(typeof(TroopUpgradeTracker))]
    public static class PatchTroopUpgradeTracker
    {
        [HarmonyReversePatch(HarmonyReversePatchType.Original)]
        [HarmonyPatch("CalculateReadyToUpgradeSafe")]
        public static int CalculateReadyToUpgradeSafe(TroopUpgradeTracker instance, ref TroopRosterElement el, PartyBase owner)
        {
            OutputUtils.DoOutputForReversePatchFailure(MethodBase.GetCurrentMethod());
            return 0;
        }

        [HarmonyPrefix]
        [HarmonyPatch("CalculateReadyToUpgradeSafe")]
        public static bool CalculateReadyToUpgradeSafe(TroopUpgradeTracker __instance, ref TroopRosterElement el, PartyBase owner, ref int __result)
        {
            if (!ReflectionUtils.IsMethodInCallStack(MethodBase.GetCurrentMethod()))
            {
                try
                {
                    __result = CalculateReadyToUpgradeSafe(__instance, ref el, owner);
                }
                catch (Exception e)
                {
                    __result = 0;
                    OutputUtils.DoOutputForException(e);
                }
                return false;
            }
            return true;
        }
    }
}
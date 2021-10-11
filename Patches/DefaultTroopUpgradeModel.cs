using HarmonyLib;
using System;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.GameComponents;

namespace Debugger
{
    [HarmonyPatch(typeof(DefaultPartyTroopUpgradeModel))]
    public static class PatchDefaultPartyTroopUpgradeModel
    {
        [HarmonyPrefix]
        [HarmonyPatch("CanTroopGainXp")]
        public static bool CanTroopGainXp(DefaultPartyTroopUpgradeModel __instance, PartyBase owner, CharacterObject character, ref bool __result)
        {
            if (!ReflectionUtils.IsMethodInCallStack(MethodBase.GetCurrentMethod()))
            {
                try
                {
                    __result = __instance.CanTroopGainXp(owner, character);
                }
                catch (Exception e)
                {
                    __result = false;
                    OutputUtils.DoOutputForException(e);
                }
                return false;
            }
            return true;
        }
    }
}
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.GameComponents;

namespace Debugger
{
    [HarmonyPatch(typeof(DefaultPartyTroopUpgradeModel))]
    public static class PatchDefaultPartyTroopUpgradeModel
    {
        [HarmonyPrefix]
        [HarmonyPatch("CanTroopGainXp")]
        public static bool CanTroopGainXp(PartyBase owner, CharacterObject character, ref bool __result)
        {
            if (owner is null || character is null)
            {
                OutputUtils.DoOutput($"Debugger prevented crashes from an invalid CanTroopGainXp call.", true);
                __result = false;
                return false;
            }
            return true;
        }
    }
}
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.GameComponents.Map;

namespace Debugger
{
    [HarmonyPatch(typeof(DefaultDiplomacyModel))]
    public static class PatchDefaultDiplomacyModel
    {
        [HarmonyPrefix]
        [HarmonyPatch("GetHeroesForEffectiveRelation")]
        public static bool GetHeroesForEffectiveRelation(Hero hero1, Hero hero2, out Hero effectiveHero1, out Hero effectiveHero2)
        {
            effectiveHero1 = hero1;
            effectiveHero2 = hero2;
            if (hero1 is null || hero2 is null)
            {
                OutputUtils.DoOutput($"Debugger prevented crashes from a GetHeroesForEffectiveRelation call that was passed null heroes.", true);
                return false;
            }
            return true;
        }
    }
}
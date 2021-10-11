using HarmonyLib;
using System;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.GameComponents.Map;

namespace Debugger
{
    [HarmonyPatch(typeof(DefaultDiplomacyModel))]
    public static class PatchDefaultDiplomacyModel
    {
        [HarmonyPrefix]
        [HarmonyPatch("GetHeroesForEffectiveRelation")]
        public static bool GetHeroesForEffectiveRelation(DefaultDiplomacyModel __instance, Hero hero1, Hero hero2, out Hero effectiveHero1, out Hero effectiveHero2)
        {
            effectiveHero1 = hero1;
            effectiveHero2 = hero2;
            if (!ReflectionUtils.IsMethodInCallStack(MethodBase.GetCurrentMethod()))
            {
                try
                {
                    __instance.GetHeroesForEffectiveRelation(hero1, hero2, out effectiveHero1, out effectiveHero2);
                }
                catch (Exception e)
                {
                    OutputUtils.DoOutputForException(e);
                }
                return false;
            }
            return true;
        }
    }
}
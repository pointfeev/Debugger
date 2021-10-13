using HarmonyLib;
using System;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;

namespace Debugger
{
    [HarmonyPatch(typeof(CaravansCampaignBehavior))]
    public static class PatchCaravansCampaignBehavior
    {
        [HarmonyPrefix]
        [HarmonyPatch("OnMapEventEnded")]
        public static bool OnMapEventEnded(CaravansCampaignBehavior __instance, MapEvent mapEvent)
        {
            if (!ReflectionUtils.IsMethodInCallStack(MethodBase.GetCurrentMethod()))
            {
                try
                {
                    typeof(CaravansCampaignBehavior).GetMethod("OnMapEventEnded", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(__instance, new object[] {
                        mapEvent
                    });
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
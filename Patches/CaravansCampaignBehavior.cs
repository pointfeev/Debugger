using HarmonyLib;
using System;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;

namespace Debugger
{
    [HarmonyPatch(typeof(CaravansCampaignBehavior))]
    public static class PatchCaravansCampaignBehavior
    {
        [HarmonyFinalizer]
        [HarmonyPatch("OnMapEventEnded")]
        public static Exception OnMapEventEnded(Exception __exception)
        {
            return FinalizeUtils.Finalizer(__exception);
        }
    }
}
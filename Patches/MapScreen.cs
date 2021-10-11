using HarmonyLib;
using SandBox.View.Map;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace Debugger
{
    [HarmonyPatch(typeof(MapScreen))]
    public static class PatchMapScreen
    {
        private static void FixMachineArray(int? correctSize, string containerSide, SiegeEvent.SiegeEnginesContainer container, SiegeEvent.SiegeEngineConstructionProgress[] progresses)
        {
            if (!(container is null) && !(progresses is null) && correctSize > progresses.Length)
            {
                List<SiegeEvent.SiegeEngineConstructionProgress> asList = progresses.ToList();
                asList.AddRange(new SiegeEvent.SiegeEngineConstructionProgress[(int)correctSize - asList.Count]);
                typeof(SiegeEvent.SiegeEnginesContainer).GetField(nameof(progresses), BindingFlags.Public | BindingFlags.Instance)
                    .SetValue(container, progresses.ToArray());
                OutputUtils.DoOutput($"Debugger attempted to fix an incorrect {containerSide} {nameof(progresses)} to prevent crashes.", true);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch("TickSiegeMachineCircles")]
        public static bool TickSiegeMachineCircles()
        {
            SiegeEvent siege = PlayerSiege.PlayerSiegeEvent;
            IPartyVisual visuals = siege?.BesiegedSettlement?.Party?.Visuals;
            SiegeEvent.SiegeEnginesContainer defender = siege?.GetSiegeEventSide(BattleSideEnum.Defender)?.SiegeEngines;
            SiegeEvent.SiegeEnginesContainer attacker = siege?.GetSiegeEventSide(BattleSideEnum.Attacker)?.SiegeEngines;
            FixMachineArray(visuals?.GetDefenderSiegeEngineFrameCount(), "Defender", defender, defender?.DeployedRangedSiegeEngines);
            FixMachineArray(visuals?.GetAttackerRangedSiegeEngineFrameCount(), "Attacker", attacker, attacker?.DeployedRangedSiegeEngines);
            FixMachineArray(visuals?.GetAttackerBatteringRamSiegeEngineFrameCount() + visuals?.GetAttackerTowerSiegeEngineFrameCount(), "Attacker", defender, attacker?.DeployedMeleeSiegeEngines);
            return true;
        }
    }
}
using HarmonyLib;
using SandBox.ViewModelCollection.MapSiege;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace Debugger
{
    [HarmonyPatch(typeof(MapSiegePOIVM))]
    public static class PatchMapSiegePOIVM
    {
        private static SiegeEngineType GetMachineType(MapSiegePOIVM instance)
        {
            int machineTypeNum = instance.MachineType;
            string machineTypeStr = Enum.GetName(typeof(MapSiegePOIVM.MachineTypes), machineTypeNum);
            if (!(machineTypeStr is null) && !machineTypeStr.Equals("None"))
            {
                PropertyInfo propertyInfo = typeof(DefaultSiegeEngineTypes).GetProperty(machineTypeStr);
                if (!(propertyInfo is null))
                {
                    return (SiegeEngineType)propertyInfo.GetMethod.Invoke(null, new object[0]);
                }
            }
            return null;
        }

        private static void FixMachineArray(MapSiegePOIVM instance, string containerSide, SiegeEvent.SiegeEnginesContainer container, SiegeEvent.SiegeEngineConstructionProgress[] progresses)
        {
            if (!(container is null) && !(progresses is null) && instance.MachineIndex >= progresses.Length)
            {
                List<SiegeEvent.SiegeEngineConstructionProgress> asList = progresses.ToList();
                asList.AddRange(new SiegeEvent.SiegeEngineConstructionProgress[instance.MachineIndex + 1 - asList.Count]);
                SiegeEngineType machineType = GetMachineType(instance);
                SiegeEvent.SiegeEngineConstructionProgress progress = null;
                if (!(machineType is null))
                {
                    progress = new SiegeEvent.SiegeEngineConstructionProgress(machineType, 0, machineType.BaseHitPoints);
                    progress.Activate(true);
                }
                asList.Insert(instance.MachineIndex, progress);
                typeof(SiegeEvent.SiegeEnginesContainer).GetField(nameof(progresses), BindingFlags.Public | BindingFlags.Instance)
                    .SetValue(container, progresses.ToArray());
                OutputUtils.DoOutput($"Debugger attempted to fix an incorrect {containerSide} {nameof(progresses)} to prevent crashes.", true);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch("GetDesiredMachine")]
        public static bool GetDesiredMachine(MapSiegePOIVM __instance)
        {
            SiegeEvent siege = PlayerSiege.PlayerSiegeEvent;
            SiegeEvent.SiegeEnginesContainer defender = siege?.GetSiegeEventSide(BattleSideEnum.Defender)?.SiegeEngines;
            SiegeEvent.SiegeEnginesContainer attacker = siege?.GetSiegeEventSide(BattleSideEnum.Attacker)?.SiegeEngines;
            switch (__instance.Type)
            {
                case MapSiegePOIVM.POIType.DefenderSiegeMachine:
                    FixMachineArray(__instance, "Defender", defender, defender?.DeployedRangedSiegeEngines);
                    break;

                case MapSiegePOIVM.POIType.AttackerRamSiegeMachine:
                case MapSiegePOIVM.POIType.AttackerTowerSiegeMachine:
                    FixMachineArray(__instance, "Attacker", attacker, attacker?.DeployedMeleeSiegeEngines);
                    break;

                case MapSiegePOIVM.POIType.AttackerRangedSiegeMachine:
                    FixMachineArray(__instance, "Attacker", attacker, attacker?.DeployedRangedSiegeEngines);
                    break;
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch("RefreshHitpoints")]
        public static bool RefreshHitpoints(MapSiegePOIVM __instance)
        {
            Settlement settlement = PlayerSiege.PlayerSiegeEvent?.BesiegedSettlement;
            if (!(settlement is null) && __instance.Type == MapSiegePOIVM.POIType.WallSection &&
                __instance.MachineIndex >= settlement.SettlementWallSectionHitPointsRatioList.Count)
            {
                OutputUtils.DoOutput($"Debugger nullified a RefreshHitpoints call for an invalid WallSection POI to prevent crashes.");
                return false;
            }
            return true;
        }
    }
}
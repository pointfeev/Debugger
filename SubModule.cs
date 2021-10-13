using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace Debugger
{
    public class SubModule : MBSubModuleBase
    {
        private bool harmonyPatched = false;

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            if (!harmonyPatched)
            {
                Harmony harmony = new Harmony("pointfeev.debugger");
                harmony.PatchAll();
                FinalizeUtils.FinalizeMethods(harmony, "PocColorModAgentVisualsAddMeshes", "Postfix", "PocColor");
                FinalizeUtils.FinalizeMethods(harmony, "StartRebellionAction", "Apply", "Diplomacy");
                FinalizeUtils.FinalizeMethods(harmony, "HasEnoughScoreCondition", "ApplyCondition", "Diplomacy", typeNameExplicit: false);
                FinalizeUtils.FinalizeMethods(harmony, "CaravansCampaignBehaviorPatch", "OnMapEventEndedPrefix", "SupplyLines");
                FinalizeUtils.DoFinalizedMethodsOutput();
                harmonyPatched = true;
                InformationManager.DisplayMessage(new InformationMessage("Debugger initialized", Colors.Red, "Debugger"));
            }
        }
    }
}
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View;

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

                Patcher.PrefixMethods(harmony, "TaleWorlds.CampaignSystem", "TroopUpgradeTracker", "CalculateReadyToUpgradeSafe", delegate (object instance, ref object result, object[] parameters)
                {
                    if (((TroopRosterElement)parameters[0]).Character is null || ((PartyBase)parameters[1]) is null)
                    {
                        result = 0;
                        return false;
                    }
                    return true;
                });

                Patcher.FinalizeMethods(harmony, "TaleWorlds.MountAndBlade.View", "BannerVisual", "ConvertToMultiMesh", fallback: delegate (object instance, ref object result, object[] parameters)
                {
                    result = Banner.CreateOneColoredEmptyBanner(0).ConvertToMultiMesh();
                    return true;
                });

                Patcher.FinalizeMethods(harmony, "TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors", "CaravansCampaignBehavior", "OnMapEventEnded");

                Patcher.FinalizeMethods(harmony, "TaleWorlds.MountAndBlade.View", "AgentVisuals", "AddSkinArmorWeaponMultiMeshesToEntity");
                Patcher.FinalizeMethods(harmony, "PocColor", "PocColorModAgentVisualsAddMeshes", "Postfix");

                Patcher.FinalizeMethods(harmony, "Diplomacy.CivilWar.Actions", "StartRebellionAction", "Apply");
                Patcher.FinalizeMethods(harmony, "Diplomacy.DiplomaticAction", "HasEnoughScoreCondition", "ApplyCondition", modNamespaceExplicit: false);

                Patcher.FinalizeMethods(harmony, "SupplyLines", "CaravansCampaignBehaviorPatch", "OnMapEventEndedPrefix");

                //PatchUtils.FinalizeMethods(harmony, "DistinguishedService", "DSBattleLogic", "ShowBattleResults");

                harmonyPatched = true;
                Patcher.DoPatchedMethodsOutput();
                InformationManager.DisplayMessage(new InformationMessage("Debugger initialized", Colors.Red, "Debugger"));
            }
        }
    }
}
using HarmonyLib;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.View;
using static Debugger.PatchUtils;

namespace Debugger
{
    internal static class Patcher
    {
        internal static void Output()
        {
            foreach (KeyValuePair<string, int> pair in MethodsPatched)
            {
                InformationManager.DisplayMessage(new InformationMessage($"Debugger patched {pair.Value} {pair.Key} " + (pair.Value == 1 ? "method" : "methods"), Colors.Red, "Debugger"));
            }
        }

        internal static void Patch(Harmony harmony)
        {
            PrefixMethods(harmony, "TaleWorlds.CampaignSystem", "TroopUpgradeTracker", "CalculateReadyToUpgradeSafe", delegate (object instance, ref object result, object[] parameters)
            {
                if (((TroopRosterElement)parameters[0]).Character is null || ((PartyBase)parameters[1]) is null)
                {
                    result = 0;
                    return false;
                }
                return true;
            });
            PrefixMethods(harmony, "TaleWorlds.CampaignSystem", "FactionManager", "GetRelationBetweenClans", delegate (object instance, ref object result, object[] parameters)
            {
                if (((Clan)parameters[0]) is null || ((Clan)parameters[1]) is null)
                {
                    result = 0;
                    return false;
                }
                return true;
            });
            PrefixMethods(harmony, "TaleWorlds.CampaignSystem", "Clan", "GetRelationWithClan", delegate (object instance, ref object result, object[] parameters)
            {
                if (((Clan)parameters[0]) is null)
                {
                    result = 0;
                    return false;
                }
                return true;
            });
            PrefixMethods(harmony, "TaleWorlds.CampaignSystem.SandBox.GameComponents.Map", "DefaultDiplomacyModel", "GetHeroesForEffectiveRelation", delegate (object instance, ref object result, object[] parameters)
            {
                if (((Hero)parameters[0]) is null || ((Hero)parameters[1]) is null)
                {
                    parameters[2] = (Hero)parameters[0];
                    parameters[3] = (Hero)parameters[1];
                    return false;
                }
                return true;
            });
            PrefixMethods(harmony, "TaleWorlds.CampaignSystem.SandBox.GameComponents", "DefaultPartyTroopUpgradeModel", "CanTroopGainXp", delegate (object instance, ref object result, object[] parameters)
            {
                if (((PartyBase)parameters[0]) is null || ((CharacterObject)parameters[1]) is null)
                {
                    result = false;
                    return false;
                }
                return true;
            });
            PrefixMethods(harmony, "TaleWorlds.CampaignSystem.SandBox.GameComponents.Map", "DefaultDiplomacyModel", "GetScoreOfWarInternal", delegate (object instance, ref object result, object[] parameters)
            {
                if ((IFaction)parameters[0] is null || (IFaction)parameters[1] is null || (IFaction)parameters[2] is null)
                {
                    result = 0f;
                    return false;
                }
                return true;
            }, modNamespaceExplicit: false, typeNameExplicit: false);
            FinalizeMethods(harmony, "TaleWorlds.MountAndBlade.View", "BannerVisual", "ConvertToMultiMesh", fallback: delegate (object instance, ref object result, object[] parameters)
            {
                result = Banner.CreateOneColoredEmptyBanner(0).ConvertToMultiMesh();
                return true;
            });
            PrefixMethods(harmony, "Diplomacy.DiplomaticAction.Alliance.Conditions", "HasEnoughScoreCondition", "ApplyCondition", delegate (object instance, ref object result, object[] parameters)
            {
                if ((Kingdom)parameters[0] is null || (Kingdom)parameters[1] is null)
                {
                    parameters[2] = new TextObject("{=VvTTrRpl}This faction is not interested in forming an alliance with you.", null);
                    result = true;
                    return false;
                }
                return true;
            });
            PrefixMethods(harmony, "Diplomacy.DiplomaticAction.NonAggressionPact", "HasEnoughScoreCondition", "ApplyCondition", delegate (object instance, ref object result, object[] parameters)
            {
                if ((Kingdom)parameters[0] is null || (Kingdom)parameters[1] is null)
                {
                    parameters[2] = new TextObject("{=M4SGjzQP}This faction is not interested in forming a non-aggression pact with you.", null);
                    result = true;
                    return false;
                }
                return true;
            });
            FinalizeMethods(harmony, "TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors", "CaravansCampaignBehavior", "OnMapEventEnded");
            FinalizeMethods(harmony, "TaleWorlds.MountAndBlade", "Mission", "CheckMissionEnd");
            FinalizeMethods(harmony, "TaleWorlds.MountAndBlade.View", "AgentVisuals", "AddSkinArmorWeaponMultiMeshesToEntity");
            FinalizeMethods(harmony, "PocColor", "PocColorModAgentVisualsAddMeshes", "Postfix");
            FinalizeMethods(harmony, "Diplomacy.CivilWar.Actions", "StartRebellionAction", "Apply");
            FinalizeMethods(harmony, "Diplomacy.ViewModelMixin", "KingdomTruceItemVmMixin", "UpdateActionAvailability");
            FinalizeMethods(harmony, "SupplyLines", "CaravansCampaignBehaviorPatch", "OnMapEventEndedPrefix");
            FinalizeMethods(harmony, "AllegianceOverhaul.LoyaltyRebalance", "RelativesHelper", "BloodRelatives", fallback: delegate (object instance, ref object result, object[] parameters)
            {
                result = false;
                return true;
            });
        }
    }
}
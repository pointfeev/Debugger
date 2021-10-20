using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
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
            #region TaleWorlds

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
            FinalizeMethods(harmony, "TaleWorlds.CampaignSystem.SandBox.GameComponents.Map", "DefaultDiplomacyModel", "GetScoreOfWarInternal", delegate (object instance, ref object result, object[] parameters)
            {
                result = 0f;
                return true;
            });
            FinalizeMethods(harmony, "TaleWorlds.MountAndBlade.View", "BannerVisual", "ConvertToMultiMesh", delegate (object instance, ref object result, object[] parameters)
            {
                result = Banner.CreateOneColoredEmptyBanner(0).ConvertToMultiMesh();
                return true;
            });
            FinalizeMethods(harmony, "TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors", "CaravansCampaignBehavior", "OnMapEventEnded");
            FinalizeMethods(harmony, "TaleWorlds.MountAndBlade", "Mission", "CheckMissionEnd");
            FinalizeMethods(harmony, "TaleWorlds.MountAndBlade.View", "AgentVisuals", "AddSkinArmorWeaponMultiMeshesToEntity");

            #endregion TaleWorlds

            #region PocColor

            FinalizeMethods(harmony, "PocColor", "PocColorModAgentVisualsAddMeshes", "Postfix");

            #endregion PocColor

            #region Diplomacy

            bool IsFactionValid(IFaction faction)
            {
                if (!(faction as Kingdom is null) && faction.Leader is null)
                {
                    DestroyKingdomAction.Apply(faction as Kingdom);
                    return false;
                }
                if (!(faction as Clan is null) && faction.Leader is null)
                {
                    DestroyClanAction.Apply(faction as Clan);
                    return false;
                }
                return !(faction is null);
            }
            AllPurposePatchDelegate hasEnoughScoreCondition = delegate (object instance, ref object result, object[] parameters)
            {
                if (!IsFactionValid(parameters[0] as IFaction) || !IsFactionValid(parameters[1] as IFaction))
                {
                    result = false;
                    return false;
                }
                return true;
            };
            PrefixMethods(harmony, "Diplomacy.DiplomaticAction.Alliance.Conditions", "HasEnoughScoreCondition", "ApplyCondition", hasEnoughScoreCondition);
            PrefixMethods(harmony, "Diplomacy.DiplomaticAction.NonAggressionPact", "HasEnoughScoreCondition", "ApplyCondition", hasEnoughScoreCondition);
            PrefixMethods(harmony, "Diplomacy.ViewModelMixin", "KingdomTruceItemVmMixin", "UpdateActionAvailability", delegate (object instance, ref object result, object[] parameters)
            {
                Type type = instance.GetType();
                object viewModel = type.BaseType.GetCachedProperty("ViewModel").GetValue(instance);
                Type vmType = viewModel.GetType();
                IFaction faction1 = (IFaction)vmType.BaseType.GetCachedField("Faction1").GetValue(viewModel);
                IFaction faction2 = (IFaction)vmType.BaseType.GetCachedField("Faction2").GetValue(viewModel);
                if (!IsFactionValid(faction1) || !IsFactionValid(faction2)) return false;
                return true;
            });
            PrefixMethods(harmony, "Diplomacy.CampaignBehaviors", "DiplomaticAgreementBehavior", "ConsiderNonAggressionPact", delegate (object instance, ref object result, object[] parameters)
            {
                if (!IsFactionValid(parameters[0] as IFaction)) return false;
                return true;
            });
            FinalizeMethods(harmony, "Diplomacy.CivilWar.Actions", "StartRebellionAction", "Apply");

            #endregion Diplomacy

            #region SupplyLines

            FinalizeMethods(harmony, "SupplyLines", "CaravansCampaignBehaviorPatch", "OnMapEventEndedPrefix");

            #endregion SupplyLines

            #region AllegianceOverhaul

            FinalizeMethods(harmony, "AllegianceOverhaul.LoyaltyRebalance", "RelativesHelper", "BloodRelatives", delegate (object instance, ref object result, object[] parameters)
            {
                result = false;
                return true;
            });

            #endregion AllegianceOverhaul
        }
    }
}
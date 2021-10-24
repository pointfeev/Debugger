using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Core;
using TaleWorlds.Library;
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
                CharacterObject character = !(parameters[0] is null) ? ((TroopRosterElement)parameters[0]).Character : null;
                PartyBase owner = (PartyBase)parameters[1];
                bool preventError = character is null || owner is null;
                if (!preventError)
                {
                    int cost = 0;
                    for (int i = 0; i < character.UpgradeTargets.Length; i++) cost += character.GetUpgradeXpCost(owner, i);
                    if (cost == 0) preventError = true;
                }
                if (preventError)
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
            FinalizeMethods(harmony, "TaleWorlds.MountAndBlade.View", "BannerVisual", "ConvertToMultiMesh", delegate (object instance, ref object result, object[] parameters)
            {
                result = Banner.CreateOneColoredEmptyBanner(0).ConvertToMultiMesh();
                return true;
            });
            FinalizeMethods(harmony, "TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors", "RebellionsCampaignBehavior", "CreateRebelPartyAndClan");
            FinalizeMethods(harmony, "TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors", "CaravansCampaignBehavior", "OnMapEventEnded");
            FinalizeMethods(harmony, "TaleWorlds.MountAndBlade", "Mission", "CheckMissionEnd");
            FinalizeMethods(harmony, "TaleWorlds.MountAndBlade.View", "AgentVisuals", "AddSkinArmorWeaponMultiMeshesToEntity");
            MBReadOnlyList<Kingdom> checkedKingdomsList = null;
            PostfixMethods(harmony, "TaleWorlds.CampaignSystem", "CampaignObjectManager", "get_Kingdoms", delegate (object instance, ref object result, object[] parameters)
            {
                MBReadOnlyList<Kingdom> kingdomsList = (MBReadOnlyList<Kingdom>)result;
                if (checkedKingdomsList is null || checkedKingdomsList.Count != kingdomsList.Count || checkedKingdomsList.Last() != kingdomsList.Last())
                {
                    checkedKingdomsList = kingdomsList;
                    int i = 0;
                    foreach (Kingdom kingdom in checkedKingdomsList.ToList())
                    {
                        if (kingdom.Culture is null || kingdom.Leader is null)
                        {
                            if (!kingdom.IsEliminated) DestroyKingdomAction.Apply(kingdom);
                            CampaignObjectManager campaignObjectManager = Campaign.Current.CampaignObjectManager;
                            List<Kingdom> kingdoms = (List<Kingdom>)typeof(CampaignObjectManager).GetCachedField("_kingdoms").GetValue(campaignObjectManager);
                            kingdoms.Remove(kingdom);
                            typeof(CampaignObjectManager).GetCachedField("_kingdoms").SetValue(campaignObjectManager, kingdoms);
                            typeof(CampaignObjectManager).GetCachedProperty("Kingdoms").GetCachedSetMethod().Invoke(campaignObjectManager, new object[] {
                                kingdoms.GetReadOnlyList()
                            });
                            List<IFaction> factions = (List<IFaction>)typeof(CampaignObjectManager).GetCachedField("_factions").GetValue(campaignObjectManager);
                            factions.Remove(kingdom);
                            typeof(CampaignObjectManager).GetCachedField("_factions").SetValue(campaignObjectManager, factions);
                            typeof(CampaignObjectManager).GetCachedProperty("Factions").GetCachedSetMethod().Invoke(campaignObjectManager, new object[] {
                                factions.GetReadOnlyList()
                            });
                            Type campaignObjects = typeof(CampaignObjectManager).GetCachedNestedType("CampaignObjects");
                            typeof(CampaignObjectManager).GetCachedMethod("OnItemRemoved").MakeCachedGenericMethod(typeof(Kingdom)).Invoke(campaignObjectManager, new object[] {
                                Enum.ToObject(campaignObjects, campaignObjects.GetCachedField("Kingdoms").GetValue(campaignObjects)), kingdom
                            });
                            i++;
                        }
                    }
                    if (i > 0) OutputUtils.DoOutput($"Debugger just removed {i} invalid {(i == 1 ? "kingdom" : "kingdoms")} to prevent issues.");
                }
                return true;
            });

            #endregion TaleWorlds

            #region PocColor

            FinalizeMethods(harmony, "PocColor", "PocColorModAgentVisualsAddMeshes", "Postfix");

            #endregion PocColor

            #region Diplomacy

            PrefixMethods(harmony, "Diplomacy.CivilWar.Actions", "StartRebellionAction", "Apply", delegate (object instance, ref object result, object[] parameters)
            {
                Clan clan = (Clan)parameters[0]?.GetType()?.GetCachedProperty("SponsorClan")?.GetValue(parameters[0]);
                if (clan is null || clan.Kingdom is null || clan.Kingdom.RulingClan != clan) return false;
                return true;
            });

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
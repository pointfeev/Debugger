using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace Debugger
{
    public static class Console
    {
        [CommandLineFunctionality.CommandLineArgumentFunction("cure_main_hero_illness", "debugger")]
        public static string CureMainHeroIllness(List<string> _)
        {
            Campaign.Current.MainHeroIllDays = -1;
            return Hero.IsMainHeroIll ? "Failed\n" : "Success\n";
        }

        private static void ReloadEquipmentRosters()
        {
            try { Game.Current.ObjectManager.LoadXML("EquipmentRosters"); } catch { }
            try { Game.Current.ObjectManager.LoadXML("EquipmentRoster"); } catch { }
            try { Game.Current.ObjectManager.LoadXML("EquipmentSets"); } catch { }
            try { Game.Current.ObjectManager.LoadXML("EquipmentSet"); } catch { }
        }

        private static bool IsEquipmentInvalid(Equipment equipment)
        {
            return equipment is null || !equipment.IsValid ||
                equipment.GetEquipmentFromSlot(EquipmentIndex.Body).IsEmpty ||
                equipment.GetEquipmentFromSlot(EquipmentIndex.Body).IsVisualEmpty;
        }

        private static bool SetUniqueHeroEquipmentFromLists(Hero hero, List<Equipment> battleEquipmentSets, List<Equipment> civilianEquipmentSets, bool removeInvalid = false)
        {
        check_lists:
            if (battleEquipmentSets.Any() || civilianEquipmentSets.Any())
            {
                Equipment battleEquipment = battleEquipmentSets.Count > 0 ? battleEquipmentSets[hero.RandomValueDeterministic % battleEquipmentSets.Count] : null;
                Equipment civilianEquipment = civilianEquipmentSets.Count > 0 ? civilianEquipmentSets[hero.RandomValueDeterministic % civilianEquipmentSets.Count] : null;
                if (removeInvalid && (IsEquipmentInvalid(battleEquipment) || IsEquipmentInvalid(civilianEquipment)))
                {
                    if (!(battleEquipment is null) && IsEquipmentInvalid(battleEquipment)) battleEquipmentSets.Remove(battleEquipment);
                    if (!(civilianEquipment is null) && IsEquipmentInvalid(civilianEquipment)) civilianEquipmentSets.Remove(civilianEquipment);
                    goto check_lists;
                }
                bool set = false;
                if (!hero.BattleEquipment.IsEquipmentEqualTo(battleEquipment))
                {
                    typeof(Hero).GetProperty("BattleEquipment", BindingFlags.Public | BindingFlags.Instance).SetMethod.Invoke(hero, new object[] { battleEquipment.Clone() });
                    set = true;
                }
                if (!hero.CivilianEquipment.IsEquipmentEqualTo(civilianEquipment))
                {
                    typeof(Hero).GetProperty("CivilianEquipment", BindingFlags.Public | BindingFlags.Instance).SetMethod.Invoke(hero, new object[] { civilianEquipment.Clone() });
                    set = true;
                }
                if (set) return true;
            }
            return false;
        }

        [CommandLineFunctionality.CommandLineArgumentFunction("reset_all_heroes_equipment_to_default", "debugger")]
        public static string ResetAllHeroesEquipmentToDefault(List<string> _)
        {
            try
            {
                ReloadEquipmentRosters();
                List<Hero> heroes = Hero.AllAliveHeroes.ToList();
                heroes.AddRange(Hero.DeadOrDisabledHeroes.ToList());
                int numReset = 0;
                foreach (Hero hero in heroes)
                {
                    CharacterObject characterObject = hero.CharacterObject;
                    if (SetUniqueHeroEquipmentFromLists(hero,
                        characterObject.AllEquipments.Where(t => !t.IsEmpty() && !t.IsCivilian).ToList(),
                        characterObject.AllEquipments.Where(t => !t.IsEmpty() && t.IsCivilian).ToList()))
                    {
                        numReset++;
                    }
                }
                if (numReset > 0)
                {
                    return $"Reset {numReset} heroes' equipment sets to default";
                }
                else
                {
                    return "No heroes' equipment sets need resetting to default\n";
                }
            }
            catch (Exception e)
            {
                return "Encountered an exception: " + e.ToString() + "\n";
            }
        }

        [CommandLineFunctionality.CommandLineArgumentFunction("fix_all_naked_heroes", "debugger")]
        public static string FixAllNakedHeroes(List<string> _)
        {
            try
            {
                List<Hero> heroes = Hero.AllAliveHeroes.ToList();
                heroes.AddRange(Hero.DeadOrDisabledHeroes.ToList());
                List<MBEquipmentRoster> equipmentRosters = Game.Current.ObjectManager.GetObjectTypeList<MBEquipmentRoster>().ToList();
                foreach (Hero hero in heroes)
                {
                    equipmentRosters.Add((MBEquipmentRoster)typeof(BasicCharacterObject).GetField("_equipmentRoster", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(hero.CharacterObject));
                }
                int numEquipmentRemovedFromRosters = 0;
                int numRostersRemoved = 0;
                foreach (MBEquipmentRoster equipmentRoster in equipmentRosters)
                {
                    int numEquipmentRemovedFromRoster = 0;
                    List<Equipment> equipments = (List<Equipment>)typeof(MBEquipmentRoster).GetField("_equipments", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(equipmentRoster);
                    for (int i = 1; i < equipments.Count; i++)
                    {
                        Equipment equipment = equipments.ElementAtOrDefault(i);
                        if (IsEquipmentInvalid(equipment))
                        {
                            equipments.RemoveAt(i);
                            numEquipmentRemovedFromRoster++;
                        }
                    }
                    if (!equipments.Any())
                    {
                        Game.Current.ObjectManager.UnregisterObject(equipmentRoster);
                        numRostersRemoved++;
                    }
                    else
                    {
                        numEquipmentRemovedFromRosters += numEquipmentRemovedFromRoster;
                    }
                }
                int numFixed = 0;
                int numCantFix = 0;
                foreach (Hero hero in heroes)
                {
                    if (IsEquipmentInvalid(hero.BattleEquipment))
                    {
                        CharacterObject characterObject = hero.CharacterObject;
                        SetUniqueHeroEquipmentFromLists(hero,
                            characterObject.AllEquipments.Where(t => !t.IsEmpty() && !t.IsCivilian).ToList(),
                            characterObject.AllEquipments.Where(t => !t.IsEmpty() && t.IsCivilian).ToList(),
                            true);
                    }
                    else continue;
                    if (IsEquipmentInvalid(hero.BattleEquipment))
                    {
                        SetUniqueHeroEquipmentFromLists(hero,
                            CharacterObject.ChildTemplates.GetRandomElementWithPredicate(t => t.Culture == hero.Culture && t.IsFemale == hero.IsFemale).BattleEquipments.ToList(),
                            CharacterObject.ChildTemplates.GetRandomElementWithPredicate(t => t.Culture == hero.Culture && t.IsFemale == hero.IsFemale).CivilianEquipments.ToList(),
                            true);
                    }
                    else { numFixed++; continue; }
                    if (IsEquipmentInvalid(hero.BattleEquipment))
                    {
                        SetUniqueHeroEquipmentFromLists(hero,
                            CharacterObject.Templates.GetRandomElementWithPredicate(t => t.Culture == hero.Culture && t.IsFemale == hero.IsFemale).BattleEquipments.ToList(),
                            CharacterObject.Templates.GetRandomElementWithPredicate(t => t.Culture == hero.Culture && t.IsFemale == hero.IsFemale).CivilianEquipments.ToList(),
                            true);
                    }
                    else { numFixed++; continue; }
                    if (IsEquipmentInvalid(hero.BattleEquipment))
                    {
                        numCantFix++;
                    }
                    else { numFixed++; continue; }
                }
                if (numEquipmentRemovedFromRosters + numRostersRemoved + numFixed + numCantFix > 0)
                {
                    return (numEquipmentRemovedFromRosters > 0 ? "Removed " + numEquipmentRemovedFromRosters + " invalid equipment sets\n" : "") +
                        (numRostersRemoved > 0 ? "Unregistered " + numRostersRemoved + " invalid equipment rosters\n" : "") +
                        (numFixed > 0 ? "Fixed equipment of " + numFixed + " heroes\n" : "") +
                        (numCantFix > 0 ? "Failed to fix equipment for " + numCantFix + " heroes\n" : "");
                }
                else
                {
                    return "No heroes are missing body armor\n";
                }
            }
            catch (Exception e)
            {
                return "Encountered an exception: " + e.ToString() + "\n";
            }
        }
    }
}
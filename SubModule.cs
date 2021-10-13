using HarmonyLib;
using System;
using System.Reflection;
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
                FinalizeMethod(harmony, "PocColorModAgentVisualsAddMeshes", "Postfix", "PocColor");
                FinalizeMethod(harmony, "StartRebellionAction", "Apply", "Diplomacy");
                FinalizeMethod(harmony, "CaravansCampaignBehaviorPatch", "OnMapEventEndedPrefix", "SupplyLines");
                harmonyPatched = true;
                InformationManager.DisplayMessage(new InformationMessage("Debugger initialized", Colors.Red, "Debugger"));
            }
        }

        private static void FinalizeMethod(Harmony harmony, string typeName, string methodName, string modName)
        {
            MethodInfo method = AccessTools.Method(AccessTools.TypeByName(typeName), methodName);
            if (!(method is null))
            {
                harmony.Patch(method, finalizer: new HarmonyMethod(AccessTools.Method(typeof(SubModule), "Finalizer")));
                InformationManager.DisplayMessage(new InformationMessage($"Debugger finalized methods for {modName}", Colors.Red, "Debugger"));
            }
        }

        internal static Exception Finalizer(Exception __exception)
        {
            OutputUtils.DoOutputForException(__exception);
            return null;
        }
    }
}
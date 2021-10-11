using HarmonyLib;
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
                MethodInfo method = AccessTools.Method(AccessTools.TypeByName("PocColorModAgentVisualsAddMeshes"), "Postfix");
                if (!(method is null))
                {
                    harmony.Patch(method, finalizer: new HarmonyMethod(AccessTools.Method(typeof(PatchAgentVisuals), "AddSkinArmorWeaponMultiMeshesToEntityPOC")));
                    InformationManager.DisplayMessage(new InformationMessage("Debugger finalized methods from PocColor mod", Colors.Red, "Debugger"));
                }
                harmonyPatched = true;
                InformationManager.DisplayMessage(new InformationMessage("Debugger initialized", Colors.Red, "Debugger"));
            }
        }
    }
}
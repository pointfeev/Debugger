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
                harmonyPatched = true;
                Patcher.Patch(new Harmony("pointfeev.debugger"));
                Patcher.Output();
                InformationManager.DisplayMessage(new InformationMessage("Debugger initialized", Colors.Red, "Debugger"));
            }
        }
    }
}
using HarmonyLib;
using System;
using TaleWorlds.MountAndBlade.View;

namespace Debugger
{
    [HarmonyPatch(typeof(AgentVisuals))]
    public static class PatchAgentVisuals
    {
        [HarmonyFinalizer]
        [HarmonyPatch("AddSkinArmorWeaponMultiMeshesToEntity")]
        public static Exception AddSkinArmorWeaponMultiMeshesToEntity(Exception __exception)
        {
            return SubModule.Finalizer(__exception);
        }
    }
}
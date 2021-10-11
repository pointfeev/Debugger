using HarmonyLib;
using System;
using System.Reflection;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade.View;

namespace Debugger
{
    [HarmonyPatch(typeof(BannerVisual))]
    public static class PatchBannerVisual
    {
        [HarmonyPrefix]
        [HarmonyPatch("ConvertToMultiMesh")]
        public static bool ConvertToMultiMesh(BannerVisual __instance, ref MetaMesh __result)
        {
            if (!ReflectionUtils.IsMethodInCallStack(MethodBase.GetCurrentMethod()))
            {
                try
                {
                    __result = __instance.ConvertToMultiMesh();
                }
                catch (Exception e)
                {
                    __result = Banner.CreateOneColoredEmptyBanner(0).ConvertToMultiMesh();
                    OutputUtils.DoOutputForException(e);
                }
                return false;
            }
            return true;
        }
    }
}
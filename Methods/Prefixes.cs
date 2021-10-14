using System.Reflection;
using static Debugger.PatchUtils;

namespace Debugger
{
    internal static class Prefixes
    {
        private static bool DoPrefix(MethodBase originalMethod, object instance, ref object result, params object[] parameters)
        {
            if (MethodDelegate.TryGetValue(originalMethod, out PatchDelegate @delegate))
            {
                bool ret = @delegate.Invoke(instance, ref result, parameters);
                if (!ret) OutputUtils.DoOutputForMethod(originalMethod);
                return ret;
            }
            return true;
        }

        internal static bool Prefix(MethodBase __originalMethod, object __instance)
        {
            object __result = null;
            return DoPrefix(__originalMethod, __instance, ref __result);
        }

        internal static bool PrefixWith1Parameters(MethodBase __originalMethod, object __instance, object __0)
        {
            object __result = null;
            return DoPrefix(__originalMethod, __instance, ref __result, __0);
        }

        internal static bool PrefixWith2Parameters(MethodBase __originalMethod, object __instance, object __0, object __1)
        {
            object __result = null;
            return DoPrefix(__originalMethod, __instance, ref __result, __0, __1);
        }

        internal static bool PrefixWith3Parameters(MethodBase __originalMethod, object __instance, object __0, object __1, object __2)
        {
            object __result = null;
            return DoPrefix(__originalMethod, __instance, ref __result, __0, __1, __2);
        }

        internal static bool PrefixWith4Parameters(MethodBase __originalMethod, object __instance, object __0, object __1, object __2, object __3)
        {
            object __result = null;
            return DoPrefix(__originalMethod, __instance, ref __result, __0, __1, __2, __3);
        }

        internal static bool PrefixWith5Parameters(MethodBase __originalMethod, object __instance, object __0, object __1, object __2, object __3, object __4)
        {
            object __result = null;
            return DoPrefix(__originalMethod, __instance, ref __result, __0, __1, __2, __3, __4);
        }

        internal static bool PrefixWith6Parameters(MethodBase __originalMethod, object __instance, object __0, object __1, object __2, object __3, object __4, object __5)
        {
            object __result = null;
            return DoPrefix(__originalMethod, __instance, ref __result, __0, __1, __2, __3, __4, __5);
        }

        internal static bool PrefixWith7Parameters(MethodBase __originalMethod, object __instance, object __0, object __1, object __2, object __3, object __4, object __5, object __6)
        {
            object __result = null;
            return DoPrefix(__originalMethod, __instance, ref __result, __0, __1, __2, __3, __4, __5, __6);
        }

        internal static bool PrefixWith8Parameters(MethodBase __originalMethod, object __instance, object __0, object __1, object __2, object __3, object __4, object __5, object __6, object __7)
        {
            object __result = null;
            return DoPrefix(__originalMethod, __instance, ref __result, __0, __1, __2, __3, __4, __5, __6, __7);
        }

        internal static bool PrefixWith9Parameters(MethodBase __originalMethod, object __instance, object __0, object __1, object __2, object __3, object __4, object __5, object __6, object __7, object __8)
        {
            object __result = null;
            return DoPrefix(__originalMethod, __instance, ref __result, __0, __1, __2, __3, __4, __5, __6, __7, __8);
        }

        internal static bool PrefixWithReturn(MethodBase __originalMethod, object __instance, ref object __result)
        {
            return DoPrefix(__originalMethod, __instance, ref __result);
        }

        internal static bool PrefixWithReturnWith1Parameters(MethodBase __originalMethod, object __instance, ref object __result, object __0)
        {
            return DoPrefix(__originalMethod, __instance, ref __result, __0);
        }

        internal static bool PrefixWithReturnWith2Parameters(MethodBase __originalMethod, object __instance, ref object __result, object __0, object __1)
        {
            return DoPrefix(__originalMethod, __instance, ref __result, __0, __1);
        }

        internal static bool PrefixWithReturnWith3Parameters(MethodBase __originalMethod, object __instance, ref object __result, object __0, object __1, object __2)
        {
            return DoPrefix(__originalMethod, __instance, ref __result, __0, __1, __2);
        }

        internal static bool PrefixWithReturnWith4Parameters(MethodBase __originalMethod, object __instance, ref object __result, object __0, object __1, object __2, object __3)
        {
            return DoPrefix(__originalMethod, __instance, ref __result, __0, __1, __2, __3);
        }

        internal static bool PrefixWithReturnWith5Parameters(MethodBase __originalMethod, object __instance, ref object __result, object __0, object __1, object __2, object __3, object __4)
        {
            return DoPrefix(__originalMethod, __instance, ref __result, __0, __1, __2, __3, __4);
        }

        internal static bool PrefixWithReturnWith6Parameters(MethodBase __originalMethod, object __instance, ref object __result, object __0, object __1, object __2, object __3, object __4, object __5)
        {
            return DoPrefix(__originalMethod, __instance, ref __result, __0, __1, __2, __3, __4, __5);
        }

        internal static bool PrefixWithReturnWith7Parameters(MethodBase __originalMethod, object __instance, ref object __result, object __0, object __1, object __2, object __3, object __4, object __5, object __6)
        {
            return DoPrefix(__originalMethod, __instance, ref __result, __0, __1, __2, __3, __4, __5, __6);
        }

        internal static bool PrefixWithReturnWith8Parameters(MethodBase __originalMethod, object __instance, ref object __result, object __0, object __1, object __2, object __3, object __4, object __5, object __6, object __7)
        {
            return DoPrefix(__originalMethod, __instance, ref __result, __0, __1, __2, __3, __4, __5, __6, __7);
        }

        internal static bool PrefixWithReturnWith9Parameters(MethodBase __originalMethod, object __instance, ref object __result, object __0, object __1, object __2, object __3, object __4, object __5, object __6, object __7, object __8)
        {
            return DoPrefix(__originalMethod, __instance, ref __result, __0, __1, __2, __3, __4, __5, __6, __7, __8);
        }
    }
}
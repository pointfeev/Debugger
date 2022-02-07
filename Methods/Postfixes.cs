using System.Reflection;

using static Debugger.PatchUtils;

namespace Debugger
{
    internal static class Postfixes
    {
        private static void DoPostfix(MethodBase originalMethod, object instance, ref object result, params object[] parameters)
        {
            if (MethodDelegate.TryGetValue(originalMethod, out AllPurposePatchDelegate @delegate))
            {
                bool ret = @delegate.Invoke(instance, ref result, parameters);
                if (!ret)
                {
                    OutputUtils.DoOutputForMethod(originalMethod);
                }
            }
        }

        internal static void Postfix(MethodBase __originalMethod, object __instance)
        {
            object __result = null;
            DoPostfix(__originalMethod, __instance, ref __result);
        }

        internal static void PostfixWith1Parameters(MethodBase __originalMethod, object __instance, object __0)
        {
            object __result = null;
            DoPostfix(__originalMethod, __instance, ref __result, __0);
        }

        internal static void PostfixWith2Parameters(MethodBase __originalMethod, object __instance, object __0, object __1)
        {
            object __result = null;
            DoPostfix(__originalMethod, __instance, ref __result, __0, __1);
        }

        internal static void PostfixWith3Parameters(MethodBase __originalMethod, object __instance, object __0, object __1, object __2)
        {
            object __result = null;
            DoPostfix(__originalMethod, __instance, ref __result, __0, __1, __2);
        }

        internal static void PostfixWith4Parameters(MethodBase __originalMethod, object __instance, object __0, object __1, object __2, object __3)
        {
            object __result = null;
            DoPostfix(__originalMethod, __instance, ref __result, __0, __1, __2, __3);
        }

        internal static void PostfixWith5Parameters(MethodBase __originalMethod, object __instance, object __0, object __1, object __2, object __3, object __4)
        {
            object __result = null;
            DoPostfix(__originalMethod, __instance, ref __result, __0, __1, __2, __3, __4);
        }

        internal static void PostfixWith6Parameters(MethodBase __originalMethod, object __instance, object __0, object __1, object __2, object __3, object __4, object __5)
        {
            object __result = null;
            DoPostfix(__originalMethod, __instance, ref __result, __0, __1, __2, __3, __4, __5);
        }

        internal static void PostfixWith7Parameters(MethodBase __originalMethod, object __instance, object __0, object __1, object __2, object __3, object __4, object __5, object __6)
        {
            object __result = null;
            DoPostfix(__originalMethod, __instance, ref __result, __0, __1, __2, __3, __4, __5, __6);
        }

        internal static void PostfixWith8Parameters(MethodBase __originalMethod, object __instance, object __0, object __1, object __2, object __3, object __4, object __5, object __6, object __7)
        {
            object __result = null;
            DoPostfix(__originalMethod, __instance, ref __result, __0, __1, __2, __3, __4, __5, __6, __7);
        }

        internal static void PostfixWith9Parameters(MethodBase __originalMethod, object __instance, object __0, object __1, object __2, object __3, object __4, object __5, object __6, object __7, object __8)
        {
            object __result = null;
            DoPostfix(__originalMethod, __instance, ref __result, __0, __1, __2, __3, __4, __5, __6, __7, __8);
        }

        internal static void PostfixWithReturn(MethodBase __originalMethod, object __instance, ref object __result) => DoPostfix(__originalMethod, __instance, ref __result);

        internal static void PostfixWithReturnWith1Parameters(MethodBase __originalMethod, object __instance, ref object __result, object __0) => DoPostfix(__originalMethod, __instance, ref __result, __0);

        internal static void PostfixWithReturnWith2Parameters(MethodBase __originalMethod, object __instance, ref object __result, object __0, object __1) => DoPostfix(__originalMethod, __instance, ref __result, __0, __1);

        internal static void PostfixWithReturnWith3Parameters(MethodBase __originalMethod, object __instance, ref object __result, object __0, object __1, object __2) => DoPostfix(__originalMethod, __instance, ref __result, __0, __1, __2);

        internal static void PostfixWithReturnWith4Parameters(MethodBase __originalMethod, object __instance, ref object __result, object __0, object __1, object __2, object __3) => DoPostfix(__originalMethod, __instance, ref __result, __0, __1, __2, __3);

        internal static void PostfixWithReturnWith5Parameters(MethodBase __originalMethod, object __instance, ref object __result, object __0, object __1, object __2, object __3, object __4) => DoPostfix(__originalMethod, __instance, ref __result, __0, __1, __2, __3, __4);

        internal static void PostfixWithReturnWith6Parameters(MethodBase __originalMethod, object __instance, ref object __result, object __0, object __1, object __2, object __3, object __4, object __5) => DoPostfix(__originalMethod, __instance, ref __result, __0, __1, __2, __3, __4, __5);

        internal static void PostfixWithReturnWith7Parameters(MethodBase __originalMethod, object __instance, ref object __result, object __0, object __1, object __2, object __3, object __4, object __5, object __6) => DoPostfix(__originalMethod, __instance, ref __result, __0, __1, __2, __3, __4, __5, __6);

        internal static void PostfixWithReturnWith8Parameters(MethodBase __originalMethod, object __instance, ref object __result, object __0, object __1, object __2, object __3, object __4, object __5, object __6, object __7) => DoPostfix(__originalMethod, __instance, ref __result, __0, __1, __2, __3, __4, __5, __6, __7);

        internal static void PostfixWithReturnWith9Parameters(MethodBase __originalMethod, object __instance, ref object __result, object __0, object __1, object __2, object __3, object __4, object __5, object __6, object __7, object __8) => DoPostfix(__originalMethod, __instance, ref __result, __0, __1, __2, __3, __4, __5, __6, __7, __8);
    }
}
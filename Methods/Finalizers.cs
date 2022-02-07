using System;
using System.Reflection;

using static Debugger.PatchUtils;

namespace Debugger
{
    internal static class Finalizers
    {
        private static Exception DoFinalizer(Exception exception, MethodBase originalMethod, object instance, ref object result, params object[] parameters)
        {
            if (!(exception is null))
            {
                if (MethodDelegate.TryGetValue(originalMethod, out AllPurposePatchDelegate @delegate))
                {
                    @delegate.Invoke(instance, ref result, parameters);
                }
                OutputUtils.DoOutputForMethod(originalMethod);
            }
            return null;
        }

        internal static Exception Finalizer(Exception __exception, MethodBase __originalMethod, object __instance)
        {
            object __result = null;
            return DoFinalizer(__exception, __originalMethod, __instance, ref __result);
        }

        internal static Exception FinalizerWith1Parameters(Exception __exception, MethodBase __originalMethod, object __instance, object __0)
        {
            object __result = null;
            return DoFinalizer(__exception, __originalMethod, __instance, ref __result, __0);
        }

        internal static Exception FinalizerWith2Parameters(Exception __exception, MethodBase __originalMethod, object __instance, object __0, object __1)
        {
            object __result = null;
            return DoFinalizer(__exception, __originalMethod, __instance, ref __result, __0, __1);
        }

        internal static Exception FinalizerWith3Parameters(Exception __exception, MethodBase __originalMethod, object __instance, object __0, object __1, object __2)
        {
            object __result = null;
            return DoFinalizer(__exception, __originalMethod, __instance, ref __result, __0, __1, __2);
        }

        internal static Exception FinalizerWith4Parameters(Exception __exception, MethodBase __originalMethod, object __instance, object __0, object __1, object __2, object __3)
        {
            object __result = null;
            return DoFinalizer(__exception, __originalMethod, __instance, ref __result, __0, __1, __2, __3);
        }

        internal static Exception FinalizerWith5Parameters(Exception __exception, MethodBase __originalMethod, object __instance, object __0, object __1, object __2, object __3, object __4)
        {
            object __result = null;
            return DoFinalizer(__exception, __originalMethod, __instance, ref __result, __0, __1, __2, __3, __4);
        }

        internal static Exception FinalizerWith6Parameters(Exception __exception, MethodBase __originalMethod, object __instance, object __0, object __1, object __2, object __3, object __4, object __5)
        {
            object __result = null;
            return DoFinalizer(__exception, __originalMethod, __instance, ref __result, __0, __1, __2, __3, __4, __5);
        }

        internal static Exception FinalizerWith7Parameters(Exception __exception, MethodBase __originalMethod, object __instance, object __0, object __1, object __2, object __3, object __4, object __5, object __6)
        {
            object __result = null;
            return DoFinalizer(__exception, __originalMethod, __instance, ref __result, __0, __1, __2, __3, __4, __5, __6);
        }

        internal static Exception FinalizerWith8Parameters(Exception __exception, MethodBase __originalMethod, object __instance, object __0, object __1, object __2, object __3, object __4, object __5, object __6, object __7)
        {
            object __result = null;
            return DoFinalizer(__exception, __originalMethod, __instance, ref __result, __0, __1, __2, __3, __4, __5, __6, __7);
        }

        internal static Exception FinalizerWith9Parameters(Exception __exception, MethodBase __originalMethod, object __instance, object __0, object __1, object __2, object __3, object __4, object __5, object __6, object __7, object __8)
        {
            object __result = null;
            return DoFinalizer(__exception, __originalMethod, __instance, ref __result, __0, __1, __2, __3, __4, __5, __6, __7, __8);
        }

        internal static Exception FinalizerWithReturn(Exception __exception, MethodBase __originalMethod, object __instance, ref object __result) => DoFinalizer(__exception, __originalMethod, __instance, ref __result);

        internal static Exception FinalizerWithReturnWith1Parameters(Exception __exception, MethodBase __originalMethod, object __instance, ref object __result, object __0) => DoFinalizer(__exception, __originalMethod, __instance, ref __result, __0);

        internal static Exception FinalizerWithReturnWith2Parameters(Exception __exception, MethodBase __originalMethod, object __instance, ref object __result, object __0, object __1) => DoFinalizer(__exception, __originalMethod, __instance, ref __result, __0, __1);

        internal static Exception FinalizerWithReturnWith3Parameters(Exception __exception, MethodBase __originalMethod, object __instance, ref object __result, object __0, object __1, object __2) => DoFinalizer(__exception, __originalMethod, __instance, ref __result, __0, __1, __2);

        internal static Exception FinalizerWithReturnWith4Parameters(Exception __exception, MethodBase __originalMethod, object __instance, ref object __result, object __0, object __1, object __2, object __3) => DoFinalizer(__exception, __originalMethod, __instance, ref __result, __0, __1, __2, __3);

        internal static Exception FinalizerWithReturnWith5Parameters(Exception __exception, MethodBase __originalMethod, object __instance, ref object __result, object __0, object __1, object __2, object __3, object __4) => DoFinalizer(__exception, __originalMethod, __instance, ref __result, __0, __1, __2, __3, __4);

        internal static Exception FinalizerWithReturnWith6Parameters(Exception __exception, MethodBase __originalMethod, object __instance, ref object __result, object __0, object __1, object __2, object __3, object __4, object __5) => DoFinalizer(__exception, __originalMethod, __instance, ref __result, __0, __1, __2, __3, __4, __5);

        internal static Exception FinalizerWithReturnWith7Parameters(Exception __exception, MethodBase __originalMethod, object __instance, ref object __result, object __0, object __1, object __2, object __3, object __4, object __5, object __6) => DoFinalizer(__exception, __originalMethod, __instance, ref __result, __0, __1, __2, __3, __4, __5, __6);

        internal static Exception FinalizerWithReturnWith8Parameters(Exception __exception, MethodBase __originalMethod, object __instance, ref object __result, object __0, object __1, object __2, object __3, object __4, object __5, object __6, object __7) => DoFinalizer(__exception, __originalMethod, __instance, ref __result, __0, __1, __2, __3, __4, __5, __6, __7);

        internal static Exception FinalizerWithReturnWith9Parameters(Exception __exception, MethodBase __originalMethod, object __instance, ref object __result, object __0, object __1, object __2, object __3, object __4, object __5, object __6, object __7, object __8) => DoFinalizer(__exception, __originalMethod, __instance, ref __result, __0, __1, __2, __3, __4, __5, __6, __7, __8);
    }
}
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace Debugger
{
    internal static class FinalizeUtils
    {
        private static Dictionary<string, int> modMethodsFinalized = new Dictionary<string, int>();

        internal static void FinalizeMethods(Harmony harmony, string typeName, string methodName, string modName, bool typeNameExplicit = true, bool methodNameExplicit = true)
        {
            List<MethodInfo> methods = new List<MethodInfo>();
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies().Reverse())
            {
                if (assembly == typeof(SubModule).Assembly) continue;
                foreach (Type type in assembly.GetTypes().Reverse())
                {
                    if (typeNameExplicit ? type.Name == typeName : type.Name.Contains(typeName))
                    {
                        void AddMethod(MethodInfo method)
                        {
                            if (!(method is null))
                            {
                                if (method.GetGenericArguments().Any())
                                {
                                    foreach (Type genericMethodArg in method.GetGenericArguments().Reverse())
                                    {
                                        methods.Add(method.MakeGenericMethod(genericMethodArg));
                                    }
                                }
                                else
                                {
                                    methods.Add(method);
                                }
                            }
                        }
                        if (type.GetGenericArguments().Any())
                        {
                            foreach (Type genericTypeArg in type.GetGenericArguments())
                            {
                                foreach (MethodInfo method in type.MakeGenericType(genericTypeArg).GetMethods((BindingFlags)(-1)).Reverse())
                                {
                                    if (methodNameExplicit ? method.Name == methodName : method.Name.Contains(methodName))
                                    {
                                        AddMethod(method);
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (MethodInfo method in type.GetMethods((BindingFlags)(-1)).Reverse())
                            {
                                if (methodNameExplicit ? method.Name == methodName : method.Name.Contains(methodName))
                                {
                                    AddMethod(method);
                                }
                            }
                        }
                    }
                }
            }
            if (methods.Any())
            {
                int count = 0;
                foreach (MethodInfo method in methods)
                {
                    try
                    {
                        harmony.Patch(method, finalizer: new HarmonyMethod(AccessTools.Method(typeof(FinalizeUtils), "Finalizer")));
                        count++;
                        //InformationManager.DisplayMessage(new InformationMessage(method.ReflectedType.Name + "." + method.Name, Colors.Red, "Debugger"));
                    }
                    catch
                    {
                        //InformationManager.DisplayMessage(new InformationMessage(method.ReflectedType.Name + "." + method.Name, Colors.Red, "Debugger"));
                    }
                }
                if (modMethodsFinalized.TryGetValue(modName, out int i))
                {
                    modMethodsFinalized[modName] = i + count;
                }
                else
                {
                    modMethodsFinalized[modName] = count;
                }
            }
        }

        internal static void DoFinalizedMethodsOutput()
        {
            foreach (KeyValuePair<string, int> pair in modMethodsFinalized)
            {
                if (pair.Value <= 0) continue;
                InformationManager.DisplayMessage(new InformationMessage($"Debugger finalized {pair.Value} " + (pair.Value == 1 ? "method" : "methods") + $" for {pair.Key}", Colors.Red, "Debugger"));
            }
        }

        internal static Exception Finalizer(Exception __exception)
        {
            OutputUtils.DoOutputForException(__exception);
            return null;
        }
    }
}
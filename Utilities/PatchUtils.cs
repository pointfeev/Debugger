using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Debugger
{
    internal static class PatchUtils
    {
        internal delegate bool PatchDelegate(object instance, ref object result, params object[] parameters);
        internal static Dictionary<MethodBase, PatchDelegate> MethodDelegate = new Dictionary<MethodBase, PatchDelegate>();
        internal static Dictionary<string, int> MethodsPatched = new Dictionary<string, int>();

        private static void PatchMethods(string patchType, Harmony harmony, string nameSpace, string typeName, string methodName, PatchDelegate patchDelegate, bool modNamespaceOnly, bool modNamespaceExplicit, bool typeNameExplicit, bool methodNameExplicit)
        {
            List<MethodInfo> methods = GetMethods(nameSpace, typeName, methodName, modNamespaceOnly, modNamespaceExplicit, typeNameExplicit, methodNameExplicit);
            if (methods.Any())
            {
                foreach (MethodInfo method in methods)
                {
                    string methodToUse = patchType;
                    if (method.ReturnType != typeof(void)) methodToUse += "WithReturn";
                    if (!(patchDelegate is null)) MethodDelegate[method] = patchDelegate;
                    int parameters = method.GetParameters().Count();
                    if (parameters > 0) methodToUse += $"With{parameters}Parameters";
                    if (patchType == "Prefix") harmony.Patch(method, prefix: new HarmonyMethod(AccessTools.Method(typeof(Prefixes), methodToUse)));
                    else if (patchType == "Finalizer") harmony.Patch(method, finalizer: new HarmonyMethod(AccessTools.Method(typeof(Finalizers), methodToUse)));
                }
                string rootNameSpace = nameSpace;
                int nameSpaceSub = rootNameSpace.IndexOf('.');
                if (nameSpaceSub != -1) rootNameSpace = rootNameSpace.Substring(0, nameSpaceSub);
                //if (rootNameSpace == "SandBox") rootNameSpace = "TaleWorlds";
                if (MethodsPatched.TryGetValue(rootNameSpace, out int i)) MethodsPatched[rootNameSpace] = i + methods.Count;
                else MethodsPatched[rootNameSpace] = methods.Count;
            }
        }

        internal static void PrefixMethods(Harmony harmony, string nameSpace, string typeName, string methodName, PatchDelegate prefix, bool modNamespaceOnly = true, bool modNamespaceExplicit = true, bool typeNameExplicit = true, bool methodNameExplicit = true)
        {
            PatchMethods("Prefix",
                harmony: harmony,
                nameSpace: nameSpace,
                typeName: typeName,
                methodName: methodName,
                patchDelegate: prefix,
                modNamespaceOnly: modNamespaceOnly,
                modNamespaceExplicit: modNamespaceExplicit,
                typeNameExplicit: typeNameExplicit,
                methodNameExplicit: methodNameExplicit);
        }

        internal static void FinalizeMethods(Harmony harmony, string nameSpace, string typeName, string methodName, PatchDelegate fallback = null, bool modNamespaceOnly = true, bool modNamespaceExplicit = true, bool typeNameExplicit = true, bool methodNameExplicit = true)
        {
            PatchMethods("Finalizer",
                harmony: harmony,
                nameSpace: nameSpace,
                typeName: typeName,
                methodName: methodName,
                patchDelegate: fallback,
                modNamespaceOnly: modNamespaceOnly,
                modNamespaceExplicit: modNamespaceExplicit,
                typeNameExplicit: typeNameExplicit,
                methodNameExplicit: methodNameExplicit);
        }

        private static List<MethodInfo> GetMethods(string nameSpace, string typeName, string methodName, bool modNamespaceOnly = true, bool modNamespaceExplicit = true, bool typeNameExplicit = true, bool methodNameExplicit = true)
        {
            List<MethodInfo> methods = new List<MethodInfo>();
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies().Reverse())
            {
                if (assembly == typeof(SubModule).Assembly) continue;
                foreach (Type type in assembly.GetTypes().Reverse())
                {
                    bool canUseNamespace = (modNamespaceOnly ? (modNamespaceExplicit ? type.Namespace == nameSpace : type.Namespace?.Contains(nameSpace)) : true).GetValueOrDefault(false);
                    bool canUseType = (typeNameExplicit ? type.Name == typeName : type.Name?.Contains(typeName)).GetValueOrDefault(false);
                    if (canUseNamespace && canUseType)
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
            return methods;
        }
    }
}
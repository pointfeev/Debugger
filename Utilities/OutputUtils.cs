using System.Collections.Generic;
using System.Reflection;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace Debugger
{
    internal static class OutputUtils
    {
        private static readonly List<string> outputs = new List<string>();

        internal static void DoOutput(string output, bool allowRepeat = false)
        {
            if (allowRepeat || !outputs.Contains(output))
            {
                if (!outputs.Contains(output))
                {
                    outputs.Add(output);
                }
                InformationManager.DisplayMessage(new InformationMessage(output, Colors.Red, "Debugger"));
            }
        }

        internal static void DoOutputForMethod(MethodBase method, bool allowRepeat = false)
        {
            DoOutput("Debugger just prevented issues at " + method.ReflectedType.FullName + "." + method.Name, allowRepeat);
        }
    }
}
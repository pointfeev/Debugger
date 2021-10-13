using System;
using System.Collections.Generic;
using System.Reflection;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace Debugger
{
    public static class OutputUtils
    {
        private static readonly List<string> outputs = new List<string>();

        public static void DoOutput(string output, bool allowRepeat = false)
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

        public static void DoOutputForException(Exception e, bool allowRepeat = false)
        {
            string[] stackTrace = e.StackTrace.Split('\n');
            string location = string.Empty;
            for (int i = 0; i <= 3; i++)
            {
                string line = stackTrace.ElementAtOrValue(i, null);
                if (!(line is null) && !line.Contains("Debugger") && !line.Contains("ThrowHelper"))
                {
                    location = line.Substring(line.IndexOf("at"));
                    break;
                }
            }
            string output = "Debugger prevented crashes " + location;// + message;
            DoOutput(output, allowRepeat);
        }

        public static void DoOutputForReversePatchFailure(MethodBase currentMethod)
        {
            DoOutput(currentMethod.DeclaringType + "." + currentMethod.Name + " was a stub; reverse patch failed!");
        }
    }
}
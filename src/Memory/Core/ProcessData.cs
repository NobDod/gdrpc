using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace GDRPC.Memory.Core
{
    class ProcessData
    {
        public static Process getProcess(string processName)
        {
            Process[] process = Process.GetProcessesByName(processName);
            if (process.Count() < 1) //if process is null
                return null;
            return process[0];
        }

        //get process
        public static Process getProcess(int pid)
        {
            try { return Process.GetProcessById(pid); }
            catch { return null; }
        }

        //get module address
        public static IntPtr GetModuleAddress(Process process, string moduleFullName)
        {
            foreach (ProcessModule module in process.Modules)
            {
                if (moduleFullName == module.ModuleName)
                    return module.BaseAddress;
            }
            return (IntPtr)(-1);
        }

        //get module
        public static ProcessModule GetModule(Process process, string moduleFullName)
        {
            foreach (ProcessModule module in process.Modules)
            {
                if (moduleFullName == module.ModuleName)
                    return module;
            }
            return null;
        }
    }
}

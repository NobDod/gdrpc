using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GDRPC.Memory.Core
{
    class ImportLibrary
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] buffer, int size, ref int lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, byte[] buffer, int size, out int lpNumberOfBytesWritten);
    }
}

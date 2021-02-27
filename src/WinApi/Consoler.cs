using System;
using System.Runtime.InteropServices;

namespace GDRPC.WinApi
{
    class Consoler
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();

        private static int isCached = -1;
        public static bool IsConsole()
        {
            if (isCached != -1)
                return (isCached == 1);
            if (GetConsoleWindow() != IntPtr.Zero)
                try { isCached = Console.WindowHeight > 0 ? 1 : 0; return true; } catch { }
            isCached = 0;
            return false;
        }
    }
}

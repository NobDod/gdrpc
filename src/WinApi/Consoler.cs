using System;
using System.IO;
using System.Runtime.InteropServices;

namespace GDRPC.WinApi
{
    class Consoler
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("kernel32.dll")]
        private static extern void AllocConsole();

        [DllImport("kernel32.dll")]
        private static extern void FreeConsole();

        private static int isCached = -1;

        private static bool isAppConsole = false;
        public static bool IsConsoleOpenedFromApp { get => isAppConsole; }

        public static bool IsConsole()
        {
            if (isCached != -1)
                return (isCached == 1);
            if (GetConsoleWindow() != IntPtr.Zero)
                try { isCached = Console.WindowHeight > 0 ? 1 : 0; return true; } catch { }
            isCached = 0;
            return false;
        }

        /// <summary>
        /// Открыть консоль (даже если вы не создавали)
        /// </summary>
        /// <param name="cacheChange">Изменить кэш о открытом консоли на открытый.</param>
        /// <param name="changeSetConsole">Изменить вывод консоля</param>
        public static void CreateConsole(bool changeSetConsole = false, bool cacheChange = false)
        {
            if (cacheChange)
                isCached = 1;
            AllocConsole();
            if (changeSetConsole)
                OutConsoleChange.OverrideRedirection();
        }

        /// <summary>
        /// Закрыть консоль (даже если вы не создавали)
        /// </summary>
        /// <param name="cacheChange">Изменить кэш о открытом консоли на не на открытый.</param>
        public static void CloseConsole(bool cacheChange = false)
        {
            if (cacheChange)
                isCached = 0;
            FreeConsole();
        }

        class OutConsoleChange
        {
            private const UInt32 StdOutputHandle = 0xFFFFFFF5;

            [DllImport("kernel32.dll", SetLastError = true)]
            private static extern IntPtr GetStdHandle(int nStdHandle);

            [DllImport("kernel32.dll", SetLastError = true)]
            private static extern bool SetStdHandle(int nStdHandle, IntPtr hHandle);

            private const int STD_OUTPUT_HANDLE = -11;
            private const int STD_INPUT_HANDLE = -10;
            private const int STD_ERROR_HANDLE = -12;

            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern IntPtr CreateFile([MarshalAs(UnmanagedType.LPTStr)] string filename, [MarshalAs(UnmanagedType.U4)] uint access, [MarshalAs(UnmanagedType.U4)] FileShare share, IntPtr securityAttributes, [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition, [MarshalAs(UnmanagedType.U4)] FileAttributes flagsAndAttributes, IntPtr templateFile);

            private const uint GENERIC_WRITE = 0x40000000;
            private const uint GENERIC_READ = 0x80000000;
            public static void OverrideRedirection()
            {
                var hOut = GetStdHandle(STD_OUTPUT_HANDLE);
                var hRealOut = CreateFile("CONOUT$", GENERIC_READ | GENERIC_WRITE, FileShare.Write, IntPtr.Zero, FileMode.OpenOrCreate, 0, IntPtr.Zero);
                if (hRealOut != hOut)
                {
                    SetStdHandle(STD_OUTPUT_HANDLE, hRealOut);
                    System.Console.SetOut(new StreamWriter(System.Console.OpenStandardOutput(), System.Console.OutputEncoding) { AutoFlush = true });
                }
            }
        }

    }
}

class Log
{
    public static void Write(string text) => Console.Write(text);
    public static void Write(string text, params object[] args) => Console.Write(text, args);

    public static void WriteLine(string text) => Console.WriteLine(text);
    public static void WriteLine(string text, params object[] args) => Console.WriteLine(text, args);
}
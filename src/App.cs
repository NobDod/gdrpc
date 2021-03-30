using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace GDRPC
{
    public partial class AppRunner
    {
        private static DateTime startApp;
        public static DateTime StartApp { get => startApp; }

        //old programm directory
        //public static string oldPD;

        /// <summary>
        /// Run wihout console
        /// </summary>
        public static void Run() => Main(null);

        /// <summary>
        /// Stop wihout console
        /// </summary>
        public static void Stop() {
            App.App.Stop();
        }

        /// <summary>
        /// Run with console
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            startApp = DateTime.UtcNow;
            //find my process folder :)
            //oldPD = Environment.CurrentDirectory;
            //if dll in other folder
            Environment.CurrentDirectory = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

            //checkingn content
            Discord.Discord.CheckFileDiscord();
#if DEBUG
            if (!WinApi.Consoler.IsConsole())
                WinApi.Consoler.CreateConsole(true, true);
            Console.Title = "GDRPC";
            Console.WriteLine("Geometry Dash Rich Presence");
            Console.WriteLine("Program directory: {0}", Environment.CurrentDirectory);
            Log.WriteLine("[AppRunner]: " + startApp.ToString());
            App.App.Run().Wait();
            Log.WriteLine("[AppRunner]: App has stoped. Restarting (2sec)");
            Task.Delay(2000).Wait();
            Main(args);
#else
            App.App.Run().Wait();
            Log.WriteLine("Goodbye :(");
#endif
        }
    }
}

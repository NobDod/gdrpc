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
            try { App.App.Stop(); } catch { }
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

#if DEBUG
            if (!WinApi.Consoler.IsConsole())
                WinApi.Consoler.CreateConsole(true, true);
            Console.Title = "GDRPC";
            Console.WriteLine("Geometry Dash Rich Presence");
            Console.WriteLine("Program directory: {0}", Environment.CurrentDirectory);
            Log.WriteLine("[AppRunner]: " + startApp.ToString());
#endif
            try
            {
                //checking content
                if (Discord.Discord.CheckFileDiscord())
                    App.App.Run().Wait();
            }
            catch (Exception ex)
            {
                Log.WriteLine("[Exception]: " + ex.Message);
#if DEBUG
                Debug.WriteLine(ex.StackTrace);
#endif
                Stop();
                Log.WriteLine("Delaying 3 seconds");
                Task.Delay(3000).Wait();
            }
#if DEBUG
            Log.WriteLine("[AppRunner]: App has stoped. Restarting (2sec)");
            Task.Delay(2000).Wait();
            Main(args);
#endif
        }
    }
}

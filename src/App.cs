using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace GDRPC
{
    public class AppRunner
    {
        private static DateTime startApp;
        public static DateTime StartApp { get => startApp; }

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
#if DEBUG
            if (!WinApi.Consoler.IsConsole())
                WinApi.Consoler.CreateConsole(true, true);
            Console.Title = "GDRPC";
            Console.WriteLine("Geometry Dash Rich Presence");
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

        /// <summary>
        /// Create message box
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="icon"></param>
        /// <param name="button"></param>
        public static void MessageBox(string title, string text, long icon, long button)
        {
            int handle = (int)Process.GetCurrentProcess().MainWindowHandle;
            if (handle == 0 && !WinApi.Consoler.IsConsole())
            {
                WinApi.Consoler.CreateConsole(false, false);
                handle = (int)WinApi.Consoler.GetConsoleWindow();
            }
            WinApi.MessageBox.Show(handle, text, title, (uint)(icon | button));
            if (handle == (int)WinApi.Consoler.GetConsoleWindow() && !WinApi.Consoler.IsConsole())
                WinApi.Consoler.CloseConsole();
        }
        public class MessageBoxFast
        {
            public static void Error(string text, bool isExit=false, bool tryAgain=false)
            {
                MessageBox("GDRPC", text, MB.Icon.Error, MB.Buttons.Ok);
                if(isExit)
                    Environment.Exit(5);
                else if (tryAgain)
                {
                    Stop();
                    Run();
                }
                return;
            }
        }
    }

    /// <summary>
    /// Этот класс необязательный, но отлично сойдет кому требуется AppRunner с интерфейсом. 
    /// </summary>
    class AppRunnerInterfaces
    {
        //analog
        [InterfaceType(ComInterfaceType.InterfaceIsDual)]
        public interface IAppRunnerInterface
        {
            /// <summary>
            /// Run without console
            /// </summary>
            [DispId(1)]
            void Run();

            /// <summary>
            /// Stop without console
            /// </summary>
            [DispId(2)]
            void Stop();
        };
        public class AppRunnerExport : IAppRunnerInterface
        {
            /// <summary>
            /// Run without console
            /// </summary>
            public void Run() => AppRunner.Run();
            /// <summary>
            /// Stop wihout console
            /// </summary>
            public void Stop() => AppRunner.Stop();
        }
    }
}

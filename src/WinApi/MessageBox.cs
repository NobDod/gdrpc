using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GDRPC.WinApi
{
    class MessageBoxImport
    {
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto, EntryPoint = "MessageBox")]
        public static extern int Show(int hWnd, String text, String caption, uint type);
    }
   
}
namespace GDRPC
{
    public partial class AppRunner
    {
        /// <summary>
        /// Create message box
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="icon"></param>
        /// <param name="button"></param>
        public static void MessageBox(string title, string text, long icon, long button)
        {
            //don`t stopping game process.
            int handle = 0;

            if (!WinApi.Consoler.IsConsole())
            {
                WinApi.Consoler.CreateConsole(false, false);
                handle = (int)WinApi.Consoler.GetConsoleWindow();
            }
            WinApi.MessageBoxImport.Show(handle, text, title, (uint)(icon | button));
            if (handle == (int)WinApi.Consoler.GetConsoleWindow() && !WinApi.Consoler.IsConsole())
                WinApi.Consoler.CloseConsole();
        }

        /// <summary>
        /// Fatal error GDRPC. Don`t use this class.
        /// </summary>
        public class MessageBoxFast
        {
            public static void Error(string text)
            {
                //go use restart :)
                Stop();
                MessageBox("GDRPC", text, MB.Icon.Error, MB.Buttons.Ok);
                Run();
                return;
            }
        }
    }
}
namespace GDRPC.MB
{
    //not full
    //add: https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-messagebox
    class Icon
    {
        public const long Warning = 0x00000030L, Error = 0x00000010L, Info = 0x00000040L;
    }
    class Buttons
    {
        public const long Ok = 0x00000000L;
    }
}

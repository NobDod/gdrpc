using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GDRPC.WinApi
{
    class MessageBox
    {
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto, EntryPoint = "MessageBox")]
        public static extern int Show(int hWnd, String text, String caption, uint type);
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

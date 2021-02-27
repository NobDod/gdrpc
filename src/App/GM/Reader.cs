using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDRPC.App.GM
{
    class Reader
    {
        public static T Read<T>(int mainAddres, int[] offests) where T : struct
        {
            return App.GameManager.Read<T>(IntPtr.Add(App.GameManager.Read<IntPtr>(offests), mainAddres).ToInt64());
        }
        public static int SceneID => Read<int>(0x1DC, new[] { 0x3222D0 });
    }
}

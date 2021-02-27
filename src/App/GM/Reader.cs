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
        public static T Read<T>(int[] offests) where T : struct
        {
            return App.GameManager.Read<T>(App.GameManager.Read<IntPtr>(offests).ToInt64());
        }

        /// <summary>
        /// ID сцен. 
        /// </summary>
        public static int SceneID => Read<int>(0x1DC, new[] { 0x3222D0 });
        
        public class Level
        {
            /// <summary>
            /// Открыт ли уровень?
            /// </summary>
            public static bool IsOpened => Read<bool>(new[] { 0x003222D0, 0x164, 0x22C, 0x114 });
        }

        public class Editor
        {
            /// <summary>
            /// Открыт ли редактор?
            /// </summary>
            public static bool IsOpened => Read<bool>(new[] { 0x003222D0, 0x168 });

            /// <summary>
            /// Количество блоков
            /// </summary>
            public static int BlockCount => Read<int>(0x3A0, new[] { 0x003222D0, 0x168 });
        }
    }
}

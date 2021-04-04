using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDRPC.App.GM
{
    class Reader
    {
        public static Memory.MemoryReader gm { get => App.GameManager; }
        /// <summary>
        /// ID scene. 
        /// </summary>
        public static int SceneID => gm.Read<int>(0x3222D0, 0x1DC);
        
        public class Level
        {
            public class Utils
            {
                //господи, XPOS = , 30 минут фиксил баг класс
                public static float XPOS => gm.Read<float>(0x3222D0, 0x164, 0x224, 0x34);
                public static float LenLevel => gm.Read<float>(0x3222D0, 0x164, 0x3B4);
                public static int LevelID => gm.Read<int>(0x3222D0, 0x2A0);
            }

            /// <summary>
            /// is opened level?
            /// </summary>
            public static bool IsOpened => gm.Read<bool>(0x3222D0, 0x164, 0x22C, 0x114);

            /// <summary>
            /// is activated practice mode? (not working)
            /// </summary>
            public static bool IsPracticeMode => gm.Read<bool>(0x3222D0, 0x164, 0x495);

            /// <summary>
            /// level ID
            /// </summary>
            public static int LevelID => gm.Read<int>(0x3222D0, 0x164, 0x22C, 0x114, 0xF8);

            /// <summary>
            /// stars in level
            /// </summary>
            public static int Stars => gm.Read<int>(0x3222D0, 0x164, 0x22C, 0x114, 0x2AC);

            /// <summary>
            /// attempts
            /// </summary>
            public static int Attempts => gm.Read<int>(0x3222D0, 0x164, 0x4A8);

            /// <summary>
            /// best percent
            /// </summary>
            public static int BestPercent => gm.Read<int>(0x3222D0, 0x164, 0x22C, 0x114, 0x248);

            /// <summary>
            /// best practice percent
            /// </summary>
            public static int BestPracticePercent => gm.Read<int>(0x3222D0, 0x164, 0x22C, 0x114, 0x26C);

            /// <summary>
            /// percent in level
            /// </summary>
            public static int Percent
            {
                get 
                {
                    try
                    {
                        int value = (int)(Utils.XPOS / Utils.LenLevel * 100);
                        if (value < 100)
                            return value;
                        return 100;
                    }
                    catch
                    {
                        return 0;
                    }
                }
            }
            /// <summary>
            /// is auto level?
            /// </summary>
            public static bool IsAuto => gm.Read<bool>(0x3222D0, 0x164, 0x22C, 0x114, 0x2B0);

            /// <summary>
            /// is demon level?
            /// </summary>
            public static bool IsDemon => gm.Read<bool>(0x3222D0, 0x164, 0x22C, 0x114, 0x29C);

            /// <summary>
            /// Diff
            /// </summary>
            public static int Diff => gm.Read<int>(0x3222D0, 0x164, 0x22C, 0x114, 0x1E4);

            /// <summary>
            /// Diff Demon
            /// </summary>
            public static int DiffDemon => gm.Read<int>(0x3222D0, 0x164, 0x22C, 0x114, 0x2A0);

            /// <summary>
            /// level type
            /// </summary>
            public static int LevelType => gm.Read<int>(0x3222D0, 0x164, 0x22C, 0x114, 0x364);

            /// <summary>
            /// level name
            /// </summary>
            public static string LevelName => gm.ReadString(0x3222D0, 0x164, 0x22C, 0x114, 0xFC);

            /// <summary>
            /// creator name
            /// </summary>
            public static string CreatorName => gm.ReadString(0x3222D0, 0x164, 0x22C, 0x114, 0x144);
        }

        public class Editor
        {
            /// <summary>
            /// is opened editor?
            /// </summary>
            public static bool IsOpened => gm.Read<bool>(0x3222D0, 0x168);

            /// <summary>
            /// block count
            /// </summary>
            public static int BlockCount => gm.Read<int>(0x3222D0, 0x168, 0x3A0);
        }

    }
}

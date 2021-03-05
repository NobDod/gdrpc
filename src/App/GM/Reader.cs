using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDRPC.App.GM
{
    class Reader
    {
        /// <summary>
        /// Чтение памяти с GM (GameManager) функции. Облегченная версия.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mainAddres">Основной адрес откуда требуется считать (иногда их нету)</param>
        /// <param name="offests">оффсеты.</param>
        /// <returns></returns>
        public static T Read<T>(int mainAddres, int[] offests) where T : struct => App.GameManager.Read<T>(offests, mainAddres);
        public static T Read<T>(int[] offests) where T : struct => App.GameManager.Read<T>(offests, true);

        /// <summary>
        /// ID сцен. 
        /// </summary>
        public static int SceneID => Read<int>(0x1DC, new[] { 0x3222D0 });
        
        public class Level
        {
            public class Utils
            {
                //господи, XPOS = , 30 минут фиксил баг класс
                public static float XPOS => Read<float>(0x67C, new int[] { 0x003222D0, 0x164, 0x224, 0x4E8, 0xB4 });


                public static float LenLevel => Read<float>(0x3B4, new int[] { 0x003222D0, 0x164 });
                public static int LevelID => Read<int>(0x2A0, new[] { 0x003222D0 });
            }

            /// <summary>
            /// Открыт ли уровень?
            /// </summary>
            public static bool IsOpened => Read<bool>(new[] { 0x003222D0, 0x164, 0x22C, 0x114 });

            /// <summary>
            /// Уровень запущен в практик режиме
            /// </summary>
            public static bool IsPracticeMode => Read<bool>(new[] { 0x003222D0, 0x164, 0x495 });

            /// <summary>
            /// ID уровня
            /// </summary>
            public static int LevelID => Read<int>(0xF8, new[] { 0x003222D0, 0x164, 0x22C, 0x114 });

            /// <summary>
            /// Количество звезд в уровне
            /// </summary>
            public static int Stars => Read<int>(0x2AC, new[] { 0x003222D0, 0x164, 0x22C, 0x114 });

            /// <summary>
            /// Попытки (не всего в уровне)
            /// </summary>
            public static int Attempts => Read<int>(0x4A8, new[] { 0x003222D0, 0x164 });

            /// <summary>
            /// Количество процентов пройденого уровня
            /// </summary>
            public static int BestProcent => Read<int>(0x248, new[] { 0x003222D0, 0x164, 0x22C, 0x114 });

            /// <summary>
            /// Количество практичных процентов пройденого уровня
            /// </summary>
            public static int BestPracticeProcent => Read<int>(0x26C, new[] { 0x003222D0, 0x164, 0x22C, 0x114 });

            /// <summary>
            /// Текущие проценты 
            /// </summary>
            //public static int Procent => Read<int>(0x450, new[] { 0x003222D0, 0x164, 0x22C, 0x114 });
            public static int Procent => (int)(Utils.XPOS / Utils.LenLevel * 100);

            /// <summary>
            /// Diff: авто уровень?
            /// </summary>
            public static bool IsAuto => Read<bool>(0x2B0, new[] { 0x003222D0, 0x164, 0x22C, 0x114 });

            /// <summary>
            /// Diff: демон уровень?
            /// </summary>
            public static bool IsDemon => Read<bool>(0x29C, new[] { 0x003222D0, 0x164, 0x22C, 0x114 });

            /// <summary>
            /// Diff
            /// </summary>
            public static int Diff => Read<int>(0x1E4, new[] { 0x003222D0, 0x164, 0x22C, 0x114 });

            /// <summary>
            /// Diff Demon
            /// </summary>
            public static int DiffDemon => Read<int>(0x2A0, new[] { 0x003222D0, 0x164, 0x22C, 0x114 });

            /// <summary>
            /// тип уровня
            /// </summary>
            public static int LevelType => Read<int>(0x364, new[] { 0x003222D0, 0x164, 0x22C, 0x114 });
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

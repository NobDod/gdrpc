using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDRPC.App.GM
{
    partial class GameDiff
    {
        /// <summary>
        /// Получить иконку уровня
        /// </summary>
        public static string GetName(Diffs diff)
        {
            if (diff == Diffs.na)
                return "na";
            else if (diff == Diffs.auto)
                return "auto";
            else if (diff == Diffs.easy)
                return "easy";
            else if (diff == Diffs.normal)
                return "normal";
            else if (diff == Diffs.hard)
                return "hard";
            else if (diff == Diffs.harder)
                return "harder";
            else if (diff == Diffs.insane)
                return "insane";
            else if (diff == Diffs.demon)
                return "demon_hard";
            return "na";
        }
        /// <summary>
        /// Получить иконку уровня
        /// </summary>
        public static string GetName(int id)
        {
            if (id < 3)
                return GetName(Diffs.easy);
            else if (id < 5)
                return GetName(Diffs.normal);
            else if (id < 6)
                return GetName(Diffs.hard);
            else if (id < 10 || (id == 19 || id == 17))
                return GetName(Diffs.harder);
            else if (id < 14)
                return GetName(Diffs.insane);
            else if (id == 16 || id == 17 || id == 21)
                return GetName(Diffs.insane);
            else if (id == 14 || id == 18 || id == 20)
                return GetName(Diffs.demon);
            else if (id == 22)
                return GetName(Diffs.normal);
            else if (id == 23)
                return GetName(Diffs.harder);
            else if (id == 24)
                return GetName(Diffs.hard);
            else if (id == 3001)
                return GetName(Diffs.normal);
            return "na";
        }
        /// <summary>
        /// Получить иконку уровня
        /// </summary>
        public static string GetName(DiffDemons diff)
        {
            if (diff == DiffDemons.hard)
                return "demon_hard";
            else if (diff == DiffDemons.easy)
                return "demon_easy";
            else if (diff == DiffDemons.meduim)
                return "demon_medium";
            else if (diff == DiffDemons.insane)
                return "demon_insase";
            else if (diff == DiffDemons.extreme)
                return "demon_extreme";
            return "demon_hard";
        }
    }

    class GameLevel
    {
        /// <summary>
        /// Get local level name
        /// </summary>
        /// <param name="id">by ID</param>
        /// <returns></returns>
        public static string getName(int id)
        {
            switch (id)
            {
                case 1:
                    return "Stereo Madness";
                case 2:
                    return "Back on Track";
                case 3:
                    return "Polargeist";
                case 4:
                    return "Dry Out";
                case 5:
                    return "Base After Base";
                case 6:
                    return "Cant let go";
                case 7:
                    return "Jumper";
                case 8:
                    return "Time Machine";
                case 9:
                    return "Cycles";
                case 10:
                    return "XStep";
                case 11:
                    return "Clutterfunk";
                case 12:
                    return "Theory of Everything";
                case 13:
                    return "Electroman Adventures";
                case 14:
                    return "Clubstep";
                case 15:
                    return "Electrodynamix";
                case 16:
                    return "Hexagon Force";
                case 17:
                    return "Blast Processing";
                case 18:
                    return "Theory of Everything 2";
                case 19:
                    return "Geometrical Dominator";
                case 20:
                    return "Deadlocked";
                case 21:
                    return "Fingerdash";
                case 22:
                    return "Striker";
                case 23:
                    return "Airborne Robots";
                case 24:
                    return "Viking Arena";
                case 3001:
                    return "The challenge";
            }
            return "Invalid level " + id;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDRPC.App.GM
{
    partial class GameDiff
    {
        public enum Diffs
        {
            na = 0,
            auto = 1,
            easy = 10,
            normal = 20,
            hard = 30,
            harder = 40,
            insane = 50,
            demon = 60
        }

        public enum DiffDemons
        {
            easy = 3,
            meduim = 4,
            insane = 5,
            extreme = 6,
            hard = 1//default.
        }
    }
    enum Scenes
    {
        UNKNOWN = -1,
        MAIN = 0,
        SELECT = 1,
        OLD_MY_LEVELS = 2,
        EDITOR_OR_LEVEL = 3,
        SEARCH = 4,
        UNUSED = 5,
        LEADERBOARD = 6,
        ONLINE = 7,
        OFFICIAL_LEVELS = 8,
        OFFICIAL_LEVEL = 9,
        THE_CHALLENGE = 12
    }
    enum LevelType
    {
        NULL = 0,
        OFFICIAL = 1,
        EDITOR = 2,

        /// <summary>
        /// ONLINE
        /// </summary>
        SAVED = 3,
        ONLINE = 4
    }
}

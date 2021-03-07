using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDRPC.App
{
    class AppLevel
    {
        public static async Task Run()
        {
            GM.GameDiff.Diffs Diff;
            string iconSet, levelName = GM.Reader.Level.LevelName, creatorName = GM.Reader.Level.CreatorName;

            bool isDemon = GM.Reader.Level.IsDemon;
            int levelID = GM.Reader.Level.LevelID;
            GM.Scenes scene = (GM.Scenes)GM.Reader.SceneID;
            GM.LevelType type = (GM.LevelType)GM.Reader.Level.LevelType;
            if (scene == GM.Scenes.OFFICIAL_LEVEL)
            {
                if(levelID == 0) levelID = GM.Reader.Level.Utils.LevelID;
                iconSet = GM.GameDiff.GetName(levelID);
                Diff = GM.GameDiff.Diffs.auto;
                creatorName = "RobTop";
            }
            else
            {
                Diff = (isDemon ? GM.GameDiff.Diffs.demon : (GM.GameDiff.Diffs)GM.Reader.Level.Diff);
                iconSet = (Diff == GM.GameDiff.Diffs.demon ? GM.GameDiff.GetName((GM.GameDiff.DiffDemons)GM.Reader.Level.DiffDemon) : GM.GameDiff.GetName(Diff));
                if (String.IsNullOrWhiteSpace(creatorName))
                    creatorName = "{NAME_NOT_PROVIDED}";
            }
            if (String.IsNullOrWhiteSpace(levelName))
                levelName = "{NAME_NOT_PROVIDED}";

            DateTime t = DateTime.UtcNow;
            while (GM.Reader.Level.IsOpened)
            {
                int procent = GM.Reader.Level.Procent, totalProcent = GM.Reader.Level.BestProcent, attempts = GM.Reader.Level.Attempts,
                    stars = GM.Reader.Level.Stars;

                //ожидание загрузки уровня
                if (procent < 0)
                    continue;
#if DEBUG
                Log.WriteLine("[AppEvent (Level)]: Level: {0} (prac: {8}), diff: {1} ({2}), procent: {5} (p:{7}) (t:{6}), isAuto: {3}, isDemon: {4}",
                    levelID, Diff.ToString(), iconSet, "-", isDemon.ToString(), procent, totalProcent, GM.Reader.Level.BestPracticeProcent, GM.Reader.Level.IsPracticeMode.ToString());
                Log.WriteLine("[AppEvent (Level)]: X POS: {0}, Len: {1}, Attempts: {2}",
                   GM.Reader.Level.Utils.XPOS, GM.Reader.Level.Utils.LenLevel, attempts);
                Log.WriteLine("[AppEvent (Level)]: Level Name: {0} by {1}\n",
                    levelName, creatorName);
#endif
                bool isOnline = (type == GM.LevelType.SAVED || type == GM.LevelType.ONLINE);
                Discord.RichPresence rpc = App.defaultRpc;

                if (type == GM.LevelType.EDITOR)
                    rpc.Details = "Testing a level";
                else
                    rpc.Details = levelName + " by " + creatorName;
                rpc.State = "Percent: " + procent + "%, attempts: " + attempts;
                rpc.Timestamps.Start = t;
                rpc.Assets.SmallImageKey = iconSet;
                rpc.Assets.SmallImageText = (isOnline ? "ID: " + levelID + ", s" : "S") + "tars: " + stars + ", best percent: " + totalProcent + "%";
                Discord.Discord.SetPresence(rpc);
                await Task.Delay(1000);
            }
        }
    }
}

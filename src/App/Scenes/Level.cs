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
            string iconSet;
            GM.GameDiff.Diffs Diff;
            bool isDemon = GM.Reader.Level.IsDemon;
            int levelID = GM.Reader.Level.LevelID;
            GM.Scenes scene = (GM.Scenes)GM.Reader.SceneID;
            GM.LevelType type = (GM.LevelType)GM.Reader.Level.LevelType;

            //если уровень официальный
            if (scene == GM.Scenes.OFFICIAL_LEVEL)
            {
                if(levelID == 0)
                    levelID = GM.Reader.Level.Utils.LevelID;
                iconSet = GM.GameDiff.GetName(levelID);
                Diff = GM.GameDiff.Diffs.auto;
            }
            else
            {
                Diff =
                    //demon level
                    (isDemon ? GM.GameDiff.Diffs.demon : (GM.GameDiff.Diffs)GM.Reader.Level.Diff);
                iconSet =
                    //isDemon
                    (Diff == GM.GameDiff.Diffs.demon ? GM.GameDiff.GetName((GM.GameDiff.DiffDemons)GM.Reader.Level.DiffDemon) :
                    //other
                    GM.GameDiff.GetName(Diff));
            }
            DateTime t = DateTime.UtcNow;
            while (GM.Reader.Level.IsOpened)
            {
                int procent = GM.Reader.Level.Procent, totalProcent = GM.Reader.Level.BestProcent, attempts = GM.Reader.Level.Attempts,
                    stars = GM.Reader.Level.Stars;

                //ожидание загрузки уровня
                if (procent < 0)
                    continue;
#if DEBUG
                Console.WriteLine("Level: {0}, diff: {1} ({2}), procent: {5} (t:{6}), isAuto: {3}, isDemon: {4}",
                    levelID, Diff.ToString(), iconSet, "-", isDemon.ToString(), procent, totalProcent) ;
                Console.WriteLine("X POS: {0}, Len: {1}, Attempts: {2}",
                   GM.Reader.Level.Utils.XPOS, GM.Reader.Level.Utils.LenLevel, attempts);
#endif
                Discord.RichPresence rpc = App.defaultRpc;
                if(type == GM.LevelType.OFFICIAL)
                    rpc.Details = "Playing a official level";
                else if (type == GM.LevelType.EDITOR)
                    rpc.Details = "Testing a level";
                else
                    rpc.Details = "Playing a level";
                rpc.State = "Percent: " + procent + "%, attempts: " + attempts;
                rpc.Timestamps.Start = t;
                rpc.Assets.SmallImageKey = iconSet;
                rpc.Assets.SmallImageText = "Stars: " + stars + ", best percent: " +totalProcent + "%";
                Discord.Discord.SetPresence(rpc);
                await Task.Delay(1000);
            }
        }
    }
}

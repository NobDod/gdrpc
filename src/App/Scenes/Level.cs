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
            //wait for correct data. please, don`t remove task delay. thanks.
            await Task.Delay(250);

            //level type, without scene.
            GM.LevelType type = (GM.LevelType)GM.Reader.Level.LevelType;

            string icon_name = "", levelName = GM.Reader.Level.LevelName,
                   creatorName = GM.Reader.Level.CreatorName;
            bool isDemon = GM.Reader.Level.IsDemon, isOnline = (type == GM.LevelType.ONLINE || type == GM.LevelType.SAVED);
            int levelID = GM.Reader.Level.LevelID, levelStars = GM.Reader.Level.Stars;

            //level started
            DateTime levelStarted = DateTime.UtcNow;

            //get icon
            if (!isOnline)
            {
                //if creator name is null
                //all official level by robtop
                if (String.IsNullOrWhiteSpace(creatorName))
                    creatorName = "RobTop";

                icon_name = GM.GameDiff.GetName(levelID);
            }
            else
            {
                //if creator name is null
                if (String.IsNullOrWhiteSpace(creatorName))
                    creatorName = "{NOT_PROVIDED}";
                //if level name is null
                if (String.IsNullOrWhiteSpace(levelName))
                    levelName = "{NOT_PROVIDED}";

                if (isDemon) icon_name = GM.GameDiff.GetName((GM.GameDiff.DiffDemons)GM.Reader.Level.DiffDemon);
                else icon_name = GM.GameDiff.GetName((GM.GameDiff.Diffs)GM.Reader.Level.Diff);
            }

            while (GM.Reader.Level.IsOpened)
            {
                int currentPercent = GM.Reader.Level.Procent, bestPercent = GM.Reader.Level.BestProcent,
                    currentAttempts = GM.Reader.Level.Attempts;

                //level not loaded.
                if (currentPercent < 0)
                    continue;

                #region Debug Log
#if DEBUG
                Log.WriteLine("[Event: Level]: {0} by {1} (ID: {7}, stars: {2}, icon: {3}). Percent: {4} (total: {5}, prac total: {6})", 
                    levelName, creatorName, levelStars, icon_name, currentPercent, bestPercent, GM.Reader.Level.BestPracticeProcent, levelID);
                Log.WriteLine("[Event: Level]: X POS: {0}, Len: {1}, Attempts: {2}",
                   GM.Reader.Level.Utils.XPOS, GM.Reader.Level.Utils.LenLevel, currentAttempts);
#endif
                #endregion

                //SET PRESENCE
                Discord.RichPresence rpc = App.defaultRpc;
                //set details (level name, or testing)
                if (type == GM.LevelType.EDITOR)
                    rpc.Details = "Testing an level";
                else
                    rpc.Details = $"{levelName} by {creatorName}";
                //set state
                rpc.State = $"Completed: {currentPercent}%, {currentAttempts} attempt";
                //set small text
                rpc.Assets.SmallImageText = (isOnline ? $"ID: {levelID}, s" : "S") + $"tars: {levelStars}, best completed: {bestPercent}%";
                //set icons
                rpc.Assets.SmallImageKey = icon_name;
                rpc.Timestamps.Start = levelStarted;

                //set
                Discord.Discord.SetPresence(rpc);
                await Task.Delay(100);
            }
        }
    }
}

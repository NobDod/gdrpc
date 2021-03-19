using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDRPC.App
{
    class AppMenu
    {
        public static async Task Run()
        {
            //wait for correct data. please, don`t remove task delay. thanks.
            await Task.Delay(250);

            //current rpc
            Discord.RichPresence rpc = App.defaultRpc;
            rpc.Details = "In menu";

            //if level not opened and editor not opened.
            while (!GM.Reader.Level.IsOpened && !GM.Reader.Editor.IsOpened)
            {
                GM.Scenes scene = (GM.Scenes)GM.Reader.SceneID;

                //helps
                bool offLevel = scene == GM.Scenes.OFFICIAL_LEVELS || scene == GM.Scenes.OFFICIAL_LEVEL, 
                    onlineLevel = scene == GM.Scenes.SELECT || scene == GM.Scenes.EDITOR_OR_LEVEL;
                //end
#if DEBUG
                Log.WriteLine("[Event: Menu]: Scene: {0}", scene.ToString());
#endif
                //scene
                if(scene == GM.Scenes.SEARCH)
                {
                    rpc.State = "Searching";
                    rpc.Assets.SmallImageKey = "search";
                }
                else if (offLevel || onlineLevel)
                {
                    rpc.State = "Selecting " + (offLevel ? "official" : "online") + " level";
                    rpc.Assets.SmallImageKey = "btn_play";
                }
                else
                {
                    rpc.State = "";
                    rpc.Assets.SmallImageKey = "";
                }
                //set presence
                
                Discord.Discord.SetPresence(rpc);
                await Task.Delay(100);
            }
        }
    }
}

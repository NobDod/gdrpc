using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDRPC.App
{
    class AppEditor
    {
        public static async Task Run()
        {
            GM.Scenes scene = (GM.Scenes)GM.Reader.SceneID;
            DateTime t = DateTime.UtcNow;
            while (GM.Reader.Editor.IsOpened)
            {
                int blocks = GM.Reader.Editor.BlockCount;
#if DEBUG
                Console.WriteLine("Blocks: {0}", blocks);
#endif
                Discord.RichPresence rpc = App.defaultRpc;
                rpc.Details = "Editing level";
                rpc.State = "Blocks: " + blocks;
                rpc.Timestamps.Start = t;
                rpc.Assets.SmallImageKey = "creator_point";
                Discord.Discord.SetPresence(rpc);
                await Task.Delay(1000);
            }
        }
    }
}

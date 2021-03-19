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
            DateTime t = DateTime.UtcNow;
            while (GM.Reader.Editor.IsOpened)
            {
                int blocks = GM.Reader.Editor.BlockCount;
#if DEBUG
                Log.WriteLine("[Event: Editor]: Blocks: {0}", blocks);
#endif
                Discord.RichPresence rpc = App.defaultRpc;
                rpc.Details = "Editing an level";
                rpc.State = "Blocks: " + blocks;
                rpc.Timestamps.Start = t;
                rpc.Assets.SmallImageKey = "creator_point";
                Discord.Discord.SetPresence(rpc);
                await Task.Delay(100);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDRPC.Discord
{
    class Discord
    {
        public static void Initialize(string appID)
        {
            /*fix bug with callback system*/
            if (!System.IO.File.Exists(DiscordLib.LibName))
                AppRunner.MSG.Error("Discord library rich presence (w-32) not found.");
            DiscordLib.EventHandlers handlers = new DiscordLib.EventHandlers();
            handlers.readyCallback += ready;
            DiscordLib.Initialize(appID, ref handlers, true, null);
            DiscordLib.RunCallbacks();
        }

        public static void SetPresence(RichPresence presence)
        {
            DiscordLib.RichPresence presence2 = new DiscordLib.RichPresence();
            presence2.largeImageKey = presence.Assets.LargeImageKey;
            presence2.largeImageText = presence.Assets.LargeImageText;
            presence2.smallImageKey = presence.Assets.SmallImageKey;
            presence2.smallImageText = presence.Assets.SmallImageText;

            //richPresence
            presence2.partyId = presence.PartyId;
            presence2.partyMax = presence.PartyMax;
            presence2.partySize = presence.PartySize;
            presence2.state = presence.State;
            presence2.details = presence.Details;

            presence2.startTimestamp = DateTimeToTimestamp(presence.Timestamps.Start);
            presence2.endTimestamp = DateTimeToTimestamp(presence.Timestamps.End);
            DiscordLib.UpdatePresence(ref presence2);
        }

        public static void Deinitialize() => DiscordLib.Shutdown();

        private static long DateTimeToTimestamp(DateTime dt)
        {
            if (dt == null || (dt.Year < 1000))
                return 0;
            return (dt.Ticks - 621355968000000000) / 10000000;
        }

        public static void ready()
        {
            Console.WriteLine("Discord ready");
        }
    }
}

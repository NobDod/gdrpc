using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDRPC.Discord
{
    class Discord
    {
        /// <summary>
        /// Help function, checking file discord.
        /// </summary>
        public static bool CheckFileDiscord()
        {
            return System.IO.File.Exists(DiscordLib.LibName);
        }

        /// <summary>
        /// Initializing discord rich presence
        /// </summary>
        /// <param name="appID">application id.</param>
        public static void Initialize(ulong appID)
        {
            if (!CheckFileDiscord())
                return;
            DiscordLib.EventHandlers handlers = new DiscordLib.EventHandlers();
            handlers.readyCallback += Ready;
            DiscordLib.Initialize(appID.ToString(), ref handlers, true, null);
            DiscordLib.RunCallbacks();
            Log.WriteLine("[DiscordRPC]: Initialized discord RPC.");
        }

        /// <summary>
        /// Set presence
        /// </summary>
        /// <param name="presence">presence</param>
        public static void SetPresence(RichPresence presence)
        {
            DiscordLib.RichPresence presence2 = new DiscordLib.RichPresence
            {
                largeImageKey = presence.Assets.LargeImageKey,
                largeImageText = presence.Assets.LargeImageText,
                smallImageKey = presence.Assets.SmallImageKey,
                smallImageText = presence.Assets.SmallImageText,

                //richPresence
                partyId = presence.PartyId,
                partyMax = presence.PartyMax,
                partySize = presence.PartySize,
                state = presence.State,
                details = presence.Details,

                startTimestamp = DateTimeToTimestamp(presence.Timestamps.Start),
                endTimestamp = DateTimeToTimestamp(presence.Timestamps.End)
            };
            DiscordLib.UpdatePresence(ref presence2);
        }


        /// <summary>
        /// Stop discord rich presence.
        /// </summary>
        public static void Deinitialize()
        {
            DiscordLib.Shutdown();
            Log.WriteLine("[DiscordRPC]: discord RPC shutdowned.");
        }

        private static long DateTimeToTimestamp(DateTime dt)
        {
            if (dt == null || (dt.Year < 1000))
                return 0;
            return (dt.Ticks - 621355968000000000) / 10000000;
        }

        private static void Ready()
        {
            Log.WriteLine("[DiscordRPC]: Ready ");
        }
    }
}

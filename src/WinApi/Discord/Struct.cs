using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GDRPC.Discord
{
    public struct Assets
    {
        public string LargeImageKey;
        public string LargeImageText;
        public string SmallImageKey;
        public string SmallImageText;
    }

    public struct Timestamps
    {
        public DateTime Start;
        public DateTime End;
    }
    public struct RichPresence
    {
        public string State;
        public string Details;
        public Timestamps Timestamps;
        public string PartyId;
        public int PartySize;
        public int PartyMax;
        public string NatchSecret;
        public string JoinSecret;
        public string SpectateSecret;
        public sbyte Istance;

        public Assets Assets;
    }
    public struct JoinRequest
    {
        public string userId;
        public string username;
        public string avatar;
    }
    public enum Reply : int
    {
        No = 0,
        Yes = 1,
        Ignore = 2
    }
}

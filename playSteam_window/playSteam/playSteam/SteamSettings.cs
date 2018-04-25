using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace playSteam
{
    [Serializable]
    public class SteamSettings
    {
        [Serializable]
        public struct Mates
        {
            public string MateId;
            public string MateFriendlyName;
        }

        public string ApiKey { get; set; } = "";
        public string StemId { get; set; } = "";
        public Mates CurrentMateId;
        

        public List<Mates> prevMates = new List<Mates>();
    }
}

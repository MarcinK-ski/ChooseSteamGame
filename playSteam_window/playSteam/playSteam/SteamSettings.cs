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

            public override string ToString()   //Need to ComboBox
            {
                return MateFriendlyName + ": " + MateId;
            }
        }

        public string ApiKey { get; set; } = "";
        public string StemId { get; set; } = "";
        public Mates CurrentMateId;
        

        private List<Mates> prevMates = new List<Mates>();

        public bool AddMate(Mates currentMate)
        {
            foreach(var mate in prevMates)
            {
                if (mate.MateId == currentMate.MateId)
                    return false;
            }

            prevMates.Add(currentMate);

            return true;
        }

        public List<Mates> GetLastMates()
        {
            return prevMates;
        }
    }
}

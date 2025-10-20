using NMEAMon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NMEAMon.Models
{
    public class NMEA2000
    {
        public long PGN { get; set; } = 0;
        public string Raw { get; set; } = string.Empty;

        public List<string> Source  { get; set; } = new List<string>();
             
        public List<string> Data { get; set; } = new List<string>();

        public byte[] byteArray { get; set; } = Array.Empty<byte>();
        public NMEA2000(string raw)
        {
            Raw = raw;
            Source = new StringParser().CommaListToStringList(Raw);
            if(Source.Count >0)
            {
                
                if(long.TryParse(Source[0], out long pgn))
                {
                    PGN = pgn;
                    
                    for (int i = 1; i < Source.Count; i++)
                    {   
                        Data.Add(Source[i]);
                    }
                    byteArray = Data.Select(s => byte.TryParse(s, out byte b) ? b : (byte)0).ToArray();

                }
            }
        }
        public override string ToString()
        {
            return Raw;
            

        }
    }
}

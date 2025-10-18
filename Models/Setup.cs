using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NMEAMon.Models
{

    public class Setup
    {
        public int Port;
        public bool Night;
        public int Orientation;
        public bool KeepActive;
        public bool UseGPSPOS;
        public bool UseGPSHEADING;
        public bool UseGPSSOG;
        public bool Fakewind;


        public Setup()
        {
            Port = 10110;
            Night = false;
            Orientation = 1;
            KeepActive = true;
            UseGPSPOS = true;
            UseGPSHEADING = false;
            UseGPSSOG = false;
            Fakewind = false;
        }

        public Setup Copy()
        {
            Setup P = new Setup();
            P.Night = Night;
            P.Port = Port;
            P.Orientation = Orientation;
            P.KeepActive = KeepActive;
            P.UseGPSPOS = UseGPSPOS;

            P.UseGPSHEADING = UseGPSHEADING;
            P.UseGPSSOG = UseGPSSOG;
            P.Fakewind = Fakewind;
            return P;

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMEAMon.Models
{

    public class Record
    {
        public int ID;
        public int userId;
        public int boatId;
        public double latitude;
        public double longitude;
        public long time;
        public double depth;
        public double headingMag;
        public double headingTrue;
        public double SOG;
        public double SOW;
        public double windTrueDir;
        public double windTrueSpeed;
        public double windAppDir;
        public double windAppSpeed;
        public double VPWSPD;
        public int POLARDATA;



        public void RecordNew()
        {

            ID = 0;
            userId = 0;
            boatId = 0;
            latitude = 0;
            longitude = 0;
            time = 0;
            depth = 0;
            headingMag = 0;
            headingTrue = 0;
            SOG = 0;
            SOW = 0;
            windTrueDir = 0;
            windTrueSpeed = 0;
            windAppDir = 0;
            windAppSpeed = 0;
            VPWSPD = 0;
            POLARDATA = 0;


        }

        public Record Copy()
        {
            Record P = new Record();
            P.ID = ID;
            P.userId = userId;
            P.boatId = boatId;
            P.latitude = latitude;
            P.longitude = longitude;
            P.time = time;
            P.depth = depth;
            P.headingMag = headingMag;
            P.headingTrue = headingTrue;
            P.SOG = SOG;
            P.SOW = SOW;
            P.windTrueDir = windTrueDir;
            P.windTrueSpeed = windTrueSpeed;
            P.windAppDir = windAppDir;
            P.windAppSpeed = windAppSpeed;
            P.VPWSPD = VPWSPD;

            return P;

        }

        public void Makefromlive()//MainActivity MA)
        {

            userId = 1;
            boatId = 1;
            time = System.DateTime.Now.Ticks;
            //if (MA.CBRECORD.isChecked() == true) POLARDATA = 1;
            //POLARDATA = 0;


        }



    }

}

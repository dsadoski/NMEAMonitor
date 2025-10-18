using NMEAMon.Models;
using NMEAMon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMEAMon.Services
{
    public class NmeaService
    {
        Setup S;
        public Boolean CalcWind;

        public NmeaService(Setup s)
        {
            S = s.Copy();
            CalcWind = false;


        }

        public Record ParseSentence(string message, Record LV)
        {
            if (message.Length < 4) return LV.Copy();
            if (message[0] != '$') return LV.Copy();

            int i;

            string[] NSTR = new StringParser().CommaListToString(message);

            if (NSTR.Length < 2) return LV.Copy();
            string txt = "";
            for (i = 3; i < NSTR[0].Length; i++)
            {
                txt += NSTR[0][i];
            }



            switch(txt)
            {

                case "GLL":
                    LV = NMEA_GLL(NSTR, LV);
                    break;
                case "DBT":
                    LV = NMEA_DBT(NSTR, LV);
                    break;
                case "HDM":
                    LV = NMEA_HDM(NSTR, LV);
                    break;
                case "HDT":
                    LV = NMEA_HDT(NSTR, LV);
                    break;
                case "MWD": 
                    LV = NMEA_MWD(NSTR, LV);
                    break;
                case "MWV": 
                    LV = NMEA_MWV(NSTR, LV);
                    break;
                case "VHW": 
                    LV = NMEA_VHW(NSTR, LV);
                    break;
                case "VPW": 
                    LV = NMEA_VPW(NSTR, LV);
                    break;

            }


            


            // ADD RMC Sentence
            //if(FRAGACTIVE==FRAG1.POSITION)


            //FRAG1.UpdateNumbers();



            /*FD.ShowData();

                FW.ShowData();*/
            // VTG Course over ground;
            return LV.Copy();


        }


        public Record NMEA_DBT(string[] ARY, Record LV)// depth below transducer
        {
            /*
                    DBT - Depth below transducer
                    1   2 3   4 5   6 7
                        |   | |   | |   | |
                    0$--DBT,x.x,f,x.x,M,x.x,F*hh<CR><LF>
                        Field Number:
                    1Water depth, feet
                    2f = feet
                    3Water depth, meters
                    4M = meters
                    5Water depth, Fathoms
                    6F = Fathoms
                    7Checksum
                    In real-world sensors, sometimes not all three conversions are reported. So you might see something like $SDDBT,,f,22.5,M,,F*cs
                    Example: $SDDBT,7.8,f,2.4,M,1.3,F*0D*/

            int i;

            ////TV1.append(string.valueOf(ARY.Length));
            //for(i=0;i<ARY.Length;i++)//TV1.append(ARY[i]+"\n");
            for (i = 1; i < ARY.Length - 1; i++)
            {

                if (ARY[i + 1]=="f")
                {
                    LV.depth = DoubleGet(ARY[i]);


                }



            }


            return LV.Copy();

        }

        public Record NMEA_HDM(string[] ARY, Record LV)// depth below transducer
        {

            if (ARY.Length < 2) return LV.Copy(); ;



            if (S.UseGPSHEADING == false) LV.headingMag = DoubleGet(ARY[1]);
            CalcWind = true;
            return LV.Copy();





        }

        public Record NMEA_HDT(string[] ARY, Record LV)// depth below transducer
        {


            if (ARY.Length < 2) return LV.Copy(); ;

            if (S.UseGPSHEADING == false)
            {
                LV.headingTrue = DoubleGet(ARY[1]);
                CalcWind = true;
            }




            return LV.Copy();

        }

        public Record NMEA_MWD(string[] ARY, Record LV)
        {
            /*MWD - Wind Direction & Speed
    The direction from which the wind blows across the earth’s surface, with respect to north, and the speed of
    the wind.
    $--MWD,x.x,T,x.x,M,x.x,N,x.x,M*hh<CR><LF>
    Wind speed, meters/second
    Wind speed, knots
    Wind direction, 0 to 359 degrees Magnetic
    Wind direction, 0 to 359 degrees True*/

            if (ARY.Length < 8) return LV.Copy(); ;

            // only use if youare getting SOG frominternal GPS

            if (S.UseGPSSOG == false)
            {
                LV.windTrueDir = DoubleGet(ARY[1]);
                //D.WTRU.D.DIRMAG = DoubleGet(ARY[3]);
                LV.windTrueSpeed = DoubleGet(ARY[5]);
            }



            int i;
            return LV.Copy();

        }


        public Record NMEA_MWV(string[] ARY, Record LV)
        {
            /*When the reference field is set to R (Relative), data is provided giving the wind angle in relation to the
     vessel's bow/centerline and the wind speed, both relative to the (moving) vessel. Also called apparent
     wind, this is the wind speed as felt when standing on the (moving) ship.
     When the reference field is set to T (Theoretical, calculated actual wind), data is provided giving the wind
     angle in relation to the vessel's bow/centerline and the wind speed as if the vessel was stationary. On a
     moving ship these data can be calculated by combining the measured relative wind with the vessel's own
     speed.
     Example 1: If the vessel is heading west at 7 knots and the wind is from the east at 10 knots the relative
     wind is 3 knots at 180 degrees. In this same example the theoretical wind is 10 knots at 180 degrees (if the
     boat suddenly stops the wind will be at the full 10 knots and come from the stern of the vessel 180 degrees
     from the bow).
     Example 2: If the vessel is heading west at 5 knots and the wind is from the southeast at 7.07 knots the
     relative wind is 5 knots at 270 degrees. In this same example the theoretical wind is 7.07 knots at 225
     degrees (if the boat suddenly stops the wind will be at the full 7.07 knots and come from the port-quarter
     of the vessel 225 degrees from the bow).
     $--MWV,x.x,a,x.x,a,A*hh<CR><LF>
     Status, A = Data Valid, V = Data invalid
     Wind speed units, K/M/N/S
     Wind speed
     Reference, R = Relative
     T = Theoretical*/

            if (ARY.Length < 4)
            {
                //TV1.append("Failed on Length\n");
                return LV.Copy();
            }

            if (ARY[2] == "R" )
            {
                //TV1.append("Failed on R\n");
                return LV.Copy();//only want relative;
            }
            //if(ARY[5].charAt(0)!='V')return;//not valid
            /*D.WAPP.D.Clear();
            D.WAPP.D.DIRMAG=DoubleGet(ARY[1]);
            D.WAPP.D.DIRTRU=DoubleGet(ARY[1]);*/
            LV.windAppDir = DoubleGet(ARY[1]);

            //double T;// for conversion at a later date
            int i;
            for (i = 1; i < ARY.Length - 1; i++)
            {


                /*if (ARY[i+1].equals("K")) T = DoubleGet(ARY[i]);// kilometers?
                else if (ARY[i+1].equals("M")) D.WAPP.D.SPDMS = DoubleGet(ARY[i]);//meters?
                else */
                if (ARY[i + 1] == "N") LV.windAppSpeed = DoubleGet(ARY[i]);//Knots?
                                                                           //else if (ARY[i+1].equals("S")) T = DoubleGet(ARY[i]);//Statute miles?
            }


            CalcWind = true;
            return LV.Copy();
        }

        public Record NMEA_VHW(string[] ARY, Record LV)
        {
            /*The compass heading to which the vessel points and the speed of the vessel relative to the water.
    $--VHW,x.x,T,x.x,M,x.x,N,x.x,K*hh<CR><LF>
    Speed, km/hr
    Speed, knots
    Heading, degrees Magnetic
    Heading, degrees True*/

            int i;

            if (ARY.Length < 9) return LV.Copy();

            //D.Speed.D.Clear();


            for (i = 1; i < ARY.Length - 1; i++)
            {
                if (ARY[i + 1] == "T")
                {
                    if (S.UseGPSHEADING == false) LV.headingTrue = DoubleGet(ARY[i]);
                }
                if (ARY[i + 1] == "M")
                {
                    if (S.UseGPSHEADING == false) LV.headingMag = DoubleGet(ARY[i]);
                }
                if (ARY[i + 1] == "N")
                {
                    LV.SOW = DoubleGet(ARY[i]);
                }
                if (ARY[i + 1] == "K")
                {
                    //D.Speed.D.SPDKMH=DoubleGet(ARY[i]);
                }


            }
            CalcWind = true;
            return LV.Copy();
        }

        public double DoubleGet(string msg)
        {
            if (msg.Length < 1) return 0;

            double T = double.Parse(msg);
            if (double.IsInfinity(T) || double.IsNaN(T))
            {
                return 0;
            }


            return T;
        }

        public Record NMEA_VPW(string[] ARY, Record LV)
        {
            /*
            VPW - Speed - Measured Parallel to Wind
    The component of the vessel's velocity vector parallel to the direction of the true wind direction.
    Sometimes called "speed made good to windward" or "velocity made good to windward".
    $--VPW,x.x,N,x.x,M*hh<CR><LF>
    Speed, meters/second, "-" = downwind
    Speed, knots, "-" = downwind
             */
            if (ARY.Length < 3) return LV.Copy();
            //D.Vpw.D.Clear();

            /**if(ARY[2].equals("N"))D.Vpw.D.SPDKTS=DoubleGet(ARY[1]);
             if(ARY[2].equals("M"))D.Vpw.D.SPDMS=DoubleGet(ARY[1]);*/


            if (S.UseGPSHEADING == false && S.UseGPSSOG == false) LV.VPWSPD = DoubleGet(ARY[1]);


            return LV.Copy();
        }

        public Record NMEA_GLL(string[] ARY, Record LV)
        {
            /*
            0 Message ID	$GPGLL	GLL protocol header
            1 Latitude	3723.2475	ddmm.mmmm
    2 N/S indicator	N	N =North or S = south -1
    3 Longitude	12158.3416	dddmm.mmmm
    4 E/W indicator	W	E =East or W = West -1
    5 UTC time	161229.487	hhmmss.sss
    6 Status	A	A = data valid or V = data not valid
    7 Mode	A	A =Autonomous , D =DGPS, E =DR (This field is only present in NMEA version 3.0)
    8Checksum	*41
    <CR><LF>		End of message termination*/
            if (ARY.Length < 10) return LV.Copy();

            if (ARY[6]=="V") return LV.Copy();//not valid data


            if (S.UseGPSHEADING == false)
            {
                LV.latitude = DoubleGet(ARY[1]);
                if (ARY[2] == "S") LV.latitude = LV.latitude * -1;
                LV.longitude = DoubleGet(ARY[3]);
                if (ARY[4] == "W") LV.longitude = LV.longitude * -1;
            }
            /*UTC=DoubleGet(ARY[5]);
            time=GetNow();*/


            return LV.Copy();
        }

    }
}

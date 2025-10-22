
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
        public Record record;

        public NmeaService(Setup s)
        {
            S = s.Copy();
            CalcWind = false;
            record = new Record();
        }

        public Record ParseSentence(string message)
        {

            record.ErrMessage = string.Empty;
            if (message.Length < 4) return record.Copy();
            if (message[0] != '$') return record.Copy();

            int i;

            var splitMessage = message.Split('*');// remove checksum

            string[] NSTR = new StringParser().CommaListToString(splitMessage[0]);

            if (NSTR.Length < 2) return record.Copy();
            string txt = "";
            for (i = 3; i < NSTR[0].Length; i++)
            {
                txt += NSTR[0][i];
            }



            switch (txt)
            {
                case "BAT":
                    record = NMEA_BAT(NSTR, record);
                    break;
                case "GLL":
                    record = NMEA_GLL(NSTR, record);
                    break;
                case "DBT":
                    record = NMEA_DBT(NSTR, record);
                    break;
                case "DPT":
                    record = NMEA_DPT(NSTR, record);
                    break;
                case "HDM":
                    record = NMEA_HDM(NSTR, record);
                    break;
                case "HDG":
                    record = NMEA_HDG(NSTR, record);
                    break;
                case "HDT":
                    record = NMEA_HDT(NSTR, record);
                    break;
                case "MWD":
                    record = NMEA_MWD(NSTR, record);
                    break;
                case "MWV":
                    record = NMEA_MWV(NSTR, record);
                    break;

                case "SPD":
                    record = NMEA_SPD(NSTR, record);
                    break;

                case "TEMP":
                    record = NMEA_TEMP(NSTR, record);
                    break;

                case "VHW":
                    record = NMEA_VHW(NSTR, record);
                    break;
                case "VPW":
                    record = NMEA_VPW(NSTR, record);
                    break;

                case "VTG":
                    record = NMEA_VTG(NSTR, record);
                    break;

                case "WND":
                    record = NMEA_VTG(NSTR, record);
                    break;

                default:
                    record.ErrMessage = txt;
                    break;

            }





            // ADD RMC Sentence
            //if(FRAGACTIVE==FRAG1.POSITION)


            //FRAG1.UpdateNumbers();



            /*FD.ShowData();

                FW.ShowData();*/
            // VTG Course over ground;
            return record;


        }


        public Record NMEA_DBT(string[] stray, Record record)// depth below transducer
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

            ////TV1.append(string.valueOf(stray.Length));
            //for(i=0;i<stray.Length;i++)//TV1.append(stray[i]+"\n");
            for (i = 1; i < stray.Length - 1; i++)
            {

                if (stray[i + 1] == "f")
                {
                    record.depth = DoubleGet(stray[i]);


                }



            }


            return record.Copy();

        }

        public Record NMEA_HDM(string[] stray, Record record)// depth below transducer
        {

            if (stray.Length < 2) return record.Copy(); ;



            if (S.UseGPSHEADING == false) record.headingMag = DoubleGet(stray[1]);
            CalcWind = true;
            return record.Copy();





        }

        public Record NMEA_HDT(string[] stray, Record record)// depth below transducer
        {


            if (stray.Length < 2) return record.Copy(); ;

            if (S.UseGPSHEADING == false)
            {
                record.headingTrue = DoubleGet(stray[1]);
                CalcWind = true;
            }




            return record.Copy();

        }

        public Record NMEA_MWD(string[] stray, Record record)
        {
            /*MWD - Wind Direction & Speed
    The direction from which the wind blows across the earth’s surface, with respect to north, and the speed of
    the wind.
    $--MWD,x.x,T,x.x,M,x.x,N,x.x,M*hh<CR><LF>
    Wind speed, meters/second
    Wind speed, knots
    Wind direction, 0 to 359 degrees Magnetic
    Wind direction, 0 to 359 degrees True*/

            if (stray.Length < 8) return record.Copy(); ;

            // only use if youare getting SOG frominternal GPS

            if (S.UseGPSSOG == false)
            {
                record.windTrueDir = DoubleGet(stray[1]);
                //D.WTRU.D.DIRMAG = DoubleGet(stray[3]);
                record.windTrueSpeed = DoubleGet(stray[5]);
            }



            int i;
            return record.Copy();

        }


        public Record NMEA_MWV(string[] stray, Record record)
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

            if (stray.Length < 4)
            {
                //TV1.append("Failed on Length\n");
                return record.Copy();
            }


            record.windAppDir = DoubleGet(stray[1]);

            //double T;// for conversion at a later date
            int i;

            record.windAppSpeed = DoubleGet(stray[3]);
            // sending radians, so testing
            double T = DoubleGet(stray[6]);
            /*record.ErrMessage = (T* (180/Math.PI)).ToString()+", ";
            record.ErrMessage += (T*.1 * (180 / Math.PI)).ToString() + ", ";
            record.ErrMessage += (T * .01 * (180 / Math.PI)).ToString() + ", ";
            record.ErrMessage*/
            //Arduino Pukes on PI, so doing it here

            record.windAppDir = T * (180 / Math.PI);


            if (stray[4] == "M")
            {
                record.windAppSpeed = record.windAppSpeed * 1.94384; // m/s to knots
            }


            CalcWind = true;
            return record.Copy();
        }

        public Record NMEA_VHW(string[] stray, Record record)
        {
            /*The compass heading to which the vessel points and the speed of the vessel relative to the water.
    $--VHW,x.x,T,x.x,M,x.x,N,x.x,K*hh<CR><LF>
    Speed, km/hr
    Speed, knots
    Heading, degrees Magnetic
    Heading, degrees True*/

            int i;

            if (stray.Length < 9) return record.Copy();

            //D.Speed.D.Clear();
            record.SOW = DoubleGet(stray[5]);
            record.headingMag = DoubleGet(stray[3]);
            CalcWind = true;
            return record.Copy();
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

        public Record NMEA_VPW(string[] stray, Record record)
        {
            /*
            VPW - Speed - Measured Parallel to Wind
    The component of the vessel's velocity vector parallel to the direction of the true wind direction.
    Sometimes called "speed made good to windward" or "velocity made good to windward".
    $--VPW,x.x,N,x.x,M*hh<CR><LF>
    Speed, meters/second, "-" = downwind
    Speed, knots, "-" = downwind
             */
            if (stray.Length < 3) return record.Copy();
            //D.Vpw.D.Clear();

            /**if(stray[2].equals("N"))D.Vpw.D.SPDKTS=DoubleGet(stray[1]);
             if(stray[2].equals("M"))D.Vpw.D.SPDMS=DoubleGet(stray[1]);*/


            if (S.UseGPSHEADING == false && S.UseGPSSOG == false) record.VPWSPD = DoubleGet(stray[1]);


            return record.Copy();
        }

        public Record NMEA_VTG(string[] stray, Record record)
        {
            /*Track made good and speed over ground*/
            if (stray.Length < 3) return record.Copy();

            double T;

            if (S.UseGPSHEADING == false)
            {
                record.COG = DoubleGet(stray[1]);

                while (record.COG > 360)
                {
                    record.COG = record.COG - 360;
                }

                while (record.COG < 0)
                {
                    record.COG = record.COG + 360;
                }

            }
            if (S.UseGPSSOG == false)
            {
                record.SOG = DoubleGet(stray[5]);
            }


            return record.Copy();
        }

        public Record NMEA_WND(string[] stray, Record record)
        {
            /*Track made good and speed over ground*/
            if (stray.Length < 3) return record.Copy();

            double T;

            if (S.UseGPSHEADING == false)
            {
                record.COG = DoubleGet(stray[1]);

                while (record.COG > 360)
                {
                    record.COG = record.COG - 360;
                }

                while (record.COG < 0)
                {
                    record.COG = record.COG + 360;
                }

            }
            if (S.UseGPSSOG == false)
            {
                record.SOG = DoubleGet(stray[5]);
            }


            return record.Copy();
        }

        public Record NMEA_GLL(string[] stray, Record record)
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
            if (stray.Length < 7) return record.Copy();

            if (stray[6] == "V") return record.Copy();//not valid data


            if (S.UseGPSHEADING == false)
            {
                record.latitude = DoubleGet(stray[1]);
                if (stray[2] == "S") record.latitude = record.latitude * -1;
                record.longitude = DoubleGet(stray[3]);
                if (stray[4] == "W") record.longitude = record.longitude * -1;
            }
            /*UTC=DoubleGet(stray[5]);
            time=GetNow();*/


            return record.Copy();
        }

        public Record NMEA_TEMP(string[] stray, Record record)
        {

            record.waterTemp = DoubleGet(stray[1]);
            return record.Copy();
        }
        public Record NMEA_DPT(string[] stray, Record record)
        {
            // depth in Meters convert to feet
            record.depth = DoubleGet(stray[1]);
            record.depth = record.depth * 3.281;
            return record.Copy();
        }

        public Record NMEA_HDG(string[] stray, Record record)
        {
            record.headingTrue = DoubleGet(stray[1]);
            return record.Copy();
        }

        public Record NMEA_SPD(string[] stray, Record record)
        {
            record.SOG = DoubleGet(stray[1]);
            return record.Copy();
        }


        public Record NMEA_BAT(string[] stray, Record record)
        {
            record.voltage = DoubleGet(stray[1]);
            return record.Copy();
        }
    }

}


using NMEAMon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;





namespace NMEAMon.Services
{
    public class N2KService
    {
        public Record record;
        public N2KService()
        {
            record = new Record();
        }

        public Record N2KParse(long PGN, byte[] data)
        {
            record.time = System.DateTime.Now.Ticks;

            switch (PGN)
            {

                case 59904:
                    {
                        // Engine Parameters, Dynamic
                        // Example fields: RPM (bytes 3-4), Load (bytes 5), Fuel rate (bytes 6-7), etc.
                        if (data.Length < 8) return record;
                        /*int rpm = BytesToInt(data, 3, 2);       // RPM in 1/10th RPM
                        int load = data[5];                    // Engine load in percentage
                        int fuelRate = BytesToInt(data, 6, 2);  // Fuel rate in liters per hour

                        // result = emitENG(rpm, load, fuelRate);*/
                        break;
                    }
               case 60928:
                    {
                        // Depth data
                        if (data.Length < 4) return record;
                        record.depth = BitConverter.ToUInt32(data, 0) * 0.01f;
                        //var depthRaw = BytesToInt(data, 0, 4);  // Depth in 1/10th meters
                        //double depth = depthRaw * 0.1f;             // Convert to meters

                        // result = emitDEP(depth);
                        break;
                    }

                case 65288:
                    {
                        // Battery Voltage data
                        if (data.Length < 2) return record;
                        record.voltage = BitConverter.ToUInt16(data, 1) * 0.1f;
                        //int voltageRaw = BytesToInt(data, 1, 2);  // Voltage in 1/10th volts
                        //double voltage = voltageRaw * 0.1f;           // Convert to volts

                        // result = emitBAT(voltage);
                        break;
                    }

                case 65359:
                    {
                        // Speed Through Water
                        if (data.Length < 4) return record;
                        var a = BitConverter.ToUInt16(data, 1) * .01;
                        var b = BitConverter.ToUInt16(data, 2) * .01;
                        var c = BitConverter.ToUInt16(data, 3) * .01;
                        var d = BitConverter.ToUInt16(data, 4) * .01;
                        var e = BitConverter.ToUInt16(data, 5) * .01;
                        var temp = BitConverter.ToUInt16(data, 3) * .01;
                        record.SOW = BitConverter.ToUInt16(data, 3) * 0.01f;
                        //int speedRaw = BytesToInt(data, 3, 2);  // Speed in 1/10th knots
                        //double speed = speedRaw * 0.1f;             // Convert to knots

                        // result = emitSTW(speed);
                        break;
                    }
                case 126720:
                    {
                        // Wind Information
                        if (data.Length < 6) return record;
                        var a = BitConverter.ToUInt16(data, 1) * .01;
                        var b = BitConverter.ToUInt16(data, 2) * .01;
                        var c = BitConverter.ToUInt16(data, 3) * .01;
                        var d = BitConverter.ToUInt16(data, 4) * .01;
                        var e = BitConverter.ToUInt16(data, 5) * .01;

                        /*This PGN is proprietary garbage that may or may not mix with another sentence                         * 
                         * record.windTrueSpeed =this.MetersPerSecondToKnots(BitConverter.ToUInt16(data, 3) * 0.1f);
                        record.windTrueDir = RadiansToDegrees(BitConverter.ToUInt16(data, 5) * 0.1f);*/
                        //int windSpeedRaw = BytesToInt(data, 3, 2);  // Wind speed in 1/10th m/s
                        //int windAngleRaw = BytesToInt(data, 5, 2);  // Wind angle in 1/10th degrees
                        //double windSpeed = windSpeedRaw * 0.1f;         // Convert to m/s
                        //double windAngle = windAngleRaw * 0.1f;         // Convert to degrees

                        // result = emitWND(windSpeed, windAngle);
                        break;
                    }

                case 126993:
                    {
                        if (data.Length < 4) return record;
                        //int time = BytesToInt(data, 0, 2);
                        int state = data[2];
                        int events = data[3];
                        // result = emitHBT(time, state, events);
                        break;
                    }


                case 127245:
                    {  // Rudder
                       // Field: position (signed) often at bytes 4..5 (per spec offset varies). We'll attempt bytes 4..5 signed 16 bit, resolution 0.0001 rad? but many docs say degrees * 0.1
                        if (data.Length < 6) return record;
                        //int posRaw = BytesToInt(data, 4, 2);
                        // Spec varies — many implementations use 0.0001 rad. We'll assume degrees * 0.0001 rad for safety:
                        //double posRad = posRaw * 1e-4f;
                        //double posDeg = RadiansToDegrees(posRad);
                        // result = emitRSA(posDeg);
                        break;
                    }

                case 127250:
                    {  // Vessel Heading: usually 2 bytes heading (1e-4 rad)
                        if (data.Length < 2) return record;


                        var a = BitConverter.ToUInt16(data, 1) * .01;
                        var b = BitConverter.ToUInt16(data, 2) * .01;
                        var c = BitConverter.ToUInt16(data, 3) * .01;
                        var d = BitConverter.ToUInt16(data, 4) * .01;

                        var headingBits = BitConverter.ToUInt16(data, 4);

                        var HeadingRadians = headingBits * 0.0001 * Math.PI / 180.0;
                        record.headingMag = c; // HeadingRadians * 180.0 / Math.PI;
                         
                        /*var hd_raw = (int)BytesToDouble(data, 4, 4);
                        
                        double hdRad = hd_raw * 1e-4f;
                        double hdDeg = RadiansToDegrees(hdRad);*/
                        // result = emitHDT(hdDeg);
                        break;
                    }

                case 127258:
                    {
                        if (data.Length < 6) return record;
                        //double variation = BytesToDouble(data, 2,4) * (180.0 / Math.PI);  // radians to degrees
                        // result = emitMAG(variation);
                        break;
                    }


                case 128259:
                    {  // Speed: water referenced (0.01 m/s)
                        if (data.Length < 4) return record;
                        // Field2: Speed water referenced at offset 1 (per many layouts) - but vendor variations exist
                        int speed_water_raw = BitConverter.ToUInt16(data, 1);
                        double speed_mps = speed_water_raw * 0.01f;
                        double speed_kn = MetersPerSecondToKnots(speed_mps);
                        record.SOW = speed_kn;
                        /*if(data.Length>4)
                        {
                            int rawdir = BitConverter.ToUInt16(data, 3);
                            record.headingTrue = rawdir * 0.0001;
                        }*/
                        // We don't always have heading here; emit VHW with blank heading (0.0)
                        // result = emitVHW(0.0f, speed_kn);
                        break;
                    }

                case 128267:
                    {  // Water depth
                        if (data.Length < 4) return record;
                        // Field 2 usually bytes 1..2 or 0..1 depending on device; try bytes 1..2 (transducer depth)
                        int depth_raw = BitConverter.ToUInt16(data, 1);
                        record.depth = depth_raw * 0.01f* 3.28084;
                        // Many docs show depth in meters with resolution 0.01 (0.01 m)
                        //double depth_m = depth_raw * 0.01f;
                        // result = emitDPT(depth_m, 0.0f);
                        break;
                    }

                case 128275:
                    {
                        if (data.Length < 8) return record;
                        //double log = BytesToDouble(data, 0, 4);
                        //double trip = BytesToDouble(data, 4, 4);
                        // result = emitLOG(log, trip);
                        break;
                    }


                case 129025:
                    {  // Position, Rapid Update (lat/lon - 1e-7 deg)
                        if (data.Length < 8) return record;
                        int lat = BitConverter.ToInt32(data, 0);
                        int lon = BitConverter.ToInt32(data, 4);
                        record.latitude = lat * 1e-7f;
                        record.longitude = lon * 1e-7f;
                        //                        double lonDeg = lon * 1e-7f;
                        // Emit GLL (basic) — user may prefer GPRMC when time available
                        // result = emitGLL(latDeg, lonDeg);
                        break;
                    }

                case 129029:
                    {  // GNSS Position Data (fast-packet sometimes) - best-effort
                       // This PGN can contain time/date and higher precision. We'll attempt to parse some common single-frame layout:
                       // Many devices: byte0=SID, byte1..4 = Latitude? The layout varies; safer to try: check if data.Length >=8 and parse lat/lon as pairs.
                        if (data.Length >= 8)
                        {
                            int lat = BitConverter.ToInt32(data, 0);
                            int lon = BitConverter.ToInt32(data, 4);
                            record.latitude = lat * 1e-7f;
                            record.longitude = lon * 1e-7f;
                            // Try same extraction as 129025 (some devices send lat/lon fragments)
                            //int lat = BytesToInt(data, 0, 4);
                            // int lon = BytesToInt(data, 4, 4);
                            //double latDeg = lat * 1e-7f;
                            //double lonDeg = lon * 1e-7f;
                            // We may not have UTC time fields here; emit GLL and leave RMC to 126992+129029 combined (not implemented fully)
                            // result = emitGLL(latDeg, lonDeg);
                        }
                        break;
                    }

                case 129026:
                    {  // COG & SOG Rapid Update
                        /*Field 1: Sequence ID identifies the sequence this data is associated with so that the data can be synchronized with other vessel data for this same sequence being sent in another PGN.
               Field 2: COG Reference--this field is used to indicate the direction reference of the course over ground. True North reference = 0.
               Field 3: Reserved (6 bits)
               Field 4: Course Over Ground--this field is used to indicate the course over ground (COG) in resolution of 1x10-4 radians.
               Field 5: Speed Over Ground--this field is used to indicate the speed over ground (SOG) in resolution of 1x10-2 meters/second.
               Field 6: Reserved for use by NMEA. (16 bits)*/
                        // chat GPT bullshit Common layout: byte0 = SID, byte1..4 = COG (uint32, 1e-4 rad), byte5..6 = SOG (uint16, 0.01 m/s)
                        if (data.Length < 10) return record;
                        int cog_raw = BitConverter.ToInt32(data, 9);  // 4 bytes
                        int sog_raw = BitConverter.ToUInt16(data, 5);
                        double cogRad = (double)cog_raw * 1e-4f;  // radians
                        double cogDeg = RadiansToDegrees(cogRad);
                        double sog_mps = sog_raw * 0.01f;
                        double sog_kn = MetersPerSecondToKnots(sog_mps);
                        record.COG = cogDeg;
                        record.SOG = sog_raw * 0.01f * 3.28084;
                        // result = emitVTG(cogDeg, sog_kn);
                        break;
                    }
                case 129283:
                    {
                        if (data.Length < 6) return record;
                        //double xte = BytesToDouble(data, 1, 4);
                        int direction = data[5];
                        // result = emitXTE(xte, direction);
                        break;
                    }
                case 129284:
                    {
                        // Fuel Management
                        if (data.Length < 6) return record;
                        //int fuelLevelRaw = BytesToInt(data, 3, 2);  // Fuel level in percentage
                        //int fuelRateRaw = BytesToInt(data, 5, 2);   // Fuel consumption rate

                        //double fuelLevel = fuelLevelRaw * 0.1f;  // Convert to percentage
                        //double fuelRate = fuelRateRaw * 0.1f;    // Convert to liters per hour

                        // result = emitFUEL(fuelLevel, fuelRate);
                        break;
                    }

                case 129539:
                    {
                        // Heading
                        if (data.Length < 4) return record;
                        var a= BitConverter.ToUInt16(data, 1) *.01;
                        var b= BitConverter.ToUInt16(data, 2) * .01;
                        var c= BitConverter.ToUInt16(data, 3) * .01;
                        var d= BitConverter.ToUInt16(data, 4) * .01;
                        int headingRaw = BitConverter.ToUInt16(data, 1);  // Heading in 1/10th degrees
                        
                        
                        var A= RadiansToDegrees(a);
                        var B= RadiansToDegrees(b);
                        var C= RadiansToDegrees(c);
                        var D= RadiansToDegrees(d);
                        record.headingTrue = c;

                        /*record.headingTrue = headingRaw * 0.01f;
                        record.headingMag = headingRaw * 0.01f;  */

                        //double heading = headingRaw * 0.1f;           // Convert to degrees

                        // result = emitHDG(heading);
                        break;
                    }

                case 129540:
                    {
                        if (data.Length < 1) return record;
                        int count = data[0];  // Number of satellite info blocks
                        // result = emitSAT(count);
                        break;
                    }

                case 130306:
                    {  // Wind data (apparent angle + speed)
                       // Common layout (single frame): SID (1), Wind Speed (2 or 1), Wind Angle (2), Reference etc.
                        if (data.Length < 5) return record;

                        ushort speedRaw = BitConverter.ToUInt16(data, 1);
                        double windSpeedMps = speedRaw * 0.01;
                        record.windAppSpeed = MetersPerSecondToKnots(windSpeedMps);
                        ushort dirRaw = BitConverter.ToUInt16(data, 3);
                        double windDirectionRad = dirRaw * 0.0001;
                        record.windAppDir = RadiansToDegrees(windDirectionRad);
                        char ma = (char)data[5];
                        // Many sensors: bytes1..2 = wind speed (0.1 m/s) or (0.01), bytes3..4 = angle (0.0001 rad)
                        // We'll attempt: speed = bytes1..2 * 0.1 m/s ; angle_raw = bytes3..4 * 0.0001 rad
                        //int sp_raw = (int)BytesToInt(data, 1, 2);
                        //int ang_raw = (int)BytesToInt(data, 3, 2);
                        /*double wind_mps = sp_raw * 0.1f;  // assumption: 0.1 m/s scaling (common)
                        double wind_kn = MetersPerSecondToKnots(wind_mps);
                        double angleRad = ang_raw * 1e-4f;
                        double angleDeg = RadiansToDegrees(angleRad);*/
                        // 130306 commonly reports apparent wind angle — mark as relative
                        // result = emitMWV(angleDeg, true, wind_mps);
                        break;
                    }
                case 130310:
                    {
                        // Temperature
                        if (data.Length < 3) return record;
                        ushort rawWaterTemp = BitConverter.ToUInt16(data, 1);
                        
                        
                        double WaterTemperatureKelvin = rawWaterTemp * 0.01;
                        record.waterTemp= (WaterTemperatureKelvin - 273.15) * 9 / 5 + 32;
                        //int tempRaw = (int)BytesToInt(data, 2, 2);  // Temperature in 1/100th Celsius
                        //double temperature = tempRaw * 0.01f;              // Convert to Celsius

                        // result = emitTEMP(temperature);
                        break;
                    }

                case 130312:
                    {
                        if (data.Length < 6) return record;
                        //int rawTemp = BytesToInt(data, 4, 2);
                        //double tempC = (rawTemp * 0.01) - 273.15;  // Kelvin to Celsius
                        // result = emitTMP(tempC);
                        break;
                    }

                case 130822:
                    {
                        // Wind Speed and Angle
                        if (data.Length < 6) return record;
                        
                        var a = BitConverter.ToUInt16(data, 1) * .01;
                        var b = BitConverter.ToUInt16(data, 2) * .01;
                        var c = BitConverter.ToUInt16(data, 3) * .01;
                        var d = BitConverter.ToUInt16(data, 4) * .01;
                        var e = BitConverter.ToUInt16(data, 5) * .01;
                        //int windSpeedRaw = BytesToInt(data, 3, 2);  // Wind speed in 1/10th m/s
                        //int windAngleRaw = BytesToInt(data, 5, 2);  // Wind angle in 1/10th degrees
                        //double windSpeed = windSpeedRaw * 0.1f;         // Convert to m/s
                        //double windAngle = windAngleRaw * 0.1f;         // Convert to degrees

                        // result = emitWNDSPD(windSpeed, windAngle);
                        break;
                    }

                    default:
                    {
                        // Unhandled PGN
                        return record;
                        break;
                    }
            }

            return record;
        }

        

        public double RadiansToDegrees(double radians)
        {
            return radians * (180.0 / Math.PI);
        }
        
        public double MetersPerSecondToKnots(double mps)
        {
            return mps * 1.94384;
        }
    }
}

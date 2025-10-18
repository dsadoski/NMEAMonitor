using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NMEAMon.Services
{
    public class StringParser
    {

        public void stringParser()
        {
        }


        public string[] TildaListToStrings(string txt)
        {
            int i;
            string tmp = "";
            int CNT = 0;
            string[] ARY;

            if (txt.Length == 1 && txt[0] == ' ') return new string[0];

            for (i = 0; i < txt.Length; i++)
            {
                if (txt[i] == '~')
                {
                    tmp = "";
                    CNT++;
                }
                else tmp += txt[i];
            }
            if (tmp.Length > 0) CNT++;

            ARY = new string[CNT];
            tmp = "";
            CNT = 0;

            for (i = 0; i < txt.Length; i++)
            {
                if (txt[i] == '~')
                {
                    ARY[CNT] = tmp;
                    tmp = "";
                    CNT++;
                }
                else tmp += txt[i];
            }
            if (tmp.Length > 0 && CNT < ARY.Length)
            {
                ARY[CNT] = tmp;


            }


            return ARY;


        }


        public string[] CommaListToString(string txt)
        {
            int i;
            string tmp = "";
            int CNT = 0;
            string[] ARY;

            if (txt.Length == 1 && txt[0] == ' ') return new string[0];

            for (i = 0; i < txt.Length; i++)
            {
                if (txt[i] == ',')
                {
                    tmp = "";
                    CNT++;
                }
                else tmp += txt[i];
            }
            if (tmp.Length > 0) CNT++;

            ARY = new string[CNT];
            tmp = "";
            CNT = 0;

            for (i = 0; i < txt.Length; i++)
            {
                if (txt[i] == ',')
                {
                    ARY[CNT] = tmp;
                    tmp = "";
                    CNT++;
                }
                else tmp += txt[i];
            }
            if (tmp.Length > 0 && CNT < ARY.Length)
            {
                ARY[CNT] = tmp;


            }


            return ARY;


        }


        public double[] TildaListToDoubles(string txt)
        {
            int i;
            string tmp = "";
            int CNT = 0;
            double[] ARY;

            if (txt.Length == 1 && txt[0] == ' ') return new double[0];

            for (i = 0; i < txt.Length; i++)
            {
                if (txt[i] == '~')
                {
                    tmp = "";
                    CNT++;
                }
                else tmp += txt[i];
            }
            if (tmp.Length > 0) CNT++;

            ARY = new double[CNT];
            tmp = "";
            CNT = 0;

            for (i = 0; i < txt.Length; i++)
            {
                if (txt[i] == '~')
                {
                    ARY[CNT] =  double.Parse(tmp);
                    tmp = "";
                    CNT++;
                }
                else tmp += txt[i];
            }
            if (tmp.Length > 0 && CNT < ARY.Length)
            {
                ARY[CNT] = double.Parse(tmp);
                tmp = "";
                CNT++;
            }
            return ARY;


        }

        public int[] TildaListToInts(string txt)
        {
            int i;
            string tmp = "";
            int CNT = 0;
            int[] ARY;

            if (txt.Length == 1 && txt[0] == ' ') return new int[0];

            for (i = 0; i < txt.Length; i++)
            {
                if (txt[i] == '~')
                {
                    tmp = "";
                    CNT++;
                }
                else tmp += txt[i];
            }
            if (tmp.Length > 0) CNT++;

            ARY = new int[CNT];
            tmp = "";
            CNT = 0;

            for (i = 0; i < txt.Length; i++)
            {
                if (txt[i] == '~')
                {
                    ARY[CNT] = Int32.Parse(tmp);


                    tmp = "";
                    CNT++;
                }
                else tmp += txt[i];
            }
            if (tmp.Length > 0 && CNT < ARY.Length)
            {
                ARY[CNT] = Int32.Parse(tmp);
                tmp = "";
                CNT++;
            }


            return ARY;


        }


        public long[] TildaListToLongs(string txt)
        {
            int i;
            string tmp = "";
            int CNT = 0;
            long[] ARY;

            if (txt.Length == 1 && txt[0] == ' ') return new long[0];

            for (i = 0; i < txt.Length; i++)
            {
                if (txt[i] == '~')
                {
                    tmp = "";
                    CNT++;
                }
                else tmp += txt[i];
            }
            if (tmp.Length > 0) CNT++;

            ARY = new long[CNT];
            tmp = "";
            CNT = 0;

            for (i = 0; i < txt.Length; i++)
            {
                if (txt[i] == '~')
                {
                    ARY[CNT] = long.Parse(tmp);
                    tmp = "";
                    CNT++;
                }
                else tmp += txt[i];
            }
            if (tmp.Length   > 0 && CNT < ARY.Length)
            {
                ARY[CNT] = long.Parse(tmp);
                tmp = "";
                CNT++;
            }


            return ARY;


        }


    }



}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapSheetCode
{
    class Algo
    {
        //主算法实现
        public List<Point> AlgoPoints = new List<Point>();
        public string AlgoSacle; //选择的比例尺信息

        public Algo(List<Point> points,string scale)
        {
            AlgoPoints = points;
            AlgoSacle = scale;
        }

        public void Go()
        {
            Step1(); //第一步，计算行列号
            Step2(); //第二步，计算对应点的图幅编码
        }

        public void Step1()
        {
            //第一步，计算行列号
            foreach(var p in AlgoPoints)
            {
                var Row = (int)(p.B / 4.0) + 1;
                var Col = (int)(p.L / 6.0) + 31;
                p.Row = Row;
                p.Col = Col;
                p.letter_100K = GetRowLetter(Row); //获取行号的字母
            }
        }


        public void Step2()
        {
            //第二步，计算对应点的图幅编码
            foreach(var p in AlgoPoints)
            {
                string AlgoLetterCode;
                double AlgoDetaB;
                double AlgoDetaL;
                GetLetterAndDeta(out AlgoLetterCode, out AlgoDetaB, out AlgoDetaL);
                var resultRol = (int)(4 / AlgoDetaB) - (int)((p.B % 4.0) / AlgoDetaB);
                var resultCol = (int)((p.L % 6) / AlgoDetaL) + 1;
                var resultString = $"{p.letter_100K}{p.Col:D2}{AlgoLetterCode}{resultRol:D3}{resultCol:D3}";
                p.newMapCode = resultString;
            }
        }

        public void GetLetterAndDeta(out string LetterCode,out double DetaB,out double DetaL)
        {
            LetterCode = "";
            DetaB = 0;
            DetaL = 0;

            //获取对应比例尺下的字母编号，信息
            if(AlgoSacle == "1：100万")
            {
                DetaL = 6.0;
                DetaB = 4.0;
                LetterCode = "A";
            }
            else if (AlgoSacle == "1：50万")
            {
                DetaL = 3.0;
                DetaB = 2.0;
                LetterCode = "B";
            }
            else if (AlgoSacle == "1：25万")
            {
                DetaL = 1.5;
                DetaB = 1;
                LetterCode = "C";
            }
            else if (AlgoSacle == "1：10万")
            {
                DetaL = 0.5;
                DetaB = 1.0/3.0;
                LetterCode = "D";
            }
            else if (AlgoSacle == "1：5万")
            {
                DetaL = 1.0/4.0;
                DetaB = 1.0/6.0;
                LetterCode = "E";
            }
            else if (AlgoSacle == "1：2.5万")
            {
                DetaL = 1.0/8.0;
                DetaB = 1.0/12.0;
                LetterCode = "F";
            }
            else if (AlgoSacle == "1：1万")
            {
                DetaL = 1.0/16.0;
                DetaB = 1.0/24.0;
                LetterCode = "G";
            }
            else if (AlgoSacle == "1：5000")
            {
                DetaL = 1.0/32.0;
                DetaB = 1.0/48.0;
                LetterCode = "H";
            }
        }
        // 获取行号字母
        private string GetRowLetter(int rowNumber)
        {

            if (rowNumber >= 1 && rowNumber <= 22)
            {
                return ((char)('A' + rowNumber - 1)).ToString();
            }
            return "";
        }

    }
}

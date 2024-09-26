using System;
using System.Text.RegularExpressions;

namespace lono
{
    class satellite
    {
        public string name;
        public double X;
        public double Y;
        public double Z;
        public double sate_x;
        public double sate_y;
        public double sate_z;
        public Time satellitetime;
        public double Bp = 30.0 / 180.0 * Math.PI;
        public double Lp = 114.0 / 180 * Math.PI;
        public double A;//高度角
        public double E;//方位角
        public double AIP;//地磁维度
        public double BIP;//地磁维度
        public double CIP;//地磁维度
        public double conIP;//地磁维度

        //以下是计算电离层延迟量的变量
        public double[] alist = {0.1397 * Math.Pow(10, -7), -0.7451 * Math.Pow(10, -8), -0.5960 * Math.Pow(10, -7), 0.1192 * Math.Pow(10, -6) };
        public double[] blist = { 0.1270 * Math.Pow(10, 6), -0.1966 * Math.Pow(10, 6), 0.6554 * Math.Pow(10, 5), 0.2621 * Math.Pow(10, 6), };
        public double A1 = (5 * Math.Pow(10, -9));
        public double A3 = 50400;
        public double A2, A4;
        public double t;
        public double F;
        public double Tion;
        public double Dion;
        public double C = 299792458;

        public satellite(Time time,string linedata)
        {
            satellitetime = time;
            var buf = Regex.Split(linedata, @"\s+");//正则表达式的内容；
            name = buf[0];
            sate_x = Convert.ToDouble(buf[1]) * 1000;
            sate_y = Convert.ToDouble(buf[2]) * 1000;
            sate_z = Convert.ToDouble(buf[3]) * 1000;
            var XYZ = CalXYZ();
            X = XYZ[0,0];
            Y = XYZ[1,0];
            Z = XYZ[2,0];
            if(Y > 0)
            {
                A = (Math.Atan(Y / X) * 180.0 / Math.PI);
                if (X > 0)
                {

                }
                else
                {
                    A = 180 + A;
                }
            }
            else
            {
                A = (Math.Atan(Y / X) * 180.0 / Math.PI);
                if (X > 0)
                {
                    A = 360 + A;
                }
                else
                {
                    A = 180 + A;
                } 
            }
            E = (Math.Atan(Z / (Math.Sqrt(X * X + (Y * Y)))) * 180 /Math.PI);
            calIP();//计算IP的地磁维度

            GotoAlgo();//计算卫星的电离层延迟量
        }
        public double[,] CalXYZ()
        {
            //先搞定矩阵相乘，这里采用纯手写，先锻炼思维
            double[,] left = { { -Math.Sin(Bp) * Math.Cos(Lp) , -Math.Sin(Bp) * Math.Sin(Lp) , Math.Cos(Bp) }, 
                               { -Math.Sin(Lp) , Math.Cos(Lp) , 0}, 
                               {Math.Cos(Bp) * Math.Cos(Lp) , Math.Cos(Bp) * Math.Sin(Lp) , Math.Sin(Bp)} };
            double[,] right = { { sate_x + 2225669.7744}, { sate_y - 4998936.1598 }, { sate_z - 3265908.9678 } };
            var XYZ = CalleftXright(left,right);//矩阵相乘
            return XYZ;
        }

        public double[,] CalleftXright(double[,] left, double[,] right)//矩阵相乘的函数
        {
            int rowLeft = left.GetLength(0);//左矩阵的行数
            int colLeft = left.GetLength(1);//左矩阵的列数
            int rowRight = right.GetLength(0);//右矩阵的行数
            int colRight = right.GetLength(1);//右矩阵的列数
            double[,] result = new double[rowLeft,colRight];
            if(colLeft == rowRight)
            {
                for(int i = 0;i < rowLeft;i++)
                {
                    
                    for(int j = 0;j < colRight;j++)
                    {
                        double res = 0;
                        for (int k = 0; k < rowRight;k++)
                        {
                            res += left[i, k] * right[k, j];
                        }
                        result[i, j] = res;
                    }

                }
                
            }
            else
            {
                Console.WriteLine("左矩阵行数不等于右矩阵的列数！");
            }
            return result;
        }

        public void calIP()
        {
            conIP = (0.0137 / (E / 360.0 * 2 * Math.PI + 0.11) - 0.022);
            AIP = (Bp + conIP * Math.Cos(A / 360 * 2 * Math.PI));
            BIP = Lp + conIP * Math.Sin(A / 360 * 2 * Math.PI) / Math.Cos(AIP);
            CIP = AIP + (0.064 * Math.Cos(BIP - 1.617));
        }

        public void GotoAlgo()
        {
            A2 = alist[0] + alist[1] * CIP + alist[2] * CIP * CIP + alist[3] * CIP * CIP * CIP;
            A4 = blist[0] + blist[1] * CIP + blist[2] * CIP * CIP + blist[3] * CIP * CIP * CIP;
            t = 43200 * BIP + satellitetime.second_of_day;
            F = 1 + 16 * (0.53 - E / 180 * Math.PI) * (0.53 - E / 180 * Math.PI) * (0.53 - E / 180 * Math.PI);

            double temp = (2 * Math.PI * (t - A3) / A4);
            if(Math.Abs(temp) < 1.57)
            {
                Tion = F * (A1 + A2 * Math.Cos(temp));
            }
            else
            {
                Tion = F * A1;
            }
            Dion = Tion * C;
            double eps = Math.Pow(10,-5);
            if(E < eps)
            {
                Tion = 0;
                Dion = 0;
            }

        }
    }
}
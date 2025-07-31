using System;
using System.Collections.Generic;

namespace SpaceResection
{
    class Algo
    {
        //主算法
        public double x0, y0, m, f;
        public double Xs0 = 0, Ys0 = 0, Zs0 = 0, fai, omega, kapa; // 外方位元素初始值
        public double a1, a2, a3, b1, b2, b3, c1, c2, c3;//旋转矩阵的9个元素
        public List<Point> Points = new List<Point>();
        #region 确定初始值
        public void GetOrigin(List<Point> points, double m, double f, double x0, double y0)
        {
            this.x0 = x0;
            this.y0 = y0;
            this.m = m;
            this.f = f;
            Points = points.ConvertAll(p => p.Clone()); // 深拷贝

            points.ForEach(item =>
            {
                Xs0 += item.X;
                Ys0 += item.Y;
            });
            Xs0 /= points.Count;
            Ys0 /= points.Count;
            Zs0 = m * f;
            fai = omega = kapa = 0;
        }
        #endregion

        #region 计算旋转矩阵R
        public void CalR()
        {
            a1 = Math.Cos(fai) * Math.Cos(kapa) - Math.Sin(fai) * Math.Sin(omega) * Math.Sin(kapa);
            a2 = -Math.Cos(fai) * Math.Sin(kapa) - Math.Sin(fai) * Math.Sin(omega) * Math.Cos(kapa);
            a3 = -Math.Sin(fai) * Math.Cos(omega);
            b1 = Math.Cos(omega) * Math.Sin(kapa);
            b2 = Math.Cos(omega) * Math.Cos(kapa);
            b3 = -Math.Sin(omega);
            c1 = Math.Sin(fai) * Math.Cos(kapa) + Math.Cos(fai) * Math.Sin(omega) * Math.Sin(kapa);
            c2 = -Math.Sin(fai) * Math.Sin(kapa) + Math.Cos(fai) * Math.Sin(omega) * Math.Cos(kapa);
            c3 = Math.Cos(fai) * Math.Cos(omega);
        }
        #endregion

        #region 逐点计算像点坐标值
        public void Calxy()
        {
            Points.ForEach(item =>
            {
                item.x_new = x0 - f * (a1 * (item.X - Xs0) + b1 * (item.Y - Ys0) + c1 * (item.Z - Zs0)) / (a3 * (item.X - Xs0) + b3 * (item.Y - Ys0) + c3 * (item.Z - Zs0));
                item.y_new = y0 - f * (a2 * (item.X - Xs0) + b2 * (item.Y - Ys0) + c2 * (item.Z - Zs0)) / (a3 * (item.X - Xs0) + b3 * (item.Y - Ys0) + c3 * (item.Z - Zs0));
            });
        }
        #endregion

        #region 计算A矩阵里的12个参数
        public void CalA()
        {
            Points.ForEach(item =>
            {
                double H = -(item.Z - Zs0);
                item.a11 = -f / H;
                item.a12 = 0.0;
                item.a13 = -item.x_new / H;
                item.a21 = 0.0;
                item.a22 = -f / H;
                item.a23 = -item.y_new / H;
                item.a14 = -f * (1.0 + (item.x_new * item.x_new) / (f * f));
                item.a15 = -item.x_new * item.y_new / f;
                item.a16 = item.y_new;
                item.a24 = -item.x_new * item.y_new / f;
                item.a25 = -f * (1.0 + (item.y_new * item.y_new) / (f * f));
                item.a26 = -item.x_new;

                item.tLX = item.x - item.x_new;
                item.tLY = item.y - item.y_new;
            });
        }
        #endregion

        #region 循环迭代
        public Result Go()
        {
            Result result = new Result(); // 结果数据保存
            int maxIterations = 50; // 增加最大迭代次数
            int iteration = 0;

            while (iteration < maxIterations)
            {
                iteration++;

                CalR();
                Calxy();
                CalA();

                // 矩阵A
                Matrix MatrixA = new Matrix(Points.Count * 2, 6);
                //矩阵L
                Matrix MatrixL = new Matrix(Points.Count * 2, 1);
                int MatrixAtemp = 0; //临时用的计数器，因为Points.ForEach无法得到循环到了第几次i。
                Points.ForEach(item =>
                {
                    MatrixA[MatrixAtemp, 0] = item.a11;
                    MatrixA[MatrixAtemp, 1] = item.a12;
                    MatrixA[MatrixAtemp, 2] = item.a13;
                    MatrixA[MatrixAtemp, 3] = item.a14;
                    MatrixA[MatrixAtemp, 4] = item.a15;
                    MatrixA[MatrixAtemp, 5] = item.a16;

                    MatrixA[MatrixAtemp + 1, 0] = item.a21;
                    MatrixA[MatrixAtemp + 1, 1] = item.a22;
                    MatrixA[MatrixAtemp + 1, 2] = item.a23;
                    MatrixA[MatrixAtemp + 1, 3] = item.a24;
                    MatrixA[MatrixAtemp + 1, 4] = item.a25;
                    MatrixA[MatrixAtemp + 1, 5] = item.a26;

                    MatrixL[MatrixAtemp, 0] = item.tLX;
                    MatrixL[MatrixAtemp + 1, 0] = item.tLY;

                    MatrixAtemp += 2;
                });

                // 解算
                // MatrixX = (MatrixA转置 * MatrixA)的逆 * MatrixA转置 * MatrixL
                Matrix MatrixAT = MatrixA.Transpose();
                Matrix MatrixATA = Matrix.Multiply(MatrixAT, MatrixA);
                Matrix MatrixATAInverse = MatrixATA.Inverse();
                Matrix MatrixATMultiplyL = Matrix.Multiply(MatrixAT, MatrixL);
                Matrix MatrixX = Matrix.Multiply(MatrixATAInverse, MatrixATMultiplyL);

                // 得到改正值
                double dXs = MatrixX[0, 0];
                double dYs = MatrixX[1, 0];
                double dZs = MatrixX[2, 0];
                double detafai = MatrixX[3, 0];
                double detaomega = MatrixX[4, 0];
                double detakapa = MatrixX[5, 0];

                double flag = 0.1 / 60.0 * (Math.PI / 180.0); // 0.1分等于多少弧度

                // 使用绝对值比较
                if (Math.Abs(detafai) < flag && Math.Abs(detaomega) < flag && Math.Abs(detakapa) < flag)
                {
                    // 如果小于限差

                    // 将结果保存到类里
                    result.XS0_new = Xs0;
                    result.YS0_new = Ys0;
                    result.ZS0_new = Zs0;
                    result.a1_new = a1;
                    result.a2_new = a2;
                    result.a3_new = a3;
                    result.b1_new = b1;
                    result.b2_new = b2;
                    result.b3_new = b3;
                    result.c1_new = c1;
                    result.c2_new = c2;
                    result.c3_new = c3;
                    return result;
                }
                else
                {
                    //否则新值加上改正值
                    Xs0 += dXs;
                    Ys0 += dYs;
                    Zs0 += dZs;
                    fai = Math.Round(fai + detafai, 10); // 保留10位小数防止累积误差
                    omega = Math.Round(omega + detaomega, 10);
                    kapa = Math.Round(kapa + detakapa, 10);

                }

            }

            if (iteration >= maxIterations)
            {
                Console.WriteLine("达到最大迭代次数");
                return result;
            }
            return result;
        }
        #endregion
    }
    class Result
    {
        //保存结果
        public double XS0_new = 0, YS0_new = 0, ZS0_new = 0, a1_new = 0, a2_new = 0, a3_new = 0, b1_new = 0, b2_new = 0, b3_new = 0, c1_new = 0, c2_new = 0, c3_new = 0;
    }
    //矩阵运算
    class Matrix
    {
        private readonly double[,] data;
        public int Rows { get; }
        public int Cols { get; }

        public Matrix(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            data = new double[rows, cols];
        }

        public double this[int row, int col]
        {
            get => data[row, col]; // 获取元素值
            set => data[row, col] = value; // 设置元素值
        }

        //计算两个矩阵的乘积。
        public static Matrix Multiply(Matrix a, Matrix b)
        {
            if (a.Cols != b.Rows)
                throw new Exception("矩阵维度不匹配！");
            Matrix result = new Matrix(a.Rows, b.Cols); // 结果矩阵
            // 矩阵乘法运算
            for (int i = 0; i < result.Rows; i++)
                for (int j = 0; j < result.Cols; j++)
                    for (int k = 0; k < a.Cols; k++)
                        result[i, j] += a[i, k] * b[k, j];
            return result;
        }


        // 计算矩阵的转置。
        public Matrix Transpose()
        {
            Matrix result = new Matrix(Cols, Rows);
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Cols; j++)
                    result[j, i] = data[i, j];
            return result;
        }

        //逆矩阵。
        public Matrix Inverse()
        {
            if (Rows != Cols) throw new InvalidOperationException("不是方阵！");
            const double epsilon = 1e-12;

            int n = Rows;
            Matrix aug = new Matrix(n, 2 * n);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    aug[i, j] = data[i, j];
                aug[i, i + n] = 1;
            }

            for (int i = 0; i < n; i++)
            {
                int maxRow = i;
                for (int k = i + 1; k < n; k++)
                    if (Math.Abs(aug[k, i]) > Math.Abs(aug[maxRow, i]))
                        maxRow = k;
                if (maxRow != i)
                {
                    for (int j = 0; j < 2 * n; j++)
                    {
                        double temp = aug[i, j];
                        aug[i, j] = aug[maxRow, j];
                        aug[maxRow, j] = temp;
                    }
                }
                double div = aug[i, i];
                if (Math.Abs(div) < epsilon)
                    throw new Exception("不可逆");
                for (int j = 0; j < 2 * n; j++)
                    aug[i, j] /= div;
                for (int k = 0; k < n; k++)
                {
                    if (k == i) continue;
                    double factor = aug[k, i];
                    if (Math.Abs(factor) < epsilon) continue;
                    for (int j = 0; j < 2 * n; j++)
                        aug[k, j] -= factor * aug[i, j];
                }
            }
            Matrix inv = new Matrix(n, n);
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    inv[i, j] = aug[i, j + n];
            return inv;
        }
    }
}

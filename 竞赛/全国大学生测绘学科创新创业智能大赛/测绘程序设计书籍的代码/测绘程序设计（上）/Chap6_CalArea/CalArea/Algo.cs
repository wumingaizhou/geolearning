using System;
using System.Collections.Generic;

namespace CalArea
{
    class Algo
    {
        public List<Triangle> triangles = new List<Triangle>();
        public String Run(List<Point> pts)
        {
            Point p1 = pts[0];
            Point p2 = pts[1];
            Point p3 = pts[pts.Count - 1];
            var triorigin = new Triangle(p1,p2,p3);
            triangles.Add(triorigin);
            for (int i = 1;i < pts.Count - 2;i++)
            {
                if(i % 2 == 1)
                {
                    p1 = pts[(i + 3) / 2 - 1];
                    p2 = pts[pts.Count + 2 - (i + 3) / 2 - 1];
                    p3 = pts[(i + 5) / 2 - 1];
                    var tri3 = new Triangle(p1,p2,p3);
                    triangles.Add(tri3);
                }
                if(i % 2 == 0)
                {
                    p1 = pts[pts.Count + 3 - (i + 4) / 2 - 1];
                    p2 = pts[(i + 4) / 2 - 1];
                    p3 = pts[pts.Count - i / 2 - 1];
                    var tri3 = new Triangle(p1, p2, p3);
                    triangles.Add(tri3);
                }
            }
            var res = Tores(triangles);
            return res;
        }
        public String Tores(List<Triangle> triangles)
        {
            string res = $"三角形三边：    面积：\n";
            double total = 0;
            foreach (Triangle d in triangles)
            {
                res += $"{d.A.PointName}{d.B.PointName}{d.C.PointName}   {d.S}\n";
                total += d.S;
            }
            res += "总面积：" +  Convert.ToString(total);
            return res;
        }
    }
}
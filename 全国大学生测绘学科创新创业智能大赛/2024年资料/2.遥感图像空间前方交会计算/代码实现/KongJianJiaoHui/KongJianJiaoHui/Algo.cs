using System;
using System.Collections.Generic;
namespace KongJianJiaoHui
{
    class Algo
    {
        #region 计算空间辅助坐标
        public static void CalStep1(ref Session session1,ref Session session2)
        {
            List<Session> sessions = new List<Session>();
            sessions.Add(session1);
            sessions.Add(session2);
            foreach(Session session in sessions)
            {
                session.u = (Math.Cos(session.fai) * Math.Cos(session.k) - Math.Sin(session.fai) * Math.Sin(session.omega) * Math.Sin(session.k)) * session.x;
                session.u += (-Math.Cos(session.fai) * Math.Sin(session.k) - Math.Sin(session.fai) * Math.Sin(session.omega) * Math.Cos(session.k)) * session.y;
                session.u += (-Math.Sin(session.fai) * Math.Cos(session.omega)) * (-session.f);

                session.v = (Math.Cos(session.omega) * Math.Sin(session.k)) * session.x;
                session.v += (Math.Cos(session.omega) * Math.Cos(session.k)) * session.y;
                session.v += (-Math.Sin(session.omega)) * (-session.f);

                session.w = (Math.Sin(session.fai) * Math.Cos(session.k) + Math.Cos(session.fai) * Math.Sin(session.omega) * Math.Sin(session.k)) * session.x;
                session.w += (-Math.Sin(session.fai) * Math.Sin(session.k) + Math.Cos(session.fai) * Math.Sin(session.omega) * Math.Cos(session.k)) * session.y;
                session.w += (Math.Cos(session.fai) * Math.Cos(session.omega)) * (-session.f);
            }
        }
        #endregion

        #region 计算投影系数以及坐标
        public static void CalStep2and3(ref Session session1,ref Session session2,ref Output Result)
        {
            double Bu = session2.Xs - session1.Xs;
            double Bv = session2.Ys - session1.Ys;
            double Bw = session2.Zs - session1.Zs;
            double N1 = (Bu * session2.w - Bw * session2.u) / (session1.u * session2.w - session2.u * session1.w);
            double N2 = (Bu * session1.w - Bw * session1.u) / (session1.u * session2.w - session2.u * session1.w);
            double X = session1.Xs + N1 * session1.u;
            double Y = 0.5 * (session1.Ys + N1 * session1.v + session2.Ys + N2 * session2.v);
            double Z = session1.Zs + N1 * session1.w;
            Result.N1 = N1;
            Result.N2 = N2;
            Result.X = X;
            Result.Y = Y;
            Result.Z = Z;
        }
        #endregion
    }
}
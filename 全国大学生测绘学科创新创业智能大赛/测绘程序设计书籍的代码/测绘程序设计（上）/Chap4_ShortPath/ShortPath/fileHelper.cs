using System;
using System.IO;
using System.Collections.Generic;

namespace ShortPath
{
    class fileHelper
    {
        public static SessionList Read(string pathname)
        {
            var reader = new StreamReader(pathname);
            var sessions = new List<Session>();
            while(!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if(line.Length > 0)
                {
                    var session = new Session();
                    session.Parse(line);
                    sessions.Add(session);
                }
            }
            reader.Close();
            SessionList sessionlist = new SessionList(sessions);
            return sessionlist;
        }
        public override string ToString()
        {
            return base.ToString();
        }

    }
}
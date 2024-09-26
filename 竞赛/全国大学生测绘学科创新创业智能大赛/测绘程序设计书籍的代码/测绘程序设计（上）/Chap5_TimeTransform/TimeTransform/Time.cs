using System;
namespace TimeTransform
{
    class Time
    {
        public int year;
        public int month;
        public int day;
        public int hour;
        public int minute;
        public double second;
        public double jd;//儒略日
        public int accday;//年积日
        public string Fish;
        public int[] loopyear = new int[12] { 1, 32, 61, 92, 122, 153, 183, 214, 245, 275, 306, 336 };//闰年
        public int[] noloopyear = new int[12] { 1, 32, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335 };//平年
        public void Parse(string line)
        {
            if(line.Length > 0)
            {
                var buf = line.Split(' ');
                year = Convert.ToInt32(buf[0]);
                month = Convert.ToInt32(buf[1]);
                day = Convert.ToInt32(buf[2]);
                hour = Convert.ToInt32(buf[3]);
                minute = Convert.ToInt32(buf[4]);
                second = Convert.ToDouble(buf[5]);
            }
            jd = 1721013.5 + 367 * year;
            jd -= Math.Floor((7 * (year + Math.Floor((month + 9) / 12.0))) / 4.0);
            jd += Math.Floor((275 * month) / 9.0);
            jd += day + (hour + (minute + second / 60.0) / 60.0) / 24.0;
            getAcc();//获得年积日;
            getFish();//打鱼还是晒网;
        }
        public void getAcc()
        {
            if(year % 4 == 0 && year % 100 != 0 || year % 400 == 0)//闰年
            {
                accday = loopyear[month - 1];
                accday += day - 1;
            }
            else
            {
                accday = noloopyear[month - 1];
                accday += day - 1;
            }

        }
        public void getFish()
        {
            Fish = $"{year}  {month}  {day}：";
            if(accday % 5 >= 1 && accday % 5 <= 3)
            {
                Fish += "打鱼\n";
            }
            else
            {
                Fish += "晒网\n";
            }
        }
        public override string ToString()//获得公历
        {
            return $"{year}  {month}  {day}  {hour}  {minute}  {second}\n";
        }

    }
}
#pragma once
#include<iostream>
#include<cmath>
#include<initializer_list>
#include<vector>
#include"date.h"

#define DAY_SECOND 86400
#define ROUND_SECOND 604800
#define UG_GAP 315960000

using namespace std;
typedef long long time_t;

struct commontime
{
    commontime()
    {}

    commontime(vector<unsigned> data, double second)
        :_year(data[0]),
        _month(data[1]),
        _day(data[2]),
        _hour(data[3]),
        _minute(data[4]),
        _second(second)
    {}

    friend ostream& operator<<(ostream& out, const commontime& com);

    friend istream& operator>>(istream& in, commontime& com);

    unsigned _year;
    unsigned _month;
    unsigned _day;
    unsigned _hour;
    unsigned _minute;
    double _second;
};

ostream& operator<<(ostream& out, const commontime& com)
{
    out << com._year << "-" << com._month << "-" << com._day << " "
        << com._hour << ":" << com._minute << ":" << com._second << endl;
    return out;
}

istream& operator>>(istream& in, commontime& com)
{
    in >> com._year >> com._month >> com._day >> com._hour >> com._minute >> com._second;
    return in;
}

struct GPStime
{
    GPStime()
    {}

    GPStime(unsigned week,double second)
        :_week(week),
        _second(second)
    {}

    friend ostream& operator<<(ostream& out, const GPStime& gps);

    friend istream& operator>>(istream& in, GPStime& gps);

    unsigned _week;
    double _second;
};

ostream& operator<<(ostream& out, const GPStime& gps)
{
    out << gps._week << " " << gps._second<<endl;
    return out;
}

istream& operator>>(istream& in, GPStime& gps)
{
    in >> gps._week >> gps._second;
    return in;
}

struct gtime_t
{
    gtime_t()
    {}

    gtime_t(time_t time,double second)
        :_time(time),
        _second(second)
    {}

    friend ostream& operator<<(ostream& out, const gtime_t& gt);

    friend istream& operator>>(istream& in, gtime_t& gt);

    time_t _time;
    double _second;
};

ostream& operator<<(ostream& out, const gtime_t& gt)
{
    out << gt._time << " " << gt._second << endl;
    return out;
}

istream& operator>>(istream& in, gtime_t& gt)
{
    in >> gt._time >> gt._second;
    return in;
}


//总的时间系统
//当传入一个时间之后，自动同步另外两个时间
class atime
{
public:
    atime()
    {}

    atime(commontime& com)
        :_com(com)
    {
        _gt = ComToUnix(_com);
        _gps = UnixToGPS(_gt);
    }

    atime(GPStime& gps)
        :_gps(gps)
    {
        _gt = GPSToUnix(_gps);
        _com = UnixToCom(_gt);
    }

    atime(gtime_t& gt)
        :_gt(gt)
    {
        _com = UnixToCom(_gt);
        _gps = UnixToGPS(_gt);
    }

    static gtime_t ComToUnix(const commontime& com)
    {
        gtime_t ret;
        int day = Date(com._year, com._month, com._day) - Date(1970, 1, 1);
        ret._time = day * DAY_SECOND + com._hour * 3600 + com._minute * 60 + (time_t)floor(com._second);
        ret._second = com._second - floor(com._second);
        return ret;
    }

    static gtime_t GPSToUnix(const GPStime& gps)
    {
        gtime_t ret;
        ret._time = gps._week * 7 * DAY_SECOND + (time_t)gps._second + UG_GAP;
        ret._second = gps._second - (int)gps._second;

        return ret;
    }

    static commontime UnixToCom(const gtime_t& gt)
    {
        commontime ret;
        ret._day = (int)(gt._time / DAY_SECOND);
        int second = (int)(gt._time - ret._day * DAY_SECOND);
        ret._hour = second % 3600;
        ret._minute = (second - ret._hour * 3600) % 60;
        ret._second = (second - ret._hour * 3600 - ret._minute * 60) % 60;

        Date tmp = Date(1970, 1, 1) + ret._day;
        ret._year = tmp._year;
        ret._month = tmp._month;
        ret._day = tmp._day;

        return ret;
    }

    static GPStime UnixToGPS(const gtime_t& gt)
    {
        GPStime ret;
        double sec = gt._time - UG_GAP;
        ret._week = (int)(sec / ROUND_SECOND);
        ret._second = (double)(sec - ret._week * ROUND_SECOND) + gt._second;

        return ret;
    }

    const commontime& GetCommontime()
    {
        return _com;
    }

    const GPStime& GetGPStime()
    {
        return _gps;
    }

    const gtime_t& GetGtime()
    {
        return _gt;
    }
private:
    commontime _com;
    GPStime _gps;
    gtime_t _gt;
};



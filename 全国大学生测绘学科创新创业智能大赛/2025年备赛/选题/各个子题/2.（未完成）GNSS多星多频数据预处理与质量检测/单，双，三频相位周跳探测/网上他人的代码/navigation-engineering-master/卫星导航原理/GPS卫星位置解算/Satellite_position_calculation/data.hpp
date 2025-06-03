#pragma once
#include<fstream>
#include<string>
#include"time.hpp"
#include"space.hpp"
#include<unordered_map>

#define PATHNAME "./brdc0320.21n"//文件名称

using namespace std;

struct satellite_data_block
{
	double Serial;//序列号
	
	double year;
	double month;
	double day;
	double hour;
	double minute;
	double second;//年月日时分秒
	
	double a0;//卫星钟差
	double a1;//卫星钟偏
	double a2;//卫星钟偏移
	double AODE;//数据龄期

	double Crs;//轨道半径改正项
	double deltan;//平均角速度改正项
	double M0;//平近点角
	double Cuc;//升交点角距改正项
	double e;//轨道偏心率
	double Cus;//升交点角距改正项
	double sqrtA;//轨道长半轴平方根
	double TOE;//星历的参考时刻  周内秒
	double Cic;//轨道倾角的改正项
	double OMEGA;//升交点经度
	double Cis;//轨道倾角改正项
	double i0;//轨道倾角
	double Crc;//轨道半径的改正项
	double omega;//近地点角距
	double deltaomega;//升交点赤经变化
	double IDOT;//轨道倾角的变率
	//对于单点定位，以上数据足以，但是为了保持卫星数据完整，以下保持了所有的数据
	
	double CA;//CA码
	double GPSweek;//gps周数
	double L2P;//L2P码
	double SVA;//卫星精度
	double SVH;//卫星健康
	double TGD;//电离层延迟
	double IODC;//数据质量
	double shoot_time;//发射时间
	double h;//拟合区间
};


//从标准文件中读取数据，删去头文件
//为了代码的可读性，在年月日时分秒的秒数据后手动加上了一个空格
class read_data
{
public:
	read_data(const string& path_name = PATHNAME)
		:_path_name(path_name)
	{
		read();
	}

	void read()
	{
		fstream ifs(_path_name, ios_base::in);

		if (!ifs)	exit(-1);
		while (!ifs.eof())
		{
			string data;
			for (int i = 0; i < 8; i++)
			{
				//采用的是读取整个数据块的方法
				char buffer[82];
				ifs.getline(buffer, 81);
				data += buffer;
			}

			get_data(data);
		}
	}
	vector<satellite_data_block> _data;
	unordered_map<int, int> _serial;
private:
	string _path_name;
	

	double _to_double(string& s)
	{
		for (auto& e : s)
		{
			if (e == 'D')	e = 'E';
		}

		return stod(s);
	}

	string get_part(string& data,int& cur)
	{
		while (data[cur] == ' ')	cur++;
		int last = cur;
		int record = 2;
		bool keep = true;
		while(1)
		{
			last++;
			if (data[last] == ' ') break;
			if (data[last] == '-' || data[last] == '+')	record--;
			if (record == 0)	break;
		}

		string ret = string(data.begin() + cur, data.begin() + last);
		cur = last;
		return ret;
	}

	void get_data(string& data)
	{
		double init[36];
		int cur = 0;
		for (int i = 0; i < 36; i++)
		{
			string tmp = get_part(data, cur);
			init[i] = _to_double(tmp);
		}

		satellite_data_block ins;

		memcpy(&ins, init, sizeof(double) * 36);

		_serial.insert(make_pair(ins.Serial, _data.size()));
		_data.push_back(ins);
	}
};
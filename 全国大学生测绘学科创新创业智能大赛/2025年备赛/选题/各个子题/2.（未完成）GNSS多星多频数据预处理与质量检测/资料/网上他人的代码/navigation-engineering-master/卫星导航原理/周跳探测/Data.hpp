#pragma once
#include<cmath>
#include<vector>
#define clight 299792458
#define FL1 1.5754E+09
#define FL2 1.2276E+09

using namespace std;

struct carrier
{
	double f;//频率，从L1和L2波段中选择
	double Lambda;//波长,f*l=c
	double Fai;//不足一周计数
	double N;//整周模糊度
	double count;//整周计数
	double distance;//伪距
	double TEC;//电子含量
};

class Dual_frequency_carrier
{
public:
	Dual_frequency_carrier(carrier c1,carrier c2)
		:_carrier1(c1),
		_carrier2(c2)
	{}

	carrier _carrier1;
	carrier _carrier2;
};

class LineObservation
{
public:
	LineObservation(const Dual_frequency_carrier& carriers)
		:_carriers(carriers)
	{}

	double Observation()
	{
		return _observation;
	}
protected:
	Dual_frequency_carrier _carriers;
	double _observation;

	virtual double _Observation() = 0;
};

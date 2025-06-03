#pragma once
#include"Data.hpp"

class GeometryFree : public LineObservation
{
public:
	GeometryFree(const Dual_frequency_carrier& carriers)
		:LineObservation(carriers)
	{
		_Observation();
	}
protected:
	virtual double _Observation()
	{
		carrier car1 = _carriers._carrier1;
		carrier car2 = _carriers._carrier2;
		double A1 = -40.3 * car1.TEC;
		double Vion1 = A1 / pow(car1.f, 2);

		_observation = car1.Lambda * car1.N - car2.Lambda * car2.N + (1 - pow(car1.f, 2) / pow(car2.f, 2)) * Vion1;
		return _observation;
	}
};

//GF周跳探测
class GF_detect
{
public:
	GF_detect(const vector<Dual_frequency_carrier>& data)
	{
		//把所有的载波变为GF观测值
		for (const auto& e : data)
		{
			_data.push_back(GeometryFree(e));
		}

		_Detect();
	}

	bool Detect()
	{
		return _detect;
	}

	vector<int> Slip()
	{
		return _slip;
	}

private:
	vector<GeometryFree> _data;
	bool _detect;//是否发生周跳，发生周跳为true
	vector<int> _slip;//发生周跳的具体时间点
	double MAX_GAP = 5E-4;//最大阈值

	bool _Detect()
	{
		//遍历每一个历元差，如果差值超过了阈值，那么一定发生了周跳
		for (int i = 1; i < _data.size(); i++)
		{
			if (_data[i].Observation() - _data[i - 1].Observation() > MAX_GAP)
			{
				_detect = true;//发生了周跳
				_slip.push_back(i);//发生周跳的时间点
			}
		}

		return _detect;
	}
};
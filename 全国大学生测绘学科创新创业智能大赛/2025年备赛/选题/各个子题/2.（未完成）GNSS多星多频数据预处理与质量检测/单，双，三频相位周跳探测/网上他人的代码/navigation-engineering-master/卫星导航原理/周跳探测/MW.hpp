#pragma once
#include"Data.hpp"

class MW :public LineObservation
{
public:
	MW(const Dual_frequency_carrier& carriers)
		:LineObservation(carriers)
	{
		_Observation();
	}
private:
	virtual double _Observation()
	{
		carrier car1 = _carriers._carrier1;
		carrier car2 = _carriers._carrier2;

		double f1 = car1.f;
		double f2 = car2.f;
		double L1 = car1.Lambda;
		double L2 = car2.Lambda;
		double P1 = car1.distance;
		double P2 = car2.distance;

		_observation = (f1 - f2) / (f1 + f2) * (P1 / L1 + P2 / L2) - (car1.Fai - car2.Fai);//周数
		return _observation;
	}
};

class MW_detect
{
	MW_detect(const vector<Dual_frequency_carrier>& data)
	{

		//将载波数据计算为MW组合
		for (const auto& e : data)
		{
			_data.push_back(MW(e));
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
	vector<MW> _data;
	bool _detect;//是否发生周跳，发生周跳为true
	vector<int> _slip;//周跳具体时间点
	double MAX_GAP = 0.5;//最大阈值：0.5周

	bool _Detect()
	{
		double sum = _data[0].Observation();
		double average = _data[0].Observation();
		double sigma = 0.5;
		bool record = false;

		for (int i = 1; i < _data.size(); i++)
		{
			//如果可能发生周跳
			if (record)
			{
				//判断是周跳还是误差
				if (abs(_data[i].Observation() - _data[i - 1].Observation()) <= 1)
				{
					_detect = true;
					_slip.push_back(i);
				}
			}

			if (abs(_data[i].Observation() - average) >= sigma * 4)
				record = true;//可能发生周跳，也可能是误差

			sum += _data[i].Observation();
			average = sum / (i + 1);
			sigma = sigma * sigma + (pow(_data[i].Observation() - average, 2) - sigma * sigma) / (i + 1);
		}

		return _detect;
	}
};
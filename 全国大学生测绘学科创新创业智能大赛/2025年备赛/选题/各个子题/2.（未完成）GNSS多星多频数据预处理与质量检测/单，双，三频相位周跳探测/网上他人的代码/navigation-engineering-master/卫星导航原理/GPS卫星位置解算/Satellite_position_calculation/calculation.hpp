#pragma once
#include<cmath>
#include"data.hpp"
#define GM 3.9860050E14
#define OMGE 7.2921151467E-5

commontime _curtime = commontime({ 2021,2,1,0,0 }, 30);//当前观测时间
atime curtime = atime(_curtime);

//计算的完整过程，必须按顺序依次进行
class calculate
{
public:
	vector<double> _calculate(const satellite_data_block& s)
	{
		sqrt_A(s.sqrtA);
		get_time(s.TOE, s.GPSweek);
		new_angular_velocity(s.deltan);
		mean_anomaly(s.M0);
		Eccentric_Anomaly(s.e);
		True_anomaly(s.e);
		argument_of_latitude(s.omega);
		Correction_of_uk(s.Cuc, s.Cus);
		Correction_of_rk(s.Crc, s.Crs);
		Correction_of_ik(s.Cic, s.Cis);
		get_uk();
		get_rk(s.e);
		get_ik(s.i0, s.IDOT);
		get_OMGk(s.OMEGA, s.deltaomega, s.TOE);
		vector<double> xyz = get_xyz();
		return xyz;
	}
private:
	void sqrt_A(double sqrt_a)
	{
		sqrtA = sqrt_a;
		angular_velocity = sqrt(GM) / pow(sqrtA, 3);
	}
	double get_time(double Toe,double week,atime cur = curtime)
	{
		//因为没有考虑信号传播时间，最终有小部分误差
		GPStime _bt(week, Toe);
		atime bt(_bt);
		double ret = cur.GetGtime()._time-bt.GetGtime()._time;
		
		if (ret > ROUND_SECOND/2)	ret -= ROUND_SECOND;
		else if (ret < -ROUND_SECOND / 2)	ret += ROUND_SECOND;
		

		_tk = ret;

		return ret;
	}

	double new_angular_velocity(double deltan)
	{
		_angular_velocity = angular_velocity + deltan;

		return _angular_velocity;
	}

	double mean_anomaly(double M0)
	{
		_Mk = M0 + _tk * _angular_velocity;
		return _Mk;
	}

	double Eccentric_Anomaly(double e)
	{
		double Mk = _Mk;
		auto get_Ek = [e,Mk](double Ep)
			{
				return Ep - (Ep - e * sin(Ep) - Mk) / (1 - e * cos(Ep));
			};

		double E0 = _Mk;
		double E1 = get_Ek(E0);
		while (abs(E1 - E0) >= 1E-13)
		{
			E0 = E1;
			E1 = get_Ek(E0);
		}

		_Ek = E1;
		return _Ek;
	}

	double True_anomaly(double e)
	{
		_Vk = atan((sqrt(1 - e * e) * sin(_Ek) )
			/ (cos(_Ek) - e));

		if (abs(_Vk - _Mk) > Pi / 2)
			_Vk += _Vk > _Mk ? -Pi : Pi;

		return _Vk;
	}

	double argument_of_latitude(double omega)
	{
		_argument_of_latitude = _Vk + omega;
		return _argument_of_latitude;
	}

	double Correction_of_uk(double cuc,double cus)
	{
		_Cuk = cus * sin(2 * _argument_of_latitude) + cuc * cos(2 * _argument_of_latitude);
		return _Cuk;
	}

	double Correction_of_rk(double crc, double crs)
	{
		_Crk = crs * sin(2 * _argument_of_latitude) + crc * cos(2 * _argument_of_latitude);
		return _Crk;
	}

	double Correction_of_ik(double cic, double cis)
	{
		_Cik = cis * sin(2 * _argument_of_latitude) + cic * cos(2 * _argument_of_latitude);
		return _Cik;
	}

	double get_uk()
	{
		_Uk = _argument_of_latitude + _Cuk;
		return _Uk;
	}

	double get_rk(double e)
	{
		_Rk = sqrtA*sqrtA * (1 - e * cos(_Ek)) + _Crk;
		return _Rk;
	}
	
	double get_ik(double i0, double idot)
	{
		_Ik = i0 + _Cik + idot * _tk;
		return _Ik;
	}

	double get_OMGk(double OMG0,double OMGd,double toe)
	{
		_OMGk = OMG0 + (OMGd - OMGE) * _tk - OMGE * toe;
		return _OMGk;
	}

	vector<double> get_xyz()
	{
		double _xk = _Rk * cos(_Uk);
		double _yk = _Rk * sin(_Uk);

		double x = _xk * cos(_OMGk) - _yk * cos(_Ik) * sin(_OMGk);
		double y = _xk * sin(_OMGk) + _yk * cos(_Ik) * cos(_OMGk);
		double z = _yk * sin(_Ik);

		return { x,y,z };
	}

private:
	double sqrtA;
	double angular_velocity;
	double _tk;
	double _angular_velocity;
	double _Mk;
	double _Ek;
	double _Vk;
	double _argument_of_latitude;
	double _Cuk;
	double _Crk;
	double _Cik;
	double _Uk;
	double _Rk;
	double _Ik;

	double _OMGk;
};


//最终类，传入序列号会返回序列号对应的xyz
//也可以传入自定义的数据块
class satellite_position_calculation
{
public:
	satellite_position_calculation(int Serial)
		:r()
	{
		if (r._serial.find(Serial) != r._serial.end())
		{
			s = r._data[r._serial[Serial]];
			xyz=c._calculate(s);
		}
		else
		{
			perror("no such serial");
			return;
		}
	}

	satellite_position_calculation(const satellite_data_block& data)
		:s(data)
	{
		xyz=c._calculate(s);
	}

	vector<double> xyz;
private:
	calculate c;
	read_data r;
	satellite_data_block s;
};
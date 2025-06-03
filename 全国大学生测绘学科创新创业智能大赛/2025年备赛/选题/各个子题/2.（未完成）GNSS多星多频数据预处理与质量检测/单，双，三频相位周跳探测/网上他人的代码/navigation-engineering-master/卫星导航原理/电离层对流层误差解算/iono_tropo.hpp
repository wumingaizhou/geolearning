#pragma once
#include"calculation.hpp"
#include"data.hpp"

#define alpha0 0.8382E-8
#define alpha1 -0.7451E-8
#define alpha2 -0.596E-7
#define alpha3 0.596E-7

#define Beta0 0.8806E5
#define Beta1 -0.3277E5
#define Beta2 -0.1966E6
#define Beta3 0.1966E6

extern atime curtime;

class base_data
{
protected:
	base_data(int Serial, const vector<double>& rr)
		:rs_place(Serial),
		_rs(rs_place.xyz[0], rs_place.xyz[1], rs_place.xyz[2]),
		/*_rs(12712882.254  ,23247798.196 ,-2637709.427),*/
		_rr(rr[0], rr[1], rr[2])
	{
		Distance_from_receiver_to_satellite();
		Unit_vector();
		rs_topocentric();
		azimuth();
		Height_angle();
		central_angle();
	}
	satellite_position_calculation rs_place;//解算卫星坐标
	Position<Space_Coordinates> _rs;//卫星位置
	Position<Space_Coordinates> _rr;//接收站位置

	double _d;//卫星到接收站的距离
	vector<double> _e;//卫星到接收站的单位向量
	vector<double> _enu;//卫星在站星坐标系下的位置ENU
	double _az;//方位角
	double _el;//高度角
	double _central_angle;//中心角

	double Distance_from_receiver_to_satellite()
	{
		double tmp = 0;
		for (int i = 0; i < 3; i++)
			tmp += pow((_rs.Get__Space_Coordinates()[i] - _rr.Get__Space_Coordinates()[i]), 2);

		_d = sqrt(tmp);
		return _d;
	}

	vector<double> Unit_vector()
	{
		_e.resize(3);
		for (int i = 0; i < 3; i++)
		{
			_e[i] = (_rs.Get__Space_Coordinates()[i] - _rr.Get__Space_Coordinates()[i]) / _d;
		}

		return _e;
	}

	vector<double> rs_topocentric()
	{
		_enu.resize(3);
		double sinL = sin(_rr.Get__Geodetic_Coordinate_System()[1] * Pi / 180);
		double cosL = cos(_rr.Get__Geodetic_Coordinate_System()[1] * Pi / 180);
		double sinB = sin(_rr.Get__Geodetic_Coordinate_System()[0] * Pi / 180);
		double cosB = cos(_rr.Get__Geodetic_Coordinate_System()[0] * Pi / 180);

		auto matrix_multiplication = [this](double factor1, double factor2, double factor3)
			{
				return factor1 * (_rs.Get__Space_Coordinates()[0] - _rr.Get__Space_Coordinates()[0])
					+ factor2 * (_rs.Get__Space_Coordinates()[1] - _rr.Get__Space_Coordinates()[1])
					+ factor3 * (_rs.Get__Space_Coordinates()[2] - _rr.Get__Space_Coordinates()[2]);
			};

		_enu[0] = matrix_multiplication(-sinL, cosL, 0);
		_enu[1] = matrix_multiplication(-sinB * cosL, -sinB * sinL, cosB);
		_enu[2] = matrix_multiplication(cosB * cosL, cosB * sinL, sinB);

		return _enu;
	}

	double azimuth()
	{
		_az = atan2(_enu[0],_enu[1]);
		return _az;
	}

	double Height_angle()
	{
		_el = asin(_enu[2] / _d);
		return _el;
	}

	double central_angle()
	{
		_central_angle = 0.0137 / ((_el / Pi) + 0.11) - 0.022;
		return _central_angle;
	}
};

class iono_calculate:public base_data
{
public:
	iono_calculate(int Serial, const vector<double>& rr)
		:base_data(Serial,rr)
	{}

	double iono_delay()
	{
		return _iono_calculate();
	}

private:
	double _FAIi;//电离层穿刺点的纬度
	double _Lambdai;//电离层穿刺点经度
	double _FAIm;//电离层穿刺点的地磁纬度
	double _t;//GPS周内秒
	double _Ai;//电离层延迟的幅度
	double _cyclei;//电离层延迟的周期
	double _Xi;//电离层延迟的相位
	double _F;//倾斜因子

	double FAIi()
	{
		_FAIi = _rr.Get__Geodetic_Coordinate_System()[0] / 180 + _central_angle * cos(_az);
		if (_FAIi > 0.416)	_FAIi = 0.416;
		if (_FAIi < -0.416)	_FAIi = -0.416;
		return _FAIi;
	}

	double Lambdai()
	{
		_Lambdai = _rr.Get__Geodetic_Coordinate_System()[1] / 180 +
			(_central_angle * sin(_az)) / cos(_FAIi * Pi);
		return _Lambdai;
	}

	double FAIm()
	{
		_FAIm = _FAIi + 0.064 * cos((_Lambdai - 1.617) * Pi);
		return _FAIm;
	}

	double get_t()
	{
		double tmp = 43200 * _Lambdai + curtime.GetGPStime()._second;
		_t = tmp - floor(tmp / 86400) * 86400;
		return _t;
	}

	double Ai()
	{
		_Ai = alpha0 + _FAIm * (alpha1 + _FAIm * (alpha2 + _FAIm * alpha3));
		if (_Ai < 0)	_Ai = 0;
		return _Ai;
	}

	double cyclei()
	{
		_cyclei = Beta0 + _FAIm * (Beta1 + _FAIm * (Beta2 + _FAIm * Beta3));
		if (_cyclei < 72000)	_cyclei = 72000;
		return _cyclei;
	}

	double Xi()
	{
		_Xi = 2 * Pi * (_t - 50400) / _cyclei;
		return _Xi;
	}

	double obliquity_factor()
	{
		_F = 1.0 + 16.0 * pow((0.53 - _el / Pi), 3);
		return _F;
	}

	double Ionospheric_delay()
	{
		if (fabs(_Xi) >= 1.57)
			return 5e-9 * clight * _F;
		else
			return clight * (5E-9 + _Ai * (1 - 0.5 * _Xi * _Xi + pow(_Xi, 4) / 24.0)) * _F;
	}

	double _iono_calculate()
	{
		FAIi();
		Lambdai();
		FAIm();
		get_t();
		Ai();
		cyclei();
		Xi();
		obliquity_factor();
		return Ionospheric_delay();
	}
};


#define humi 0.7//相对湿度

class tropo_calculate:public base_data
{
public:
	tropo_calculate(int Serial, const vector<double>& rr)
		:base_data(Serial,rr)
	{}

	double tropo_delay()
	{
		calculate();
		return _trp;
	}
private:
	double _p;//大气总压强
	double _T;//空气绝对温度
	double _e;//偏心率
	double _trph;//对流层干延迟
	double _trpw;//对流层湿延迟
	double _trp;//对流层总延迟

	void calculate()
	{
		
		_T = 15.0 - 6.5E-3 * _rr.Get__Geodetic_Coordinate_System()[2] + 273.16;
		_e = 6.108 * humi * exp((17.15 * _T - 4684.0) / (_T - 38.45));
		_p = 1013.25 * pow((1 - 2.2557 * 1e-5 * _rr.Get__Geodetic_Coordinate_System()[2]), 5.2568);

		double z = Pi / 2 - _el;
		_trph = 0.0022768 * _p / (cos(z) * (1.0 - 0.00266 *
			cos(2 * _rr.Get__Geodetic_Coordinate_System()[0] * Pi / 180)
			- 0.00028 * _rr.Get__Geodetic_Coordinate_System()[2] / 1000));
		_trpw = 0.002277 * (1255.0 / _T + 0.05) * _e / cos(z);
		_trp = _trph + _trpw;
	}
};
#pragma once
#include<iostream>
#include<vector>
#include<cmath>
using namespace std;
#define Pi 3.1415926535897932384626433832795
#define LONG_AXIS 6378137
#define SHORT_AXIS 6356755
#define f 1.0/298.257223563

class Geodetic_Coordinate_System
{
public:
	Geodetic_Coordinate_System(double Long_Axis = LONG_AXIS, double Short_Axis = SHORT_AXIS)
	{
		_Long_Axis = Long_Axis;
		_Short_Axis = Short_Axis;
		/*_Eccentricity = pow((pow(_Long_Axis, 2) - pow(_Short_Axis, 2)), 0.5) / _Long_Axis;*/
		_Eccentricity = sqrt(2 * f - f * f);
	}
	double To_X(double B, double L, double H)
	{
		double Curvature_Radius = _Long_Axis / pow(1 - pow(_Eccentricity * sin(B * Pi / 180), 2), 0.5);
		return (Curvature_Radius + H) * cos(B * Pi / 180) * cos(L * Pi / 180);
	}
	double To_Y(double B, double L, double H)
	{
		double Curvature_Radius = _Long_Axis / pow(1 - pow(_Eccentricity * sin(B * Pi / 180), 2), 0.5);
		return (Curvature_Radius + H) * cos(B * Pi / 180) * sin(L * Pi / 180);
	}
	double To_Z(double B, double L, double H)
	{
		double Curvature_Radius = _Long_Axis / pow(1 - pow(_Eccentricity * sin(B * Pi / 180), 2), 0.5);
		return (Curvature_Radius * (1 - pow(_Eccentricity, 2)) + H) * sin(B * Pi / 180);
	}
private:
	double _Long_Axis;
	double _Short_Axis;
	double _Eccentricity;
};

class Space_Coordinates
{
public:
	Space_Coordinates(double Long_Axis = LONG_AXIS, double Short_Axis = SHORT_AXIS)
	{
		_Long_Axis = Long_Axis;
		_Short_Axis = Short_Axis;
		/*_Eccentricity = pow((pow(_Long_Axis, 2) - pow(_Short_Axis, 2)), 0.5) / _Long_Axis;*/
		_Eccentricity = sqrt(2 * f - f * f);
		_Eccentricity2 = pow((pow(_Long_Axis, 2) - pow(_Short_Axis, 2)), 0.5) / _Short_Axis;
	}
	double To_Latitude(double X, double Y, double Z)
	{
		double R = sqrt(X * X + Y * Y);
		double B1 = atan2(Z , R);
		double B2;
		while (1)
		{
			double W1 = sqrt(1 - _Eccentricity * _Eccentricity * sin(B1) * sin(B1));
			double N1 = LONG_AXIS / W1;
			B2 = atan((Z + N1 * _Eccentricity * _Eccentricity * sin(B1)) / R);
			if (abs(B2 - B1) <= 1e-11)	break;
			B1 = B2;
		}
		return B2 * 180 / Pi;
	}
	double To_Longitude(double X, double Y, double Z)
	{
		return atan2(Y, X) * 180 / Pi;
	}
	double To_Height(double X, double Y, double Z)
	{
		double B = To_Latitude(X, Y, Z);
		double N = LONG_AXIS / sqrt(1 - pow(_Eccentricity * sin(B * Pi / 180), 2));
		double FAI = atan(Z/pow(pow(X, 2) + pow(Y, 2), 0.5));
		return sqrt(X * X + Y * Y + Z * Z) * cos(FAI) / cos(B * Pi / 180) - N;
	}
private:
	double _Long_Axis;
	double _Short_Axis;
	double _Eccentricity;
	double _Eccentricity2;
};

template<class Base_Coordinates>
class Position
{
public:
	//若基础坐标系是空间坐标系，则传入X/Y/Z
	//若基础坐标系是大地坐标系，则传入B/L/H
	Position(double data1, double data2, double data3);

	//获取空间坐标系的坐标X/Y/Z
	double* Get__Space_Coordinates()
	{
		return _Space_Coordinates;
	}

	//获取大地坐标系的坐标B/L/H
	double* Get__Geodetic_Coordinate_System()
	{
		return _Geodetic_Coordinate_System;
	}


private:
	double _Space_Coordinates[3];
	double _Geodetic_Coordinate_System[3];
	Base_Coordinates _Base_Coordinates;
};

Position<Space_Coordinates>::Position(double data1, double data2, double data3)
{
	double X = data1;
	double Y = data2;
	double Z = data3;

	_Space_Coordinates[0] = X;
	_Space_Coordinates[1] = Y;
	_Space_Coordinates[2] = Z;

	_Geodetic_Coordinate_System[0] = _Base_Coordinates.To_Latitude(X, Y, Z);
	_Geodetic_Coordinate_System[1] = _Base_Coordinates.To_Longitude(X, Y, Z);
	_Geodetic_Coordinate_System[2] = _Base_Coordinates.To_Height(X, Y, Z);
}

Position<Geodetic_Coordinate_System>::Position(double data1, double data2, double data3)
{
	double B = data1;
	double L = data2;
	double H = data3;

	_Geodetic_Coordinate_System[0] = B;
	_Geodetic_Coordinate_System[1] = L;
	_Geodetic_Coordinate_System[2] = H;

	_Space_Coordinates[0] = _Base_Coordinates.To_X(B, L, H);
	_Space_Coordinates[1] = _Base_Coordinates.To_Y(B, L, H);
	_Space_Coordinates[2] = _Base_Coordinates.To_Z(B, L, H);
}
#pragma once
#include<vector>
#include<cmath>
#include<initializer_list>

using namespace std;

template<typename T>
struct Point
{
	Point()
	{}

	Point(T x, T y)
		:_x(x),
		_y(y)
	{}

	Point(const vector<T>& point)
	{
		if (point.size() != 2)
		{
			perror("no valid data");
			return;
		}
		else
		{
			_x = point[0];
			_y = point[1];
		}
	}

	T _x;
	T _y;
};

struct Line
{
	template<class T>
	static Line GetLine(const Point<T>& LinePoint1, const Point<T>& LinePoint2)
	{
		Line ret;
		ret._CoefficientX = LinePoint2._y - LinePoint1._y;
		ret._CoefficientY = LinePoint1._x - LinePoint2._x;
		ret._CoefficientC = LinePoint2._x * LinePoint1._y - LinePoint1._x * LinePoint2._y;
		return ret;
	}

	double _CoefficientX;
	double _CoefficientY;
	double _CoefficientC;
};

template<typename T>
double DistanceOfPoint(const Point<T>& point1, const Point<T>& point2)
{
	return sqrt(pow(point1._x - point2._x, 2) + pow(point1._y - point2._y, 2));
}

template<typename T>
double DistanceToLine(const Point<T>& LinePoint1, const Point<T>& LinePoint2, const Point<T>& src)
{
	Line line = Line::GetLine(LinePoint1, LinePoint2);
	return DistanceToLine(line, src);
}

template<typename T>
double DistanceToLine(const Line& line, const Point<T>& src)
{
	return abs(line._CoefficientX * src._x + line._CoefficientY * src._y + line._CoefficientC) /
		sqrt(line._CoefficientX * line._CoefficientX + line._CoefficientY * line._CoefficientY);
}

template<typename T>
double AngleOfLine(const Point<T>& point1, const Point<T>& point2, const Point<T>& vertex)
{
	vector<T> vec1 = { point1._x - vertex._x,point1._y - vertex._y };
	vector<T> vec2 = { point2._x - vertex._y,point2._x - vertex._y };

	double len1 = DistanceOfPoint(point1, vertex);
	double len2 = DistanceOfPoint(point2, vertex);

	return acos((vec1[0] * vec2[0] + vec1[1] * vec2[1]) / (len1 * len2));
}


#pragma once
#include"point.hpp"
#include<stack>
#include<set>

template<typename T>
class compression
{
	typedef vector<Point<T>> Points;
public:
	compression(Points& point,double d)
		:_point(point),
		_d(d)
	{}

	Points DouglasPeukcer()
	{
		if (_point.size() <= 3)	return _point;
		Points ret;
		stack<pair<int, int>> st;
		st.push({ 0, _point.size()-1 });
		set<int> remain;

		while (!st.empty())
		{
			auto [begin, end] = st.top();
			st.pop();

			if (end - begin <= 2)
			{
				remain.insert({ begin,end });
				continue;
			}

			int middle = GetMaxVerticalDistance(_point, begin, end);
			
			if (middle == -1)
			{
				remain.insert({ begin,end });
			}
			else
			{
				st.push({ begin,middle });
				st.push({ middle,end });
			}
		}

		for (auto e : remain)
		{
			ret.push_back(_point[e]);
		}

		return ret;
	}

	Points VerticalDistanceMethod()
	{
		if (_point.size() <= 3)	return _point;
		Points ret;
		set<int> erase;

		Points testpoints(3);
		testpoints[0] = _point[0];
		testpoints[1] = _point[1];
		testpoints[2] = _point[2];
		int cur = 2;
		while (cur<_point.size())
		{
			double distance = DistanceToLine(testpoints[0], testpoints[2], testpoints[1]);
			if (distance < _d)
			{
				erase.insert(cur - 1);
				cur++;
				testpoints[1] = testpoints[2];
				if (cur < _point.size())
					testpoints[2] = _point[cur];
			}
			else
			{
				cur++;
				testpoints[0] = testpoints[1];
				testpoints[1] = testpoints[2];
				if (cur < _point.size())
					testpoints[2] = _point[cur];
			}
		}

		for (int i = 0; i < _point.size(); i++)
		{
			if (erase.find(i) == erase.end())
				ret.push_back(_point[i]);
		}

		return ret;
	}

	Points LightBarrierMethod()
	{
		if (_point.size() <= 3)	return _point;
		Points ret;
		set<int> erase;

		Points testpoints(3);
		testpoints[0] = _point[0];
		testpoints[1] = _point[1];
		testpoints[2] = _point[2];
		int cur = 2;

		while (cur < _point.size())
		{
			double angle_point = AngleOfLine(testpoints[1], testpoints[2], testpoints[0]);
			double angle_range = atan2(2 / _d, DistanceOfPoint(testpoints[0], testpoints[1]));

			if (angle_point < angle_range)
			{
				erase.insert(cur - 1);
				cur++;
				testpoints[1] = testpoints[2];
				if (cur < _point.size())
					testpoints[2] = _point[cur];
			}	
			else
			{
				cur++;
				testpoints[0] = testpoints[1];
				testpoints[1] = testpoints[2];
				if (cur < _point.size())
					testpoints[2] = _point[cur];
			}
		}

		for (int i = 0; i < _point.size(); i++)
		{
			if (erase.find(i) == erase.end())
				ret.push_back(_point[i]);
		}

		return ret;
	}
private:
	Points _point;
	double _d;

	int GetMaxVerticalDistance(const Points& point, int begin, int end)
	{
		Line line = Line::GetLine(point[begin], point[end]);
		int MaxPoint = begin;
		double Maxdis = 0;
		for (int i = begin + 1; i < end; i++)
		{
			double dis = DistanceToLine(line, point[i]);
			if (dis > Maxdis)
			{
				MaxPoint = i;
				Maxdis = dis;
			}
		}

		return Maxdis < _d ? -1 : MaxPoint;
	}
};
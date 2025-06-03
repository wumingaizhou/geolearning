#include<iostream>
#include"compression.hpp"

void print(vector<Point<double>>& point)
{
	cout << "Points:" << endl;
	for (auto e : point)
	{
		cout << e._x << "  " << e._y << "  " << endl;
	}

	cout << endl << endl;
}

void test()
{
	vector<Point<double>> source =
	{
		{1,4},
		{2,3},
		{4,2},
		{6,6},
		{7,7},
		{8,6},
		{9,5},
		{10,10},
		{11,10.5},
		{12,14},
		{13,-10}
	};

	compression<double> test(source, 1);
	vector<Point<double>> com1 = test.DouglasPeukcer();
	vector<Point<double>> com2 = test.VerticalDistanceMethod();
	vector<Point<double>> com3 = test.LightBarrierMethod();

	print(source);
	print(com1);
	print(com2);
	print(com3);
}

int main()
{
	test();
}

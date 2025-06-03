#include"iono_tropo.hpp"
#include<unordered_map>

using namespace std;

unordered_map<int, vector<double>> reciver;
vector<double> place1= { 3902889.1288951016 ,463565.88712509448 ,5006581.4682619385 };
vector<double> place2= { 3902889.1288951016 ,463565.88712509448 ,5006581.4682619385 };

void init()
{
	reciver[1] = place1;
	reciver[8] = place1;
	reciver[10] = place1;
	reciver[14] = place2;
	reciver[21] = place2;
	reciver[22] = place2;
	reciver[27] = place1;
	reciver[28] = place1;
	reciver[32] = place1;
}

//void test()
//{
//	vector<double> test = { -2267752.0605993434, 5009151.1456511570, 3221301.4797024932 };
//	iono_calculate tmp = iono_calculate(1, test);
//	double iono_delay = tmp.iono_delay();
//	printf("%2d : %16f\n", 0, iono_delay);
//}

//int main()
//{
//	test();
//}

int main()
{
	init();
	vector<int> test = { 1,8,10,14,21,22,27,28,32 };
	for (auto e : test)
	{
		iono_calculate tmp1 = iono_calculate(e, reciver[e]);
		tropo_calculate tmp2(e, reciver[e]);
		double iono_delay = tmp1.iono_delay();
		double tropo_delay = tmp2.tropo_delay();
		printf("%2d inon : %16f\n", e, iono_delay);
		printf("%2d tropo : %16f\n", e, tropo_delay);
		cout << endl;
	}

	return 0;
}
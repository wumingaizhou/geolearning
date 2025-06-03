
#include"calculation.hpp"

using namespace std;

void fortest()
{
	satellite_data_block s;
	s.a0 = -0.231899321079e-6;
	s.a1 = s.a2 = 0;
	s.TOE = 0.72e4;//设置延时差为0
	s.sqrtA = 0.515365263176e4;
	s.e = 0.678421219345e-2;
	s.i0 = 0.958512160302;
	s.omega = -2.58419417299;
	s.OMEGA = -1.37835982556;
	s.M0 = -0.290282040486;
	s.deltan = 0.451411660250e-8;
	s.deltaomega = -0.819426989566e-8;
	s.IDOT = -0.253939149013e-9;
	s.Cus = 0.912137329578e-5;
	s.Cuc = 0.189989805222e-6;
	s.Cis = 0.949949026108E-07;
	s.Cic = 0.130385160446E-07;
	s.Crc = 0.201875000000E+03;
	s.Crs = 0.406250000000E+01;
	s.GPSweek = 0.931000000000E+03;
	s.TGD = 0.186264514923E-08;
	s.IODC = 0.353000000000E+03;

	satellite_position_calculation tmp(s);
	auto test = tmp.xyz;
	cout << test[0] << endl << test[1] << endl << test[2] << endl;
}

void example()
{
	vector<int> ex = { 1,8,10,14,21,22,27,28,32 };
	for (auto e : ex)
	{
		satellite_position_calculation s(e);
		auto test = s.xyz;
		printf("G%2d:\n", e);
		printf("x:%16f\n", test[0]);
		printf("y:%16f\n", test[1]);
		printf("z:%16f\n", test[2]);
		cout << endl;
	}
}

int main()
{
	example();
	return 0;
}


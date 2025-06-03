#include"space.hpp"
#include"time.hpp"

//三个测试用例都写好了，不用修改直接用
void test1()
{
	commontime gt({ 2021,5,21,2,14 }, 45.00);

	atime tmp(gt);
	cout << tmp.GetCommontime() << endl;
	cout << tmp.GetGPStime() << endl;
	cout << tmp.GetGtime() << endl;
}

void test2()
{
	commontime gt({ 2000,2,29,0,0 }, 0.00);

	atime tmp(gt);
	cout << tmp.GetCommontime() << endl;
	cout << tmp.GetGPStime() << endl;
	cout << tmp.GetGtime() << endl;

}


void test3()
{
	commontime gt({ 1999,12,31,1,1 }, 10.00);

	atime tmp(gt);
	cout << tmp.GetCommontime() << endl;
	cout << tmp.GetGPStime() << endl;
	cout << tmp.GetGtime() << endl;
}

//用于测试坐标之间的切换
void test4()
{
	Position<Geodetic_Coordinate_System> tmp(32, 112, 100);//在这里三个参数32，112，100去修改测试用例

	cout << tmp.Get__Geodetic_Coordinate_System()[0] << ' '<<
		tmp.Get__Geodetic_Coordinate_System()[1] << ' '<<
		tmp.Get__Geodetic_Coordinate_System()[2] << endl;
	cout << (int)tmp.Get__Space_Coordinates()[0]<< ' '<<
		(int)tmp.Get__Space_Coordinates()[1] << ' '<<
		(int)tmp.Get__Space_Coordinates()[2] << endl;
}

int main()
{
	//test一二三都是时间系统的测试用例
	//test4是空间系统的
	//要用的时候只要把这个4改成一二三就行了
	test4();
	return 0;
}
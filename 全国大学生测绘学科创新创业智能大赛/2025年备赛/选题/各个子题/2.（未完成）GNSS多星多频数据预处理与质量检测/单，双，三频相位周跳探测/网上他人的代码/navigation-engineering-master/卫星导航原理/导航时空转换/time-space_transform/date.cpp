#define  _CRT_SECURE_NO_WARNINGS 1
#include "Date.h"

// 构造函数用于初始化
Date::Date(int year, int month, int day)
{
	_year = year;
	_month = month;
	_day = day;
	if (_year < 1 || _month < 1 || _month>12 || _day<1 || _day>GetMonthDay(_year, _month))
	{
		assert(false);
	}
}

//判断是否为闰年
bool Date::isLeaveYear(int year)
{
	// （四年一润，百年不润） 或者 四百年一润
	return (year % 4 == 0 && year % 100 != 0) || (year % 400 == 0);
}

// 获取每个月的天数
int Date::GetMonthDay(int year, int month)
{
	int monthday[13] = { 0,31,28,31,30,31,30,31,31,30,31,30,31 };
	if (month == 2 && isLeaveYear(year))
	{
		return 29;
	}
	else
	{
		return monthday[month];
	}
}

void Date::Printf()
{
	cout << _year << " / " << _month << " / " << _day << endl;
}

// "<" 运算符重载
bool Date::operator<(const Date& d)
{
	if (_year < d._year ||
		_year == d._year && _month < d._month ||
		_year == d._year && _month == d._month && _day < d._day)
	{
		return true;
	}
	else
	{
		return false;
	}
}

// "==" 运算符重载
bool Date::operator==(const Date& d)
{
	return _year == d._year && _month == d._month && _day == d._day;
}

// "<=" 运算符重载
bool Date::operator<=(const Date& d)
{
	// 运算符重载的复用
	return *this < d || *this == d;
}

// ">" 运算符重载
bool Date::operator>(const Date& d)
{
	return !(*this <= d);
}

// ">="运算符重载、
bool Date::operator>=(const Date& d)
{
	return !(*this < d);
}

//"!="运算符重载
bool Date::operator!=(const Date& d)
{
	return !(*this == d);
}

// 日期+= 天数: d1 = d1 + 100
Date& Date::operator+=(int day)
{
	// 防止传入的天数为 负数（-100）
	if (day < 0)
	{
		return *this -= (-day);
	}
	_day += day;
	while (_day > GetMonthDay(_year, _month))
	{
		_day -= GetMonthDay(_year, _month);
		_month++;
		if (_month == 13)
		{
			_year++;
			_month = 1;
		}
	}
	return *this;
}

// 日期 + 天数 d1+100  d1不变
Date Date::operator+(int day)
{
	// 调用拷贝构造
	Date temp(*this);
	temp += day;
	return temp;
}

// 日期-= 天数 ：d1 d1 - 100
Date& Date::operator-=(int day)
{
	if (day < 0)
	{
		return *this += (-day);
	}
	_day -= day;
	while (_day <= 0)
	{
		_month--;
		if (_month == 0)
		{
			_month = 12;
			_year--;
		}
		_day += GetMonthDay(_year, _month);
	}
	return *this;
}

// 日期-天数 ： d1 - 100
Date Date::operator-(int day)
{
	// 调用拷贝构造
	Date temp(*this);
	// 复用 "-="
	temp -= day;
	return temp;
}

// 前置++ （无参的为前置，有参的为后置）
Date& Date::operator++()
{
	// 直接复用 +=
	*this += 1;
	return *this;
}

// 后置++（无参的为前置，有参的为后置）
// int i 这里的形参可以写，可以不写
Date Date::operator++(int i)
{
	Date temp(*this);
	*this += 1;
	return temp;
}

// 前置--
Date& Date::operator--()
{
	*this -= 1;
	return *this;
}

//后置--
Date Date::operator--(int i)
{
	Date temp(*this);
	*this -= 1;
	return temp;
}

// 日期 - 日期
int Date::operator-(const Date& d)
{
	// 方便后续计算正负
	int flag = 1;
	Date max = *this;
	Date min = d;
	// 确保max是大的  min是小的
	if (*this < d)
	{
		min = *this;
		max = d;
		flag = -1;  //计算正负
	}
	int n = 0;
	// 计算min和max之间的绝对值差距
	while (min != max)
	{
		min++;
		n++;
	}
	return n * flag;
}
#pragma once
#include <iostream>
#include <stdio.h>
#include <assert.h>
#include <stdbool.h>
using std::cout;
using std::cin;
using std::endl;

class Date
{
public:
	// 构造函数用于初始化 -- 声明 
	// 在声明中缺省函数需要 写清楚  在定义中缺省函数就不要写了 防止编译器分不清
	Date(int year = 1, int month = 1, int day = 1);

	//判断是否为闰年
	bool isLeaveYear(int year);

	// 获取每个月的天数
	int GetMonthDay(int year, int month);

	// 打印
	void Printf();

	// "<" 运算符重载
	bool operator<(const Date& d);

	// "==" 运算符重载
	bool operator==(const Date& d);

	// "<=" 运算符重载
	bool operator<=(const Date& d);

	// ">" 运算符重载
	bool operator>(const Date& d);

	// ">="运算符重载、
	bool operator>=(const Date& d);

	//"!="运算符重载
	bool operator!=(const Date& d);


	// 日期+= 天数: d1 = d1 + 100
	Date& operator+= (int day);

	// 日期 + 天数 d1+100  d1不变
	Date operator+(int day);

	// 日期-= 天数 ：d1 = d1 - 100
	Date& operator-=(int day);

	// 日期-天数 ： d1 - 100
	Date operator-(int day);

	// 前置++ （无参的为前置，有参的为后置）
	Date& operator++();

	// 后置++（无参的为前置，有参的为后置）
	Date operator++(int i);

	// 前置--
	Date& operator--();

	//后置--
	Date operator--(int i);

	// 日期 - 日期
	int operator-(const Date& d);
private:
	int _year;
	int _month;
	int _day;
};
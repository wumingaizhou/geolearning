#pragma once
#include<cmath>
#include<vector>
#include<queue>

using namespace std;

class SpatialAnalysis
{
	using graph = vector<vector<double>>;
public:
	//计算空间缓冲区
	graph SpaceBuffer(const graph& g, int BufferSize)
	{
		int row = g.size();
		int col = g[0].size();
		graph ret(row,vector<double>(col));

		auto makebuffer = [&](int CenterRow, int CenterCol, double modify)
			{
				//修改整个区域，需要检查是否越界
				for (int i = max(0, CenterRow - BufferSize); i <= min(row - 1, CenterRow + BufferSize); i++)
				{
					for (int j = max(0, CenterCol - BufferSize); j <= min(col - 1, CenterCol + BufferSize); j++)
					{
						ret[i][j] = modify;
					}
				}
			};

		//遍历所有数组
		for (int i = 0; i < row; i++)
		{
			for (int j = 0; j < col; j++)
			{
				if (g[i][j] != 0)
				{
					makebuffer(i, j, g[i][j]);
				}
			}
		}

		return ret;
	}

	//体积计算
	int VolumeCalculation(const graph& g, double level,double BlockSize)
	{
		int row = g.size();
		int col = g[0].size();
		double ret = 0;

		for (int i = 0; i < row; i++)
		{
			for (int j = 0; j < col; j++)
			{
				if (g[i][j] != 0)
					ret += (g[i][j] - level) * BlockSize;
			}
		}

		return ret;
	}

	//坡度计算
	vector<vector<vector<double>>> SlopeCalculation(const graph& g, double BlockLenth)
	{
		int row = g.size();
		int col = g[0].size();

		//ret[i][j]表示具体的点，而ret[i][j][0~3]分别表示向上下左右的坡度
		vector<vector<vector<double>>> ret(row,vector<vector<double>>(col,vector<double>(4)));

		auto Slope = [&](int CenterRow, int CenterCol)
			{
				vector<double> ans(4,-1);

				if (CenterRow > 0)
					ans[0] = atan2(g[CenterRow - 1][CenterCol] - g[CenterRow][CenterCol], BlockLenth);
				if (CenterRow < row - 1)
					ans[1] = atan2(g[CenterRow + 1][CenterCol] - g[CenterRow][CenterCol], BlockLenth);
				if (CenterCol > 0)
					ans[2] = atan2(g[CenterRow][CenterCol - 1] - g[CenterRow][CenterCol], BlockLenth);
				if (CenterCol < col - 1)
					ans[3] = atan2(g[CenterRow][CenterCol + 1] - g[CenterRow][CenterCol], BlockLenth);

				return ans;
			};

		for (int i = 0; i < row; i++)
		{
			for (int j = 0; j < col; j++)
			{
				ret[i][j] = Slope(i, j);
			}
		}

		return ret;
	}

	//洪水填充
	vector<vector<bool>> FloodFill(const graph& g, int SourceRow, int SourceCol)
	{
		vector<int> drow = { 1,-1,0,0 };
		vector<int> dcol = { 0,0,1,-1 };

		int row = g.size();
		int col = g[0].size();
		vector<vector<bool>> vis(row,vector<bool>(col));
		vector<vector<bool>> ret(row, vector<bool>(col));

		queue<pair<int, int>> que;
		que.push({ SourceRow,SourceCol });

		while (!que.empty())
		{
			auto [Row,Col] = que.front();
			double hight = g[Row][Col];

			for (int i = 0; i < 4; i++)
			{
				int NewRow = Row + drow[i];
				int NewCol = Col + dcol[i];
				vis[NewRow][NewCol] == true;

				if (NewRow >= 0 && NewRow < row && NewCol >= 0 && NewCol < col &&
					!vis[NewRow][NewCol]&&g[NewRow][NewCol] < hight)
				{
					que.push({ NewRow,NewCol });
					ret[NewRow][NewCol] = true;
				}
			}
		}

		return ret;
	}
};
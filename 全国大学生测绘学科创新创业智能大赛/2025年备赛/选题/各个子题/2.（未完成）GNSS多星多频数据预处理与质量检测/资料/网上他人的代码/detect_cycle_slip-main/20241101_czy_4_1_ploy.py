import numpy as np
from numpy.polynomial.polynomial import Polynomial
import matplotlib.pyplot as plt
import matplotlib
import os

# 创建输出文件夹 'res_ploy'，如果它不存在
output_folder = 'res_ploy'
if not os.path.exists(output_folder):
    os.makedirs(output_folder)

# 设置字体，避免输出图片中的中文无法显示
matplotlib.rcParams['font.sans-serif'] = ['SimHei']  # 使用黑体
matplotlib.rcParams['axes.unicode_minus'] = False  # 解决负号显示问题

# 读取数据
def read_data(filename):
    data = []
    with open(filename, 'r', encoding='utf-8', errors='ignore') as f:
        lines = f.readlines()[1:]
        for line in lines:
            values = line.split()
            week = int(values[0])
            seconds = int(values[1])
            pseudorange = float(values[2])
            l1_carrier = float(values[3])
            data.append((week, seconds, pseudorange, l1_carrier))
    return np.array(data)

# 拟合多项式
def fit_polynomial(x, y, order):
    poly = Polynomial.fit(x, y, order)
    return poly.convert()  # 转换为标准多项式形式

# 检测和修复周跳
def detect_and_repair(data, order=4, threshold=1.0):
    times = data[:, 1]
    phases = data[:, 3]
    repaired_phases = phases.copy()
    jump_times = []

    # 保存周跳信息
    with open(f"{output_folder}/探测到的周跳.txt", "w", encoding="utf-8") as file:
        for i in range(len(times) - order - 1):
            x = times[i:i + order + 1]
            y = phases[i:i + order + 1]
            poly = fit_polynomial(x, y, order)
            predicted_phase = poly(times[i + order + 1])

            actual_phase = phases[i + order + 1]
            if abs(actual_phase - predicted_phase) > threshold:
                jump_times.append(times[i + order + 1])
                file.write(f"周跳检测到: 时间点 {times[i + order + 1]}，原始L1载波相位测量观测值 {actual_phase}，拟合多项式修正后相位 {predicted_phase}\n")
                repaired_phases[i + order + 1] = predicted_phase

    return repaired_phases, jump_times

# 计算差分并绘图，标记周跳
def calculate_and_plot_differences(phases, times, threshold=2.0, repaired_phases=None, jump_times=None):
    differences = [phases]
    repaired_diffs = [repaired_phases] if repaired_phases is not None else None

    for n in range(1, 5):
        diff = np.diff(differences[-1])
        differences.append(diff)

        if repaired_phases is not None:
            repaired_diff = np.diff(repaired_diffs[-1])
            repaired_diffs.append(repaired_diff)

        # 绘制原始L1载波相位测量观测值的一次差（不显示阈值或周跳）
        if n == 1:
            plt.figure()
            plt.plot(times[:len(diff)], diff, label=f'对原始L1载波相位测量观测值的{n}次差', color='blue')
            plt.xlabel('周秒/s')
            plt.ylabel(f'{n}次差')
            plt.title(f'对L1载波相位测量观测值的{n}次差（周跳检测）')
            plt.legend()
            plt.savefig(f'{output_folder}/对L1载波相位测量观测值的{n}次差周跳检测.png')
            plt.close()
        else:
            # 保存原始差分图（显示阈值和周跳点）
            jumps = np.where(np.abs(diff) > threshold)[0]
            plt.figure()
            plt.plot(times[:len(diff)], diff, label=f'对原始L1载波相位测量观测值的{n}次差', color='blue')
            plt.scatter(times[jumps], diff[jumps], color='red', label='周跳', zorder=5, s=5)
            plt.axhline(y=threshold, color='orange', linestyle='--', label='阈值')
            plt.axhline(y=-threshold, color='orange', linestyle='--')
            plt.xlabel('周秒/s')
            plt.ylabel(f'{n}次差')
            plt.title(f'对L1载波相位测量观测值的{n}次差（周跳检测）')
            plt.legend()
            plt.savefig(f'{output_folder}/对L1载波相位测量观测值的{n}次差周跳检测.png')
            plt.close()

    # 绘制原始 L1 载波相位测量观测值图像（无阈值）
    plt.figure()
    plt.plot(times, phases, label='L1载波相位测量观测值', color='blue')
    plt.xlabel('周秒/s')
    plt.ylabel('L1载波相位测量观测值')
    plt.title('原始L1载波相位测量观测值图像')
    plt.legend()
    plt.savefig(f'{output_folder}/原始L1载波相位测量观测值图像.png')
    plt.close()

    # 绘制“检测出的周跳”图像，在原始观测值上标记四次差检测出的周跳
    if jump_times:
        plt.figure()
        plt.plot(times, phases, label='L1载波相位测量观测值', color='blue')
        plt.scatter(jump_times, phases[np.isin(times, jump_times)], color='red', label='周跳', zorder=5, s=5)
        plt.xlabel('周秒/s')
        plt.ylabel('L1载波相位测量观测值')
        plt.title('检测出的周跳')
        plt.legend()
        plt.savefig(f'{output_folder}/检测出的周跳.png')
        plt.close()

    # 绘制原始和修复后数据的对比图
    if repaired_phases is not None:
        plt.figure()
        plt.plot(times, phases, label='原始数据', color='blue')
        plt.plot(times, repaired_phases, label='拟合多项式修复后数据', color='green', linestyle='--')
        plt.xlabel('周秒/s')
        plt.ylabel('L1载波相位测量观测值')
        plt.title('原始数据和修复后数据的对比图')
        plt.legend()
        plt.savefig(f'{output_folder}/原始数据和修复后数据对比图.png')
        plt.close()

# 主程序
if __name__ == "__main__":
    filename = '观测值.txt'
    data = read_data(filename)

    # 设置周跳检测阈值
    threshold = 2.8

    # 进行周跳检测和修复
    repaired_phases, jump_times = detect_and_repair(data, order=4, threshold=threshold)

    # 计算并保存差分图像，标记周跳，并绘制原始和修复后的数据对比图
    calculate_and_plot_differences(data[:, 3], data[:, 1], threshold, repaired_phases, jump_times)

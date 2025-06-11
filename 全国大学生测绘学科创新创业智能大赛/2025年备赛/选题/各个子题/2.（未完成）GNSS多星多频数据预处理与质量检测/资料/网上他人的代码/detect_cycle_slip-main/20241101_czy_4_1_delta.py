import numpy as np
import matplotlib.pyplot as plt
import matplotlib
import os

# 创建输出文件夹 'res_delta'，如果它不存在
output_folder = 'res_delta'
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

# 计算差分并绘图，标记周跳
def calculate_and_plot_differences(phases, times, threshold=2.0):
    differences = [phases]
    
    # 保存周跳信息
    with open(f"{output_folder}/探测到的周跳.txt", "w", encoding="utf-8") as file:
        for n in range(1, 5):
            diff = np.diff(differences[-1])
            differences.append(diff)

            # 标记周跳点并输出到文件
            if n >= 2:  # 对第2、3、4次差分检测并标记周跳
                jumps = np.where(np.abs(diff) > threshold)[0]
                for jump in jumps:
                    # 获取修正前相位值
                    original_phase = differences[-2][jump]
                    # 记录到文件
                    file.write(f"周跳检测到: 时间点 {times[jump + n]}, ，原始L1载波相位测量观测值 {original_phase}, 相位4次差分值 {diff[jump]}\n")
            
            # 绘制差分图并标记周跳
            plt.figure()
            plt.plot(times[:len(diff)], diff, label=f'对原始L1载波相位测量观测值的{n}次差')
            
            if n > 1:
                plt.axhline(y=threshold, color='orange', linestyle='--', label='阈值')
                plt.axhline(y=-threshold, color='orange', linestyle='--')
                
                # 用红点标记周跳位置
                if n >= 2:
                    plt.scatter(times[jumps + n], diff[jumps], color='red', label='周跳', zorder=5, s=5)
            
            plt.xlabel('周秒/s')
            plt.ylabel(f'{n}次差')
            plt.title(f'对L1载波相位测量观测值的{n}次差（周跳检测）')
            plt.legend()
            plt.savefig(f'{output_folder}/对L1载波相位测量观测值的{n}次差周跳检测.png')
            plt.close()

    # 绘制原始 L1 载波相位测量观测值图像
    plt.figure()
    plt.plot(times, phases, label='L1载波相位测量观测值', color='blue')
    plt.xlabel('周秒/s')
    plt.ylabel('L1载波相位测量观测值')
    plt.title('原始L1载波相位测量观测值图像') 
    plt.legend()
    plt.savefig(f'{output_folder}/原始L1载波相位测量观测值图像.png')
    plt.close()

    # 绘制标记周跳的图像
    plt.figure()
    plt.plot(times, phases, label='L1载波相位测量观测值', color='blue')
    plt.scatter(times[jumps + 4], phases[jumps + 4], color='red', label='检测到的周跳', s=5)
    plt.xlabel('周秒/s')
    plt.ylabel('L1载波相位测量观测值')
    plt.title('检测到的周跳')
    plt.legend()
    plt.savefig(f'{output_folder}/检测到的周跳.png')
    plt.close()

# 主程序
if __name__ == "__main__":
    filename = '观测值.txt'
    data = read_data(filename)

    # 设置周跳检测阈值
    threshold = 2.8

    # 计算并保存差分图像，标记周跳，并绘制原始数据和检测到的周跳
    calculate_and_plot_differences(data[:, 3], data[:, 1], threshold)

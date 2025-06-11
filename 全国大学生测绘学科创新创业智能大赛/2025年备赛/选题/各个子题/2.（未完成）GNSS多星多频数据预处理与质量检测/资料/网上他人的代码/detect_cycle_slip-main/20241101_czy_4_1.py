import numpy as np
import matplotlib.pyplot as plt
import matplotlib
import os

# 创建输出文件夹 'res'，如果它不存在
output_folder = 'res'
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

# 构建 B 矩阵
def build_matrix_B(times, t0, order=4):
    B = np.zeros((len(times), order + 1))
    for i in range(len(times)):
        for j in range(order + 1):
            B[i, j] = (times[i] - t0) ** j
    return B

# 检测周跳并进行修正
def detect_cycle_slip(phases, times, threshold=1.0, m=6):
    with open(f"{output_folder}/探测到的周跳.txt", "w", encoding="utf-8") as file:
        detected_jumps = []
        i = 0
        while i < len(phases) - m:
            sample_phases = phases[i:i + m]
            sample_times = times[i:i + m]
            t0 = sample_times[0]
            
            # 构建 B 和 L 矩阵
            B = build_matrix_B(sample_times, t0)
            L = sample_phases.reshape(-1, 1)

            # 计算 X = (B.T * B)^(-1) * B.T * L
            BT_B_inv = np.linalg.inv(B.T @ B)
            X = BT_B_inv @ B.T @ L
            
            # 计算拟合值和误差
            predicted_phases = (B @ X).flatten()
            residuals = predicted_phases - sample_phases
            V = residuals.reshape(-1, 1)
            sigma = np.sqrt((V.T @ V) / (m - 5))[0, 0]
            
            # 打印用于拟合的6个周秒数据、误差项 V 和中误差 σ
            print(f"拟合使用的周秒数据: {sample_times}")
            print(f"误差项 V: {V.flatten()}")
            print(f"中误差 σ: {sigma}")

            if sigma < 0.1:
                # 预测下一个点
                next_time = times[i + m]
                B_next = np.array([(next_time - t0) ** j for j in range(5)])
                predicted_phase = B_next @ X
                actual_phase = phases[i + m]
                
                # 检查误差是否超出阈值，标记周跳
                if abs(predicted_phase - actual_phase) > threshold:
                    correction_value = actual_phase - predicted_phase
                    detected_jumps.append(i + m)
                    file.write(f"周跳检测到: 周秒 {times[i + m]}，原始L1载波相位测量观测值 {actual_phase}，拟合多项式预测相位 {predicted_phase}，误差 {correction_value}\n")
                
                # 移动窗口
                i += 1
            else:
                # 剔除最远的数据点，并重新拟合
                i += 1

    return detected_jumps

# 绘制差分图像
def plot_differences(phases, detected_jumps, times, order=4):
    titles = ['原始数据', '1次差分', '2次差分', '3次差分', '4次差分']
    
    # 原始数据与差分结果
    differences = [phases]
    for _ in range(order):
        differences.append(np.diff(differences[-1]))
    
    # 绘制每个差分图
    for i, diff_data in enumerate(differences):
        plt.figure(figsize=(10, 6))  # 为每个图形设置大小
        diff_times = times[:len(diff_data)]
        plt.plot(diff_times, diff_data, label=titles[i])
        
        if i > 1:
            plt.axhline(y=threshold, color='green', linestyle='--', label='阈值线(±{threshold})'.format(threshold=threshold))  # 绘制阈值线
            plt.axhline(y=-threshold, color='green', linestyle='--')  # 绘制阈值线
        
        # 标记周跳
        jump_times = [times[j] for j in detected_jumps if j < len(diff_data)]
        jump_values = [diff_data[j] for j in detected_jumps if j < len(diff_data)]
        plt.scatter(jump_times, jump_values, color='red', label='检测到的周跳', s=5)

        plt.legend()
        plt.ylabel(titles[i])
        plt.xlabel('周秒/s')
        plt.title(f'L1载波相位测量观测值的{titles[i]}')
        if(i>0):plt.savefig(f'{output_folder}/L1载波相位测量观测值{i}次差分图.png')  # 保存每个图形
        else:plt.savefig(f'{output_folder}/原始L1载波相位测量观测值.png')  # 显示第一个图形
        plt.close()


# 主程序
if __name__ == "__main__":
    filename = '观测值.txt'
    data = read_data(filename)

    # 设置周跳检测阈值
    threshold = 4

    # 提取时间和相位数据
    times = data[:, 1]
    phases = data[:, 3]

    # 检测并标记周跳
    detected_jumps = detect_cycle_slip(phases, times, threshold)

    # 绘制原始观测值图像和标记周跳的图像
    plt.figure()
    plt.plot(times, phases, label='L1载波相位测量观测值', color='blue')
    plt.scatter(times[detected_jumps], phases[detected_jumps], color='red', label='检测到的周跳', s=5)
    plt.xlabel('周秒/s')
    plt.ylabel('L1载波相位测量观测值')
    plt.title('检测到的周跳')
    plt.legend()
    plt.savefig(f'{output_folder}/检测到的周跳.png')
    plt.close()

    # 绘制差分图像
    plot_differences(phases, detected_jumps, times)
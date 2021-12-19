# This is a sample Python script.

# Press Shift+F10 to execute it or replace it with your code.
# Press Double Shift to search everywhere for classes, files, tool windows, actions, and settings.


'''
50 69 01 ff bc ff ea 10 30 37 4c cd ff ff d6 ff da 42 20 37 5b 2c 4f 4b
80 105 1 255 188 255 234 16 48 55 76 205 255 255 214 255 218 66 32 55 91 44 79 75


'''

import serial
import _thread
import time
import sys
import matplotlib.pyplot as plt
import socket

DATAlEN = 18
Acc = 8 * 9.8  # 量程8g
Int16min = 32768
Int16max = 32767
Int32min = 2147483648
Int32max = 2147483647

# 全局数据队列,先入先出
xA, yA, zA, quad = [0.] * 4
deviceNumber = ''

s = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
address = ('127.0.0.1', 31500)


def AccDataToDigital(data):
    '''
    加速度量程换算
    :param data:
    :return: N/s^2
    '''
    data = int('0x' + data, 16)
    signData = data * Acc / Int16max if (data < 0x8000) else (data - 0x10000) * Acc / Int16min
    return signData


def QuadDataToDigital(dataList):
    '''
    四元数换算
    :param dataList:
    :return:
    '''
    data = (int('0x' + dataList[0], 16),
            int('0x' + dataList[1], 16),
            int('0x' + dataList[2], 16),
            int('0x' + dataList[3], 16))

    signData = (data[0] / Int32max if (data[0] < 0x80000000) else (data[0] - 0x100000000) / Int32min,
                data[1] / Int32max if (data[1] < 0x80000000) else (data[1] - 0x100000000) / Int32min,
                data[2] / Int32max if (data[2] < 0x80000000) else (data[2] - 0x100000000) / Int32min,
                data[3] / Int32max if (data[3] < 0x80000000) else (data[3] - 0x100000000) / Int32min)

    return signData


def Calculate(device, DataList, delay):
    global xA, yA, zA, quad
    '''
    所有数据均为二进制比特流
    :param device: 设备号
    :param DataList: 数据列表
    :param delay: 延时
    :return:
    '''
    xAcc = DataList[0].hex() + DataList[1].hex()
    yAcc = DataList[2].hex() + DataList[3].hex()
    zAcc = DataList[4].hex() + DataList[5].hex()

    # x,y,z,w
    QuatationTuple = (DataList[6].hex() + DataList[7].hex() + DataList[8].hex() + b'\x00'.hex(),
                      DataList[9].hex() + DataList[10].hex() + DataList[11].hex() + b'\x00'.hex(),
                      DataList[12].hex() + DataList[13].hex() + DataList[14].hex() + b'\x00'.hex(),
                      DataList[15].hex() + DataList[16].hex() + DataList[17].hex() + b'\x00'.hex())
    xA = AccDataToDigital(xAcc)
    yA = AccDataToDigital(yAcc)
    zA = AccDataToDigital(zAcc)
    quad = QuadDataToDigital(QuatationTuple)

    return xA, yA, zA, quad


def getDataAndDraw(com):
    global xA, yA, zA, quad, deviceNumber
    ErrorFlag = False
    DataList = []

    startTime = time.time()
    Count = 0
    while Count<10000:
        DataList.clear()
        data = com.read()  # 阻塞等待
        if (data == b'P'):  # P
            nextdata = com.read()
            if (nextdata == b'i'):  # i
                deviceNumber = int(com.read()[0])  # 设备号
                # 数据部分,采集开始
                for dataIndex in range(DATAlEN):
                    DataList.append(com.read())
                delayTime = com.read()
                OTag = com.read()
                KTag = com.read()
                if (OTag + KTag == b'OK'):
                    xAcc, yAcc, zAcc, quad = Calculate(deviceNumber, DataList, delayTime)  # 一次数据采集完成
                    # print("device:--", deviceNumber, "--ACC: ", xAcc, "--", yAcc, "--", zAcc)
                    msg = str([deviceNumber, xA, yA, zA, quad])
                    s.sendto(msg.encode('utf-8'), address)
                    Count += 1
                else:
                    print("Error")
                    continue
        else:
            print("Error")
            continue
    endTime = time.time()

    print(Count, "--", endTime - startTime)


# Press the green button in the gutter to run the script.
if __name__ == '__main__':
    com = serial.Serial("COM6", 921600)
    _thread.start_new_thread(getDataAndDraw, (com,))
    while True:
        time.sleep(1)

    # address = ('127.0.0.1', 31500)
    # with socket.socket(socket.AF_INET, socket.SOCK_DGRAM) as s:
    #     while True:
    #         msg = str([deviceNumber, xA, yA, zA, quad])
    #         s.sendto(msg.encode('utf-8'), address)

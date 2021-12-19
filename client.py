# import socket
# import threading
# import time
#
# s = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
# # 监听端口:
# s.bind(('127.0.0.1', 9999))
# s.listen(5)
# print('Waiting for connection...')
#
#
# def tcplink():
#     print('Accept new connection from %s:%s...')
#     sock.send(b'Welcome!')
#     while True:
#         data, addr = s.recvfrom(2048)
#         if not data:
#             print("client has exist")
#             break
#         print("received:", data, "from", addr)
#     s.close()
#
#
# while True:
#     # 接受一个新连接:
#     sock, addr = s.accept()
#     # 创建新线程来处理TCP连接:
#     t = threading.Thread(target=tcplink, args=(sock, addr))
#     t.start()
#
import socket
import matplotlib.pyplot as plt
plt.figure()
plt.ion()
plt.show()
dataRect = [[[],[],[]] for i in range(7)]

def drawPicture(msg):
    data = eval(msg)
    deviceNumber = data[0]

    dataRect[deviceNumber][0].append(data[1])
    dataRect[deviceNumber][1].append(data[2])
    dataRect[deviceNumber][2].append(data[3])

    dataRect[deviceNumber][0].append(data[1])
    dataRect[deviceNumber][1].append(data[2])
    dataRect[deviceNumber][2].append(data[3])

    # plt.clf()

    # for i in range(7):
    #x轴
    plt.subplot(3,7,eval("{}".format(deviceNumber+1)))
    plt.plot(range(len(dataRect[deviceNumber][0])),dataRect[deviceNumber][0])
    plt.title("device: {}--x".format(deviceNumber))

    #y轴
    plt.subplot(3,7,eval("{}".format(deviceNumber+7+1)))
    plt.plot(range(len(dataRect[deviceNumber][1])), dataRect[deviceNumber][1])
    plt.title("device: {}--y".format(deviceNumber))

    #z轴
    plt.subplot(3,7,eval("{}".format(deviceNumber+14+1)))
    plt.plot(range(len(dataRect[deviceNumber][2])), dataRect[deviceNumber][2])
    plt.title("device: {}--z".format(deviceNumber))

    plt.pause(0.01)




address = ('127.0.0.1', 31500)
s = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
s.bind(address)

while True:
    data, addr = s.recvfrom(2048)
    if not data:
        print("client has exist")
        break
    receieve = data.decode('utf-8')
    drawPicture(receieve)
    # print("received:", data.decode('utf-8'), "from", addr)


s.close()

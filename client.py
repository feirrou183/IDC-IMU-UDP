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

address = ('127.0.0.1', 31500)
s = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
s.bind(address)

while True:
    data, addr = s.recvfrom(2048)
    if not data:
        print("client has exist")
        break
    print("received:", data.decode('utf-8'), "from", addr)

s.close()

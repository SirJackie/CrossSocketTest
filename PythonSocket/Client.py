import socket


def SafeSendMessage(s, msgStr):
    s.send(f"{len(msgStr):<10}".encode("utf-8"))
    s.send(msgStr.encode("utf-8"))


def SafeRecvMessage(s):
    msgLen = int(s.recv(10).decode("utf-8").strip())
    return s.recv(msgLen).decode("utf-8")


s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
s.connect(("127.0.0.1", 12345))

for i in range(0, 10):
    SafeSendMessage(s, "get message please")
    msg = SafeRecvMessage(s)
    print(msg)

SafeSendMessage(s, "close socket please")

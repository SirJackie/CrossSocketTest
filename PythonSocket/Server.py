import socket


def SafeSendMessage(s, msgStr):
    s.send(f"{len(msgStr):<10}".encode("utf-8"))
    s.send(msgStr.encode("utf-8"))


def SafeRecvMessage(s):
    msgLen = int(s.recv(10).decode("utf-8").strip())
    return s.recv(msgLen).decode("utf-8")


s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
s.bind(("127.0.0.1", 12345))
s.listen(1)

while True:
    clientSocket, address = s.accept()
    print(f"Client Accepted: {address}")
    while True:
        msg = SafeRecvMessage(clientSocket)
        if msg == "get message please":
            SafeSendMessage(clientSocket, "Hello World!")
        elif msg == "close socket please":
            break

    clientSocket.close()

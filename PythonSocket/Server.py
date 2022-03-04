from JSock import JSock

jsock = JSock()
jsock.StartServer(12345)

while True:
    jsock.AcceptClient()

    while True:
        msg = jsock.RecvStr()
        if msg == "get message please":
            jsock.SendStr("Hello World!")
        elif msg == "close socket please":
            jsock.CloseClient()
            break

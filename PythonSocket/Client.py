from JSock import JSock

jsock = JSock()
jsock.Connect("127.0.0.1", 12345)

for i in range(0, 10):
    jsock.SendStr("get message please")
    msg = jsock.RecvStr()
    print(msg)

jsock.SendStr("close socket please")
jsock.Close()

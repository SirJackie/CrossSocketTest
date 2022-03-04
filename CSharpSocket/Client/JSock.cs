using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;

class JSock
{
    bool debug;
    string mode;
    TcpClient s;
    NetworkStream stream;
    //clientSocket = None
    //address = None

    public JSock(bool debug_=true)
    {
        debug = debug_;
    }

    public void SendStrInsecurely(string msg)
    {
        byte[] msgBin = Encoding.UTF8.GetBytes(msg);
        stream.Write(msgBin, 0, msgBin.Length);
    }

    public string RecvStrInsecurely(int maxLen)
    {
        byte[] buffer = new byte[maxLen];
        stream.Read(buffer, 0, buffer.Length);

        // Count the Length of the Byte Array received
        int recv = 0;
        foreach (byte b in buffer)
        {
            if (b != 0)
            {
                recv++;
            }
        }

        return Encoding.UTF8.GetString(buffer, 0, recv);
    }

    public void SendStr(string msg)
    {
        SendStrInsecurely(msg.Length.ToString().PadRight(10));
        SendStrInsecurely(msg);
    }

    public string RecvStr()
    {
        int msgLen = int.Parse(RecvStrInsecurely(10).Trim());
        return RecvStrInsecurely(msgLen);
    }

    public void Connect(string ip, int port)
    {
        mode = "Client";
        while (true)
        {
            try
            {
                if (debug)
                {
                    Console.WriteLine(
                        "Connecting: (" + ip + ", " + port.ToString() + ")"
                    );
                }
                s = new TcpClient(ip, port);
                stream = s.GetStream();
                break;
            }
            catch (Exception e)
            {
                if (debug)
                {
                    Console.WriteLine("Failed to Connect, Reconnecting...");
                }
            }
        }
    }

    public void Close()
    {
        stream.Close();
        s.Close();
    }
}

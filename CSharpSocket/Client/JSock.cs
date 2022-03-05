using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;

class JSock
{
    bool debug;
    string mode;
    TcpClient client;
    TcpListener server;
    NetworkStream stream;

    public JSock(bool debug_=true)
    {
        debug = debug_;
    }

    public void StartServer(int port, int maxConnections=32)
    {
        mode = "Server";
        server = new TcpListener(
            System.Net.IPAddress.Any,
            port
        );
        server.Start();
        if (debug)
        {
            Console.WriteLine("Server Started at Port: " + port.ToString());
        }
    }

    public void AcceptClient()
    {
        if (mode != "Server")
        {
            throw new Exception(
                "You're trying to accept a client using a client socket."
            );
        }

        client = server.AcceptTcpClient();

        if (client.Client.RemoteEndPoint != null)
        {
            // If there is some informations to show
            IPEndPoint clientInfo =
                (IPEndPoint)client.Client.RemoteEndPoint;

            if (debug)
            {
                Console.WriteLine(
                    "Client Accepted: ('" +
                    clientInfo.Address.ToString() + "', " +
                    clientInfo.Port.ToString() + ")"
                );
            }
        }

        stream = client.GetStream();
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
                client = new TcpClient(ip, port);
                stream = client.GetStream();
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
        client.Close();
    }
}

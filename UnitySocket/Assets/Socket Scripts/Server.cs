using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    class Server
    {
        public static void Main(string[] args)
        {
            JSock jsock = new JSock();
            jsock.StartServer(12345);

            while (true)
            {
                jsock.AcceptClient();

                while (true)
                {
                    string msg = jsock.RecvStr();
                    if(msg == "get message please")
                    {
                        jsock.SendStr("Hello World!");
                    }
                    else if (msg == "close socket please")
                    {
                        jsock.Close();
                        break;
                    }
                }
            }
        }
    }
}

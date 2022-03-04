using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    class Client
    {

        public static void Main(string[] arg)
        {
            JSock jsock = new JSock();
            jsock.Connect("127.0.0.1", 12345);

            for (int i = 0; i < 10; i++)
            {
                jsock.SendStr("get message please");
                string msg = jsock.RecvStr();
                Console.WriteLine(msg);
            }

            jsock.SendStr("close socket please");
            jsock.Close();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class Server : MonoBehaviour {

	Thread sockThread;

	// Use this for initialization
	void Start () {
		sockThread = new Thread (new ThreadStart (SockMain));
		sockThread.IsBackground = true;  // Background Thread
		sockThread.Start ();
	}

	public static void SockMain()
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

	void OnDestroy(){
		sockThread.Abort ();
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class Client : MonoBehaviour {

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
		jsock.Connect("127.0.0.1", 12345);

		for (int i = 0; i < 10; i++)
		{
			jsock.SendStr("get message please");
			string msg = jsock.RecvStr();
			Debug.Log(msg);
		}

		jsock.SendStr("close socket please");
		jsock.Close();
	}

	void OnDestroy(){
		sockThread.Abort ();
	}
}

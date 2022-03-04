using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class Server : MonoBehaviour {

	public static void SendMessage(NetworkStream stream, string msg){
		byte[] msgBin = Encoding.UTF8.GetBytes (msg);
		stream.Write (msgBin, 0, msgBin.Length);
	}

	public static string RecvMessage(NetworkStream stream, int maxLen){
		byte[] buffer = new byte[maxLen];
		stream.Read (buffer, 0, buffer.Length);

		// Count the Length of the Byte Array received
		int recv = 0;
		foreach (byte b in buffer) {
			if (b != 0) {
				recv++;
			}
		}

		return Encoding.UTF8.GetString (buffer, 0, recv);
	}

	public static void SafeSendMessage(NetworkStream stream, string msg){
		SendMessage (stream, msg.Length.ToString ().PadRight (10));
		SendMessage(stream, msg);
	}

	public static string SafeRecvMessage(NetworkStream stream){
		int msgLen = int.Parse (RecvMessage (stream, 10).Trim ());
		return RecvMessage(stream, msgLen);
	}

	// Use this for initialization
	void Start () {

		Thread networkThread = new Thread(new ThreadStart(NetMain));
		networkThread.IsBackground = true;
		networkThread.Start ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void NetMain () {
		// --------------------------------------------------------------------------
		Debug.Log ("Network Server Thread Started, Creating Server...");
		TcpListener listener = new TcpListener (
			System.Net.IPAddress.Any,
			12345
		);
		listener.Start ();
		Debug.Log ("Server Created, Port: " + 12345.ToString ());
		// --------------------------------------------------------------------------

		while (true) {
			TcpClient client = listener.AcceptTcpClient ();

			if (client.Client.RemoteEndPoint != null) {
				// If there is some informations to show
				IPEndPoint clientInfo = 
					(IPEndPoint)client.Client.RemoteEndPoint;
				
				Debug.Log (
					"Client Accepted: ('" +
					clientInfo.Address.ToString () + "', " +
					clientInfo.Port.ToString () + ")"
				);
			}

			NetworkStream stream = client.GetStream ();
			while (true) {
				string msg = SafeRecvMessage (stream);
				if (msg == "get message please") {
					SafeSendMessage (stream, "Hello World!");
				} else if (msg == "close socket please") {
					break;
				}
			}

			stream.Close ();
			client.Close ();
		}
	}
}

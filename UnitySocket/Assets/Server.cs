using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;

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

	TcpListener listener;
	TcpClient client;
	NetworkStream stream;

	// Use this for initialization
	void Start () {

		// Start Server
		listener = new TcpListener (
			System.Net.IPAddress.Any,
			12345
		);
		listener.Start ();

		// Accept Client and Show Info
		client = listener.AcceptTcpClient ();
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
		stream = client.GetStream ();

	}

	bool noLongerUpdate = false;
	
	// Update is called once per frame
	void Update () {
		if (noLongerUpdate == false) {
			string msg = SafeRecvMessage (stream);
			if (msg == "get message please") {
				SafeSendMessage (stream, "Hello World!");
			} else if (msg == "close socket please") {
				stream.Close ();
				client.Close ();
				noLongerUpdate = true;
			}
		}

	}
}

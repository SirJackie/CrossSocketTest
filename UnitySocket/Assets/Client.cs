using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;

public class Client : MonoBehaviour {

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
		SendMessage (stream, msg);
	}

	public static string SafeRecvMessage(NetworkStream stream){
		int msgLen = int.Parse (RecvMessage (stream, 10).Trim ());
		return RecvMessage (stream, msgLen);
	}

	TcpClient client;
	NetworkStream stream;

	// Use this for initialization
	void Start () {
		client = new TcpClient ("127.0.0.1", 12345);
		stream = client.GetStream ();
	}

	int count = 0;
	
	// Update is called once per frame
	void Update () {
		if (count == 11) {
			return;
		}

		if (count == 10) {
			SafeSendMessage (stream, "close socket please");
			count++;
			return;
		}

		SafeSendMessage (stream, "get message please");
		string msg = SafeRecvMessage (stream);
		Debug.Log (msg);

		count++;
	}
}

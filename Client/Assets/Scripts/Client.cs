using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using UnityEngine;

public class Client
{
    private Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    private Player player;
    public string message="";
    public string lastMessage="";
    // Start is called before the first frame update
        public Client(Player pl)
        {
            Debug.Log("First thread:" + Thread.CurrentThread.ManagedThreadId);
            player=pl;
        }
        public void SetupClient()
        {
            try
            {
                _clientSocket.Connect(IPAddress.Loopback, 100);
                Debug.Log("Connected");
                LoopConnect();
            }
            catch (SocketException)
            {
                Debug.Log("Failed");
            }

        }

        private void LoopConnect()
        {
            Thread thread = new Thread(GetMessage);
            thread.Start();
        }

        public void SendMessage(string req)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(req);
            _clientSocket.Send(buffer);
        }

        public void GetMessage()
        {
            Debug.Log("Second thread:" + Thread.CurrentThread.ManagedThreadId);
            byte[] receivedBuf = new byte[1024];
            while (true)
            {
                int rec = _clientSocket.Receive(receivedBuf);
                byte[] data = new byte[rec];
                Array.Copy(receivedBuf, data, rec);
                string mes=Encoding.ASCII.GetString(data);
                Debug.Log(mes);
                lock(message){
                message=mes;
                }
            }
        }

        public void Disconnect()
        {
            Debug.Log("Disconnect");
            _clientSocket.Disconnect(false);
        }
}

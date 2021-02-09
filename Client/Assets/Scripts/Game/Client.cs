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
    public Queue<(Socket,string)> requestQueue=new Queue<(Socket,string)>();

        public Client()
        {
        
            var date=DateTime.Now.ToString().Replace(":"," ");
        }

        public void SetupClient()
        {
            Thread thread=new Thread(LoopConnect);
            thread.Start();
        }
        public void LoopConnect()
        {
            string ip=ConnectButton.ip!=""?ConnectButton.ip:"127.0.0.1";
            string portString=ConnectButton.port!=""?ConnectButton.port:"100";
            int port = Int32.Parse(portString);
            //controller.AddRequest(new Action(()=>controller.SetMessageForPlayer($"{ip}:{port} Connecting...")));
            AddRequest(_clientSocket,$"MessageForPlayer:{ip};{port} Connecting...");
            Debug.Log("He");
            bool connected=false;
            int attempts=0;
            while(connected==false)
            {
                try
                {
                    _clientSocket.Connect(IPAddress.Parse(ip), port);
                    //controller.AddRequest(new Action(()=>controller.SetMessageForPlayer("Connected. Waiting for other players")));
                    AddRequest(_clientSocket,"MessageForPlayer:Connected. Waiting for other players");
                    SendMessage("ClientConnected:"+ConnectButton.nick);
                    connected=true;
                    LoopGetMessage();
                }
                catch (SocketException)
                {
                    attempts++;
                }
                //controller.AddRequest(new Action(()=>controller.SetMessageForPlayer($"{ip}: Connection attempt: {attempts}")));
                AddRequest(_clientSocket,$"MessageForPlayer:{ip};{port} Connection attempt; {attempts}");
            }
        }

        private void LoopGetMessage()
        {
            Thread thread = new Thread(GetMessage);
            thread.Start();
        }

        public void SendMessage(string req)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(req);
            _clientSocket.Send(buffer);
        }

        public void GetMessage()
        {
            byte[] receivedBuf = new byte[1024];
            try{
                while (true)
                {
                    int rec = _clientSocket.Receive(receivedBuf);
                    byte[] data = new byte[rec];
                    Array.Copy(receivedBuf, data, rec);
                    string mes=Encoding.UTF8.GetString(data);
                    //controller.AddRequest(new Action(()=>controller.Reaction(_clientSocket,mes)));
                    AddRequest(_clientSocket,mes);
                    Debug.Log("Get message: "+mes);
                }
            }
            catch(SocketException)
            {
                //controller.AddRequest(new Action(()=>controller.SetMessageForPlayer("Connection problem")));
                AddRequest(_clientSocket,"MessageForPlayer:Connection problem");
            }
        }
        public void Disconnect()
        {
            Debug.Log("Disconnect");
            if(_clientSocket.Connected)_clientSocket.Disconnect(false);
        }

        public void AddRequest(Socket soc, string mes)
        {
            requestQueue.Enqueue((soc,mes));
        }
}

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
    // Start is called before the first frame update
        public Client(Player pl)
        {
            player=pl;
        }

        public void SetupClient()
        {
            Thread thread=new Thread(LoopConnect);
            thread.Start();
        }
        public void LoopConnect()
        {
            bool connected=false;
            int attempts=1;
            while(connected==false)
            {
                player.AddRequest(new Action(()=>player.SetMessageForPlayer($"Connection attempt: {attempts}")));
                try
                {
                    _clientSocket.Connect(IPAddress.Parse("127.0.0.1"), 100);
                    //_clientSocket.Connect(IPAddress.Parse("25.97.182.10"), 100);
                    player.AddRequest(new Action(()=>player.SetMessageForPlayer("Connected. Waiting for other players")));
                    SendMessage("ClientConnected");
                    connected=true;
                    LoopGetMessage();
                }
                catch (SocketException)
                {
                    attempts++;
                    Thread.Sleep(1000);
                }
            }
        }

        private void LoopGetMessage()
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
            byte[] receivedBuf = new byte[1024];
            try{
                while (true)
                {
                    int rec = _clientSocket.Receive(receivedBuf);
                    byte[] data = new byte[rec];
                    Array.Copy(receivedBuf, data, rec);
                    string mes=Encoding.ASCII.GetString(data);
                    player.AddRequest(new Action(()=>player.Reaction(_clientSocket,mes)));
                }
            }
            catch(SocketException)
            {
                player.AddRequest(new Action(()=>player.SetMessageForPlayer("Connection problem")));
            }
        }
        public void Disconnect()
        {
            Debug.Log("Disconnect");
            _clientSocket.Disconnect(false);
        }
}

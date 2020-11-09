using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    class Server
    {
        private List<Socket> _clientSockets = new List<Socket>();
        private Socket _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private Controller controller;

        public Server(Controller cont)
        {
            controller = cont;
        }

        public void SetupServer()
        {
            Console.WriteLine("Setting up server...");
            _serverSocket.Bind(new IPEndPoint(IPAddress.Any, 100));
            _serverSocket.Listen(1);
            Thread acceptThread = new Thread(AcceptLoop);
            acceptThread.Start();
        }
        public void AcceptLoop()
        {
            while (true)
            {
                Socket socket = _serverSocket.Accept();
                Console.WriteLine("Connected: " + socket.RemoteEndPoint);
                _clientSockets.Add(socket);
                //SendMessage(socket, "Congratulation, you are in the network");
                Console.WriteLine("Client connected");
                Thread receiveThread = new Thread(() => ReceiveLoop(socket));
                receiveThread.Start();
            }
        }

        private void ReceiveLoop(Socket socket)
        {
            byte[] receivedBuf = new byte[1024];
            while (true)
            {
                int rec = socket.Receive(receivedBuf);
                byte[] data = new byte[rec];
                Array.Copy(receivedBuf, data, rec);
                string text = Encoding.ASCII.GetString(data);
                Console.WriteLine("From " + socket.RemoteEndPoint + ": " + text);
                controller.Reaction(text);
            }
        }
        public void SendMessage(Socket s, string req)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(req);
            s.Send(buffer);
        }
        public void SendBroadcast(string req)
        {
            Console.WriteLine("To send: "+ req);
            byte[] buffer = Encoding.ASCII.GetBytes(req);
            foreach(Socket s in _clientSockets)
            s.Send(buffer);
        }

        public List<Socket> getClients()
        {
            return _clientSockets;
        }
    }
}

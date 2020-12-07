using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;

namespace Server
{
    class Server
    {
        private List<Socket> _clientSockets = new List<Socket>();
        public Dictionary<char, Socket> clientByPosition = new Dictionary<char, Socket>();
        private Socket _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private Controller controller;

        public Server(Controller cont)
        {
            controller = cont;
        }

        public void SetupServer()
        {
            // text = File.ReadAllText(@"ip.txt");
            List<string> datas = new List<string>(File.ReadLines(@"ip.txt"));
            IPAddress ip = IPAddress.Parse(datas[0]);
            Console.WriteLine($"Setting up server {ip}:{datas[1]}");
            _serverSocket.Bind(new IPEndPoint(ip, Int32.Parse(datas[1])));
            //_serverSocket.Bind(new IPEndPoint(IPAddress.Parse("25.97.182.10"), 100));
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
                controller.Reaction(socket,text);
            }
        }
        public void SendMessage(Socket s, string req)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(req);
            Console.WriteLine("Sending to "+s.RemoteEndPoint.ToString()+": " + req);
            s.Send(buffer);
        }
        public void SendBroadcast(string req)
        {
            Console.WriteLine("Send Broadcast: "+ req);
            byte[] buffer = Encoding.ASCII.GetBytes(req);
            foreach(Socket s in _clientSockets)
            s.Send(buffer);
        }

        public List<Socket> GetClients()
        {
            return _clientSockets;
        }

        public int GetNumberOfClients()
        {
            return _clientSockets.Count;
        }
    }
}

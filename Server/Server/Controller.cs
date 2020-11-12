using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    class Controller
    {
        CardDeck cd;
        Server server;
        public Mutex mutex = new Mutex();
        int temp = 0;

        public Controller()
        {
            cd = new CardDeck();
            server = new Server(this);
        }

        public void Start()
        {
            cd.CreateDeck();
            cd.Shuffle();

            server.SetupServer();
            Console.ReadLine();
            for(int i=0;i<4;i++)
            {
                //server.SendMessage(server.GetClients()[i], cd.Get13Cards());
            }
            Console.ReadLine();
        }

        public void Reaction(Socket socket, string message)
        {
            Console.WriteLine("From " + socket.RemoteEndPoint.ToString() + ": " + message);
            mutex.WaitOne();
            Console.WriteLine("Reakcja");
            if(message=="ClientConnected")
            {
                if(server.GetNumberOfClients()==4)
                {
                    Console.WriteLine("4 players ready. Sending cards");
                    SendCards();
                    temp = 0;
                }   
                else if(server.GetNumberOfClients()>4)
                {
                    server.SendMessage(socket, "MaxPlayers");
                }
            }
            else if (message == "CardsAcquired")
            {
                temp++;
                if (temp == 4) server.SendBroadcast("masakra");
                //starting the game
            }
            mutex.ReleaseMutex();

        }

        public void SendCards()
        {
            foreach (Socket s in server.GetClients())
            {
                Console.WriteLine("Sending cards to: " + s.RemoteEndPoint.ToString());
                server.SendMessage(s, "Cards:" + cd.Get13Cards());
            }
        }
    }
}

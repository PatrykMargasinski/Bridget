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
        AuctionPhase auctionPhase;
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
            Console.ReadLine();
        }

        public void Reaction(Socket socket, string message)
        {
            mutex.WaitOne();
            Console.WriteLine("Reakcja");
            if(message=="ClientConnected")
            {
                server.SendBroadcast("NewPlayerJoined");
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
                if (temp == 4)
                {
                    server.SendBroadcast("AuctionPhase");
                }
            }
            mutex.ReleaseMutex();

        }

        public void SendCards()
        {
            foreach (Socket s in server.GetClients())
            {
                server.SendMessage(s, "Cards:" + cd.Get13Cards());
            }
        }
    }
}

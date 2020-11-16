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
        List<Player> players = new List<Player>();
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
            string[] mes = message.Split(':');
            if(mes[0]=="ClientConnected")
            {
                Player newPlayer = new Player(mes[1]);
                players.Add(new Player(mes[1]));
                server.SendBroadcast("NewPlayerJoined:"+newPlayer.nick);
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
            else if (mes[0] == "CardsAcquired")
            {
                temp++;
                if (temp == 4)
                {
                    server.SendBroadcast("Bidding");
                }
            }
            mutex.ReleaseMutex();
        }

        public void SendCards()
        {
            foreach (Socket s in server.GetClients())
            {
                server.SendMessage(s, "Cards" + cd.Get13Cards());
            }
        }
    }
}

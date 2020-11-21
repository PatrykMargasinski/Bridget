using System;
using System.Collections.Generic;
using System.Linq;
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
        AuctionPhase auctionPhase;
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
                players.Add(newPlayer);
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
                    SendPlayersNicksAndPositions();
                    auctionPhase = new AuctionPhase('N');
                    server.SendBroadcast("Bidding:N:1:BA");
                }
            }
            else if (mes[0] == "Bid")
            {
                if(mes[2]=="Pass")
                {
                    auctionPhase.passCount++;
                    if (auctionPhase.passCount < 3) server.SendBroadcast($"Bidding:{auctionPhase.GetCurrent()}:{auctionPhase.bid}");
                    else if (auctionPhase.passCount == 3) server.SendBroadcast($"Game");
                    else throw (new ArgumentException("Why are there 4 passes?"));
                }
                else if(mes[2]=="Counter")
                {
                    auctionPhase.counter = auctionPhase.players[(3 + auctionPhase.players.ToList().IndexOf(mes[1][0]))%4];
                    auctionPhase.passCount = 0;
                    server.SendBroadcast($"Bidding:{auctionPhase.GetCurrent()}:{auctionPhase.bid}");
                }
                else if(mes[2]=="Recounter")
                {
                    auctionPhase.recounter = auctionPhase.counter;
                    auctionPhase.passCount = 0;
                    server.SendBroadcast($"Bidding:{auctionPhase.GetCurrent()}:{auctionPhase.bid}");
                }
                else
                {
                    auctionPhase.counter = '0';
                    auctionPhase.recounter = '0';
                    auctionPhase.passCount = 0;
                    auctionPhase.bid = mes[3] + ":" + mes[4];
                    server.SendBroadcast($"Bidding:{auctionPhase.GetCurrent()}:{auctionPhase.bid}");
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

        public void SendPlayersNicksAndPositions()
        {
            StringBuilder mes = new StringBuilder("Players");
            foreach(Player p in players) mes.Append($":{p.nick}:{p.position}");
            Console.WriteLine(mes.ToString());
            server.SendBroadcast(mes.ToString());
        }
    }
}

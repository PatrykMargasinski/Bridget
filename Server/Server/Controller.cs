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
                    server.SendBroadcast($"Bidding:{auctionPhase.GetNext()}:1:BA:Auction phase started: ");
                }
            }
            else if (mes[0] == "Bidding")
            {
                if(mes[2]=="Pass")
                {
                    auctionPhase.passCount++;
                    if (auctionPhase.passCount < 3) server.SendBroadcast($"Bidding:{auctionPhase.GetNext()}:{auctionPhase.bid}:{mes[1]}:passed");
                    else if (auctionPhase.passCount == 3) 
                    {
                        StringBuilder mesForGame = new StringBuilder();
                        mesForGame.Append($"Game starts with contract {auctionPhase.bid} for team {auctionPhase.GetCurrent()}{auctionPhase.GetPartner()}");
                        if (auctionPhase.counter != '0' && auctionPhase.recounter == '0') mesForGame.Append(" with counter");
                        else if (auctionPhase.counter != '0' && auctionPhase.recounter != '0') mesForGame.Append(" with recounter");
                        mesForGame.Append("\nFirst player is "+auctionPhase.playerWithFirstColor);
                        server.SendBroadcast(mesForGame.ToString());
                    }
                    else throw (new ArgumentException("Why are there 4 passes?"));
                }
                else if(mes[2]=="Counter")
                {
                    auctionPhase.counter = auctionPhase.players[(3 + auctionPhase.players.ToList().IndexOf(mes[1][0]))%4];
                    Console.WriteLine("Counter:" + auctionPhase.counter);
                    auctionPhase.passCount = 0;
                    server.SendBroadcast($"Bidding:{auctionPhase.GetNext()}:{auctionPhase.bid}:{mes[1]}:countered");
                }
                else if(mes[2]=="Recounter")
                {
                    auctionPhase.recounter = auctionPhase.counter;
                    Console.WriteLine("Recounter:" + auctionPhase.recounter);
                    auctionPhase.passCount = 0;
                    server.SendBroadcast($"Bidding:{auctionPhase.GetNext()}:{auctionPhase.bid}:{mes[1]}:recountered");
                }
                else
                {
                    auctionPhase.counter = '0';
                    auctionPhase.recounter = '0';
                    auctionPhase.passCount = 0;
                    if (auctionPhase.firstColor != mes[4]) 
                    {
                        auctionPhase.firstColor = mes[4];
                        auctionPhase.playerWithFirstColor = mes[1][0];
                    }
                    auctionPhase.bid = mes[3] + ":" + mes[4];
                    server.SendBroadcast($"Bidding:{auctionPhase.GetNext()}:{auctionPhase.bid}:{mes[1]}:bid {mes[3]}{mes[4]}");
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

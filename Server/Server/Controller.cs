using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    public class Controller
    {
        CardDeck cd;
        Server server;
        public Mutex mutex = new Mutex();
        List<Player> players = new List<Player>();
        List<Player> playersGeneral = new List<Player>();
        List<int> playersPoints = new List<int>();
        Dictionary<char, Player> playersByPosition = new Dictionary<char, Player>();
        AuctionPhase auctionPhase;
        GamePhase gamePhase;
        PlayersConfigurations playerConfigurations = new PlayersConfigurations();
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
            if (mes[0] == "ClientConnected")
            {
                playersGeneral.Add(new Player(mes[1]));
                playersPoints.Add(0);
                server.clientByNick.Add(playersGeneral[^1].nick, socket);
                server.SendBroadcast("NewPlayerJoined:" + playersGeneral[^1].nick);
                if (server.GetNumberOfClients() == 4)
                {
                    SetPlayerByConfiguration(playerConfigurations.GetConfiguration());
                    SetPositions();
                    Console.WriteLine("4 players ready. Sending cards");
                    SendCards();
                    temp = 0;
                }
                else if (server.GetNumberOfClients() > 4)
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
                    server.SendBroadcast($"Bidding:{auctionPhase.GetNext()}:0:BA:Auction phase started: ");
                }
            }
            else if (mes[0] == "Bidding")
            {
                if (mes[2] == "Pass")
                {
                    auctionPhase.passCount++;
                    if (auctionPhase.passCount < 3) server.SendBroadcast($"Bidding:{auctionPhase.GetNext()}:{auctionPhase.bid}:{mes[1]}:passed");
                    else if (auctionPhase.passCount == 3)
                    {
                        StringBuilder mesForGame = new StringBuilder();
                        //bid - auctionPhase.bid np. 2:C
                        //team {auctionPhase.GetNext/Current(), GetPartner() np N/S
                        //auctionPhase.counter/recounter
                        //first player - playerWithFirstColor
                        gamePhase = new GamePhase
                            (auctionPhase.bid,
                            auctionPhase.playerWithFirstColor,
                            auctionPhase.GetPartner(auctionPhase.playerWithFirstColor),
                            auctionPhase.counter,
                            auctionPhase.recounter);
                        int counterOrRecounter = 0;
                        if (gamePhase.recounter == true) counterOrRecounter = 2;
                        else if (gamePhase.counter == true) counterOrRecounter = 1;
                        server.SendBroadcast($"GamePhase:Initialization:{gamePhase.dummy}:{gamePhase.bid.Replace(":", "")}:{gamePhase.GetContractTeam()}:{counterOrRecounter}");
                    }
                    else throw new ArgumentException("Why are there 4 passes?");
                }
                else if (mes[2] == "Counter")
                {
                    auctionPhase.counter = auctionPhase.players[(3 + auctionPhase.players.ToList().IndexOf(mes[1][0])) % 4];
                    Console.WriteLine("Counter:" + auctionPhase.counter);
                    auctionPhase.passCount = 0;
                    server.SendBroadcast($"Bidding:{auctionPhase.GetNext()}:{auctionPhase.bid}:{mes[1]}:countered");
                }
                else if (mes[2] == "Recounter")
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
            else if (mes[0] == "GamePhase")
            {
                if (mes[1] == "DummyCards")
                {
                    StringBuilder builder = new StringBuilder("GamePhase:DummyCards");
                    foreach (string s in mes.Skip(2))
                    {
                        builder.Append(":" + s);
                    }
                    foreach (char pos in server.clientByPosition.Keys)
                    {
                        if (pos != gamePhase.dummy) server.SendMessage(server.clientByPosition[pos], builder.ToString());
                    }
                    temp = 0;
                }
                else if (mes[1] == "PartnerCards")
                {
                    StringBuilder builder = new StringBuilder("GamePhase:PartnerCards");
                    foreach (string s in mes.Skip(2))
                    {
                        builder.Append(":" + s);
                    }
                    server.SendMessage(server.clientByPosition[gamePhase.dummy], builder.ToString());
                    temp = 0;
                }
                else if (mes[1] == "ReadyToPlay")
                {
                    temp++;
                    Console.WriteLine("Ready to play: " + temp);
                    if (temp == 4)
                    {
                        temp = 0;
                        server.SendBroadcast($"GamePhase:Move:{gamePhase.GetCurrent()}:0");
                    }
                    else if (temp > 4) throw new Exception("5 players? Really?");
                }
                else if (mes[1] == "Move")
                {
                    Console.WriteLine($"Got from {mes[2]} card {mes[3]}");
                    server.SendBroadcast($"GamePhase:MoveDone:{mes[2]}:{mes[3]}");
                    if (gamePhase.GetRequiredColor() == '0') gamePhase.SetRequiredColor(mes[3][^1]);
                    gamePhase.moves.Add(mes[3], mes[2][0]);
                    if (gamePhase.moves.Count < 4)
                    {
                        server.SendBroadcast($"GamePhase:Move:{gamePhase.GetNext()}:{gamePhase.GetRequiredColor()}");
                    }
                    else if (gamePhase.moves.Count == 4)
                    {
                        char winner = gamePhase.GetMax();
                        int gotTrick = (winner == gamePhase.dummy || winner == gamePhase.declarer ? 1 : 0);
                        if (gotTrick == 1) gamePhase.gotTricks++;
                        server.SendBroadcast($"GamePhase:Winner:{gotTrick}:{winner}");
                        gamePhase.currentPlayer = gamePhase.GetIndex(winner);
                        gamePhase.SetRequiredColor('0');
                        Thread.Sleep(1000);
                        gamePhase.tricks--;
                        Console.WriteLine("Tricks: " + gamePhase.tricks);
                        if (gamePhase.tricks != 0)
                            server.SendBroadcast($"GamePhase:Move:{gamePhase.GetCurrent()}:0");
                        else
                        {
                            Score scoreInstance = new Score(gamePhase.bid, gamePhase.gotTricks, gamePhase.counter, gamePhase.recounter);
                            int score = scoreInstance.GetScore();

                            if (score > 0)
                            {
                                foreach (char player in gamePhase.GetContractTeam()) playersByPosition[player].score += score;
                            }
                            else
                            {
                                foreach (char player in gamePhase.GetDefenders()) playersByPosition[player].score += -score;
                            }
                            StringBuilder sb = new StringBuilder("Score:");
                            foreach (Player pl in playersGeneral)
                            {
                                sb.Append($" {pl.nick} {pl.score}");
                            }

                            server.SendBroadcast($"Score:{sb.ToString()}");

                            //server.SendBroadcast($"Score:{gamePhase.GetContractTeam()}:{(score >= 0 ? score : 0)}:{gamePhase.GetDefenders()}:{(score < 0 ? -score : 0)}");
                            Console.WriteLine($"Bid, gottricks,counter,recounter: {gamePhase.bid}, {gamePhase.gotTricks}, {gamePhase.counter}, {gamePhase.recounter}");
                            Console.WriteLine(score);
                            temp = 0;
                        }

                        gamePhase.moves.Clear();
                    }
                    else
                    {
                        throw new Exception("Too many moves");
                    }
                }
            }
            else if (mes[0] == "PlayAgain")
            {
                temp++;
                Console.WriteLine("Play again: " + temp);
                if (temp == 4)
                {
                    temp = 0;
                    SetPlayerByConfiguration(playerConfigurations.GetConfiguration());
                    SetPositions();
                    SendPlayersNicksAndPositions();
                    Console.WriteLine("Game starts again. Sending cards");
                    SendCards();
                }
            }
            else throw new ArgumentException($"Unknown message:{mes[0]}");
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
            foreach (Player p in players)
            {
                mes.Append($":{p.nick}:{p.position}");
            }
            Console.WriteLine(mes.ToString());
            server.SendBroadcast(mes.ToString());
        }

        public void SetPositions()
        {
            Queue<char> positions = new Queue<char>(new char[] { 'N', 'W', 'S', 'E' });
            playersByPosition.Clear();
            server.clientByPosition.Clear();
            foreach(Player player in players)
            {
                player.position= positions.Dequeue();
                playersByPosition.Add(player.position, player);
                server.clientByPosition.Add(player.position, server.clientByNick[player.nick]);
            }
        }

        public void SetPlayerByConfiguration(int [] conf)
        {
            players.Clear();
            foreach(int index in conf)
            {
                players.Add(playersGeneral[index]);
            }
        }
    }
}

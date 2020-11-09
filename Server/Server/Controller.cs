using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class Controller
    {
        CardDeck cd;
        Server server;

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
                server.SendMessage(server.getClients()[i], cd.Print13Cards());
            }
            Console.ReadLine();
        }
        public void Reaction(string message)
        {
            Console.WriteLine(message);
        }
    }
}

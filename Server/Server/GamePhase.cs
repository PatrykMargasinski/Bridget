using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    class GamePhase
    {
        public string bid;
        public char declarer;
        public char dummy;
        public bool counter=false;
        public bool recounter=false;

        public char[] players = new char[] { 'S', 'E', 'N', 'W' };
        public int currentPlayer;
        CardComparer comparer;
        public GamePhase(string bid, char declarer, char dummy, char counter, char recounter)
        {
            this.bid = bid;
            this.declarer = declarer;
            this.dummy = dummy;
            if (recounter != '0') this.recounter = true;
            else if (counter != '0') this.counter = true;
            currentPlayer = GetIndex(declarer);
        }

        public string GetContractTeam()
        {
            return ""+declarer + dummy;
        }

        public void ComparerInit()
        {
            string[] temp = bid.Split(":");
            comparer = new CardComparer(temp[1]);
        }

        public int GetIndex(char player)
        {
            return Array.IndexOf(players, player);
        }

        public char GetNext()
        {
            currentPlayer = (currentPlayer + 1) % 4;
            Console.WriteLine("Wchodze: " + currentPlayer);
            return players[currentPlayer];
        }

        public char GetCurrent()
        {
            return players[currentPlayer];
        }
    }
}

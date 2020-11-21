using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class AuctionPhase
    {
        private int currentPlayer;
        public char[] players = new char[] { 'S', 'E', 'N', 'W' };
        public int passCount = 0;
        public char counter = '0';
        public char recounter = '0';

        public string bid = "0:BA";
        public AuctionPhase(char firstPlayer)
        {
            int index = 0;
            while (players[index] != firstPlayer) index++;
            currentPlayer = index-1;
        }
        public char GetCurrent()
        {
            currentPlayer = (currentPlayer + 1) % 2;
            return players[currentPlayer];
        }
    }
}

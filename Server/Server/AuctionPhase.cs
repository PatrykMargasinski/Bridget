using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class AuctionPhase
    {
        private int currentPlayer;
        char[] players = new char[] { 'S', 'E', 'N', 'W' };

        string bid = "0:BA";
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

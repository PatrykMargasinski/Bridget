using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class AuctionPhase
    {
        private int currentPlayer;
        char[] players = new char[] { 'S', 'E', 'N', 'W' };

        string bid = "0BA";
        public AuctionPhase(char firstPlayer)
        {
            int index = 0;
            while (players[index] != firstPlayer) index++;
            currentPlayer = index;
        }
        public char GetCurrent()
        {
            return players[currentPlayer++];
        }
    }
}

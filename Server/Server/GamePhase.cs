using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    class GamePhase
    {
        public int currentPlayer;
        public int contractNumber;
        public char contractTrump;
        public char[] players = new char[] { 'S', 'E', 'N', 'W' };
        public int passCount = 0;
        public char counter = '0';
        public char recounter = '0';
        public char playerWithFirstColor = '0';
        public char dummy;
        CardComparer comparer;
        public GamePhase(char firstPlayer)
        {
            if (!players.Contains(firstPlayer)) throw new Exception("There is no player called " + firstPlayer);
            int index = 0;
            while (players[index] != firstPlayer) index++;
            currentPlayer = index - 1;
            playerWithFirstColor = firstPlayer;
        }

        public void ComparerInitialization(char trump)
        {
            comparer = new CardComparer(trump+"");
        }
        public char GetNext()
        {
            currentPlayer = (currentPlayer + 1) % 4;
            if (currentPlayer == dummy) return GetPartner(players[currentPlayer]);
            return players[currentPlayer];
        }
        public char GetCurrent()
        {
            return players[currentPlayer];
        }
        public char GetPartner()
        {
            return players[(2 + currentPlayer) % 4];
        }
        public char GetPartner(char player)
        {
            int ind = Array.IndexOf(players, player);
            return players[(2 + ind) % 4];
        }
    }
}

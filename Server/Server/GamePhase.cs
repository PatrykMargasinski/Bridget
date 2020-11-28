using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    class GamePhase
    {
        public char[] mainOrder=new char[] { 'S', 'E', 'N', 'W' };
        public char[] currentOrder;
        public string playingTeam;
        public int tricks;
        public bool counter;
        public bool recounter;

        public GamePhase(string playingTeam, int tricks, bool counter, bool recounter, char firstPlayer)
        {
            this.playingTeam = playingTeam;
            this.tricks = tricks;
            this.counter = counter;
            this.recounter = recounter;
            GenerateOrder(firstPlayer);

        }
        public void GenerateOrder(char firstPlayer)
        {
            if (!mainOrder.Contains(firstPlayer)) throw new ArgumentException("There is no such player");
            int index = Array.IndexOf(mainOrder, firstPlayer);
            currentOrder = new char[4];
            for(int i=0;i<4;i++)
            {
                currentOrder[i]= mainOrder[(4 + i + index) % 4];
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class Player
    {
        public string nick;
        public char position;
        public int score;
        public Player(string nick)
        {
            this.nick = nick;
            score = 0;
        }
    }
}

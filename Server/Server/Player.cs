using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class Player
    {
        public string nick;
        public Direction direction;
        public Player()
        {
            nick = "Noname";
        }
        public Player(string nick)
        {
            this.nick = nick;
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class Player
    {
        public static Queue<char> positions = new Queue<char>(new char[] { 'N','W','S','E'});
        public string nick;
        public char position;
        public Player()
        {
            nick = "Noname";
            position = positions.Dequeue();
        }
        public Player(string nick)
        {
            this.nick = nick;
            position = positions.Dequeue();
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}

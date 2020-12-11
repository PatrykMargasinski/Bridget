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
        public Player(string nick)
        {
            this.nick = nick;
            position = positions.Dequeue();
        }
    }
}

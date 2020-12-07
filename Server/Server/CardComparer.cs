using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class CardComparer : Comparer<string>
    {
        public CardComparer(string trump, GamePhase gamePhase)
        {
            if (trump != "BA") this.trump = trump[0];
            else this.trump = '0';
            gm = gamePhase;
        }
        private char trump;
        GamePhase gm;
        public override int Compare(string x, string y)
        {
            if (Color(x) == trump && Color(y) == trump) return Number(x).CompareTo(Number(y));
            else if (Color(x) == trump) return 1;
            else if (Color(y) == trump) return -1;
            else
            {
                if (Color(x) == gm.requiredColor && Color(y) == gm.requiredColor) return Number(x).CompareTo(Number(y));
                else if (Color(x) == gm.requiredColor) return 1;
                else if (Color(y) == gm.requiredColor) return -1;
                { 
                    if (Number(x) != Number(y)) return Number(x).CompareTo(Number(y));
                    else return Color(x).CompareTo(Color(y));
                }
            }
        }
        private int Number(string x)
        {
            string temp = x.Substring(0, x.Length - 1);
            return Int16.Parse(temp);
        }
        private char Color(string x)
        {
            return x[^1];
        }
    }
}

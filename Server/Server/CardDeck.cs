using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    class CardDeck
    { 
        List<string> cards=new List<string>();
        public void CreateDeck()
        {
            char[] symbols = new char[] { 'S', 'D', 'H', 'C' };
            for(int i=2;i<=14;i++)
            {
                foreach(char sym in symbols)
                {
                    cards.Add(i.ToString() + sym);
                }
            }

        }
        public void Shuffle()
        {
            Random ran = new Random();
            for(int i=0;i<cards.Count;i++)
            {
                int temp=ran.Next(2, 15);
                if (i == temp) temp = (temp + 1) % 13 + 2;
                (cards[i], cards[temp]) = (cards[temp], cards[i]);
            }
        }
        public void PrintThemAll()
        {
            foreach(string s in cards)
            {
                Console.WriteLine(s);
            }
        }
        public string PrintOneString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string s in cards)
            {
                sb.Append(s);
            }
            return sb.ToString();
        }
        public string Get13Cards()
        {
            List<string> strList = cards.Take(13).ToList();
            Console.WriteLine("Before: "+DeckSize());
            cards.RemoveRange(0, 13);
            Console.WriteLine("After: " + DeckSize());
            strList.Sort(new SortComparer());
            StringBuilder sb = new StringBuilder();
            for(int i=0;i<13;i++)
            {
                sb.Append(strList[i]);
            }
            return sb.ToString();
        }
        public void DeleteThemAll()
        {
            cards.Clear();
        }
        public int DeckSize()
        {
            return cards.Count;
        }
        class SortComparer : Comparer<string>
        {
            public override int Compare(string x, string y)
            {
                if (Color(x) != Color(y)) return Color(x).CompareTo(Color(y));
                else return Number(x).CompareTo(Number(y));
            }
            private int Number(string x)
            {
                string temp = x.Substring(0, x.Length - 1);
                return Int16.Parse(temp);
            }
            private char Color(string x)
            {
                return x[x.Length - 1];
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SomethingToTest
{
    public static List<string> CardGen()
    {
            System.Random ran=new System.Random();
            List<string> cards = new List<string> ();
            string[] rand = new string[] {"2", "3", "4", "5", "6", "7", "8","9", "10", "11", "12", "13","14"};
            string[] rand2 = new string[] {"C","H","D","S"};
            for (int i = 0; i < 13; i++) cards.Add(rand[ran.Next(0, 13)] + rand2[ran.Next(0, 4)]);
            cards.Sort(new SortComparer());
            return cards;
    }
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
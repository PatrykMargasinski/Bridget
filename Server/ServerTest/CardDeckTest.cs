using NUnit.Framework;
using Server;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServerTest
{
    public class Tests
    {
        CardDeck cardDeck;

        [SetUp]
        public void Setup()
        {
            cardDeck = new CardDeck();
        }

        [Test]
        public void CheckIfCreatedDeckHas52Cards()
        {
            cardDeck.CreateDeck();
            Console.WriteLine(cardDeck.Length());
            Assert.AreEqual(cardDeck.Length(), 52);
        }

        [Test]
        public void CheckIfShuffleWorks()
        {
            cardDeck.CreateDeck();

            CardDeck cardDeck2 = new CardDeck();
            cardDeck2.CreateDeck();
            cardDeck2.Shuffle();

            for(int i=0;i<cardDeck.Length();i++)
            {
                if (cardDeck.Get(i) != cardDeck2.Get(i)) Assert.Pass();
            }
            Assert.Fail(); 
        }

        [Test]
        public void CheckIfMethodGet13CardsReturns13Cards()
        {
            cardDeck.CreateDeck();
            List<string> cards = cardDeck.Get13Cards().Split(':').Skip(1).ToList();
            Assert.AreEqual(cards.Count, 13);
        }

        [Test]
        public void CheckSortComparer()
        {
            cardDeck.CreateDeck();
            List<string> cards = cardDeck.Get13Cards().Split(':').Skip(1).ToList();
            List<char> symbols = new List<char> { 'C', 'D', 'H', 'S' };
            int j = 0;
            for(int i=0;i<cards.Count;i++)
            {
                if(cards[i][^1]!=symbols[j])
                {
                    j++;
                }

                if (symbols.IndexOf(cards[i][^1]) < j) Assert.Fail();
            }
            Assert.Pass();
        }
    }
}
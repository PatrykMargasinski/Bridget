using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using NUnit.Framework;
using Server;

namespace ServerTest
{
    class CardComparerTest
    {
        Mock<IGamePhase> gamePhaseMock;
        CardComparer cardComparer;
        [SetUp]
        public void Setup()
        {
            gamePhaseMock = new Mock<IGamePhase>();
            gamePhaseMock.Setup(e => e.GetRequiredColor()).Returns('D');
            cardComparer = new CardComparer("C")
            {
                requiredColor = 'D'
            };
        }

        [Test]
        public void CheckComparingCardsWithTheSameColor()
        {
            string card1 = "4C";
            string card2 = "5C";
            Assert.IsTrue(cardComparer.Compare(card1, card2) < 0);
        }
        [Test]
        public void CheckIfCardWithTrumpColorHasPriority()
        {
            string card1 = "2C";
            string card2 = "10S";
            Assert.IsTrue(cardComparer.Compare(card1, card2) > 0);
        }
        [Test]
        public void CheckIfCardWithRequiredColorHasPriority()
        {
            string card1 = "2D";
            string card2 = "10S";
            Assert.IsTrue(cardComparer.Compare(card1, card2) > 0);
        }
    }
}

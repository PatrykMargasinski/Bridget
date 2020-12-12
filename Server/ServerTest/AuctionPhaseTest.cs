using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using NUnit.Framework;
using Server;

namespace ServerTest
{
    class AuctionPhaseTest
    {
        AuctionPhase auctionPhase;

        [SetUp]
        public void Setup()
        {
            auctionPhase = new AuctionPhase('N');
        }


        [Test]
        public void CheckIfGetNextAndGetCurrentReturnsCorrectPlayer()
        {
            char firstCurrent = auctionPhase.GetCurrent();
            char next = auctionPhase.GetNext();
            Assert.AreEqual(next, auctionPhase.GetCurrent());
            Assert.AreNotEqual(firstCurrent, auctionPhase.GetCurrent());
        }

        [Test]
        public void CheckIfAuctionPhaseReturnsPartnerCorrectly()
        {
            char current = auctionPhase.GetCurrent();
            Assert.AreEqual('E', auctionPhase.GetCurrent());
            Assert.AreEqual('W', auctionPhase.GetPartner());
            Assert.AreEqual('N', auctionPhase.GetPartner('S'));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using NUnit.Framework;
using Server;

namespace ServerTest
{
    class GamePhaseTest
    {
        GamePhase gamePhase;

        [SetUp]
        public void Setup()
        {
            gamePhase = new GamePhase("2:D",'N','S','0','0');
        }

        [Test]
        public void CheckIfGamePhaseReturnsContractTeam()
        {
            string contractTeam = gamePhase.GetContractTeam();
            Assert.IsTrue(contractTeam=="NS"|| contractTeam == "SN");
        }

        [Test]
        public void CheckIfGamePhaseReturnsDefenderTeam()
        {
            string contractTeam = gamePhase.GetDefenders();
            Assert.IsTrue(contractTeam == "WE" || contractTeam == "EW");
        }

        [Test]
        public void CheckIfGetIndexWorksCorrectly()
        {
            Assert.AreEqual(2, gamePhase.GetIndex('N'));
        }

        [Test]
        public void CheckIfGetNextAndGetCurrentReturnsCorrectPlayer()
        {
            char firstCurrent = gamePhase.GetCurrent();
            char next = gamePhase.GetNext();
            Assert.AreEqual(next, gamePhase.GetCurrent());
            Assert.AreNotEqual(firstCurrent, gamePhase.GetCurrent());
        }

        [Test]
        public void CheckIfGetMaxWorksCorrectly()
        {
            gamePhase.moves.Add("2C", 'N');
            gamePhase.moves.Add("4C", 'S');
            gamePhase.moves.Add("6C", 'W');
            gamePhase.moves.Add("5C", 'E');
            Assert.AreEqual('W', gamePhase.GetMax());
        }
    }
}

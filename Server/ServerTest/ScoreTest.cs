using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using NUnit.Framework;
using Server;

namespace ServerTest
{
    class ScoreTest
    {
        Score score;

        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void CheckIfClassChecksCorrectlyIfContractIsNotPassed()
        {
            score = new Score("2:C", 5, false, false);
            Assert.Negative(score.GetScore());
        }

        [Test]
        public void CheckIfClassChecksCorrectlyIfContractIsPassed()
        {
            score = new Score("2:C", 10, false, false);
            Assert.Positive(score.GetScore());
        }

        [Test]
        public void CheckIfClassCalculateCorrectlyOvertricks()
        {
            Score score = new Score("2:C", 9, false, false);
            Assert.AreEqual(110, score.GetScore());
        }

        [Test]
        public void CheckIfClassCalculateCorrectlyCounterPositive()
        {
            Score score = new Score("2:C", 8, true, false);
            Assert.AreEqual(180, score.GetScore());
        }
        [Test]
        public void CheckIfClassCalculateCorrectlyRecounterPositive()
        {
            Score score = new Score("2:C", 8, true, true);
            Assert.AreEqual(560, score.GetScore());
        }

        [Test]
        public void CheckIfClassCalculateCorrectlyCounterNegative()
        {
            Score score = new Score("2:C", 4, true, false);
            Assert.AreEqual(-800, score.GetScore());
        }
        [Test]
        public void CheckIfClassCalculateCorrectlyRecounterNegative()
        {
            Score score = new Score("2:C", 4, true, true);
            Assert.AreEqual(-1600, score.GetScore());
        }

        [Test]
        public void CheckIfClassCalculateCorrectlySmallSlam()
        {
            Score score = new Score("6:C", 12, false, false);
            Assert.AreEqual(920, score.GetScore());
        }

        [Test]
        public void CheckIfClassCalculateCorrectlySlam()
        {
            Score score = new Score("7:C", 13, false, false);
            Assert.AreEqual(1440, score.GetScore());
        }

    }
}

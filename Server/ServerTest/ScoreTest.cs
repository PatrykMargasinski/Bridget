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
        [Test]
        public void CheckIfClassChecksCorrectlyIfContractIsNotPassed()
        {
            score = new Score("2:C", 5, false, false,"NS","WE");
            Assert.Negative(score.GetScore());
        }
        [Test]
        public void CheckIfClassChecksCorrectlyIfContractIsPassed()
        {
            score = new Score("2:C", 10, false, false, "NS", "WE");
            Assert.Positive(score.GetScore());
        }
        [Test]
        public void CheckIfClassCalculateCorrectlyOvertricks()
        {
            score = new Score("2:C", 9, false, false, "NS", "WE");
            Assert.AreEqual(110, score.GetScore());
        }
        [Test]
        public void CheckIfClassCalculateCorrectlyCounterPositive()
        {
            score = new Score("2:C", 8, true, false, "NS", "WE");
            Assert.AreEqual(180, score.GetScore());
        }
        [Test]
        public void CheckIfClassCalculateCorrectlyRecounterPositive()
        {
            score = new Score("2:C", 8, true, true, "NS", "WE");
            Assert.AreEqual(560, score.GetScore());
        }
        [Test]
        public void CheckIfClassCalculateCorrectlyCounterNegative()
        {
            score = new Score("2:C", 4, true, false, "NS", "WE");
            Assert.AreEqual(-800, score.GetScore());
        }
        [Test]
        public void CheckIfClassCalculateCorrectlyRecounterNegative()
        {
            score = new Score("2:C", 4, true, true, "NS", "WE");
            Assert.AreEqual(-1600, score.GetScore());
        }
        [Test]
        public void CheckIfClassCalculateCorrectlySmallSlam()
        {
            score = new Score("6:C", 12, false, false, "NS", "WE");
            Assert.AreEqual(920, score.GetScore());
        }
        [Test]
        public void CheckIfClassCalculateCorrectlySlam()
        {
            score = new Score("7:C", 13, false, false, "NS", "WE");
            Assert.AreEqual(1440, score.GetScore());
        }
        [Test]
        public void CheckIfClassChecksCorrectlyCounterNegativeAndVulneable()
        {
            score = new Score("2:C", 4, true, false, "NS", "NS");
            Assert.AreEqual(-1100, score.GetScore());
        }
        [Test]
        public void CheckIfClassChecksCorrectlyRecounterNegativeAndVulneable()
        {
            score = new Score("2:C", 4, true, true, "NS", "NS");
            Assert.AreEqual(-2200, score.GetScore());
        }
        [Test]
        public void CheckIfClassCalculateCorrectlyOvertricksWhenCounterAndVulneable()
        {
            score = new Score("2:C", 9, true, false, "NS", "NS");
            Assert.AreEqual(380, score.GetScore());
        }
        [Test]
        public void CheckIfClassCalculateCorrectlyOvertricksWhenRecounterAndVulneable()
        {
            score = new Score("2:C", 9, true, true, "NS", "NS");
            Assert.AreEqual(1160, score.GetScore());
        }
    }
}
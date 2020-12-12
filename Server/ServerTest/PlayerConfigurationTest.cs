using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using NUnit.Framework;
using Server;

namespace ServerTest
{
    class PlayerConfigurationTest
    {
        PlayersConfigurations playersConfigurations;

        [SetUp]
        public void Setup()
        {
            playersConfigurations = new PlayersConfigurations();
        }

        [Test]
        public void CheckIfPlayerConfigurationsInstanceReturnsCorrentConfiguration()
        {
            int[] configuration = playersConfigurations.GetConfiguration();
            Assert.AreEqual(4, configuration.Length);
            for(int i=0;i<4;i++)
            {
                 Assert.Contains(i, configuration);
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace Server
{
    public class PlayersConfigurations
    {
        int index = 0;
        int vulnerableIndex = 0;
        List<int> indexes = new List<int> { 0, 1, 2 };
        List<string> vulnerableCases = new List<string> { "0", "NS", "WE", "both" };
        List<int[]> configurations = new List<int[]>();
        public PlayersConfigurations()
        {
            indexes= indexes.OrderBy(a => Guid.NewGuid()).ToList();
            configurations.Add(new int[] { 0, 1, 2, 3 });
            configurations.Add(new int[] { 0, 2, 1, 3 });
            configurations.Add(new int[] { 2, 0, 1, 3 });
        }

        public int[] GetConfiguration()
        {
            //changeCounter++;
            //if(changeCounter % 2==0) index++;
            return configurations[indexes[index]];
        }
        public string GetVulnerable()
        {
            return vulnerableCases[vulnerableIndex];
        }
        public void SetNextVulnerable()
        {
            vulnerableIndex = (vulnerableIndex+1)%4;
        }
    }
}

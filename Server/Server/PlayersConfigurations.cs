using System;
using System.Collections.Generic;
using System.Linq;

namespace Server
{
    class PlayersConfigurations
    {
        int index = 0;
        List<int> indexes = new List<int> { 0, 1, 2 };
        int changeCounter = -1;
        List<int[]> configurations = new List<int[]>();
        Controller controller;
        public PlayersConfigurations(Controller controller)
        {
            indexes= indexes.OrderBy(a => Guid.NewGuid()).ToList();
            configurations.Add(new int[] { 0, 1, 2, 3 });
            configurations.Add(new int[] { 0, 2, 1, 3 });
            configurations.Add(new int[] { 2, 0, 1, 3 });
            this.controller = controller;
        }

        public int[] GetConfiguration()
        {
            changeCounter++;
            if(changeCounter % 2==0) index++;
            return configurations[indexes[index]];
        }
    }
}

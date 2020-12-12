using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class PlayersConfigurations
    {
        int index = -1;
        List<int[]> configurations = new List<int[]>();
        Controller controller;
        public PlayersConfigurations(Controller controller)
        {
            configurations.Add(new int[] { 0, 1, 2, 3 });
            configurations.Add(new int[] { 0, 2, 1, 3 });
            configurations.Add(new int[] { 2, 0, 1, 3 });
            this.controller = controller;
        }

        public int[] GetConfiguration()
        {
            index = (index + 1) % 3;
            return configurations[index];
        }
    }
}

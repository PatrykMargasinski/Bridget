using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public interface IGamePhase
    {
        public string GetContractTeam();

        public string GetDefenders();

        public int GetIndex(char player);

        public char GetNext();

        public char GetCurrent();

        public char GetMax();

        public char GetRequiredColor();
        public void SetRequiredColor(char color);
    }
}

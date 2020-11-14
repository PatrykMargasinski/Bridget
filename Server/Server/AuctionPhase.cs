using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class AuctionPhase
    {
        int currentPlayer = 0;
        int passLimit = 0;
        Controller controller;
        List<string> calls;
        public AuctionPhase(Controller cont)
        {
            controller = cont;
        }
        public void Start()
        {
            calls = new List<string>();
        }
        public void AddCall(string call)
        {
            calls.Add(call);
            if (call == "pass") passLimit++;
            currentPlayer = (currentPlayer + 1) % 4;
        }
        public bool FinishedAuction()
        {
            return passLimit == 3;
        }
    }
}

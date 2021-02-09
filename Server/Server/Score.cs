using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace Server
{
    public class Score
    {
        string trump;
        string previousWinners;
        string contractTeam;
        int contractTricks;
        int gotTricks;
        bool counter;
        bool recounter;
        int score = 0;
        public string vulneableTeam;
        public Score(string contract, int gotTricks, bool counter, bool recounter, string contractTeam, string prevWinners)
        {
            string[] temp = contract.Split(":");
            this.trump = temp[1];
            this.contractTricks = Int32.Parse(temp[0]);
            this.gotTricks = gotTricks;
            this.counter = counter;
            this.recounter = recounter;
            this.previousWinners = prevWinners;
            this.contractTeam = contractTeam;
        }

        bool ContractPassed()
        {
            return gotTricks >= contractTricks + 6;
        }

        bool IsVulneable()
        {
            if (previousWinners == "0") return false;
            if (previousWinners == "both") return true;
            if (previousWinners == contractTeam) return true;
            else return false;
        }

        int ComputeScorePositiveNoCR()
        {
            int contractScore = 0;
            int bonusScore = 0;
            int overContractScore = 0;
            if (trump == "C" || trump == "D")
            {
                contractScore= (contractTricks) * 20;
                overContractScore = (gotTricks - contractTricks - 6) * 20;
            }
            else if (trump == "H" || trump == "S")
            {
                contractScore = (contractTricks) * 30;
                overContractScore = (gotTricks - contractTricks - 6) * 30;
            }
            else if (trump == "BA")
            {

                contractScore = (contractTricks - 1) * 30 + 40;
                overContractScore = (gotTricks - contractTricks - 6) * 30;
            }
            else throw new ArgumentException("There is no color like " + trump);

            if (contractScore < 100) bonusScore = 50;
            else bonusScore = IsVulneable()?500:300;

            if (contractTricks + 6 == 12) bonusScore += 500;
            else if (contractTricks + 6 == 13) bonusScore += 1000;
            return contractScore + bonusScore + overContractScore;
        }
        int ComputeScoreNegative()
        {
            int penalty = (contractTricks + 6 - gotTricks);
            if (recounter)
            {
                if (penalty == 1) score = -200;
                else if (penalty == 2) score = -600;
                else if (penalty == 3) score = -1000;
                else score = -1000 - 600 * (penalty - 3);
            }
            else if(counter)
            {
                if (penalty == 1) score = -100;
                else if (penalty == 2) score = -300;
                else if (penalty == 3) score = -500;
                else score = -500 - 300 * (penalty - 3);
            }
            else
            {
                score = penalty * -50;
            }
            return score;
        }

        int ComputeScoreNegativeVulneable()
        {
            int penalty = (contractTricks + 6 - gotTricks);
            if (recounter)
            {
                if (penalty == 1) score = -400;
                else if (penalty == 2) score = -1000;
                else if (penalty == 3) score = -1600;
                else score = -1600 - 600 * (penalty - 3);
            }
            else if (counter)
            {
                if (penalty == 1) score = -200;
                else if (penalty == 2) score = -500;
                else if (penalty == 3) score = -800;
                else score = -800 - 300 * (penalty - 3);
            }
            else
            {
                score = penalty * -100;
            }
            return score;
        }

        int ComputeScorePositiveCounter()
        {
            int contractScore = 0;
            int bonusScore = 0;
            int overContractScore = 0;
            if (trump == "C" || trump == "D")
            {
                contractScore = 2*(contractTricks) * 20;  
            }
            else if (trump == "H" || trump == "S")
            {
                contractScore = 2*(contractTricks) * 30;
            }
            else if (trump == "BA")
            {
                contractScore = 2*(contractTricks - 1) * 30 + 80;
            }
            else throw new ArgumentException("There is no color like " + trump);
            overContractScore = (gotTricks - contractTricks - 6) * 100;
            if (IsVulneable()) overContractScore *= 2;

            if (contractScore  < 100) bonusScore = 100;
            else bonusScore = IsVulneable() ? 550 : 350;

            if (contractTricks + 6 == 12) bonusScore += 500;
            else if (contractTricks + 6 == 13) bonusScore += 1000;
            return contractScore + overContractScore + bonusScore;
        }

        int ComputeScorePositiveRecounter()
        {
            int contractScore = 0;
            int bonusScore = 0;
            int overContractScore = 0;
            if (trump == "C" || trump == "D")
            {
                contractScore = 4 * (contractTricks) * 20;
            }
            else if (trump == "H" || trump == "S")
            {
                contractScore = 4 * (contractTricks) * 30;
            }
            else if (trump == "BA")
            {
                contractScore = 4 * (contractTricks - 1) * 30 + 160;
            }
            else throw new ArgumentException("There is no color like " + trump);
            overContractScore = (gotTricks - contractTricks - 6) * 200;
            if (IsVulneable()) overContractScore *= 2;

            if (contractScore < 100) bonusScore = 150;
            else bonusScore = IsVulneable() ? 600 : 400;

            if (contractTricks + 6 == 12) bonusScore += 500;
            else if (contractTricks + 6 == 13) bonusScore += 1000;
            return contractScore + overContractScore + bonusScore;
        }

        public int GetScore()
        {
            if (ContractPassed() == false)
            {
                if (IsVulneable())
                    score = ComputeScoreNegativeVulneable();
                else
                    score = ComputeScoreNegative();
            }
            else
            {
                if (recounter) score = ComputeScorePositiveRecounter();
                else if (counter) score = ComputeScorePositiveCounter();
                else score = ComputeScorePositiveNoCR();
            }
            return score;
        }
    }
}

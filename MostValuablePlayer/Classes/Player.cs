using MostValuablePlayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MostValuablePlayer.Classes
{
    public class Player
    {
        public string PlayerName { get; set; }
        public string PlayerNickname { get; set; }
        public int Number { get; set; }
        public string TeamName { get; set; }
        public Team Team { get; set; }
        public string Position { get; set; }
        public EnumSports Sports { get; set; }
        public BasketballStats BasketballStats { get; set; }
        public HandballStats HandballStats { get; set; }
        public int Contribution
        {
            get
            {
                int mvpPoints;

                if (Sports == EnumSports.Basketball)
                {
                    mvpPoints = CalculateBasketballStats(BasketballStats, Position);
                }
                else if (Sports == EnumSports.Handball)
                {
                    mvpPoints = CalculateHandballStats(HandballStats, Position);
                }
                else
                {
                    throw new Exception("Could not find sport in file!");
                }

                if (Team.IsWinner)
                    mvpPoints += 10;

                return mvpPoints;
            }
        }

        private int CalculateBasketballStats(BasketballStats basketballStats, string position)
        {
            var stats = position switch
            {
                ("G") => basketballStats.ScoredPoint * 2 + basketballStats.Rebound * 3 + basketballStats.Assist * 1,
                ("F") => basketballStats.ScoredPoint * 2 + basketballStats.Rebound * 2 + basketballStats.Assist * 2,
                ("C") => basketballStats.ScoredPoint * 2 + basketballStats.Rebound * 1 + basketballStats.Assist * 3,
                _ => throw new Exception("Could not find position!"),
            };
            return stats;
        }
        private int CalculateHandballStats(HandballStats handballStats, string position)
        {
            var stats = position switch
            {
                ("G") => 50 + handballStats.GoalsMade * 5 - handballStats.GoalsReceived * 2,
                ("F") => 20 + handballStats.GoalsMade * 1 - handballStats.GoalsReceived * 1,
                _ => throw new Exception("Could not find position!"),
            };
            return stats;
        }

    }
}

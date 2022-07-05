using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MostValuablePlayer.Classes
{
    public class Team
    {
        public string TeamName { get; set; }
        public List<Player> Players { get; set; }
        public int TotalScore { get; set; }
        public bool IsWinner { get; set; }

    }
}

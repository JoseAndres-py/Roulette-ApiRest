using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Roulette_ApiRest.Data.Entities
{
    [Serializable]
    public class Bet
    {
        public int id { get; set; }
        public int id_roulette { get; set; }
        public int id_gambler { get; set; }
        public int number { get; set; }
        public string color { get; set; }
        public int money_bet { get; set; }
        public DateTime date_bet { get; set; }
        public DateTime date_play { get; set; }
        public bool status_winner { get; set; }
    }
}


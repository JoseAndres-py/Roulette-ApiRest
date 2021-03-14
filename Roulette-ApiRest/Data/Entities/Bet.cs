using Roulette_ApiRest.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Roulette_ApiRest.Data.Entities
{
    [Serializable]
    public class Bet
    {
        public const int minNumber = 0;
        public const int maxNumber = 36;


        public int id { get; set; }
        public int id_roulette { get; set; }
        public int id_gambler { get; set; }
        [Range(minNumber, maxNumber)]
        public int? number { get; set; }
        public Color_Enum color { get; set; }
        [Range(0.1d, maximum: 10000)]
        public int money_bet { get; set; }
        public DateTime date_bet { get; set; }
        public DateTime? date_play { get; set; }
        public Winner_Enum? status_winner { get; set; }
    }
}


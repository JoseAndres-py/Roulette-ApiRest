using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Roulette_ApiRest.Data.Entities
{
    [Serializable]
    public class Roulette
    {
        public int id { get; set; }
        public int id_crupier { get; set; }
        public DateTime open_date { get; set; }
        public DateTime? close_date { get; set; }
        public int? number_winner { get; set; }
        public Color_Enum? color_winner { get; set; }
        public bool state { get; set; }
    }
}
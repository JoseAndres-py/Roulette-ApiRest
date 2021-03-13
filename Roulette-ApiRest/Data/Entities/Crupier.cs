using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Roulette_ApiRest.Data.Entities
{
    public class Crupier
    {
        public int id { get; set; }
        public string username { get; set; }
        public string access_key { get; set; }
        public bool state { get; set; }
    }
}

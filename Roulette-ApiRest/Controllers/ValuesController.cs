using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Roulette_ApiRest.Data;
using Microsoft.Extensions.Configuration;
using Roulette_ApiRest.Data.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Roulette_ApiRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    { 
        // GET: api/<ValuesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            BetRoulette rm = new BetRoulette();
            DataDB db = new DataDB("ConnectionRoulette");
            //rm.CloseRoulette(1, "93d36591-b06b-47c8-99c0-105aa735025f");
            //Bet bet = new Bet();
            //bet.id_roulette = 1;
            //bet.number = 10;
            //bet.color = Color_Enum.Black;
            //bet.money_bet = 100;
            //rm.CreateBet("60d71bfa-63f9-4b07-85e9-e9b22d828efe", bet);
            rm.ObtainRoulettes();
            return new string[] { "value1", "value2" };
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

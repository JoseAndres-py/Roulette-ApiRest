using Microsoft.AspNetCore.Mvc;
using Roulette_ApiRest.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Roulette_ApiRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouletteController : ControllerBase
    {
        // GET: api/<RouletteController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // POST api/<RouletteController>/create
        [HttpPost("create")]
        public string Create()
        {
            DbManager db = new DbManager();
            int idRoulette = db.CreateRoulette("93d36591-b06b-47c8-99c0-105aa735025f");
            return Convert.ToString(idRoulette);
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Roulette_ApiRest.Data;
using Roulette_ApiRest.Data.Entities;
using Roulette_ApiRest.Exceptions;
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
        // GET: api/<ValuesController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Roulette working correctly");
        }

        // POST api/<RouletteController>/create
        [HttpPut("create")]
        public IActionResult CreateRoulette(string access_key)
        {
            if (access_key == null) 
            { 
                return BadRequest();
            }
            BetRoulette rm = new BetRoulette();
            try
            {
                int idRoulette = rm.CreateRoulette(access_key: access_key);
                return Ok(new { id = idRoulette });
            }
            catch (Exception ex) { return ValidateExceptions(ex); }
        }

        // POST api/<RouletteController>/open
        [HttpPost("open")]
        public IActionResult OpenRoulette(string access_key, int id_roulette)
        {
            if (access_key == null)
            {
                return BadRequest();
            }
            BetRoulette rm = new BetRoulette();
            try
            {
                rm.OpenRoulette(roulette_id: id_roulette, access_key: access_key);
                return Ok(new { message = "The roulette wheel has been successfully opened" });
            }
            catch (Exception ex) { return ValidateExceptions(ex); }
        }

        // POST api/<RouletteController>/open
        [HttpPut("bet")]
        public IActionResult BetRouletteOpen(string access_key, int id_roulette, Color_Enum color, int money_bet, int? number = null)
        {
            if (access_key == null || id_roulette == 0 || money_bet == 0 )
            {
                return BadRequest();
            }
            else if (number != -1) 
            {
                color = (number % 2 == 0) ? Color_Enum.Red : Color_Enum.Black;   
            }
            BetRoulette rm = new BetRoulette();
            try
            {
                Bet bet = new Bet{ 
                    id_roulette = id_roulette,
                    number = number,
                    color = color,
                    money_bet = money_bet
                };
                rm.CreateBet(access_key: access_key, Bet: bet);
                return Ok(new { message = $"The roulette number {id_roulette} bet has been placed correctly" });
            }
            catch (Exception ex) { return ValidateExceptions(ex); }
        }

        // POST api/<RouletteController>/open
        [HttpPost("close")]
        public IActionResult CloseRouletteBets(string access_key, int id_roulette)
        {
            if (access_key == null || id_roulette == 0)
            {
                return BadRequest();
            }
            BetRoulette rm = new BetRoulette();
            try
            {
                List<Bet> BetsRoulete =  rm.CloseRoulette(roulette_id: id_roulette, access_key: access_key);
                return Ok(new { message = "The roulette wheel has been successfully closed", roulettes = BetsRoulete });
            }
            catch (Exception ex) { return ValidateExceptions(ex); }
        }

        // GET api/<RouletteController>/open
        [HttpGet("list-roulettes")]
        public IActionResult ListCreatedRoulettes()
        {
            BetRoulette rm = new BetRoulette();
            try
            {
                List<Roulette> roulettes =  rm.ObtainRoulettes();
                return Ok(new { roulettes = roulettes });
            }
            catch (Exception ex) { return ValidateExceptions(ex); }
        }


        private IActionResult ValidateExceptions(Exception ex) { 
            if( ex is InvalidOperationException)
                return BadRequest(new { message = ex.Message });
            else if (ex is ForbiddenAccessException)
                return Unauthorized(new { message = ex.Message });
            else if (ex is DataNotFoundException)
                return NotFound(new { message = ex.Message });
            else
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}

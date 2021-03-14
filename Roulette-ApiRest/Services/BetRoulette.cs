using Roulette_ApiRest.Data.Entities;
using Roulette_ApiRest.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Roulette_ApiRest.Data
{
    public class BetRoulette
    {
        private DataDB db;

        public BetRoulette()
        {
            db = new DataDB("ConnectionRoulette");
        } // Constructor

        public List<Roulette> ObtainRoulettes() {
            List<Roulette> roulettes = db.getRoulettes();
            if (!roulettes.Any())
            {
                throw new Exception("No registered roulettes found.");
            }
            else
            {
                return roulettes;
            }
        }
        public int CreateRoulette(string access_key)
        {
            Crupier crupier = db.getCrupierByAccessKey(access_key);
            if (!crupier.state)
            {
                throw new Exception("The operation could not be performed because the croupier is inactive.");
            }
            else
            {
                Roulette roulete =  db.addRoulette(crupier.id);
                return roulete.id;
            }
        }

        public void OpenRoulette(int roulette_id, string access_key)
        {
            Crupier crupier = db.getCrupierByAccessKey(access_key);
            Roulette Roulette = db.getRouletteById(roulette_id);
            if (Roulette.id_crupier != crupier.id)
            {
                throw new Exception("The operation could not be performed because the croupier is not assigned to this roulette");
            }
            else if (Roulette.state)
            {
                throw new Exception("The operation could not be performed because the roulette is already open");
            }
            else
            {
                db.updateRouletteStatus(roulette_id, true);
            }
        }

        public void CreateBet(string access_key, Bet Bet)
        {
            Gambler Gambler = db.getGamblerByAccessKey(access_key);
            Roulette Roulette = db.getRouletteById(Bet.id_roulette);
            if (Bet.money_bet > Gambler.credit)
            {
                throw new Exception("The operation could not be performed because the bambler has insufficient credit");
            }
            else if (!Roulette.state)
            {
                throw new Exception("The operation could not be performed because the roulette is closed");

            }
            else
            {
                db.addBet(Gambler, Roulette, Bet);
            }
        }

        public void CloseRoulette(int roulette_id, string access_key)
        {
            Crupier crupier = db.getCrupierByAccessKey(access_key);
            Roulette Roulette = db.getRouletteById(roulette_id);
            if (Roulette.id_crupier != crupier.id)
            {
                throw new Exception("The operation could not be performed because the croupier is not assigned to this roulette");
            }
            else if (!Roulette.state)
            {
                throw new Exception("The operation could not be performed because the roulette is already closed");
            }
            else
            {
                db.updateRouletteStatus(Roulette.id, false);
                Roulette.close_date = DateTime.Now;
                //Generate Number Roulette
                Random rnd = new Random();
                int random = rnd.Next(Bet.minNumber, Bet.maxNumber);
                db.updateBetsWinner(Roulette, 10, Color_Enum.Red);
                //Update Credit Gamblers for Bets
                foreach (Winner_Enum winner in Enum.GetValues(typeof(Winner_Enum)))
                {
                    db.addCreditResultBets(Roulette.id, winner);
                }
                
            }
        }
    }
}

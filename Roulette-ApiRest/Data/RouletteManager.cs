using Roulette_ApiRest.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Roulette_ApiRest.Data
{
    public class RouletteManager
    {
        private DbManager db;

        public RouletteManager()
        {
            db = new DbManager();
        } // Constructor

        public int CreateRoulette(string access_key)
        {
            Crupier crupier = db.getCrupierId(access_key);
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
            Crupier crupier = db.getCrupierId(access_key);
            Roulette Roulette = db.getRoulette(roulette_id);
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
            Gambler Gambler = db.getGambler(access_key);
            Roulette Roulette = db.getRoulette(Bet.id_roulette);
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

            Crupier crupier = db.getCrupierId(access_key);
            Roulette Roulette = db.getRoulette(roulette_id);
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
                db.updateRouletteStatus(roulette_id, false);
            }
        }


    }
}

using Roulette_ApiRest.Data.Entities;
using Roulette_ApiRest.Exceptions;
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
        private RouletesData rouletes_db;
        private UsersData users_db;
        private BetsData bets_db;

        public BetRoulette()
        {
            rouletes_db = new RouletesData(connection_name: "ConnectionRoulette");
            users_db = new UsersData(connection_name: "ConnectionRoulette");
            bets_db = new BetsData(connection_name: "ConnectionRoulette");
        } // Constructor

        public List<Roulette> ObtainRoulettes() {
            List<Roulette> roulettes = rouletes_db.getRoulettes();
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
            Crupier crupier = users_db.getCrupierByAccessKey(access_key: access_key);
            if (!crupier.state)
            {
                throw new ForbiddenAccessException("The operation could not be performed because the croupier is inactive.");
            }
            else
            {
                Roulette roulete = rouletes_db.addRoulette(crupier_id: crupier.id);
                return roulete.id;
            }
        }

        public void OpenRoulette(int roulette_id, string access_key)
        {
            Crupier crupier = users_db.getCrupierByAccessKey(access_key: access_key);
            Roulette Roulette = rouletes_db.getRouletteById(id: roulette_id);
            if (Roulette.id_crupier != crupier.id)
            {
                throw new ForbiddenAccessException("The operation could not be performed because the croupier is not assigned to this roulette");
            }
            else if (Roulette.state)
            {
                throw new InvalidOperationException("The operation could not be performed because the roulette is already open");
            }
            else
            {
                rouletes_db.updateRouletteStatus(roulette_id: roulette_id, state: true);
            }
        }

        public void CreateBet(string access_key, Bet Bet)
        {
            Gambler Gambler = users_db.getGamblerByAccessKey(access_key: access_key);
            Roulette Roulette = rouletes_db.getRouletteById(id: Bet.id_roulette);
            if (Bet.money_bet > Gambler.credit)
            {
                throw new Exception("The operation could not be performed because the bambler has insufficient credit");
            }
            else if (!Roulette.state)
            {
                throw new InvalidOperationException("The operation could not be performed because the roulette is closed");

            }
            else
            {
                bets_db.addBet(gambler: Gambler, roulette: Roulette, bet: Bet);
            }
        }

        public List<Bet> CloseRoulette(int roulette_id, string access_key)
        {
            Crupier crupier = users_db.getCrupierByAccessKey(access_key: access_key);
            Roulette Roulette = rouletes_db.getRouletteById(id: roulette_id);
            if (Roulette.id_crupier != crupier.id)
            {
                throw new ForbiddenAccessException("The operation could not be performed because the croupier is not assigned to this roulette");
            }
            else if (!Roulette.state)
            {
                throw new InvalidOperationException("The operation could not be performed because the roulette is already closed");
            }
            else
            {
                rouletes_db.updateRouletteStatus(roulette_id: Roulette.id, state: false);
                Roulette.close_date = DateTime.Now;
                updateResultsBets(Roulette: Roulette);
                return bets_db.getBetsByRoulette(Roulette);
            }
        }

        public void updateResultsBets(Roulette Roulette) 
        {
            try
            {
                Random rnd = new Random();
                int bet_result = rnd.Next(minValue: Bet.minNumber, maxValue: Bet.maxNumber);
                bets_db.updateBetsWinner(roulette: Roulette, number_bet: 10, color_bet: Color_Enum.Red);
                //Update Credit Gamblers for Bets
                foreach (Winner_Enum winner in Enum.GetValues(typeof(Winner_Enum)))
                {
                    bets_db.addCreditResultBets(roulette_id: Roulette.id, result: winner);
                }
            }
            catch
            {
                throw new Exception("An error has occurred in the awarding of prizes to gamblers ");
            }
        }
    }
}

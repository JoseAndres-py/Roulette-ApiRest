using Roulette_ApiRest.Data.Entities;
using Roulette_ApiRest.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Roulette_ApiRest.Data
{
    public class BetsData:DataDB
    {
        public BetsData(string connection_name) : base(connection_name)
        {
        }

        public Bet getBetById(int id)
        {
            command.CommandText = "SELECT * FROM bets WHERE id = @id; ";
            command.Parameters.Clear();
            command.Parameters.Add("@id", SqlDbType.Int).Value = id;
            List<Bet> roulettesResult = new List<Bet>();
            try
            {
                roulettesResult = Query<Bet>();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while finding the roulette in DB");
            }
            finally
            {
                if (!roulettesResult.Any())
                {
                    throw new Exception("Roulete not found with the specified id");
                }
            }
            return roulettesResult[0];
        }

        public void addBet(Gambler gambler, Roulette roulette, Bet bet)
        {
            command.CommandText = "INSERT INTO bets ([id_roulette], [id_gambler], [number], [color], [money_bet], [date_bet])  OUTPUT Inserted.id VALUES(@id_roulette, @id_gambler, @number, @color, @money, GETDATE());";
            command.Parameters.Clear();
            command.Parameters.Add("@id_roulette", SqlDbType.Int).Value = roulette.id;
            command.Parameters.Add("@id_gambler", SqlDbType.Int).Value = gambler.id;
            command.Parameters.Add("@number", SqlDbType.Int).Value = bet.number;
            command.Parameters.Add("@color", SqlDbType.NVarChar).Value = (int)bet.color;
            command.Parameters.Add("@money", SqlDbType.Int).Value = bet.money_bet;
            try
            {
                connection.Open();
                command.ExecuteReader();
            }
            catch
            {
                throw new Exception("An error occurred when adding the bets to the database");
            }
            finally
            {
                connection.Close();
            }
        }

        public void updateBetsWinner(Roulette roulette, int number_bet, Color_Enum color_bet)
        {
            command.CommandText = @"UPDATE bets
                                    SET status_winner = @lose_result,
	                                    date_play = GETDATE()
                                    WHERE id_roulette = @id_roulette AND date_bet BETWEEN @open_date AND @close_date;

                                    UPDATE bets
                                    SET status_winner = @color_result
                                    WHERE color = @color AND id_roulette = @id_roulette AND date_bet BETWEEN @open_date AND @close_date;

                                    UPDATE bets
                                    SET status_winner = @number_result
                                    WHERE number = @number AND id_roulette = @id_roulette AND date_bet BETWEEN @open_date AND @close_date;";
            command.Parameters.Clear();
            command.Parameters.Add("@id_roulette", SqlDbType.Int).Value = roulette.id;
            command.Parameters.Add("@number", SqlDbType.Int).Value = number_bet;
            command.Parameters.Add("@color", SqlDbType.Int).Value = (int)color_bet;
            command.Parameters.Add("@lose_result", SqlDbType.Int).Value = Winner_Enum.Lose;
            command.Parameters.Add("@color_result", SqlDbType.Int).Value = Winner_Enum.Color;
            command.Parameters.Add("@number_result", SqlDbType.Int).Value = Winner_Enum.Number;
            command.Parameters.Add("@open_date", SqlDbType.DateTime).Value = roulette.open_date;
            command.Parameters.Add("@close_date", SqlDbType.DateTime).Value = roulette.close_date;
            try
            {
                connection.Open();
                command.ExecuteReader();
            }
            catch
            {
                throw new Exception("An error occurred while assigning winners to the bet in the database");
            }
            finally
            {
                connection.Close();
            }
        }

        public void addCreditResultBets(int roulette_id, Winner_Enum result)
        {
            command.CommandText = @"UPDATE gamblers
                                    SET gamblers.credit = gamblers.credit + 
                                    CASE @status_winner
	                                    WHEN @lose_result THEN -bets.money_bets
	                                    WHEN @color_result THEN bets.money_bets*1.8 
	                                    WHEN @number_result THEN bets.money_bets*5 
                                    END 
                                    FROM gamblers
                                    INNER JOIN (
                                    SELECT bets.id_gambler, SUM(bets.money_bet) as money_bets  
                                    FROM bets
                                    WHERE bets.id_roulette = @id_roulette
                                    AND bets.status_winner = @status_winner
                                    GROUP BY bets.id_gambler) bets ON bets.id_gambler = gamblers.id";
            command.Parameters.Clear();
            command.Parameters.Add("@id_roulette", SqlDbType.Int).Value = roulette_id;
            command.Parameters.Add("@status_winner", SqlDbType.Int).Value = (int)result;
            command.Parameters.Add("@lose_result", SqlDbType.Int).Value = (int)Winner_Enum.Lose;
            command.Parameters.Add("@color_result", SqlDbType.Int).Value = (int)Winner_Enum.Color;
            command.Parameters.Add("@number_result", SqlDbType.Int).Value = (int)Winner_Enum.Number;
            try
            {
                connection.Open();
                command.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while add credit at winners to the bets in the database");
            }
            finally
            {
                connection.Close();
            }
        }

    }
}

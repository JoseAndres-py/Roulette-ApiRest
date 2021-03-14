using Microsoft.Extensions.Configuration;
using Roulette_ApiRest.Data.Entities;
using Roulette_ApiRest.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Roulette_ApiRest.Data
{
    public class DataDB
    {
        private SqlConnection connection;
        private SqlCommand command;
        private readonly IConfiguration _configuration;


        public DataDB(string connection_name)
        {
            _configuration = GetConfigurationSettings();
            createSqlConnection(connection_name);
        } // Constructor

        private void createSqlConnection(string connection_name) {
            connection = new SqlConnection();
            connection.ConnectionString = _configuration.GetConnectionString(connection_name);
            createSqlCommand(CommandType.Text);
        }

        private void createSqlCommand(CommandType type_query) 
        {
            command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = type_query;
        }

        public Crupier getCrupierByAccessKey(string access_key)
        {
            command.CommandText = "SELECT * FROM crupiers WHERE access_key = @access_key; ";
            command.Parameters.Clear();
            command.Parameters.Add("@access_key", SqlDbType.VarChar).Value = access_key;
            List<Crupier> crupiersResult = new List<Crupier>();
            try
            {
                crupiersResult = Query<Crupier>();
            }
            catch
            {
                throw new Exception("An error occurred while finding the crupier in DB");
            }
            finally
            {
                if (!crupiersResult.Any())
                {
                    throw new Exception("Crupier not found with the specified access key");
                }
            }

            return crupiersResult[0];
        }

        public Gambler getGamblerByAccessKey(string access_key)
        {
            command.CommandText = "SELECT * FROM gamblers WHERE access_key = @access_key; ";
            command.Parameters.Clear();
            command.Parameters.Add("@access_key", SqlDbType.VarChar).Value = access_key;
            List<Gambler> gamblersResult = new List<Gambler>();
            try
            {
                gamblersResult = Query<Gambler>();
            }
            catch
            {
                throw new Exception("An error occurred while finding the gambler in DB");
            }
            finally
            {
                if (!gamblersResult.Any())
                {
                    throw new Exception("Gambler not found with the specified access key");
                }
            }
            return gamblersResult[0];
        }

        public Roulette getRouletteById(int id)
        {
            command.CommandText = "SELECT * FROM roulettes WHERE id = @id; ";
            command.Parameters.Clear();
            command.Parameters.Add("@id", SqlDbType.Int).Value = id;
            List<Roulette> roulettesResult = new List<Roulette>();
            try
            {
                roulettesResult = Query<Roulette>();
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

        public List<Roulette> getRoulettes()
        {
            command.CommandText = "SELECT * FROM roulettes";
            command.Parameters.Clear();
            List<Roulette> roulettesResult = new List<Roulette>();
            try
            {
                roulettesResult = Query<Roulette>();
            }
            catch
            {
                throw new Exception("An error occurred while finding the roulettes in DB");
            }
            return roulettesResult;
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

        public void updateRouletteStatus(int roulette_id, bool state)
        {
            command.CommandText = "UPDATE roulettes SET state = CONVERT(bit,@state), open_date = CASE WHEN @state = CONVERT(bit,'True') THEN GETDATE() ELSE open_date END, close_date = CASE WHEN @state = CONVERT(bit,'False') THEN GETDATE()  ELSE NULL END WHERE id = @id;";
            command.Parameters.Clear();
            command.Parameters.Add("@id", SqlDbType.Int).Value = roulette_id;
            command.Parameters.Add("@state", SqlDbType.Bit).Value = state;
            try
            {
                connection.Open();
                command.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the roulette status");
            }
            finally
            {
                connection.Close();
            }

        }

        public Roulette addRoulette(int crupier_id)
        {
            command.CommandText = "INSERT INTO roulettes (id_crupier, open_date, state) OUTPUT Inserted.id VALUES(@id_crupier, GETDATE(), CONVERT(bit, 1)); ";
            command.Parameters.Clear();
            command.Parameters.Add("@id_crupier", SqlDbType.Int).Value = crupier_id;
            try
            {
                Roulette newRoulette = Query<Roulette>()[0];

                return newRoulette;
            }
            catch
            {
                throw new Exception("An error occurred when adding the roulette to the database");
            }
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
            catch(Exception ex)
            {
                throw new Exception("An error occurred while add credit at winners to the bets in the database");
            }
            finally
            {
                connection.Close();
            }
        }

        public List<obj> Query<obj>() where obj : new()
        {
            List<obj> response = new List<obj>();
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    response.Add(sqlDataConvertion<obj>(reader));
                }

                return response;
            }
            finally
            {
                connection.Close();
            }
        }

        private obj sqlDataConvertion<obj>( SqlDataReader reader) where obj : new()
        {
            obj Object = new obj();
            for (int inc = 0; inc < reader.FieldCount; inc++)
            {
                object valueDB = reader.GetValue(inc);
                if (valueDB != DBNull.Value)
                {
                    Type type = Object.GetType();
                    PropertyInfo prop = type.GetProperty(reader.GetName(inc));
                    // Nullable Data Type
                    Type propType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                    // Cast Enum Values Data Base
                    if (prop.PropertyType.IsEnum)
                        valueDB = Enum.ToObject(prop.PropertyType, valueDB);
                    prop.SetValue(Object, Convert.ChangeType(valueDB, propType), null);
                }
            }

            return Object;
        }
        
        public IConfigurationRoot GetConfigurationSettings()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
    }
}

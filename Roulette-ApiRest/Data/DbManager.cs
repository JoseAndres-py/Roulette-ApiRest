using Microsoft.Extensions.Configuration;
using Roulette_ApiRest.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Roulette_ApiRest.Data
{
    public class DbManager
    {
        private SqlConnection connection;
        public SqlCommand command;
        private readonly IConfiguration _configuration;


        public DbManager()
        {
            //Configuration Settings
            _configuration = GetConfiguration();
            //Data Base Conection
            connection = new SqlConnection();
            connection.ConnectionString = _configuration.GetConnectionString("ConnectionString");
            command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.Text;
        } // Constructor

        public bool GetUsersData(int id)
        {
            bool returnvalue = false;
            command.CommandText = "select * from gamblers where id=@id";
            command.Parameters.Clear();
            command.Parameters.Add("@id", SqlDbType.Int).Value = id;
            try
            {
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var lastname = Convert.ToInt32(reader["id"]);
                    }
                }
                returnvalue = true;
            }
            finally
            {
                connection.Close();
            }

            return returnvalue;

        }

        public Crupier getCrupierId(string access_key)
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

        public Gambler getGambler(string access_key)
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

        public Roulette getRoulette(int id)
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

        public Bet getBet(int id)
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
            command.CommandText = "UPDATE roulettes SET state = CONVERT(bit,@state), open_date = CASE WHEN 1 = CONVERT(bit,'True') THEN GETDATE() ELSE open_date END, close_date = CASE WHEN 1 = CONVERT(bit,'False') THEN GETDATE()  ELSE NULL END WHERE id = @id;";
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
                throw new Exception("An error occurred when adding the bet to the database");
            }
        }

        public List<obj> Query<obj>() where obj : new()
        {
            List<obj> response = new List<obj>();
            try
            {
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    obj Object = new obj();

                    for (int inc = 0; inc < reader.FieldCount; inc++)
                    {
                        object valueDB = reader.GetValue(inc);
                        if (valueDB != DBNull.Value)
                        {
                            Type type = Object.GetType();
                            PropertyInfo prop = type.GetProperty(reader.GetName(inc));
                            // Cast Enum Values Data Base
                            if (prop.PropertyType.IsEnum)
                                valueDB = Enum.ToObject(prop.PropertyType, valueDB);
                            prop.SetValue(Object, Convert.ChangeType(valueDB, prop.PropertyType), null);
                        }
                    }

                    response.Add(Object);
                }

                return response;
            }
            finally
            {
                connection.Close();
            }
        }

        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
    }
}

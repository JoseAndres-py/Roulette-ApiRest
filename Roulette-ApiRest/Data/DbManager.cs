using System;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using Roulette_ApiRest.Data.Entities;
using System.Reflection;

namespace Roulette_ApiRest.Data
{
    public class DbManager
    {
        private SqlConnection connection;
        private SqlCommand command;
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

        public int CreateRoulette(string access_key) {

            int idCrupier = getCrupierId(access_key);
            command.CommandText = "INSERT INTO roulettes (id_crupier, open_date, state) OUTPUT Inserted.id VALUES(@id_crupier, GETDATE(), CONVERT(bit, 1)); ";
            command.Parameters.Clear();
            command.Parameters.Add("@id_crupier", SqlDbType.Int).Value = idCrupier;
            try
            {
               Roulette newRoulette = Query<Roulette>()[0];

               return newRoulette.id;
            }
            catch
            {
                throw new Exception("An error occurred when adding the roulette to the database");
            }
        }

        public void OpenRoulette(int roulette_id, string access_key) 
        {
            
            int idCrupier = getCrupierId(access_key);
            
            Roulette Roulette = getRoulette(roulette_id);
            if (Roulette.id_crupier != idCrupier)
            {
                throw new Exception("The operation could not be performed because the croupier is not assigned to this roulette");
            }
            else if (Roulette.state)
            {
                throw new Exception("The operation could not be performed because the roulette is already open");
            }
            else 
            {
                updateRouletteStatus(roulette_id, true);
            }
        }

        public void CloseRoulette(int roulette_id, string access_key)
        {

            int idCrupier = getCrupierId(access_key);

            Roulette Roulette = getRoulette(roulette_id);
            if (Roulette.id_crupier != idCrupier)
            {
                throw new Exception("The operation could not be performed because the croupier is not assigned to this roulette");
            }
            else if (Roulette.state)
            {
                throw new Exception("The operation could not be performed because the roulette is already closed");
            }
            else
            {
                updateRouletteStatus(roulette_id, false);
            }
        }

        public int getCrupierId(string access_key)
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
                    throw new Exception("Croupier not found with the specified access key");
                }
            }
            return crupiersResult[0].id;
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
            catch(Exception ex)
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
            command.CommandText = "UPDATE roulettes SET state = CONVERT(bit,@state) WHERE id = @id;";
            command.Parameters.Clear();
            command.Parameters.Add("@id", SqlDbType.Int).Value = roulette_id;
            command.Parameters.Add("@state", SqlDbType.Bit).Value = state;
            try
            {
                connection.Open();
                command.ExecuteReader();
            }
            catch(Exception ex)
            {
                throw new Exception("An error occurred while updating the roulette status");
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
                var reader = command.ExecuteReader(); 
                while (reader.Read())
                {
                    obj Object = new obj();

                    for (int inc = 0; inc < reader.FieldCount; inc++)
                    {
                        object valueDB = reader.GetValue(inc);
                        if (valueDB != DBNull.Value) { 
                            Type type = Object.GetType();;
                            PropertyInfo prop = type.GetProperty(reader.GetName(inc));
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

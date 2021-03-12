using System;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using Roulette_ApiRest.Data.Entities;

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
            catch
            {
            }
            finally
            {
                connection.Close();
            }

            return returnvalue;

        }

        public int CreateRoulette() {

            int id_roulette = 0;
            command.CommandText = "INSERT INTO roulettes (id_crupier, open_date, state) OUTPUT Inserted.id VALUES(@id_crupier, GETDATE(), CONVERT(bit, 1)); ";
            command.Parameters.Add("@id_crupier", SqlDbType.Int).Value = 1;
            try
            {
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        id_roulette = Convert.ToInt32(reader["id"]);
                    }
                }
            }
            catch(Exception error)
            {
                throw new Exception("An error occurred when adding the roulette wheel to the database");
            }
            finally
            {
                connection.Close();
            }

            return id_roulette;
            
        }

        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
    }
}

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

        public int CreateRoulette() {

            int id_roulette = -1;
            command.CommandText = "INSERT INTO roulettes (id_crupier, open_date, state) OUTPUT Inserted.id VALUES(@id_crupier, GETDATE(), CONVERT(bit, 1)); ";
            command.Parameters.Add("@id_crupier", SqlDbType.Int).Value = getCrupierId();
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

                return id_roulette;
            }
            catch(Exception error)
            {
                throw new Exception("An error occurred when adding the roulette to the database");
            }
            finally
            {
                connection.Close();
            }
            
        }


        public int getCrupierId()
        {
            int id_crupier = -1;
            command.CommandText = "SELECT * FROM crupiers WHERE access_key = @access_key; ";
            command.Parameters.Add("@access_key", SqlDbType.VarChar).Value = "93d36591-b06b-47c8-99c0-105aa73502f";
            try
            {
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        id_crupier = Convert.ToInt32(reader["id"]);
                    }
                }
                return id_crupier;
            }
            catch (Exception error)
            {
                throw new Exception("An error occurred while finding the crupier");
            }
            finally
            {
                connection.Close();
            }
        }

        public List<obj> Query<obj>() where obj : new()
        {
            List<obj> res = new List<obj>();
            command.CommandText = "SELECT * FROM crupiers WHERE access_key = @access_key; ";
            command.Parameters.Add("@access_key", SqlDbType.VarChar).Value = "93d36591-b06b-47c8-99c0-105aa735025f";
            connection.Open();
            var reader = command.ExecuteReader(); 
            while (reader.Read())
            {
                obj Object = new obj();

                for (int inc = 0; inc < reader.FieldCount; inc++)
                {
                    Type type = Object.GetType();
                    var prueba = reader.GetName(inc);
                    PropertyInfo prop = type.GetProperty(reader.GetName(inc));
                    prop.SetValue(Object, reader.GetValue(inc), null);
                }

                res.Add(Object);
            }
            connection.Close();

            return res;

        }

        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
    }
}

using System;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Roulette_ApiRest.Data
{
    public class DbManager
    {
        private SqlConnection connection;
        private SqlCommand command;
        private readonly IConfiguration _configuration;


        public DbManager()
        {
            _configuration = GetConfiguration();
            connection = new SqlConnection();
            connection.ConnectionString = _configuration.GetConnectionString("ConnectionString");
            command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.Text;
        } // constructor

        public bool GetUsersData(int id)
        {
            bool returnvalue = false;
                command.CommandText = "select * from Users where id=@id";
                command.Parameters.Add("@id", SqlDbType.Int).Value = id;
            try{
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
            catch{ 
            }
            finally{
                connection.Close();
            }

            return returnvalue;

        }

        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
    }
}

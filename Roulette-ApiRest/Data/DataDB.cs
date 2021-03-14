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
        protected SqlConnection connection;
        protected SqlCommand command;
        protected readonly IConfiguration _configuration;


        public DataDB(string connection_name)
        {
            _configuration = GetConfigurationSettings();
            createSqlConnection(name:connection_name);
        } // Constructor

        private void createSqlConnection(string name) {
            connection = new SqlConnection();
            connection.ConnectionString = _configuration.GetConnectionString(name: name);
            createSqlCommand(CommandType.Text);
        }

        private void createSqlCommand(CommandType type_query) 
        {
            command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = type_query;
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
                    response.Add(sqlDataConvertion<obj>(reader:reader));
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
                    PropertyInfo prop = type.GetProperty(name:reader.GetName(inc));
                    // Nullable Data Type
                    Type propType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                    // Cast Enum Values Data Base
                    if (propType.IsEnum)
                        valueDB = Enum.ToObject(enumType: propType, value:valueDB);
                    prop.SetValue(Object, Convert.ChangeType(value:valueDB, conversionType:propType), null);
                }
            }

            return Object;
        }
        
        public IConfigurationRoot GetConfigurationSettings()
        {
            var builder = new ConfigurationBuilder().SetBasePath(basePath:Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
    }
}

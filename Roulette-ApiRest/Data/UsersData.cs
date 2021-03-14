using Roulette_ApiRest.Data.Entities;
using Roulette_ApiRest.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Roulette_ApiRest.Data
{
    public class UsersData:DataDB
    {
        public UsersData(string connection_name) : base(connection_name)
        {
        }
        
        public Crupier getCrupierByAccessKey(string access_key)
        {
            command.CommandText = "SELECT * FROM crupiers WHERE access_key = @access_key; ";
            command.Parameters.Clear();
            command.Parameters.Add(parameterName:"@access_key", sqlDbType:SqlDbType.VarChar).Value = access_key;
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
                    throw new DataNotFoundException("Crupier not found with the specified access key");
                }
            }

            return crupiersResult[0];
        }

        public Gambler getGamblerByAccessKey(string access_key)
        {
            command.CommandText = "SELECT * FROM gamblers WHERE access_key = @access_key; ";
            command.Parameters.Clear();
            command.Parameters.Add(parameterName: "@access_key", sqlDbType:SqlDbType.VarChar).Value = access_key;
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
                    throw new DataNotFoundException("Gambler not found with the specified access key");
                }
            }
            return gamblersResult[0];
        }

    }
}

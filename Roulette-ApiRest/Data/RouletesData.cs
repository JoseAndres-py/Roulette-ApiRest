using Roulette_ApiRest.Data.Entities;
using Roulette_ApiRest.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Roulette_ApiRest.Data
{
    public class RouletesData : DataDB
    {

        public RouletesData(string connection_name):base(connection_name)
        {
        }

        public Roulette getRouletteById(int id)
        {
            command.CommandText = "SELECT * FROM roulettes WHERE id = @id; ";
            command.Parameters.Clear();
            command.Parameters.Add(parameterName:"@id", sqlDbType:SqlDbType.Int).Value = id;
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
                    throw new DataNotFoundException("Roulete not found with the specified id");
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

        public Roulette addRoulette(int crupier_id)
        {
            command.CommandText = "INSERT INTO roulettes (id_crupier, open_date, state) OUTPUT Inserted.id VALUES(@id_crupier, GETDATE(), CONVERT(bit, 1)); ";
            command.Parameters.Clear();
            command.Parameters.Add(parameterName:"@id_crupier", sqlDbType:SqlDbType.Int).Value = crupier_id;
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

        public void updateRouletteStatus(int roulette_id, bool state)
        {
            command.CommandText = "UPDATE roulettes SET state = CONVERT(bit,@state), open_date = CASE WHEN @state = CONVERT(bit,'True') THEN GETDATE() ELSE open_date END, close_date = CASE WHEN @state = CONVERT(bit,'False') THEN GETDATE()  ELSE NULL END WHERE id = @id;";
            command.Parameters.Clear();
            command.Parameters.Add(parameterName:"@id", sqlDbType:SqlDbType.Int).Value = roulette_id;
            command.Parameters.Add(parameterName:"@state", sqlDbType:SqlDbType.Bit).Value = state;
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

    }
}

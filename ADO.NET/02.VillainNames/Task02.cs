using System.Threading.Tasks;
using System;
using ADO.NET.Connection.Strings;
using ADO.NET.Queries;
using Microsoft.Data.SqlClient;

namespace VillainNames
{
    public class Task02
    {
        public static async Task RunTask02()
        {
            SqlConnection connectionToMinionsDB = new SqlConnection(Connections.CONNECTION_TO_MINIONS_DB);
            await connectionToMinionsDB.OpenAsync();

            using (connectionToMinionsDB)
            {
                await WriteVillainsAndMinionsCount(connectionToMinionsDB);
            }
        }

        private static async Task WriteVillainsAndMinionsCount(SqlConnection connection)
        {
            SqlCommand selectVillainsAndMinionsCount = new SqlCommand(MinionsDBQueries.Task02Queries.SELECT_VILLAIN_AND_THEIR_MINIONS_COUNT_QUERY, connection);
            SqlDataReader queryReader = await selectVillainsAndMinionsCount.ExecuteReaderAsync();

            using (queryReader)
            {
                while (await queryReader.ReadAsync())
                {
                    Console.WriteLine($"{queryReader.GetString(0)} - {queryReader.GetInt32(1)}");
                }
            }
        }
    }
}
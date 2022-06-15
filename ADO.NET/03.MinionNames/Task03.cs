using ADO.NET;
using ADO.NET.Connection.Strings;
using ADO.NET.Queries;
using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;

namespace MinionNames
{
    public class Task03
    {
        public static async Task RunTask03()
        {
            Console.Write(Constants.PLEASE_ENTER_VILLAIN_ID);
            int villainId = int.Parse(Console.ReadLine());

            SqlConnection connectionToMinionsDB = new SqlConnection(Connections.CONNECTION_TO_MINIONS_DB);

            await RunQueriesToMinionsDB(villainId, connectionToMinionsDB);
        }

        private static async Task RunQueriesToMinionsDB(int id, SqlConnection connection)
        {
            await connection.OpenAsync();

            using (connection)
            {
                SqlCommand selectVillain = new SqlCommand(MinionsDBQueries.Task03Queries.SELECT_VILLAIN_QUERY, connection);
                selectVillain.Parameters.AddWithValue("@Id", id);

                string villain = (string) await selectVillain.ExecuteScalarAsync();
                try
                {
                    if (villain == null)
                    {
                        throw new Exception(string.Format(ErrorMessagesConstants.VILLAIN_DOESNT_EXIST_ERROR, id));
                    }

                    PrintVillain(villain);

                    SqlCommand selectVillainMinions = new SqlCommand(MinionsDBQueries.Task03Queries.SELECT_VILLAIN_MINIONS_QUERY, connection);
                    selectVillainMinions.Parameters.AddWithValue("@Id", id);

                    SqlDataReader queryReaderMinions = await selectVillainMinions.ExecuteReaderAsync();

                    using (queryReaderMinions)
                    {
                        await PrintMinions(queryReaderMinions);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        private static void PrintVillain(string villainName)
        {
            Console.WriteLine($"Villain: {villainName}");
        }

        private static async Task PrintMinions(SqlDataReader reader)
        {
            if (!reader.HasRows)
            {
                throw new Exception(ErrorMessagesConstants.NO_MINIONS_ERROR);
            }

            while (await reader.ReadAsync())
            {
                Console.WriteLine($"{reader["RowNum"]}. {reader["Name"]} {reader["Age"]}");
            }
        }
    }
}

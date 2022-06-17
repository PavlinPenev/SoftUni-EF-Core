using ADO.NET;
using ADO.NET.Connection.Strings;
using ADO.NET.Queries;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IncreaseAgeStoredProcedure
{
    public class Task09
    {
        public static async Task RunTask09()
        {
            Console.Write(Constants.PLEASE_ENTER_MINION_ID);
            int minionId = int.Parse(Console.ReadLine());
            string printValue = string.Empty;

            SqlConnection connectionToMinionsDB = new SqlConnection(Connections.CONNECTION_TO_MINIONS_DB);
            await connectionToMinionsDB.OpenAsync();

            using (connectionToMinionsDB)
            {
                SqlCommand createStoredProcedureGetOlderQuery = new SqlCommand(MinionsDBQueries.Task09Queries.CREATE_PROCEDURE_GET_OLDER_QUERY, connectionToMinionsDB);
                await createStoredProcedureGetOlderQuery.ExecuteNonQueryAsync();

                SqlCommand executeStoredProcedureGetOlderQuery = new SqlCommand(MinionsDBQueries.Task09Queries.EXEC_PROCEDURE_GET_OLDER_QUERY, connectionToMinionsDB);
                executeStoredProcedureGetOlderQuery.Parameters.AddWithValue("@Id", minionId);
                await executeStoredProcedureGetOlderQuery.ExecuteNonQueryAsync();

                SqlCommand selectMinionNameAgeQuery = new SqlCommand(MinionsDBQueries.Task09Queries.SELECT_MINION_NAMES_AND_AGE_QUERY, connectionToMinionsDB);
                selectMinionNameAgeQuery.Parameters.AddWithValue("@Id", minionId);

                SqlDataReader reader = await selectMinionNameAgeQuery.ExecuteReaderAsync();

                using (reader)
                {
                    while (await reader.ReadAsync())
                    {
                        printValue = $"{reader.GetString(0)} - {reader.GetInt32(1)} years old";
                    }
                }
            }

            Console.WriteLine(printValue);
        }
    }
}

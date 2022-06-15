using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using ADO.NET.Connection.Strings;
using ADO.NET.Queries;

namespace InitialSetup
{
    public class Task01
    {
        public static async Task RunTask01()
        {
            try
            {
                SqlConnection connectionToMaster = new SqlConnection(Connections.CONNECTION_TO_MASTER_DB);
                await connectionToMaster.OpenAsync();

                await using (connectionToMaster)
                {
                    SqlCommand createMinionsDB = new SqlCommand(MasterDBQueries.CREATE_MINIONS_DB_QUERY, connectionToMaster);
                    await createMinionsDB.ExecuteNonQueryAsync();
                }

                SqlConnection connectionToMinions = new SqlConnection(Connections.CONNECTION_TO_MINIONS_DB);
                await connectionToMinions.OpenAsync();

                await using (connectionToMinions)
                {
                    SqlCommand createTablesInMinionsDB = new SqlCommand(MinionsDBQueries.Task01Queries.CREATE_TABLES_MINIONS_DB_QUERY, connectionToMinions);
                    await createTablesInMinionsDB.ExecuteNonQueryAsync();

                    SqlCommand insertIntoMinionsDBTables = new SqlCommand(MinionsDBQueries.Task01Queries.INSERT_INTO_TABLES_IN_MINIONS_DB, connectionToMinions);
                    await insertIntoMinionsDBTables.ExecuteNonQueryAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}

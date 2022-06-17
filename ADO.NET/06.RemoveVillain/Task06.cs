using ADO.NET;
using ADO.NET.Connection.Strings;
using ADO.NET.Queries;
using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;

namespace RemoveVillain
{
    public class Task06
    {
        public static async Task RunTask06()
        {
            Console.Write(Constants.PLEASE_ENTER_VILLAIN_ID);
            int villainId = int.Parse(Console.ReadLine());

            string deletionMessage = string.Empty;
            int minionsCount = 0;

            SqlConnection connectionToMinionsDB = new SqlConnection(Connections.CONNECTION_TO_MINIONS_DB);
            await connectionToMinionsDB.OpenAsync();

            using (connectionToMinionsDB)
            {
                minionsCount = (int) await GetMinionsCount(connectionToMinionsDB, villainId);

                deletionMessage = (string) await RunVillainQueries(connectionToMinionsDB, villainId, minionsCount);
            }

            Console.WriteLine(deletionMessage);
        }

        private static async Task<string> RunVillainQueries(SqlConnection connectionToMinionsDB, int villainId, int minionsCount)
        {
            SqlCommand selectVillainQuery = new SqlCommand(MinionsDBQueries.Task06Queries.SELECT_VILLAINS_NAME_QUERY, connectionToMinionsDB);
            selectVillainQuery.Parameters.AddWithValue("@villainId", villainId);

            string villainName = (string) await selectVillainQuery.ExecuteScalarAsync();

            if (villainName != null)
            {
                SqlCommand deleteVillainQuery = new SqlCommand(MinionsDBQueries.Task06Queries.DELETE_VILLAIN_QUERY, connectionToMinionsDB);
                deleteVillainQuery.Parameters.AddWithValue("@villainId", villainId);

                await deleteVillainQuery.ExecuteNonQueryAsync();
                return string.Format(Constants.VILLAIN_DELETED, villainName) + Environment.NewLine + string.Format(Constants.MINIONS_RELEASED, minionsCount);
            }
            else
            {
                return ErrorMessagesConstants.NO_SUCH_VILLAIN_FOUND;
            }
        }

        private static async Task<int> GetMinionsCount(SqlConnection connection, int villainId)
        {
            SqlCommand getMinionsCountQuery = new SqlCommand(MinionsDBQueries.Task06Queries.GET_COUNT_OF_VILLAINS_MINIONS_QUERY, connection);
            getMinionsCountQuery.Parameters.AddWithValue("@villainId", villainId);

            int count = (int)await getMinionsCountQuery.ExecuteScalarAsync();

            return count;
        }
    }
}

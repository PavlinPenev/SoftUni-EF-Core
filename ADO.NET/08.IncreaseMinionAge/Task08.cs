using ADO.NET;
using ADO.NET.Connection.Strings;
using ADO.NET.Queries;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IncreaseMinionAge
{
    public class Task08
    {
        public static async Task RunTask08()
        {
            Console.Write(Constants.PLEASE_ENTER_MINION_IDS);
            List<int> minionIds = Console.ReadLine().Split().Select(int.Parse).ToList();
            List<Tuple<string, int>> minionsNameAge = new List<Tuple<string, int>>();

            SqlConnection connectionToMinionsDB = new SqlConnection(Connections.CONNECTION_TO_MINIONS_DB);
            await connectionToMinionsDB.OpenAsync();

            using (connectionToMinionsDB)
            {
                foreach (var id in minionIds)
                {
                    SqlCommand updateMinionAgeQuery = new SqlCommand(MinionsDBQueries.Task08Queries.UPDATE_MINIONS_AGE_QUERY, connectionToMinionsDB);
                    updateMinionAgeQuery.Parameters.AddWithValue("@Id", id);

                    await updateMinionAgeQuery.ExecuteNonQueryAsync();
                }

                SqlCommand selectMinionsNameAgeQuery = new SqlCommand(MinionsDBQueries.Task08Queries.SELECT_MINION_NAMES_AND_AGE_QUERY, connectionToMinionsDB);
                SqlDataReader reader = await selectMinionsNameAgeQuery.ExecuteReaderAsync();

                using (reader)
                {
                    while (await reader.ReadAsync())
                    {
                        Tuple<string, int> currentTuple = new Tuple<string, int>(reader.GetString(0), reader.GetInt32(1));

                        minionsNameAge.Add(currentTuple);
                    }
                }
            }

            Console.WriteLine(string.Join(Environment.NewLine, minionsNameAge.Select(x => $"{x.Item1} {x.Item2}")));
        }
    }
}

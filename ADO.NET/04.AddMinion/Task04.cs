using ADO.NET;
using ADO.NET.Connection.Strings;
using ADO.NET.Queries;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AddMinion
{
    public class Task04
    {
        public static async Task RunTask04()
        {
            Console.Write(Constants.PLEASE_ENTER_MINION_INFO);
            var minionInfo = Console.ReadLine().Split().Skip(1).ToList();
            Console.Write(Constants.PLEASE_ENTER_VILLAIN_INFO);
            var villainName = Console.ReadLine().Split().Skip(1).FirstOrDefault();

            try
            {
                SqlConnection connectionToMinionsDB = new SqlConnection(Connections.CONNECTION_TO_MINIONS_DB);
                await connectionToMinionsDB.OpenAsync();

                using (connectionToMinionsDB)
                {
                    await RunTownQueries(connectionToMinionsDB, minionInfo);
                    await RunVillainQueries(connectionToMinionsDB, villainName);
                    await RunMinionQueries(connectionToMinionsDB, minionInfo, villainName);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static async Task RunMinionQueries(SqlConnection connection, List<string> minionInfo, string villainName)
        {
            SqlCommand selectMinionQuery = new SqlCommand(MinionsDBQueries.Task04Queries.SELECT_MINIONS_ID_BY_NAME_QUERY, connection);
            selectMinionQuery.Parameters.AddWithValue("@Name", minionInfo[0]);

            SqlCommand selectTownQuery = new SqlCommand(MinionsDBQueries.Task04Queries.SELECT_TOWN_BY_NAME_QUERY, connection);
            selectTownQuery.Parameters.AddWithValue("@townName", minionInfo[2]);

            var townId = (int) await selectTownQuery.ExecuteScalarAsync();

            SqlCommand insertMinionQuery = new SqlCommand(MinionsDBQueries.Task04Queries.INSERT_MINION_QUERY, connection);
            insertMinionQuery.Parameters.AddWithValue("@name", minionInfo[0]);
            insertMinionQuery.Parameters.AddWithValue("@age", minionInfo[1]);
            insertMinionQuery.Parameters.AddWithValue("@townId", townId);

            SqlDataReader minionReader = await selectMinionQuery.ExecuteReaderAsync();

            if (!minionReader.HasRows)
            {
                await insertMinionQuery.ExecuteNonQueryAsync();
            }

            await minionReader.CloseAsync();

            var minionId = (int) await selectMinionQuery.ExecuteScalarAsync();

            SqlCommand selectVillainQuery = new SqlCommand(MinionsDBQueries.Task04Queries.SELECT_VILLAIN_BY_NAME_QUERY, connection);
            selectVillainQuery.Parameters.AddWithValue("@Name", villainName);

            var villainId = (int) await selectVillainQuery.ExecuteScalarAsync();

            SqlCommand assignMinionToVillainQuery = new SqlCommand(MinionsDBQueries.Task04Queries.ASSIGN_MINION_TO_VILLAIN_QUERY, connection);
            assignMinionToVillainQuery.Parameters.AddWithValue("@minionId", minionId);
            assignMinionToVillainQuery.Parameters.AddWithValue("@villainId", villainId);

            await assignMinionToVillainQuery.ExecuteNonQueryAsync();
            Console.WriteLine(
                string.Format(
                    Constants.SUCCESSFULLY_ASSIGNED_MINION_TO_VILLAIN, 
                    minionInfo[0], 
                    villainName
                ));
        }

        private static async Task RunVillainQueries(SqlConnection connection, string villainName)
        {
            SqlCommand selectVillainQuery = new SqlCommand(MinionsDBQueries.Task04Queries.SELECT_VILLAIN_BY_NAME_QUERY, connection);
            selectVillainQuery.Parameters.AddWithValue("@Name", villainName);
            
            SqlDataReader villainReader = await selectVillainQuery.ExecuteReaderAsync();

            if (!villainReader.HasRows)
            {
                SqlCommand insertVillainQuery = new SqlCommand(MinionsDBQueries.Task04Queries.INSERT_VILLAIN_QUERRY, connection);
                insertVillainQuery.Parameters.AddWithValue("villainName", villainName);

                await insertVillainQuery.ExecuteNonQueryAsync();
                Console.WriteLine(string.Format(Constants.SUCCESSFULLY_ADDED_VILLAIN, villainName));
            }

            await villainReader.CloseAsync();
        }

        private static async Task RunTownQueries(SqlConnection connection, List<string> minionInfo)
        {
            SqlCommand selectTownQuery = new SqlCommand(MinionsDBQueries.Task04Queries.SELECT_TOWN_BY_NAME_QUERY, connection);
            selectTownQuery.Parameters.AddWithValue("@townName", minionInfo[2]);
            SqlCommand insertTownQuery = new SqlCommand(MinionsDBQueries.Task04Queries.INSERT_TOWN_QUERY, connection);
            insertTownQuery.Parameters.AddWithValue("townName", minionInfo[2]);

            SqlDataReader townReader = await selectTownQuery.ExecuteReaderAsync();

            if (!townReader.HasRows)
            {
                await insertTownQuery.ExecuteNonQueryAsync();
                Console.WriteLine(string.Format(Constants.SUCCESSFULLY_ADDED_TOWN, minionInfo[2]));
            }

            await townReader.CloseAsync();
        }
    }
}

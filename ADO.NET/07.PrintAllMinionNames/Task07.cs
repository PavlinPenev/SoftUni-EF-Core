using ADO.NET.Connection.Strings;
using ADO.NET.Queries;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace PrintAllMinionNames
{
    public class Task07
    {
        public static async Task RunTask07()
        {
            List<string> minionNames = new List<string>();
            SqlConnection connectionToMinionsDB = new SqlConnection(Connections.CONNECTION_TO_MINIONS_DB);
            await connectionToMinionsDB.OpenAsync();

            using (connectionToMinionsDB)
            {
                DataTable table = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(MinionsDBQueries.Task07Queries.SELECT_ALL_MINION_NAMES_QUERY, connectionToMinionsDB);

                using (adapter)
                {
                    adapter.Fill(table);
                }

                foreach (DataRow minionRow in table.Rows)
                {
                    minionNames.Add(minionRow[0].ToString());
                }
            }

            int startIdx = 0;
            int endIdx = minionNames.Count - 1;

            for (int i = 0; i < minionNames.Count; i++)
            {
                if (i % 2 == 0)
                {
                    Console.WriteLine(minionNames[startIdx]);
                    startIdx++;
                }
                else
                {
                    Console.WriteLine(minionNames[endIdx]);
                    endIdx--;
                }
            }
        }
    }
}

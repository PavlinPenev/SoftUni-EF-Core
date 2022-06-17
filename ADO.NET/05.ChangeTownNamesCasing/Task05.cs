using ADO.NET;
using ADO.NET.Connection.Strings;
using ADO.NET.Queries;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChangeTownNamesCasing
{
    public class Task05
    {
        public static async Task RunTask05()
        {
            Console.Write(Constants.PLEASE_ENTER_COUNTRY_NAME);
            var countryName = Console.ReadLine();

            List<string> towns = new List<string>();

            var returnString = string.Empty;

            SqlConnection connectionToMinionsDB = new SqlConnection(Connections.CONNECTION_TO_MINIONS_DB);
            await connectionToMinionsDB.OpenAsync();

            using (connectionToMinionsDB)
            {
                SqlCommand selectTownsByGivenCountry = new SqlCommand(MinionsDBQueries.Task05Queries.SELECT_TOWNS_BY_GIVEN_COUNTRY_QUERY, connectionToMinionsDB);
                selectTownsByGivenCountry.Parameters.AddWithValue("@countryName", countryName);

                SqlDataReader reader = await selectTownsByGivenCountry.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    towns.Add(reader.GetString(0));
                }

                await reader.CloseAsync();

                SqlCommand updateSelectedTowns = new SqlCommand(MinionsDBQueries.Task05Queries.UPDATE_TOWNS_QUERY, connectionToMinionsDB);
                updateSelectedTowns.Parameters.AddWithValue("@countryName", countryName);

                if (towns.Count > 0)
                {
                    await updateSelectedTowns.ExecuteNonQueryAsync();
                    returnString = string.Format(Constants.TOWNS_AFFECTED, towns.Count);
                }
                else
                {
                    returnString = Constants.NO_TOWNS_AFFECTED;
                }
            }
            towns = towns.Select(x => x.ToUpper()).ToList();

            Console.WriteLine(
                returnString
                + (towns.Count > 0 
                    ? Environment.NewLine
                    + $"[{string.Join(", ", towns)}]" 
                    : "") 
            );
        }
    }
}

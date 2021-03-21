using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Dapper;
using System.Linq;

namespace PIV_2
{
    static class DapperHelper
    {
        private static readonly DynamicParameters Parameters = new DynamicParameters();
        public static int ExecuteNonQuery(string connectionString, string commandText)
        {
            using var connection = new SqlConnection(connectionString);
            return connection.Execute(commandText, Parameters);
        }
        public static void AddParameter(string text1, string text2)
        {
            Parameters.Add(text1, text2);
        }
        public static IEnumerable<Employee> ExecuteQuery(string connectionString, string commandText)
        {
            var connection = new SqlConnection(connectionString);
            var emp = connection.Query<Employee>(commandText, Parameters).ToList();
            foreach (var e in emp)
            {
                yield return e;
            }
        }
    }
}
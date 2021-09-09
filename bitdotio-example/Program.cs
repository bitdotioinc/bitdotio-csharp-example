using System;
using Npgsql;
using System.Data;

namespace bitdotio_example
{
    class Program
    {
        static void Main(string[] args)
        {
            var bitHost = "db.bit.io";

            //var bitUser = "<your username>";
            //var bitDbName = "<your repo name>";

            var bitApiKey = "<your API key here";

            // For this example, look at the sensor data from a public repo.
            var bitUser = "adam";
            var bitDbName = "sensors";

            var cs = $"Host={bitHost};Username={bitUser};Password={bitApiKey};Database={bitDbName}";

            using var con = new NpgsqlConnection(cs);
            con.Open();

            var sql = "SELECT * FROM \"adam/sensors\".\"measurements\" order by datetime desc;";

            using var cmd = new NpgsqlCommand(sql, con);

            using NpgsqlDataReader reader = cmd.ExecuteReader();
            DataTable schemaTable = reader.GetSchemaTable();

            // Show schema details
            foreach (DataRow row in schemaTable.Rows)
            {
                foreach (DataColumn column in schemaTable.Columns)
                {
                    Console.WriteLine(String.Format("{0} = {1}",
                    column.ColumnName, row[column]));
                }
            }

            // Show all data
            while (reader.Read()) 
            {
                for (int colNum = 0; colNum < reader.FieldCount; colNum++) 
                {
                    Console.Write(reader.GetName(colNum) + "=" +  reader[colNum] + " ");
                }
                Console.Write("\n");
            }
        }
    }
}

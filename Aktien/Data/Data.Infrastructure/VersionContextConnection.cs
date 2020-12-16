using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Data
{
    public class VersionContextConnection
    {
        public static string GetDatabaseConnectionstring()
        {
            NpgsqlConnectionStringBuilder npgsqlConnectionStringBuilder = new NpgsqlConnectionStringBuilder();
            npgsqlConnectionStringBuilder.Host = "localhost";
            npgsqlConnectionStringBuilder.Port = 5432;
            npgsqlConnectionStringBuilder.Database = "databaseAKTIE";
            npgsqlConnectionStringBuilder.Username = "postgres";
            npgsqlConnectionStringBuilder.Password = "masterkey";

            return npgsqlConnectionStringBuilder.ConnectionString;
        }
    }
}

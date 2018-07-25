using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace JL.Reminders.Data
{
    public class ConnectionStringFactory : IConnectionStringFactory
	{
	    public string GetConnectionString()
	    {
			MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();

		    builder.SslMode = MySqlSslMode.None;
		    builder.Server = "127.0.0.1";
		    builder.UserID = "reminders";
		    builder.Password = "f0T3~.M3#1";
		    builder.Database = "reminders";

		    return builder.ToString();
	    }
    }
}

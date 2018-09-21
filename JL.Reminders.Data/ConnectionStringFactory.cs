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
		    MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
		    {
			    SslMode = MySqlSslMode.None,
			    Server = Environment.GetEnvironmentVariable("REMINDERS_MYSQL_SERVER"),
			    UserID = Environment.GetEnvironmentVariable("REMINDERS_MYSQL_USER"),
			    Password = Environment.GetEnvironmentVariable("REMINDERS_MYSQL_PASSWORD"),
			    Database = Environment.GetEnvironmentVariable("REMINDERS_MYSQL_SCHEMA")
		    };

		    return builder.ToString();
	    }
    }
}

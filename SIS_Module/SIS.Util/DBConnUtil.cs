using System;
using SIS_Module.SIS.Entity;
using SIS_Module.SIS.Exception;
using SIS_Module.SIS.Main;
using SIS_Module.SIS.Dao;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

using Microsoft.Extensions.Configuration;

namespace SIS_Module.SIS.Util
{
    public static class DBConnectionUtil
    {
        private static string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=SISDB;Integrated Security=True;TrustServerCertificate=True";
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
/*private static IConfigurationRoot Configuration { get; set; }

static DBConnectionUtil()
{
    try
    {
        // Build the configuration from appsettings.json
        Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    }
    catch (ApplicationException ex)
    {
        // Handle any issues during configuration initialization
        Console.WriteLine("Failed to load configuration: " + ex.Message);
        throw; // Rethrow the exception
    }
}

public static SqlConnection GetConnection()
{
    string connectionString = Configuration.GetConnectionString("SISDBConnection");
    SqlConnection connection = new SqlConnection(connectionString);
    connection.Open();
    return connection; // Return the open connection
}
public static SqlConnection GetConnection()
{
    // Adjust your connection string as needed
    string connectionString = "Server=.\\sqlexpress;Database=SISDB;Trusted_Connection=True;TrustServerCertificate=True;";
    return new SqlConnection(connectionString); // Return a new connection each time
}

}
} */


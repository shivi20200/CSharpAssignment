using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using SIS_Module.SIS.Entity;
using SIS_Module.SIS.Util;
using SIS_Module.SIS.Dao;
using SIS_Module.SIS.Exception;

namespace SIS_Module.SIS.Dao
{
    public static class DBQueryBuilderUtil
    {
        public static List<Dictionary<string, object>> ExecuteDynamicQuery(string tableName, List<string> columns = null, Dictionary<string, object> conditions = null, string orderByColumn = null, string orderDirection = "ASC")
        {
            List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();

            try
            {
                using (SqlConnection connection = DBConnectionUtil.GetConnection())
                {
                    // Check the initial state of the connection
                    Console.WriteLine($"Connection State Before Opening: {connection.State}");

                    connection.Open(); // Ensure the connection is opened

                    // Check the state after opening
                    Console.WriteLine($"Connection State After Opening: {connection.State}");

                    StringBuilder queryBuilder = new StringBuilder();

                    // Build SELECT clause
                    queryBuilder.Append("SELECT ");
                    if (columns != null && columns.Count > 0)
                    {
                        queryBuilder.Append(string.Join(", ", columns));
                    }
                    else
                    {
                        queryBuilder.Append("*"); // Default: select all columns
                    }

                    queryBuilder.Append(" FROM " + tableName);

                    // Build WHERE clause if conditions exist
                    if (conditions != null && conditions.Count > 0)
                    {
                        queryBuilder.Append(" WHERE ");
                        int conditionIndex = 0;
                        foreach (var condition in conditions)
                        {
                            if (conditionIndex > 0)
                            {
                                queryBuilder.Append(" AND ");
                            }

                            queryBuilder.Append($"{condition.Key} = @Param{conditionIndex}");
                            conditionIndex++;
                        }
                    }

                    // Build ORDER BY clause if specified
                    if (!string.IsNullOrEmpty(orderByColumn))
                    {
                        queryBuilder.Append($" ORDER BY {orderByColumn} {orderDirection}");
                    }

                    using (SqlCommand command = new SqlCommand(queryBuilder.ToString(), connection))
                    {
                        // Add parameters for the conditions
                        if (conditions != null && conditions.Count > 0)
                        {
                            int paramIndex = 0;
                            foreach (var condition in conditions)
                            {
                                command.Parameters.AddWithValue($"@Param{paramIndex}", condition.Value);
                                paramIndex++;
                            }
                        }

                        // Execute the command
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var row = new Dictionary<string, object>();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    row[reader.GetName(i)] = reader.GetValue(i);
                                }
                                results.Add(row);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An error occurred while executing dynamic query: " + ex.Message);
            }
            catch (ApplicationException ex) // Catch other potential exceptions
            {
                Console.WriteLine("An unexpected error occurred: " + ex.Message);
            }

            return results;
        }
    }
}



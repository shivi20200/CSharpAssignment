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
using SIS_Module.SIS.Entity;

namespace SIS_Module.SIS.Dao
{
    public class PaymentDAO
    {
        public static List<Payment> GetAllPayments()
        {
            List<Payment> payments = new List<Payment>();
            try
            {
                using (SqlConnection connection = DBConnectionUtil.GetConnection())
                {
                    connection.Open(); // Ensure the connection is opened
                    string query = "SELECT * FROM Payments";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            payments.Add(new Payment
                            {
                                PaymentId = (int)reader["PaymentID"],
                                StudentId = (int)reader["StudentID"],
                                Amount = (decimal)reader["Amount"],
                                PaymentDate = (DateTime)reader["PaymentDate"]
                            });
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An error occurred while retrieving payments: " + ex.Message);
            }
            return payments;
        }

        public static void AddPayment(Payment payment)
        {
            try
            {
                using (SqlConnection connection = DBConnectionUtil.GetConnection())
                {
                    connection.Open(); // Open the connection
                    using (SqlTransaction transaction = connection.BeginTransaction()) // Start the transaction
                    {
                        string query = "INSERT INTO Payments (StudentID, Amount, PaymentDate) VALUES (@StudentID, @Amount, @PaymentDate)";
                        using (SqlCommand command = new SqlCommand(query, connection, transaction)) // Associate the transaction with the command
                        {
                            command.Parameters.AddWithValue("@StudentID", payment.StudentId);
                            command.Parameters.AddWithValue("@Amount", payment.Amount);
                            command.Parameters.AddWithValue("@PaymentDate", payment.PaymentDate);
                            command.ExecuteNonQuery();
                        }
                        transaction.Commit(); // Commit the transaction
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An error occurred while adding a payment: " + ex.Message);
                // Rollback if an error occurs (though the transaction is scoped to the using block)
            }
        }

        public static void UpdatePayment(Payment payment)
        {
            try
            {
                using (SqlConnection connection = DBConnectionUtil.GetConnection())
                {
                    connection.Open(); // Open the connection
                    using (SqlTransaction transaction = connection.BeginTransaction()) // Start the transaction
                    {
                        string query = "UPDATE Payments SET StudentID = @StudentID, Amount = @Amount, PaymentDate = @PaymentDate WHERE PaymentID = @PaymentID";
                        using (SqlCommand command = new SqlCommand(query, connection, transaction)) // Associate the transaction with the command
                        {
                            command.Parameters.AddWithValue("@StudentID", payment.StudentId);
                            command.Parameters.AddWithValue("@Amount", payment.Amount);
                            command.Parameters.AddWithValue("@PaymentDate", payment.PaymentDate);
                            command.Parameters.AddWithValue("@PaymentID", payment.PaymentId);
                            command.ExecuteNonQuery();
                        }
                        transaction.Commit(); // Commit the transaction
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An error occurred while updating the payment: " + ex.Message);
                // Rollback if an error occurs (though the transaction is scoped to the using block)
            }
        }
    }

}

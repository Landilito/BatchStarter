using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchStarter
{
    class Program
    {
        static void Main(string[] args)
        {

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = "Server=DESKTOP-J6ACRTS;Database=CIA_Factbook_DB;Trusted_Connection=True";

                connection.Open();
                SqlCommand query = new SqlCommand("SELECT * FROM City WHERE Name = @CityName", connection);
                query.Parameters.Add(new SqlParameter("CityName", "Washington"));

                Console.WriteLine("Programa para correr batches");

                using (SqlDataReader reader = query.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(String.Format("{0} \t | {1} \t | {2} \t | {3}",
                        reader[0], reader[1], reader[2], reader[3]));
                    }
                }
                connection.Close();
            }

        }
    }
}

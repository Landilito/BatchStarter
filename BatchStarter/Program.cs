using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace BatchStarter
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection())
                {
                    Console.WriteLine("Programa para correr batches");

                    var time = DateTime.Now.Hour;

                    connection.ConnectionString = "Server=192.168.100.42;Database=DB_SMS_Connections;User ID=usrTableau;Password=kMZdPuTyj7c5RbXn";
                    connection.Open();
                    SqlCommand query = new SqlCommand("SELECT COUNT(*)"
                                    + "FROM[DB_SMS_Connections].[dbo].[SmppInputMessage] with(nolock)"
                       + " Where connectionId = 'S_NEXTEL_MX_LONG_RX1' and date >= DATEADD(MINUTE, -2, GETDATE())", connection);

                    if (time >= 20)
                    {
                        query = new SqlCommand("SELECT COUNT(*)"
                                        + "FROM[DB_SMS_Connections].[dbo].[SmppInputMessage] with(nolock)"
                           + " Where connectionId = 'S_NEXTEL_MX_LONG_RX1' and date >= DATEADD(MINUTE, -10, GETDATE())", connection);
                    }
                    else if (time >= 0 && time <= 6)
                    {                        query = new SqlCommand("SELECT COUNT(*)"
                                        + "FROM[DB_SMS_Connections].[dbo].[SmppInputMessage] with(nolock)"
                           + " Where connectionId = 'S_NEXTEL_MX_LONG_RX1' and date >= DATEADD(MINUTE, -30, GETDATE())", connection);
                    }

                    using (SqlDataReader reader = query.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int MO = (int)reader[0];

                            Console.WriteLine(String.Format("La cantidad de MO es {0}", MO));

                            if (MO >= 1)
                            {
                                Console.WriteLine("Connection is up");
                            }
                            else
                            {
                                Console.WriteLine("Connection is down. Atempting to restart...");
                                Process batch = new Process();
                                batch.StartInfo.CreateNoWindow = true;
                                batch.StartInfo.FileName = @"Restart S_NEXTEL_MX_LONG_RX1.bat";
                                batch.Start();
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Ocurrio un error \n" + e);
            }

            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
    }
}

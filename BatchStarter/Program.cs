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
            int MO = 0;

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = "Server=DESKTOP-J6ACRTS;Database=HOLA;Trusted_Connection=True";
                connection.Open();
                SqlCommand query = new SqlCommand("SELECT COUNT(*) FROM dbo.adios Where adios1 = 2", connection);
                //SqlCommand query = new SqlCommand("SELECT @MO = COUNT(*)"
                //    +"FROM[DB_SMS_Connections].[dbo].[SmppInputMessage] with(nolock)"
                //    +" Where connectionId = 'S_NEXTEL_MX_LONG_RX1' and date >= DATEADD(MINUTE, -5, GETDATE())", connection);
                //query.Parameters.Add(new SqlParameter("kk",2));

                Console.WriteLine("Programa para correr batches");

                using (SqlDataReader reader = query.ExecuteReader())
                {
                    while (reader.Read())
                    { 
                        MO = (int)reader[0];

                        Console.WriteLine(String.Format("El valor es {0}", MO));
                    }
                }
                connection.Close();
            }

            if(MO > 1)
            {
                Console.WriteLine("Connection is up");
            }
            else
            {
                Console.WriteLine("Connection is down. ");
                               
                Process batch = new Process();
                batch.StartInfo.CreateNoWindow = true;            
                batch.StartInfo.FileName = @"kk.bat";                
                batch.Start();

                Console.WriteLine("...");

                ServiceController sc = new ServiceController("TeamViewer");
                if (sc.Status == ServiceControllerStatus.Running)
                {
                    Console.Write("Service is running.");
                }
                else
                {
                    Console.Write("Service not running.");
                }
            }            

            Console.ReadKey();
        }
    }
}

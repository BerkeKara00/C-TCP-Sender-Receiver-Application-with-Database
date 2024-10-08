using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Reflection.Emit;
using System.Text;
using System.Windows.Forms;

namespace SONDENEME1
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Random random = new Random();
            double sayi = random.NextDouble()*100;


            label1.Text = ($"Random Sayı :  {sayi}");


            try
            {
                TcpClient tcpClient = new TcpClient("127.0.0.1", 5000);

                NetworkStream stream = tcpClient.GetStream();

                byte[] dataSend = Encoding.ASCII.GetBytes(sayi.ToString());

                stream.Write(dataSend, 0, dataSend.Length);




                tcpClient.Close();

            }
            catch (Exception ex)
            {

                MessageBox.Show($"TCP ERROR ! {ex.Message}");
            }



            try
            {
                SqlConnection conn = new SqlConnection("Data Source=DESKTOP-STKDCTQ\\SQLEXPRESS;Initial Catalog=Enmos;Integrated Security=True");

                conn.Open();

                string insertQuery = "INSERT INTO Log (data, date) VALUES (@data, @date)";

                SqlCommand sqlCommand = new SqlCommand(insertQuery, conn);

                sqlCommand.Parameters.AddWithValue("@data", sayi);
                sqlCommand.Parameters.AddWithValue("@date", DateTime.Now);

                sqlCommand.ExecuteNonQuery();

                MessageBox.Show("Veritabanına başarıyla kayıt yapıldı.");


                label1.Text = ($"Random Sayı : ");


                conn.Close();

            }catch(Exception ex)
            {
                MessageBox.Show($"Veritabanı Hatası {ex.Message}");
            }
        }
    }
}

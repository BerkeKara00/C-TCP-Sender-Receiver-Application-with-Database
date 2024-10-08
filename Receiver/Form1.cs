using System;
using System.Data.SqlClient;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;


namespace Tcp_arayuz
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {

        TcpListener listener;
        int clientCount = 0;  
        

        public Form1()
        {
            InitializeComponent();
        }

        private void startListening()
        {
            try
            {
                listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 5000);
                listener.Start();

                Invoke(new Action(() =>
                {
                    labelStatus.Text = "Sunucu dinleme modunda... İstemci sayısı: 0";
                }));

                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    NetworkStream stream = client.GetStream();

                    clientCount++;

                    Invoke(new Action(() =>
                    {
                        labelStatus.Text = $"Bir istemci bağlandı! İstemci sayısı: {clientCount}";
                    }));

                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);

                    string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    string displayMessage = $"\nData: {receivedData} \nTime: {DateTime.Now}";

                    Invoke(new Action(() =>
                    {
                        label1.Text = $"Sunucudan Gelen Bilgiler : \n{displayMessage}";
                    }));

                    client.Close(); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sunucu başlatılamadı: " + ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Thread thread = new Thread(startListening);
            thread.IsBackground = true;
            thread.Start();
        }

        SqlConnection conn = new SqlConnection("DATA SOURCE=DESKTOP-STKDCTQ\\SQLEXPRESS;Initial Catalog=Enmos;Integrated Security=True");

        private void displayDatabase()
        {


            listView1.Items.Clear();
            conn.Open();
            
            SqlCommand cmd = new SqlCommand("SELECT * FROM Log", conn);

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                ListViewItem item = new ListViewItem();
                item.Text = (reader["id"].ToString());
                item.SubItems.Add(reader["data"].ToString());
                item.SubItems.Add(reader["date"].ToString());

                listView1.Items.Add(item);
            }
            conn.Close();
        }

        private void btn_display_Click(object sender, EventArgs e)
        {
            displayDatabase();
        }
    }
}

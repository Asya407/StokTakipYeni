using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace StokTakip
{
    public partial class GirisForm : Form
    {
        public GirisForm()
        {
            InitializeComponent();
        }

        private void btnGiris_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=DESKTOP-PS08KAK\\SQLEXPRESS;Initial Catalog=StokDB;Integrated Security=True;";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Kullanicilar WHERE KullaniciAdi=@kadi AND Sifre=@sifre";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@kadi", txtKullaniciAdi.Text);
                cmd.Parameters.AddWithValue("@sifre", txtSifre.Text);

                con.Open();
                int sonuc = (int)cmd.ExecuteScalar();
                con.Close();

                if (sonuc > 0)
                {
                    MessageBox.Show("Giriş başarılı!");
                    this.Hide();
                    Form1 anaForm = new Form1();
                    anaForm.Show();
                }
                else
                {
                    MessageBox.Show("Kullanıcı adı veya şifre yanlış.");
                }
            }
        }
    }
}

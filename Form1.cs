using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace StokTakip
{
    public partial class Form1 : Form
    {
        private string connectionString = "Data Source=DESKTOP-PS08KAK\\SQLEXPRESS;Initial Catalog=StokDB;Integrated Security=True;";

        public Form1()
        {
            InitializeComponent();
            this.Load += new EventHandler(Form1_Load);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UrunleriListele();
        }

        private void UrunleriListele()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Urunler";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void Temizle()
        {
            txtUrunAdi.Text = "";
            txtFiyat.Text = "";
            txtMiktar.Text = "";
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Urunler (UrunAdi, Fiyat, Miktar) VALUES (@ad, @fiyat, @miktar)";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@ad", txtUrunAdi.Text);
                    cmd.Parameters.AddWithValue("@fiyat", decimal.Parse(txtFiyat.Text));
                    cmd.Parameters.AddWithValue("@miktar", int.Parse(txtMiktar.Text));

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("Ürün başarıyla eklendi!");
                    UrunleriListele();
                    Temizle();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ekleme hatası: " + ex.Message);
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Id"].Value);

                DialogResult result = MessageBox.Show("Bu ürünü silmek istiyor musunuz?", "Onay", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        string query = "DELETE FROM Urunler WHERE Id = @id";
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@id", id);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();

                        MessageBox.Show("Ürün silindi.");
                        UrunleriListele();
                        Temizle();
                    }
                }
            }
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Id"].Value);

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Urunler SET UrunAdi = @ad, Fiyat = @fiyat, Miktar = @miktar WHERE Id = @id";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@ad", txtUrunAdi.Text);
                    cmd.Parameters.AddWithValue("@fiyat", decimal.Parse(txtFiyat.Text));
                    cmd.Parameters.AddWithValue("@miktar", int.Parse(txtMiktar.Text));
                    cmd.Parameters.AddWithValue("@id", id);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("Ürün güncellendi.");
                    UrunleriListele();
                    Temizle();
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                txtUrunAdi.Text = row.Cells["UrunAdi"].Value.ToString();
                txtFiyat.Text = row.Cells["Fiyat"].Value.ToString();
                txtMiktar.Text = row.Cells["Miktar"].Value.ToString();
            }
        }
    }
}

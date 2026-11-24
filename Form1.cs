using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace PRAKTIKUM2_DESKTOP_XII_242507013_Rais_Prayata_Wiguna
{
    public partial class Form1 : Form
    {
        static string connect = "server=localhost;username=root;password=;database=db_apotek_gudang;";
        public Form1()
        {
            InitializeComponent();
        }
        int selectedId = 0;
        //string ex = '';
        //int selectedId = 0;


private void TampilkanData()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connect))
                {
                    conn.Open();
                    string query = "SELECT id_suplemen, kode_barang, nama_barang, kategori, stok, satuan, harga_satuan, batas_minimum, terakhir_update FROM tb_suplemen";
                    MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvSuplemen.DataSource = dt;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Koneksi gagal" + ex.Message);
            }
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            TampilkanData();
        }
        private void BersihkanForm()
        {
            
            txtKode.Clear();
            txtNama.Clear();
            txtStok.Clear();
            txtHarga.Clear();
            txtBatasMin.Clear();

            cmbKategori.SelectedIndex = -1 ;
            cmbSatuan.SelectedIndex = -1;
            //txtKode.Enabled = true;
            dgvSuplemen.ClearSelection();
            txtKode.Focus();
        }

        private void btnBersihkan_Click(object sender, EventArgs e)
        {
            BersihkanForm();
        }

        private void dgvSuplemen_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvSuplemen.Rows[e.RowIndex];
                txtKode.Text = row.Cells[1].Value?.ToString();
                txtNama.Text = row.Cells[2].Value?.ToString();
                cmbKategori.SelectedText = row.Cells[3].Value?.ToString();
                txtStok.Text = row.Cells[4].Value?.ToString();
                cmbSatuan.SelectedText = row.Cells[5].Value?.ToString();
                txtHarga.Text = row.Cells[6].Value?.ToString();
                txtBatasMin.Text = row.Cells[7].Value?.ToString();
                textBox1.Text = row.Cells[0].Value?.ToString();
            }
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtKode.Text) ||
                string.IsNullOrWhiteSpace(txtNama.Text) ||
                string.IsNullOrWhiteSpace(txtStok.Text) ||
                string.IsNullOrWhiteSpace(txtHarga.Text) ||
                string.IsNullOrWhiteSpace(txtBatasMin.Text))
            {
                MessageBox.Show("ada kolom yang kosong");
                return;
            }
            try
            {
                using (MySqlConnection conect = new MySqlConnection(connect))
                {
                    conect.Open();

                    string cekQuery = "SELECT COUNT(*) FROM tb_suplemen WHERE kode_barang = @kode";
                    MySqlCommand cekCmd = new MySqlCommand(cekQuery, conect);
                    cekCmd.Parameters.AddWithValue("@kode", txtKode.Text.Trim());

                    if (Convert.ToInt32(cekCmd.ExecuteScalar()) > 0)
                    {
                        MessageBox.Show("Kode barang sudah digunakan");
                        return;
                    }


                    string insertQuery = @"INSERT INTO tb_suplemen (kode_barang, nama_barang, kategori, stok, satuan, harga_satuan, batas_minimum) 
                                               VALUES (@kode, @nama, @kategori, @stok, @satuan, @harga, @batas)";
                    MySqlCommand cmd = new MySqlCommand(insertQuery, conect);
                    cmd.Parameters.AddWithValue("@kode", txtKode.Text.Trim());
                    cmd.Parameters.AddWithValue("@nama", txtNama.Text.Trim());
                    cmd.Parameters.AddWithValue("@kategori", cmbKategori.Text);
                    cmd.Parameters.AddWithValue("@stok", txtStok.Text);
                    cmd.Parameters.AddWithValue("@satuan", cmbSatuan.Text);
                    cmd.Parameters.AddWithValue("@harga", txtHarga.Text);
                    cmd.Parameters.AddWithValue("@batas", txtBatasMin.Text);
                    cmd.ExecuteNonQuery();


                    MessageBox.Show("Data berhasil disimpan" + MessageBoxButtons.OK);
                    TampilkanData();
                    BersihkanForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("data gagal disimpan" + ex.Message + MessageBoxButtons.OK);
            }
        }

        private void btnUbah_Click(object sender, EventArgs e)
        {
            MySqlConnection conn = new MySqlConnection(connect);
            try
            {
                conn.Open();

                string query = "UPDATE tb_suplemen SET nama_barang, kategori, stok, satuan, harga_satuan, batas_minimum WHERE kode_barang = @kode_barang";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@kode", txtKode.Text.Trim());
                cmd.Parameters.AddWithValue("@nama", txtNama.Text.Trim());
                cmd.Parameters.AddWithValue("@kategori", cmbKategori.Text);
                cmd.Parameters.AddWithValue("@stok", txtStok.Text);
                cmd.Parameters.AddWithValue("@satuan", cmbSatuan.Text);
                cmd.Parameters.AddWithValue("@harga", txtHarga.Text);
                cmd.Parameters.AddWithValue("@batas", txtBatasMin.Text);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Data diubah" + MessageBoxButtons.OK);
                TampilkanData();
                BersihkanForm();
            }
            catch(Exception ex)
            {
                MessageBox.Show("data gagal diubah" + ex.Message + MessageBoxButtons.OK);
            }
        }



        private void btnHapus_Click(object sender, EventArgs e)
        {
            //if (selectedId == 0)
            //{
            //    MessageBox.Show("pilih baris yg akan dihapus");
            //    return;
            //}
            if (selectedId == 0)
            {
                MessageBox.Show("ambil data terlebih dahulu");
                return;
            }


            //DialogResult confirm = MessageBox.Show("hapus?");

            //if (confirm == DialogResult.Yes)
            {
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connect))
                    {
                        conn.Open();
                        string deleteQuery = "DELETE FROM tb_suplemen WHERE id_suplemen = @id";
                        MySqlCommand cmd = new MySqlCommand(deleteQuery, conn);
                        cmd.Parameters.AddWithValue("@id", selectedId);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Data dihapus");


                        TampilkanData();
                        BersihkanForm();
                        selectedId = 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal menghapus data: " + ex.Message);
                }
            }
        }
    }
}

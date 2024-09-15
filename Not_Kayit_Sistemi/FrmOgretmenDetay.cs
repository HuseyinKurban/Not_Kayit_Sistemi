using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Not_Kayit_Sistemi
{
    public partial class FrmOgretmenDetay : Form
    {
        public FrmOgretmenDetay()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-32Q9FH5;Initial Catalog=DbNotKayıt;Integrated Security=True;");

        void sinifortalamasi()
        {
            lblgecensayisi.Text = dbNotKayıtDataSet.TBLDERS.Count(x => x.DURUM == true).ToString();
            lblkalansayisi.Text = dbNotKayıtDataSet.TBLDERS.Count(x => x.DURUM == false).ToString();
            lblsınıfortalaması.Text = dbNotKayıtDataSet.TBLDERS.Average(x => x.ORTALAMA).ToString("0.00");
        }
        private void FrmOgretmenDetay_Load(object sender, EventArgs e)
        {
            this.tBLDERSTableAdapter.Fill(this.dbNotKayıtDataSet.TBLDERS);
            sinifortalamasi();
        }

        private void Btnogrencikaydet_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("insert into TBLDERS (OGRNUMARA,OGRAD,OGRSOYAD) values (@p1,@p2,@p3)", baglanti);
            komut.Parameters.AddWithValue("@p1", msknumara.Text);
            komut.Parameters.AddWithValue("@p2", txtad.Text);
            komut.Parameters.AddWithValue("@p3", txtsoyad.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show(txtad.Text + " " + txtsoyad.Text + " Sisteme Kaydedildi", "Bilgi");
            sinifortalamasi();
            this.tBLDERSTableAdapter.Fill(this.dbNotKayıtDataSet.TBLDERS);
            

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;

            msknumara.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();
            txtad.Text = dataGridView1.Rows[secilen].Cells[2].Value.ToString();
            txtsoyad.Text = dataGridView1.Rows[secilen].Cells[3].Value.ToString();

            txtsinav1.Text = dataGridView1.Rows[secilen].Cells[4].Value.ToString();
            txtsinav2.Text = dataGridView1.Rows[secilen].Cells[5].Value.ToString();
            txtsinav3.Text = dataGridView1.Rows[secilen].Cells[6].Value.ToString();
            lblortalama.Text = dataGridView1.Rows[secilen].Cells[7].Value.ToString();

        }

        private void Btnguncelle_Click(object sender, EventArgs e)
        {

            double ortalama, s1, s2, s3;
            string durum;
            s1 = Convert.ToDouble(txtsinav1.Text);
            s2 = Convert.ToDouble(txtsinav2.Text);
            s3 = Convert.ToDouble(txtsinav3.Text);
            ortalama = (s1 + s2 + s3) / 3;
            lblortalama.Text = ortalama.ToString("0.00");


            if (ortalama >= 50)
            {
                durum = "True";
            }
            else
            {
                durum = "False";
            }


            baglanti.Open();
            SqlCommand komut = new SqlCommand("Update TBLDERS set OGRS1=@p1,OGRS2=@p2,OGRS3=@p3,ORTALAMA=@p4, DURUM=@p5 where OGRNUMARA=@p6", baglanti);
            komut.Parameters.AddWithValue("@p1", txtsinav1.Text);
            komut.Parameters.AddWithValue("@p2", txtsinav2.Text);
            komut.Parameters.AddWithValue("@p3", txtsinav3.Text);
            komut.Parameters.AddWithValue("@p4", decimal.Parse(lblortalama.Text));
            komut.Parameters.AddWithValue("@p5", durum);
            komut.Parameters.AddWithValue("@p6", msknumara.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Öğrenci Notları Güncellendi", "Bilgi");
            sinifortalamasi();
            this.tBLDERSTableAdapter.Fill(this.dbNotKayıtDataSet.TBLDERS);


        }
    }
}

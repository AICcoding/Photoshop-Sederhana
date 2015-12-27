using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Photoshop_Sederhana
{
    public partial class Form1 : Form
    {
        public int mode, filter_standar, filter_advanced, panjang_kernel;
        public int skala_pembesaran;
        //gambar awal = gambar asli
        //gambar edit = gambar untuk edit
        //gambar akhir = hasil olahan
        public Image<Bgr, Byte> gambar_awal, gambar_edit, gambar_akhir, gambar_edit_filter;
        public int[,] kernel;


        public Form1()
        {
            InitializeComponent();
            panjang_kernel = 3;
            skala_pembesaran = 1000;
            button1.Enabled = false;
            button2.Enabled = false;
        }

        #region Event

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            keluar_aplikasi(e);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buat_baru();
            button1.Enabled = false;
            button2.Enabled = false;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            masukkan_gambar();
            button1.Enabled = false;
            button2.Enabled = false;
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpan_gambar();
            button1.Enabled = false;
            button2.Enabled = false;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
           keluar_aplikasi();
        }

        private void convertToBinerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            konversi_biner();
            button1.Enabled = true;
            button2.Enabled = true;
        }

        private void convertToGrayscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            konversi_grayscale();
            button1.Enabled = true;
            button2.Enabled = true;
        }

        private void convertToMatrixToolStripMenuItem_Click(object sender, EventArgs e)
        {
            konversi_matrik();
        }

        private void convertToNegativeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            konverti_negatif();
            button1.Enabled = true;
            button2.Enabled = true;
        }

        private void changeBrightnessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ubah_brightness_contras();
        }       

        private void nonLinearImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pemetaan_nonLinear();
            button1.Enabled = true;
            button2.Enabled = true;
        }

        private void filterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            edit_filter();
            button1.Enabled = false;
            button2.Enabled = false;
        }

        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tampilkan_histogram();
        }

        private void fourierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tampilkan_fourier_transform();
        }

        private void checkForUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cek_for_update();
        }

        private void aboutPhotoshopSederhanaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tentang_photoshop();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            ubah_ukuran_gambar_awal();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            ubah_ukuran_gambar_akhir();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            apply();
            button1.Enabled = false;
            button2.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cancel();
            button1.Enabled = false;
            button2.Enabled = false;
        }

        private void skalaPembesaranGambarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ubah_rasio_zoom();
        }

        #endregion

        #region Method

        private void buat_baru()
        {
            gambar_awal.Dispose();
            gambar_edit.Dispose();
            gambar_akhir.Dispose();
            pictureBox1.Image = null;
            pictureBox2.Image = null;
            trackBar1.Value = 0;
            trackBar2.Value = 0;
            button1.Enabled = false;
            button2.Enabled = false;
        }

        private void masukkan_gambar()
        {
            OpenFileDialog pilih_gambar = new OpenFileDialog();
            pilih_gambar.Filter = "File gambar (*.BMP; *.JPG; *.PNG)|*.BMP; *.JPG; *.PNG";
            if (pilih_gambar.ShowDialog() == DialogResult.OK)
            {
                gambar_awal = new Image<Bgr, byte>(pilih_gambar.FileName);
                gambar_edit = new Image<Bgr, byte>(pilih_gambar.FileName);
                gambar_akhir = new Image<Bgr, byte>(pilih_gambar.FileName);

                //gambar_awal.Width + (panjang_kernel - 1), gambar_awal.Height + (panjang_kernel - 1)

                pictureBox1.Size = new Size(gambar_awal.Width, gambar_awal.Height);
                pictureBox2.Size = new Size(gambar_edit.Width, gambar_edit.Height);

                pictureBox1.Image = gambar_awal.ToBitmap();
                pictureBox2.Image = gambar_edit.ToBitmap();
            
            }
        }

        private void tambah_bingkai()
        {
            gambar_edit_filter = new Image<Bgr, byte>(gambar_awal.Width + (panjang_kernel - 1), gambar_awal.Height + (panjang_kernel - 1));

            Byte[,,] GetPixel_e = gambar_edit.Data; //Mengambil warna dari gambar awal
            Byte[,,] SetPixel_e = gambar_edit_filter.Data; //Mengeset warna ke gambar edit

            int r, g, b;
            for (int i = 0; i < gambar_edit_filter.Width; i++)
            {
                for (int j = 0; j < gambar_edit_filter.Height; j++)
                {
                    if (i < ((panjang_kernel - 1) / 2))//pemberian nilai 0
                    {
                        SetPixel_e[j, i, 0] = (byte)0;
                        SetPixel_e[j, i, 1] = (byte)0;
                        SetPixel_e[j, i, 2] = (byte)0;
                    }
                    else if (j < ((panjang_kernel - 1) / 2)) //pemberian nilai 0
                    {
                        SetPixel_e[j, i, 0] = (byte)0;
                        SetPixel_e[j, i, 1] = (byte)0;
                        SetPixel_e[j, i, 2] = (byte)0;
                    }
                    else if (j >= (gambar_edit_filter.Height - ((panjang_kernel - 1) / 2)))
                    {
                        SetPixel_e[j, i, 0] = (byte)0;
                        SetPixel_e[j, i, 1] = (byte)0;
                        SetPixel_e[j, i, 2] = (byte)0;
                    }
                    else if (i >= (gambar_edit_filter.Width - ((panjang_kernel - 1) / 2)))
                    {
                        SetPixel_e[j, i, 0] = (byte)0;
                        SetPixel_e[j, i, 1] = (byte)0;
                        SetPixel_e[j, i, 2] = (byte)0;
                    }
                    else
                    {
                        b = GetPixel_e[(j - ((panjang_kernel - 1) / 2)), (i - ((panjang_kernel - 1) / 2)), 0];
                        g = GetPixel_e[(j - ((panjang_kernel - 1) / 2)), (i - ((panjang_kernel - 1) / 2)), 1];
                        r = GetPixel_e[(j - ((panjang_kernel - 1) / 2)), (i - ((panjang_kernel - 1) / 2)), 2];

                        SetPixel_e[j, i, 0] = (byte)b;
                        SetPixel_e[j, i, 1] = (byte)g;
                        SetPixel_e[j, i, 2] = (byte)r;
                    }
                }
            }
        }

        private void simpan_gambar()
        {
            MessageBox.Show("kode konden ade brow...");            
        }

        private void keluar_aplikasi(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (MessageBox.Show("Apakah anda yakin ingin menutup aplikasi ini ?",
                               "Tutup Photoshop Sederhana",
                                MessageBoxButtons.OKCancel,
                                MessageBoxIcon.Information) == DialogResult.OK)
                    Environment.Exit(1);
                else
                    e.Cancel = true; // to don't close form is user change his mind
            }
        }

        private void keluar_aplikasi()
        {
            DialogResult keluar;

            keluar = MessageBox.Show("Apakah anda yakin ingin menutup aplikasi ini ?", "Tutup Photoshop Sederhana", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (keluar == DialogResult.Yes)
            {
                Application.Exit();
            }   
        }

        private void tentang_photoshop()
        {
            MessageBox.Show("Photoshop sederhana versi 1.0\n\nAplikasi ini merupakan mini photoshop yang dibuat oleh anak bangsa.\n\n1. I Wayan Ariantha Sentanu\t[1308605009]\n2. I Gede Pramarta Sedana\t[1308605027]\n3. I Putu Agus Suarya Wibawa\t[1308605034]\n4. Daniel Kurniawan\t\t[1308605039]",
                                  "About Photoshop sederhana", MessageBoxButtons.OK,
                                  MessageBoxIcon.Information,
                                  0);
        }

        private void cek_for_update()
        {
            MessageBox.Show("Anda menggunakan versi terbaru dari aplikasi Photoshop sederhana!",
                                  "Aplikasi up to date", MessageBoxButtons.OK,
                                  MessageBoxIcon.Information,
                                  0);
        }

        private void ubah_ukuran_gambar_awal()
        {
            try
            {
                pictureBox1.Width = gambar_awal.Width;
                pictureBox1.Height = gambar_awal.Height;

                pictureBox1.Width += (Convert.ToInt16(trackBar1.Value) * pictureBox1.Width) / skala_pembesaran;
                pictureBox1.Height += (Convert.ToInt16(trackBar1.Value) * pictureBox1.Height) / skala_pembesaran;
            }
            catch
            {

            }
        }

        private void ubah_ukuran_gambar_akhir()
        {
            try
            {
                pictureBox2.Width = gambar_edit.Width;
                pictureBox2.Height = gambar_edit.Height;

                pictureBox2.Width += (Convert.ToInt16(trackBar2.Value) * pictureBox2.Width) / skala_pembesaran;
                pictureBox2.Height += (Convert.ToInt16(trackBar2.Value) * pictureBox2.Height) / skala_pembesaran;
            }
            catch
            {

            }
        }

        private void konversi_biner()
        {
            gambar_akhir = new Image<Bgr, byte>(gambar_awal.Width, gambar_awal.Height);
            Color warna_pixel, warna_baru;
            Byte[, ,] GetPixel_e = gambar_edit.Data; //Mengambil warna dari gambar awal
            Byte[, ,] SetPixel_e = gambar_akhir.Data; //Mengeset warna ke gambar edit

            int rata, nGreen, nRed, nBlue, r, g, b;
            for (int i = 0; i <= gambar_akhir.Height - 1; i++)
            {
                for (int j = 0; j <= gambar_akhir.Width - 1; j++)
                {
                    b = GetPixel_e[i, j, 0];
                    g = GetPixel_e[i, j, 1];
                    r = GetPixel_e[i, j, 2];
                    warna_pixel = Color.FromArgb(r, g, b);

                    nRed = warna_pixel.R;
                    nGreen = warna_pixel.G;
                    nBlue = warna_pixel.B;
                    rata = Convert.ToInt32((nRed + nBlue + nGreen) / 3);
                    if (rata > 127)
                        rata = 255;
                    else
                        rata = 0;
                    warna_baru = Color.FromArgb(rata, rata, rata);
                    SetPixel_e[i, j, 0] = (byte)warna_baru.B;
                    SetPixel_e[i, j, 1] = (byte)warna_baru.G;
                    SetPixel_e[i, j, 2] = (byte)warna_baru.R;
                }
            }
            pictureBox2.Image = gambar_akhir.ToBitmap();
        }

        private void konversi_grayscale()
        {
            gambar_akhir = new Image<Bgr, byte>(gambar_awal.Width, gambar_awal.Height);
            Color warna_pixel;
            Byte[, ,] GetPixel_e = gambar_edit.Data; //Mengambil warna dari gambar awal
            Byte[, ,] SetPixel_e = gambar_akhir.Data; //Mengeset warna ke gambar edit

            int r, g, b, grayscale;
            for (int i = 0; i <= gambar_akhir.Height - 1; i++)
            {
                for (int j = 0; j <= gambar_akhir.Width - 1; j++)
                {
                    b = GetPixel_e[i, j, 0];
                    g = GetPixel_e[i, j, 1];
                    r = GetPixel_e[i, j, 2];
                    warna_pixel = Color.FromArgb(r, g, b);

                    grayscale = (int)((warna_pixel.R * .3) + (warna_pixel.G * .59) + (warna_pixel.B * .11));
                    SetPixel_e[i, j, 0] = (byte)grayscale;
                    SetPixel_e[i, j, 1] = (byte)grayscale;
                    SetPixel_e[i, j, 2] = (byte)grayscale;
                }
            }
            pictureBox2.Image = gambar_akhir.ToBitmap();
        }

        public void apply()
        {
            gambar_edit = gambar_akhir.Clone();
            pictureBox2.Image = gambar_edit.ToBitmap();
        }

        public void cancel()
        {
            pictureBox2.Image = gambar_edit.ToBitmap();
        }

        private void konversi_matrik()
        {
            Form_Matrix matrik = new Form_Matrix(gambar_edit);
            matrik.Show();
        }

        private void konverti_negatif()
        {
            gambar_akhir = new Image<Bgr, byte>(gambar_awal.Width, gambar_awal.Height);
            Byte[, ,] GetPixel_e = gambar_edit.Data; //Mengambil warna dari gambar awal
            Byte[, ,] SetPixel_e = gambar_akhir.Data; //Mengeset warna ke gambar edit

            int r, g, b;
            for (int i = 0; i < gambar_edit.Height; i++)
            {
                for (int j = 0; j < gambar_edit.Width; j++)
                {
                    b = 255 - GetPixel_e[i, j, 0];
                    g = 255 - GetPixel_e[i, j, 1];
                    r = 255 - GetPixel_e[i, j, 2];

                    SetPixel_e[i, j, 0] = (byte)b;
                    SetPixel_e[i, j, 1] = (byte)g;
                    SetPixel_e[i, j, 2] = (byte)r;
                }
            }
            pictureBox2.Image = gambar_akhir.ToBitmap();
        }

        private void pemetaan_nonLinear()
        {
            Byte[, ,] GetPixel_e = gambar_edit.Data; //Mengambil warna dari gambar awal
            Byte[, ,] SetPixel_e = gambar_akhir.Data; //Mengeset warna ke gambar edit
            int r, g, b;
            Double tmp;
            for (int i = 0; i < gambar_edit.Height; i++)
            {
                for (int j = 0; j < gambar_edit.Width; j++)
                {
                    tmp = Math.Log(1 + Convert.ToDouble(GetPixel_e[i, j, 0]));
                    b = (int)tmp;
                    if (b > 255) b = 255;
                    else if (b < 0) b = 0;

                    tmp = Math.Log(1 + Convert.ToDouble(GetPixel_e[i, j, 1]));
                    g = (int)tmp;
                    if (g > 255) g = 255;
                    else if (g < 0) g = 0;

                    tmp = Math.Log(1 + Convert.ToDouble(GetPixel_e[i, j, 2]));
                    r = (int)tmp;
                    if (r > 255) r = 255;
                    else if (r < 0) r = 0;

                    SetPixel_e[i, j, 0] = (byte)b;
                    SetPixel_e[i, j, 1] = (byte)g;
                    SetPixel_e[i, j, 2] = (byte)r;
                }
            }
            pictureBox2.Image = gambar_akhir.ToBitmap();
        }

        private void ubah_rasio_zoom()
        {
            Ubah_skala_pembesaran otherForm = new Ubah_skala_pembesaran();
            otherForm.Show();
        }

        private void ubah_brightness_contras()
        {
            Brightness_dan_Contras a = new Brightness_dan_Contras();
            a.Show();
        }

        public void ubah_brightness_contras(int brightness, float contras)
        {
            gambar_akhir = (contras * gambar_edit) + brightness;
            pictureBox2.Image = gambar_akhir.ToBitmap();
        }

        private void tampilkan_histogram()
        {
            Histogram a = new Histogram();
            a.Show();
        }

        private void tampilkan_fourier_transform()
        {
            Fourier_Transform a = new Fourier_Transform();
            a.Show();
        }

        private void edit_filter()
        {
            Filter a = new Filter();
            a.Show();
        }

        #region Filter
        private int cari_nilai_mak(int i, int j, string RGB)
        {
            int r, g, b;
            Byte[,,] GetPixel_e = gambar_edit_filter.Data;

            int hasil = -1;
            if (RGB == "red")
            {
                try
                {
                    /*b = GetPixel_e[i, j, 0];
                    g = GetPixel_e[i, j, 1];
                    r = GetPixel_e[i, j, 2];*/

                    hasil = GetPixel_e[i, j+1, 2];

                    if (hasil < GetPixel_e[i - 1, j + 1, 2])
                        hasil = GetPixel_e[i - 1, j + 1, 2];

                    if (hasil < GetPixel_e[i - 1, j, 2])
                        hasil = GetPixel_e[i - 1, j, 2];

                    if (hasil < GetPixel_e[i - 1, j - 1, 2])
                        hasil = GetPixel_e[i - 1, j - 1, 2];

                    if (hasil < GetPixel_e[i, j - 1, 2])
                        hasil = GetPixel_e[i, j - 1, 2];

                    if (hasil < GetPixel_e[i + 1, j - 1, 2])
                        hasil = GetPixel_e[i + 1, j - 1, 2];

                    if (hasil < GetPixel_e[i + 1, j, 2])
                        hasil = GetPixel_e[i + 1, j, 2];

                    if (hasil < GetPixel_e[i + 1, j + 1, 2])
                        hasil = GetPixel_e[i + 1, j + 1, 2];
                }
                catch
                {
                    hasil = -1;
                }
            }
            else if (RGB == "green")
            {
                try
                {
                    hasil = GetPixel_e[i, j + 1, 1];

                    if (hasil < GetPixel_e[i - 1, j + 1, 1])
                        hasil = GetPixel_e[i - 1, j + 1, 1];

                    if (hasil < GetPixel_e[i - 1, j, 1])
                        hasil = GetPixel_e[i - 1, j, 1];

                    if (hasil < GetPixel_e[i - 1, j - 1, 1])
                        hasil = GetPixel_e[i - 1, j - 1, 1];

                    if (hasil < GetPixel_e[i, j - 1, 1])
                        hasil = GetPixel_e[i, j - 1, 1];

                    if (hasil < GetPixel_e[i + 1, j - 1, 1])
                        hasil = GetPixel_e[i + 1, j - 1, 1];

                    if (hasil < GetPixel_e[i + 1, j, 1])
                        hasil = GetPixel_e[i + 1, j, 1];

                    if (hasil < GetPixel_e[i + 1, j + 1, 1])
                        hasil = GetPixel_e[i + 1, j + 1, 1];
                }
                catch
                {
                    hasil = -1;
                }
            }
            else if (RGB == "blue")
            {
                try
                {
                    hasil = GetPixel_e[i, j + 1, 0];

                    if (hasil < GetPixel_e[i - 1, j + 1, 0])
                        hasil = GetPixel_e[i - 1, j + 1, 0];

                    if (hasil < GetPixel_e[i - 1, j, 0])
                        hasil = GetPixel_e[i - 1, j, 0];

                    if (hasil < GetPixel_e[i - 1, j - 1, 0])
                        hasil = GetPixel_e[i - 1, j - 1, 0];

                    if (hasil < GetPixel_e[i, j - 1, 0])
                        hasil = GetPixel_e[i, j - 1, 0];

                    if (hasil < GetPixel_e[i + 1, j - 1, 0])
                        hasil = GetPixel_e[i + 1, j - 1, 0];

                    if (hasil < GetPixel_e[i + 1, j, 0])
                        hasil = GetPixel_e[i + 1, j, 0];

                    if (hasil < GetPixel_e[i + 1, j + 1, 0])
                        hasil = GetPixel_e[i + 1, j + 1, 0];
                }
                catch
                {
                    hasil = -1;
                }
            }
            return hasil;
        }

        private int cari_nilai_min(int i, int j, string RGB)
        {
            int r, g, b;
            Byte[,,] GetPixel_e = gambar_edit_filter.Data;

            int hasil = -1;
            if (RGB == "red")
            {
                try
                {
                    /*b = GetPixel_e[i, j, 0];
                    g = GetPixel_e[i, j, 1];
                    r = GetPixel_e[i, j, 2];*/

                    hasil = GetPixel_e[i, j + 1, 2];

                    if (hasil > GetPixel_e[i - 1, j + 1, 2])
                        hasil = GetPixel_e[i - 1, j + 1, 2];

                    if (hasil > GetPixel_e[i - 1, j, 2])
                        hasil = GetPixel_e[i - 1, j, 2];

                    if (hasil > GetPixel_e[i - 1, j - 1, 2])
                        hasil = GetPixel_e[i - 1, j - 1, 2];

                    if (hasil > GetPixel_e[i, j - 1, 2])
                        hasil = GetPixel_e[i, j - 1, 2];

                    if (hasil > GetPixel_e[i + 1, j - 1, 2])
                        hasil = GetPixel_e[i + 1, j - 1, 2];

                    if (hasil > GetPixel_e[i + 1, j, 2])
                        hasil = GetPixel_e[i + 1, j, 2];

                    if (hasil > GetPixel_e[i + 1, j + 1, 2])
                        hasil = GetPixel_e[i + 1, j + 1, 2];
                }
                catch
                {
                    hasil = -1;
                }
            }
            else if (RGB == "green")
            {
                try
                {
                    hasil = GetPixel_e[i, j + 1, 1];

                    if (hasil > GetPixel_e[i - 1, j + 1, 1])
                        hasil = GetPixel_e[i - 1, j + 1, 1];

                    if (hasil > GetPixel_e[i - 1, j, 1])
                        hasil = GetPixel_e[i - 1, j, 1];

                    if (hasil > GetPixel_e[i - 1, j - 1, 1])
                        hasil = GetPixel_e[i - 1, j - 1, 1];

                    if (hasil > GetPixel_e[i, j - 1, 1])
                        hasil = GetPixel_e[i, j - 1, 1];

                    if (hasil > GetPixel_e[i + 1, j - 1, 1])
                        hasil = GetPixel_e[i + 1, j - 1, 1];

                    if (hasil > GetPixel_e[i + 1, j, 1])
                        hasil = GetPixel_e[i + 1, j, 1];

                    if (hasil > GetPixel_e[i + 1, j + 1, 1])
                        hasil = GetPixel_e[i + 1, j + 1, 1];
                }
                catch
                {
                    hasil = -1;
                }
            }
            else if (RGB == "blue")
            {
                try
                {
                    hasil = GetPixel_e[i, j + 1, 0];

                    if (hasil > GetPixel_e[i - 1, j + 1, 0])
                        hasil = GetPixel_e[i - 1, j + 1, 0];

                    if (hasil > GetPixel_e[i - 1, j, 0])
                        hasil = GetPixel_e[i - 1, j, 0];

                    if (hasil > GetPixel_e[i - 1, j - 1, 0])
                        hasil = GetPixel_e[i - 1, j - 1, 0];

                    if (hasil > GetPixel_e[i, j - 1, 0])
                        hasil = GetPixel_e[i, j - 1, 0];

                    if (hasil > GetPixel_e[i + 1, j - 1, 0])
                        hasil = GetPixel_e[i + 1, j - 1, 0];

                    if (hasil > GetPixel_e[i + 1, j, 0])
                        hasil = GetPixel_e[i + 1, j, 0];

                    if (hasil > GetPixel_e[i + 1, j + 1, 0])
                        hasil = GetPixel_e[i + 1, j + 1, 0];
                }
                catch
                {
                    hasil = -1;
                }
            }
            return hasil;
        }

        private int cari_median(int i, int j, String RGB)
        {
            Byte[,,] GetPixel_e = gambar_edit_filter.Data;

            int[] data = new int[9];
            int tmp, hasil;
            if (RGB == "red")
            {
                data[0] = GetPixel_e[i, j + 1, 2];
                data[1] = GetPixel_e[i - 1, j + 1, 2];
                data[2] = GetPixel_e[i - 1, j, 2];
                data[3] = GetPixel_e[i - 1, j - 1, 2];

                data[4] = GetPixel_e[i, j - 1, 2];

                data[5] = GetPixel_e[i + 1, j - 1, 2];
                data[6] = GetPixel_e[i + 1, j, 2];
                data[7] = GetPixel_e[i + 1, j + 1, 2];
                data[8] = GetPixel_e[i, j, 2];
            }
            else if (RGB == "green")
            {
                data[0] = GetPixel_e[i, j + 1, 1];
                data[1] = GetPixel_e[i - 1, j + 1, 1];
                data[2] = GetPixel_e[i - 1, j, 1];
                data[3] = GetPixel_e[i - 1, j - 1, 1];

                data[4] = GetPixel_e[i, j - 1, 1];

                data[5] = GetPixel_e[i + 1, j - 1, 1];
                data[6] = GetPixel_e[i + 1, j, 1];
                data[7] = GetPixel_e[i + 1, j + 1, 1];
                data[8] = GetPixel_e[i, j, 1];
            }
            else if (RGB == "blue")
            {
                data[0] = GetPixel_e[i, j + 1, 0];
                data[1] = GetPixel_e[i - 1, j + 1, 0];
                data[2] = GetPixel_e[i - 1, j, 0];
                data[3] = GetPixel_e[i - 1, j - 1, 0];

                data[4] = GetPixel_e[i, j - 1, 0];

                data[5] = GetPixel_e[i + 1, j - 1, 0];
                data[6] = GetPixel_e[i + 1, j, 0];
                data[7] = GetPixel_e[i + 1, j + 1, 0];
                data[8] = GetPixel_e[i, j, 0];
            }

            for (int a = 0; a < 8; a++)
            {
                for (int b = 0; b < 8 - a; b++)
                {
                    if (data[b] > data[b + 1])
                    {
                        tmp = data[b];
                        data[b] = data[b + 1];
                        data[b + 1] = tmp;
                    }
                }
            }
            hasil = data[4];
            return hasil;
        }

        public void filter_batas(int nilai_batas)
        {
            tambah_bingkai();
            int[] nilai_mak = new int[3];
            int[] nilai_min = new int[3];
            int r, g, b;

            Byte[,,] GetPixel_e = gambar_edit_filter.Data;
            Byte[,,] SetPixel_e = gambar_akhir.Data;

            nilai_batas = (nilai_batas - 1) / 2;
            for (int i = nilai_batas; i < gambar_edit_filter.Height - nilai_batas; i++)
            {
                for (int j = nilai_batas; j < gambar_edit_filter.Width - nilai_batas; j++)
                {
                    nilai_mak[0] = cari_nilai_mak(i, j, "red");
                    nilai_mak[1] = cari_nilai_mak(i, j, "green");
                    nilai_mak[2] = cari_nilai_mak(i, j, "blue");

                    nilai_min[0] = cari_nilai_min(i, j, "red");
                    nilai_min[1] = cari_nilai_min(i, j, "green");
                    nilai_min[2] = cari_nilai_min(i, j, "blue");

                    if (GetPixel_e[i, j, 2] < nilai_min[0])
                        r = nilai_min[0];
                    else if (GetPixel_e[i, j, 2] > nilai_mak[0])
                        r = nilai_mak[0];
                    else
                        r = GetPixel_e[i, j, 2];

                    if (GetPixel_e[i, j, 1] < nilai_min[1])
                        g = nilai_min[1];
                    else if (GetPixel_e[i, j, 1] > nilai_mak[1])
                        g = nilai_mak[1];
                    else
                        g = GetPixel_e[i, j, 1];

                    if (GetPixel_e[i, j, 0] < nilai_min[2])
                        b = nilai_min[2];
                    else if (GetPixel_e[i, j, 0] > nilai_mak[2])
                        b = nilai_mak[2];
                    else
                        b = GetPixel_e[i, j, 0];

                    //SETPIXEL
                    SetPixel_e[i - nilai_batas, j - nilai_batas, 0] = (byte)b;
                    SetPixel_e[i - nilai_batas, j - nilai_batas, 1] = (byte)g;
                    SetPixel_e[i - nilai_batas, j - nilai_batas, 2] = (byte)r;

                    /*if (filter_advanced != -1)
                    {
                        gambar_hasil_sementara_e.Data[j, i, 0] = (byte)b;
                        gambar_hasil_sementara_e.Data[j, i, 1] = (byte)g;
                        gambar_hasil_sementara_e.Data[j, i, 2] = (byte)r;
                    }*/
                }
            }
            pictureBox2.Image = gambar_akhir.ToBitmap();
        }

        public void filter_pererataan(int nilai_batas)
        {
            tambah_bingkai();
            Byte[,,] GetPixel_e = gambar_edit_filter.Data;
            Byte[,,] SetPixel_e = gambar_akhir.Data;

            int[] nilai_total = new int[3];
            int r, g, b;
            double tmp;

            nilai_batas = (nilai_batas - 1) / 2;

            for (int i = nilai_batas; i < gambar_edit_filter.Height - nilai_batas; i++)
            {
                for (int j = nilai_batas; j < gambar_edit_filter.Width - nilai_batas; j++)
                {
                    nilai_total[0] = 0; //Blue
                    nilai_total[0] += GetPixel_e[i, j, 0];
                    nilai_total[0] += GetPixel_e[i + 1, j - 1, 0];
                    nilai_total[0] += GetPixel_e[i, j - 1, 0];
                    nilai_total[0] += GetPixel_e[i - 1, j - 1, 0];
                    nilai_total[0] += GetPixel_e[i - 1, j, 0];
                    nilai_total[0] += GetPixel_e[i - 1, j + 1, 0];
                    nilai_total[0] += GetPixel_e[i, j + 1, 0];
                    nilai_total[0] += GetPixel_e[i + 1, j + 1, 0];


                    nilai_total[1] = 0; //Green
                    nilai_total[1] += GetPixel_e[i, j, 1];
                    nilai_total[1] += GetPixel_e[i + 1, j - 1, 1];
                    nilai_total[1] += GetPixel_e[i, j - 1, 1];
                    nilai_total[1] += GetPixel_e[i - 1, j - 1, 1];
                    nilai_total[1] += GetPixel_e[i - 1, j, 1];
                    nilai_total[1] += GetPixel_e[i - 1, j + 1, 1];
                    nilai_total[1] += GetPixel_e[i, j + 1, 1];
                    nilai_total[1] += GetPixel_e[i + 1, j + 1, 1];

                    nilai_total[2] = 0; //Red
                    nilai_total[2] += GetPixel_e[i, j, 2];
                    nilai_total[2] += GetPixel_e[i + 1, j - 1, 2];
                    nilai_total[2] += GetPixel_e[i, j - 1, 2];
                    nilai_total[2] += GetPixel_e[i - 1, j - 1, 2];
                    nilai_total[2] += GetPixel_e[i - 1, j, 2];
                    nilai_total[2] += GetPixel_e[i - 1, j + 1, 2];
                    nilai_total[2] += GetPixel_e[i, j + 1, 2];
                    nilai_total[2] += GetPixel_e[i + 1, j + 1, 2];


                    tmp = Math.Round(nilai_total[0] / 9F);
                    b = Convert.ToInt16(tmp);

                    tmp = Math.Round(nilai_total[1] / 9F);
                    g = Convert.ToInt16(tmp);

                    tmp = Math.Round(nilai_total[2] / 9F);
                    r = Convert.ToInt16(tmp);

                    //SETPIXEL
                    SetPixel_e[i - nilai_batas, j - nilai_batas, 0] = (byte)b;
                    SetPixel_e[i - nilai_batas, j - nilai_batas, 1] = (byte)g;
                    SetPixel_e[i - nilai_batas, j - nilai_batas, 2] = (byte)r;

                    /*if (filter_advanced != -1)
                    {
                        gambar_hasil_sementara_e.Data[j, i, 0] = (byte)b;
                        gambar_hasil_sementara_e.Data[j, i, 1] = (byte)g;
                        gambar_hasil_sementara_e.Data[j, i, 2] = (byte)r;
                    }*/
                }
            }
            pictureBox2.Image = gambar_akhir.ToBitmap();
        }

        public void filter_median(int nilai_batas)
        {
            tambah_bingkai();
            Byte[,,] GetPixel_e = gambar_edit_filter.Data;
            Byte[,,] SetPixel_e = gambar_akhir.Data;

            int r, g, b;

            nilai_batas = (nilai_batas - 1) / 2;

            for (int i = nilai_batas; i < gambar_edit_filter.Width - nilai_batas; i++)
            {
                for (int j = nilai_batas; j < gambar_edit_filter.Height - nilai_batas; j++)
                {
                    r = cari_median(i, j, "red");
                    g = cari_median(i, j, "green");
                    b = cari_median(i, j, "blue");

                    //SETPIXEL
                    SetPixel_e[i - nilai_batas, j - nilai_batas, 0] = (byte)b;
                    SetPixel_e[i - nilai_batas, j - nilai_batas, 1] = (byte)g;
                    SetPixel_e[i - nilai_batas, j - nilai_batas, 2] = (byte)r;

                    /*if (filter_advanced != -1)
                    {
                        gambar_hasil_sementara_e.Data[j, i, 0] = (byte)b;
                        gambar_hasil_sementara_e.Data[j, i, 1] = (byte)g;
                        gambar_hasil_sementara_e.Data[j, i, 2] = (byte)r;
                    }*/
                }
            }
            pictureBox2.Image = gambar_akhir.ToBitmap();
        }
        
        public void low_pass_filter(int nilai_batas, int panjang_kernel, int[,] kernel)
        {
            int sum_matrik = 0; //jumlahkan semua kernel
            nilai_batas = (nilai_batas - 1) / 2;
            for (int i = 0; i < panjang_kernel; i++)
            {
                for (int j = 0; j < panjang_kernel; j++)
                {
                    sum_matrik += kernel[i, j];
                }
            }

            if (sum_matrik != 0)
            {
                int R, G, B, totalR, totalG, totalB;
                tambah_bingkai();
                for (int i = ((panjang_kernel - 1) / 2); i < gambar_edit_filter.Width - ((panjang_kernel - 1) / 2); i++)
                {
                    for (int j = ((panjang_kernel - 1) / 2); j < gambar_edit_filter.Height - ((panjang_kernel - 1) / 2); j++)
                    {
                        totalR = 0;
                        totalG = 0;
                        totalB = 0;
                        for (int x = 0 - nilai_batas, k = 0; x < panjang_kernel - nilai_batas; x++, k++)
                        {
                            for (int y = 0 - nilai_batas, l = 0; y < panjang_kernel - nilai_batas; y++, l++)
                            {
                                B = gambar_edit_filter.Data[j + y, i + x, 0];
                                G = gambar_edit_filter.Data[j + y, i + x, 1];
                                R = gambar_edit_filter.Data[j + y, i + x, 2];

                                totalR += (kernel[k, l] * R);
                                totalG += (kernel[k, l] * G);
                                totalB += (kernel[k, l] * B);
                            }
                        }

                        totalR /= sum_matrik;
                        totalG /= sum_matrik;
                        totalB /= sum_matrik;

                        /*if (totalR > 255)
                            totalR = 255;
                        else if (totalR < 0)
                            totalR = 0;
                        if (totalG > 255)
                            totalG = 255;
                        else if (totalG < 0)
                            totalG = 0;
                        if (totalB > 255)
                            totalB = 255;
                        else if (totalB < 0)
                            totalB = 0;*/

                        gambar_akhir.Data[j - nilai_batas, i - nilai_batas, 0] = (byte)totalB;
                        gambar_akhir.Data[j - nilai_batas, i - nilai_batas, 1] = (byte)totalG;
                        gambar_akhir.Data[j - nilai_batas, i - nilai_batas, 2] = (byte)totalR;
                    }
                }
                pictureBox2.Image = gambar_akhir.ToBitmap();
            }
            else
            {
                MessageBox.Show("Silahkan input kernel sesuai dengan aturan Low Pass Filter!");
            }
        }

        public void high_pass_filter(int nilai_batas, int panjang_kernel, int[,] kernel)
        {
            nilai_batas = (nilai_batas - 1) / 2;

            int R, G, B, totalR, totalG, totalB;
            tambah_bingkai();
            for (int i = ((panjang_kernel - 1) / 2); i < gambar_edit_filter.Width - ((panjang_kernel - 1) / 2); i++)
            {
                for (int j = ((panjang_kernel - 1) / 2); j < gambar_edit_filter.Height - ((panjang_kernel - 1) / 2); j++)
                {
                    totalR = 0;
                    totalG = 0;
                    totalB = 0;
                    for (int x = 0 - nilai_batas, k = 0; x < panjang_kernel - nilai_batas; x++, k++)
                    {
                        for (int y = 0 - nilai_batas, l = 0; y < panjang_kernel - nilai_batas; y++, l++)
                        {
                            B = gambar_edit_filter.Data[j + y, i + x, 0];
                            G = gambar_edit_filter.Data[j + y, i + x, 1];
                            R = gambar_edit_filter.Data[j + y, i + x, 2];

                            totalR += (kernel[k, l] * R);
                            totalG += (kernel[k, l] * G);
                            totalB += (kernel[k, l] * B);
                        }
                    }

                    if (totalR > 255)
                        totalR = 255;
                    else if (totalR < 0)
                        totalR = 0;
                    if (totalG > 255)
                        totalG = 255;
                    else if (totalG < 0)
                        totalG = 0;
                    if (totalB > 255)
                        totalB = 255;
                    else if (totalB < 0)
                        totalB = 0;

                    gambar_akhir.Data[j - nilai_batas, i - nilai_batas, 0] = (byte)totalB;
                    gambar_akhir.Data[j - nilai_batas, i - nilai_batas, 1] = (byte)totalG;
                    gambar_akhir.Data[j - nilai_batas, i - nilai_batas, 2] = (byte)totalR;
                }
            }
            pictureBox2.Image = gambar_akhir.ToBitmap();
        }

        public void high_boost_filter(int nilai_batas, int panjang_kernel, int[,] kernel)
        {
            nilai_batas = (nilai_batas - 1) / 2;
            bool status = true;

            for (int i = 0; i < panjang_kernel; i++)
            {
                for (int j = 0; j < panjang_kernel; j++)
                {
                    if ((i != ((panjang_kernel - 1) / 2)) && (j != ((panjang_kernel - 1) / 2)))
                    {
                        if (kernel[i, j] != -1)
                        {
                            status = false;
                        }
                    }
                }
            }

            if (((kernel[((panjang_kernel - 1) / 2), ((panjang_kernel - 1) / 2)]) > ((panjang_kernel * panjang_kernel) - 1)) && (status == true))
            {
                int R, G, B, totalR, totalG, totalB;
                tambah_bingkai();
                for (int i = ((panjang_kernel - 1) / 2); i < gambar_edit_filter.Width - ((panjang_kernel - 1) / 2); i++)
                {
                    for (int j = ((panjang_kernel - 1) / 2); j < gambar_edit_filter.Height - ((panjang_kernel - 1) / 2); j++)
                    {
                        totalR = 0;
                        totalG = 0;
                        totalB = 0;
                        for (int x = 0 - nilai_batas, k = 0; x < panjang_kernel - nilai_batas; x++, k++)
                        {
                            for (int y = 0 - nilai_batas, l = 0; y < panjang_kernel - nilai_batas; y++, l++)
                            {
                                B = gambar_edit_filter.Data[j + y, i + x, 0];
                                G = gambar_edit_filter.Data[j + y, i + x, 1];
                                R = gambar_edit_filter.Data[j + y, i + x, 2];

                                totalR += (kernel[k, l] * R);
                                totalG += (kernel[k, l] * G);
                                totalB += (kernel[k, l] * B);
                            }
                        }

                        if (totalR > 255)
                            totalR = 255;
                        else if (totalR < 0)
                            totalR = 0;
                        if (totalG > 255)
                            totalG = 255;
                        else if (totalG < 0)
                            totalG = 0;
                        if (totalB > 255)
                            totalB = 255;
                        else if (totalB < 0)
                            totalB = 0;

                        gambar_akhir.Data[j - nilai_batas, i - nilai_batas, 0] = (byte)totalB;
                        gambar_akhir.Data[j - nilai_batas, i - nilai_batas, 1] = (byte)totalG;
                        gambar_akhir.Data[j - nilai_batas, i - nilai_batas, 2] = (byte)totalR;
                    }
                }
                pictureBox2.Image = gambar_akhir.ToBitmap();
            }
            else
            {
                MessageBox.Show("Silahkan input kernel sesuai dengan aturan Hight bost Filter!");
            }
        }

        #endregion

        #endregion











    }
}

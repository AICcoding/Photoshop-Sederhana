using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Photoshop_Sederhana
{
    public partial class Fourier_Transform : Form
    {
        System.Windows.Forms.Form f = System.Windows.Forms.Application.OpenForms["Form1"];
        Image<Bgr, Byte> gambar_awal, gambar_akhir;
        Image<Bgr, Byte> DFT_red_e, DFT_green_e, DFT_blue_e, DFT_grayscale_e;

        public Fourier_Transform()
        {
            InitializeComponent();

            comboBox2.Text = "Pilih canel";
            comboBox2.Items.Add("Grayscale");
            comboBox2.Items.Add("Red");
            comboBox2.Items.Add("Green");
            comboBox2.Items.Add("Blue");

            gambar_awal = ((Form1)f).gambar_awal.Clone();

            pictureBox1.Image = gambar_awal.ToBitmap();

            button3.Enabled = false;
        }

        private void gambarAwal_ke_DFT()
        {
            DFT_red_e = new Image<Bgr, byte>(gambar_awal.Width, gambar_awal.Height);
            DFT_green_e = new Image<Bgr, byte>(gambar_awal.Width, gambar_awal.Height);
            DFT_blue_e = new Image<Bgr, byte>(gambar_awal.Width, gambar_awal.Height);
            DFT_grayscale_e = new Image<Bgr, byte>(gambar_awal.Width, gambar_awal.Height);

            Byte[, ,] GetPixel_e = gambar_awal.Data; //Mengambil warna dari gambar awal

            Byte[, ,] SetPixelR_e = DFT_red_e.Data; //Mengeset warna ke gambar akhir
            Byte[, ,] SetPixelG_e = DFT_green_e.Data; //Mengeset warna ke gambar akhir
            Byte[, ,] SetPixelB_e = DFT_blue_e.Data; //Mengeset warna ke gambar akhir
            Byte[, ,] SetPixelGS_e = DFT_grayscale_e.Data; //Mengeset warna ke gambar akhir

            int N, M, phi;
            double r, g, b, gs;
            double mag;

            N = gambar_awal.Width;
            M = gambar_awal.Height;
            phi = 180;

            for (int u = 0; u < M; u++)
            {
                for (int v = 0; v < N; v++)
                {
                    r = 0;
                    g = 0;
                    b = 0;
                    gs = 0;

                    for (int x = 0; x < M; x++)
                    {
                        for (int y = 0; y < N; y++)
                        {
                            gs += (((float)GetPixel_e[x, y, 0] + (float)GetPixel_e[x, y, 1] + (float)GetPixel_e[x, y, 2]) / 3F) * Math.Cos(2 * Math.PI * (((float)u * (float)x / (float)M) + ((float)v * (float)y / (float)N)));
                            r += ((float)GetPixel_e[x, y, 2]) * cos(2 * phi * (((float)u * (float)x / (float)M) + ((float)v * (float)y / (float)N)));
                            g += ((float)GetPixel_e[x, y, 1]) * cos(2 * phi * (((float)u * (float)x / (float)M) + ((float)v * (float)y / (float)N)));
                            b += ((float)GetPixel_e[x, y, 0]) * cos(2 * phi * (((float)u * (float)x / (float)M) + ((float)v * (float)y / (float)N)));
                        }
                    }

                    if (r > 255)
                        r = 255;
                    else if (r < 0)
                        r = 0;

                    if (g > 255)
                        g = 255;
                    else if (g < 0)
                        g = 0;

                    if (b > 255)
                        b = 255;
                    else if (b < 0)
                        b = 0;

                    if (gs < 0)
                        gs = 0;
                    else if (gs > 255)
                        gs = 255;

                    SetPixelR_e[u, v, 0] = 0;
                    SetPixelR_e[u, v, 1] = 0;
                    SetPixelR_e[u, v, 2] = (byte)r;

                    SetPixelG_e[u, v, 0] = 0;
                    SetPixelG_e[u, v, 1] = (byte)g;
                    SetPixelG_e[u, v, 2] = 0;

                    SetPixelB_e[u, v, 0] = (byte)b;
                    SetPixelB_e[u, v, 1] = 0;
                    SetPixelB_e[u, v, 2] = 0;

                    SetPixelGS_e[u, v, 0] = (byte)gs;
                    SetPixelGS_e[u, v, 1] = (byte)gs;
                    SetPixelGS_e[u, v, 2] = (byte)gs;
                }
            }
            pictureBox2.Image = DFT_grayscale_e.ToBitmap();
        }

        private void DFT_ke_gambarAwal()
        {
            gambar_akhir = new Image<Bgr, byte>(gambar_awal.Width, gambar_awal.Height);

            Byte[, ,] GetPixel_e = DFT_grayscale_e.Data; //Mengambil warna dari gambar awal

            Byte[, ,] SetPixel_e = gambar_akhir.Data; //Mengeset warna ke gambar akhir       

            int N, M, phi;
            double gs;

            N = gambar_awal.Width;
            M = gambar_awal.Height;
            phi = 180;

            for (int u = 0; u < M; u++)
            {
                for (int v = 0; v < N; v++)
                {
                    gs = 0;
                    for (int x = 0; x < M; x++)
                    {
                        for (int y = 0; y < N; y++)
                        {
                            gs += (((float)GetPixel_e[x, y, 0] + (float)GetPixel_e[x, y, 1] + (float)GetPixel_e[x, y, 2]) / 3F) * cos(2 * phi * (((float)u * (float)x / (float)M) + ((float)v * (float)y / (float)N)));
                        }
                    }

                    gs = (1F / M * N) * gs;

                    if (gs < 0)
                        gs = 0;
                    else if (gs > 255)
                        gs = 255;

                    SetPixel_e[u, v, 0] = (byte)gs;
                    SetPixel_e[u, v, 1] = (byte)gs;
                    SetPixel_e[u, v, 2] = (byte)gs;
                }
            }
            pictureBox3.Image = gambar_akhir.ToBitmap();
        }

        private double cos(double sudut)
        {
            double hasil, tmp;
            tmp = sudut * Math.PI / 180F;
            hasil = Math.Cos(tmp);
            hasil = Math.Round(hasil, 4);
            return hasil;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DFT_ke_gambarAwal();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            gambarAwal_ke_DFT();
            button3.Enabled = true;
        }


    }
}

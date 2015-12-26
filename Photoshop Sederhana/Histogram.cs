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
using ZedGraph;


namespace Photoshop_Sederhana
{
    public partial class Histogram : Form
    {
        System.Windows.Forms.Form f = System.Windows.Forms.Application.OpenForms["Form1"];
        GraphPane histogram_awal, histogram_akhir;

        public Histogram()
        {
            InitializeComponent();

            comboBox1.Text = "Pilih canel";
            comboBox1.Items.Add("Gabungan");
            comboBox1.Items.Add("Red");
            comboBox1.Items.Add("Green");
            comboBox1.Items.Add("Blue");
            comboBox2.Text = "Pilih canel";
            comboBox2.Items.Add("Gabungan");
            comboBox2.Items.Add("Red");
            comboBox2.Items.Add("Green");
            comboBox2.Items.Add("Blue");

            histogram_awal = zedGraphControl1.GraphPane;
            histogram_awal.Title = "Histogram Gambar Awal";
            histogram_awal.YAxis.Title = "Jumlah Pixel";
            histogram_awal.XAxis.Title = "Nilai Pixel";

            histogram_akhir = zedGraphControl2.GraphPane;
            histogram_akhir.Title = "Histogram Gambar Akhir";
            histogram_akhir.YAxis.Title = "Jumlah Pixel";
            histogram_akhir.XAxis.Title = "Nilai Pixel";
        }

        private void membuat_histogram_emgu(Image<Bgr, byte> gambar, GraphPane histogram, string RGB)
        {
            int[] nilai_pixel = new int[256];
            PointPairList data_pixel = new PointPairList();
            BarItem kurva;
          
            int r, g, b, grey;
            Bgr pixel;

            if (RGB == "red") //jika red
            {
                for (int i = 0; i < gambar.Width; i++)
                {
                    for (int j = 0; j < gambar.Height; j++)
                    {
                        pixel = gambar[j, i];
                        r = (int)pixel.Red;
                        nilai_pixel[r] += 1;
                        /*MessageBox.Show(r.ToString());
                        MessageBox.Show(nilai_pixel[r].ToString());*/
                    }
                }
                for (int i = 0; i < 256; i++)
                {
                    data_pixel.Add(i, nilai_pixel[i]);
                }
                kurva = histogram.AddBar("Nilai Pixel", data_pixel, Color.Red);
                kurva.Bar.Fill = new Fill(Color.Red);
                //histogram.AxisFill = new Fill(Color.Red);
            }
            else if (RGB == "green") //jika green
            {
                for (int i = 0; i < gambar.Width; i++)
                {
                    for (int j = 0; j < gambar.Height; j++)
                    {
                        pixel = gambar[j, i];
                        g = (int)pixel.Green;
                        nilai_pixel[g] += 1;
                    }
                }
                for (int i = 0; i < 256; i++)
                {
                    data_pixel.Add(i, nilai_pixel[i]);
                }
                kurva = histogram.AddBar("Nilai Pixel", data_pixel, Color.Green);
                kurva.Bar.Fill = new Fill(Color.Green);
            }
            else if (RGB == "blue") //jika blue
            {
                for (int i = 0; i < gambar.Width; i++)
                {
                    for (int j = 0; j < gambar.Height; j++)
                    {
                        pixel = gambar[j, i];
                        b = (int)pixel.Blue;
                        nilai_pixel[b] += 1;
                    }
                }
                for (int i = 0; i < 256; i++)
                {
                    data_pixel.Add(i, nilai_pixel[i]);
                }
                kurva = histogram.AddBar("Nilai Pixel", data_pixel, Color.Blue);
                kurva.Bar.Fill = new Fill(Color.Blue);
            }
            else if (RGB == "gabungan") //jika gabungan
            {
                int[,] n_p = new int[256, 3];
                int nilai_mak = 0;
                for (int i = 0; i < gambar.Width; i++)
                {
                    for (int j = 0; j < gambar.Height; j++)
                    {

                        pixel = gambar[j, i];
                        r = (int)pixel.Red;
                        g = (int)pixel.Green;
                        b = (int)pixel.Blue;

                        n_p[r, 0] += 1;
                        n_p[g, 1] += 1;
                        n_p[b, 2] += 1;
                    }
                }
                for (int i = 0; i < 256; i++)
                {
                    /*if (nilai_mak < n_p[i, 0]) nilai_mak = n_p[i, 0]; //jika r lebih besar
                    else if (nilai_mak < n_p[i, 1]) nilai_mak = n_p[i, 1]; //jika g lebih besar
                    else if (nilai_mak < n_p[i, 2]) nilai_mak = n_p[i, 2]; //jika b lebih besar

                    data_pixel.Add(i, nilai_mak);
                    nilai_mak = 0;*/

                    nilai_mak = nilai_mak + n_p[i, 0] + n_p[i, 1] + n_p[i, 2];
                    data_pixel.Add(i, nilai_mak);
                    nilai_mak = 0;
                }

                kurva = histogram.AddBar("Nilai Pixel", data_pixel, Color.Black);
                kurva.Bar.Fill = new Fill(Color.Black);
            }
            else if (RGB == "luminositi")
            {
                for (int i = 0; i < gambar.Width; i++)
                {
                    for (int j = 0; j < gambar.Height; j++)
                    {
                        pixel = gambar[j, i];
                        grey = (int)((pixel.Red * .3) + (pixel.Green * .59) + (pixel.Blue * .11));
                        nilai_pixel[grey] += 1;
                    }
                }
                for (int i = 0; i < 256; i++)
                {
                    data_pixel.Add(i, nilai_pixel[i]);
                }
                kurva = histogram.AddBar("Nilai Pixel", data_pixel, Color.Black);
                //kurva.Bar.Fill = new Fill(Color.White);
            }
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton5.Checked)
            {
                if (((Form1)f).gambar_awal != null)
                {                  
                    comboBox1.Visible = false;
                    histogram_awal.CurveList.Clear();
                    membuat_histogram_emgu(((Form1)f).gambar_awal, histogram_awal, "luminositi");
                    zedGraphControl1.AxisChange();
                    zedGraphControl1.Refresh();                 
                }
            }
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton6.Checked)
            {
                comboBox1.Visible = true;
                comboBox1.Text = "Gabungan";
                if (((Form1)f).gambar_awal != null)
                {
                    
                        histogram_awal.CurveList.Clear();
                        membuat_histogram_emgu(((Form1)f).gambar_awal, histogram_awal, "gabungan");
                        zedGraphControl1.AxisChange();
                        zedGraphControl1.Refresh();
                    
                }
            }
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton7.Checked)
            {
                if (((Form1)f).gambar_edit != null)
                {           
                    comboBox2.Visible = false;
                    histogram_akhir.CurveList.Clear();
                    membuat_histogram_emgu(((Form1)f).gambar_edit, histogram_akhir, "luminositi");
                    zedGraphControl2.AxisChange();
                    zedGraphControl2.Refresh();               
                }
            }
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton8.Checked)
            {
                if (((Form1)f).gambar_edit != null)
                {
                    
                        comboBox2.Visible = true;
                        comboBox2.Text = "Gabungan";
                        histogram_akhir.CurveList.Clear();
                        membuat_histogram_emgu(((Form1)f).gambar_edit, histogram_akhir, "gabungan");
                        zedGraphControl2.AxisChange();
                        zedGraphControl2.Refresh();
                    
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                //Rumus membuat histogram gabungan BELUM SELESAI...
                histogram_awal.CurveList.Clear();
                membuat_histogram_emgu(((Form1)f).gambar_awal, histogram_awal, "gabungan");
                zedGraphControl1.AxisChange();
                zedGraphControl1.Refresh();
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                //Rumus membuat histogram red BELUM SELESAI...
                histogram_awal.CurveList.Clear();
                membuat_histogram_emgu(((Form1)f).gambar_awal, histogram_awal, "red");
                zedGraphControl1.AxisChange();
                zedGraphControl1.Refresh();
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                //Rumus membuat histogram green BELUM SELESAI...
                histogram_awal.CurveList.Clear();
                membuat_histogram_emgu(((Form1)f).gambar_awal, histogram_awal, "green");
                zedGraphControl1.AxisChange();
                zedGraphControl1.Refresh();
            }
            else if (comboBox1.SelectedIndex == 3)
            {
                //Rumus membuat histogram blue BELUM SELESAI...
                histogram_awal.CurveList.Clear();
                membuat_histogram_emgu(((Form1)f).gambar_awal, histogram_awal, "blue");
                zedGraphControl1.AxisChange();
                zedGraphControl1.Refresh();
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == 0)
            {
                //Rumus membuat histogram gabungan BELUM SELESAI...
                histogram_akhir.CurveList.Clear();
                membuat_histogram_emgu(((Form1)f).gambar_edit, histogram_akhir, "gabungan");
                zedGraphControl2.AxisChange();
                zedGraphControl2.Refresh();
            }
            else if (comboBox2.SelectedIndex == 1)
            {
                //Rumus membuat histogram red BELUM SELESAI...
                histogram_akhir.CurveList.Clear();
                membuat_histogram_emgu(((Form1)f).gambar_edit, histogram_akhir, "red");
                zedGraphControl2.AxisChange();
                zedGraphControl2.Refresh();
            }
            else if (comboBox2.SelectedIndex == 2)
            {
                //Rumus membuat histogram green BELUM SELESAI...
                histogram_akhir.CurveList.Clear();
                membuat_histogram_emgu(((Form1)f).gambar_edit, histogram_akhir, "green");
                zedGraphControl2.AxisChange();
                zedGraphControl2.Refresh();
            }
            else if (comboBox2.SelectedIndex == 3)
            {
                //Rumus membuat histogram blue BELUM SELESAI...
                histogram_akhir.CurveList.Clear();
                membuat_histogram_emgu(((Form1)f).gambar_edit, histogram_akhir, "blue");
                zedGraphControl2.AxisChange();
                zedGraphControl2.Refresh();
            }
        }

    }
}

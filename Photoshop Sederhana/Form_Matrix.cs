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
    public partial class Form_Matrix : Form
    {
        Image<Bgr, Byte> gambar;
        public Form_Matrix(Image<Bgr, Byte> gambar)
        {
            InitializeComponent();
            this.gambar = gambar.Clone();
            buat_matrik();
        }

        private void buat_matrik()
        {
            Byte[, ,] GetPixel_e = gambar.Data; //Mengambil warna dari gambar awal
            var sb = new StringBuilder();
            int rata, r, g, b;
            textBox1.Width = gambar.Width * 8;
            textBox1.Height = gambar.Height * 14;
            textBox1.Text = "";
            for (int i = 0; i < gambar.Height; i++)
            {
                for (int j = 0; j < gambar.Width; j++)
                {
                    b = GetPixel_e[i, j, 0];
                    g = GetPixel_e[i, j, 1];
                    r = GetPixel_e[i, j, 2];
                    rata = Convert.ToInt32((r + g + b) / 3);

                    if (rata > 127)
                    {
                        sb.Append("1");
                    }
                    else
                    {
                        sb.Append("0");
                    }
                }
                sb.Append("\n");
            }
            textBox1.Text = sb.ToString();
        }
    }
}

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
    public partial class Brightness_dan_Contras : Form
    {
        System.Windows.Forms.Form f = System.Windows.Forms.Application.OpenForms["Form1"];

        public Brightness_dan_Contras()
        {
            InitializeComponent();

            textBox1.Text = trackBar3.Value.ToString();
            textBox2.Text = (trackBar4.Value / 10F).ToString();
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            textBox1.Text = trackBar3.Value.ToString();
            ((Form1)f).ubah_brightness_contras(trackBar3.Value, (float)trackBar4.Value / 10F);

            //((Form1)f).button1.Enabled = true;
            //((Form1)f).button2.Enabled = true;
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            textBox2.Text = (trackBar4.Value / 10F).ToString();
            ((Form1)f).ubah_brightness_contras(trackBar3.Value, (float)trackBar4.Value / 10F);

            //((Form1)f).button1.Enabled = true;
            //((Form1)f).button2.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ((Form1)f).apply();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ((Form1)f).cancel();
            this.Close();
        }

        private void Brightness_dan_Contras_FormClosing(object sender, FormClosingEventArgs e)
        {
            ((Form1)f).cancel();
            this.Close();
        }
    }
}

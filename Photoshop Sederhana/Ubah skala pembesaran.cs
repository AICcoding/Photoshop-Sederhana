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
    public partial class Ubah_skala_pembesaran : Form
    {
        System.Windows.Forms.Form f = System.Windows.Forms.Application.OpenForms["Form1"];

        public Ubah_skala_pembesaran()
        {
            InitializeComponent();
            numericUpDown1.Value = ((Form1)f).skala_pembesaran;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ((Form1)f).skala_pembesaran = (int)numericUpDown1.Value;
            MessageBox.Show("Rasio pembesaran berhasil disimpan!",
                                  "Berhasil", MessageBoxButtons.OK,
                                  MessageBoxIcon.Information,
                                  0);
            this.Close();
        }       
    }
}

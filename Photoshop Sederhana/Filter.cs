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
    public partial class Filter : Form
    {
        System.Windows.Forms.Form f = System.Windows.Forms.Application.OpenForms["Form1"];

        public Filter()
        {
            InitializeComponent();
        }
        int mode, filter_standar, filter_advanced, panjang_kernel;
        int[,] kernel;

        /*private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton9.Checked == true)
            {
                mode = 1;
            }
        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton10.Checked == true)
            {
                mode = 2;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                radioButton1.Enabled = true;
                radioButton2.Enabled = true;
                radioButton3.Enabled = true;
            }
            else if (checkBox1.Checked == false)
            {
                radioButton1.Enabled = false;
                radioButton2.Enabled = false;
                radioButton3.Enabled = false;
                radioButton1.Checked = false;
                radioButton2.Checked = false;
                radioButton3.Checked = false;

                filter_standar = -1;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                radioButton4.Enabled = true;
                //radioButton7.Enabled = true;
                //radioButton8.Enabled = true;
                radioButton5.Enabled = true;
                radioButton6.Enabled = true;

            }
            else if (checkBox2.Checked == false)
            {
                radioButton4.Enabled = false;
                radioButton7.Enabled = false;
                radioButton8.Enabled = false;
                radioButton5.Enabled = false;
                radioButton6.Enabled = false;

                radioButton4.Checked = false;
                radioButton7.Checked = false;
                radioButton8.Checked = false;
                radioButton5.Checked = false;
                radioButton6.Checked = false;

                filter_advanced = -1;
            }
        }*/

        private void radioButton16_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton16.Checked == true)
            {
                filter_standar = 1;
            }
        }
        private void radioButton15_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton15.Checked == true)
            {
                filter_standar = 2;
            }
        }
        private void radioButton14_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton14.Checked == true)
            {
                filter_standar = 3;
            }
        }
        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton11.Checked == true)
            {
                filter_advanced = 1;
                kernelLowPassAwal();
                radioButton10.Enabled = true;
                radioButton9.Enabled = true;

                radioButton10.Checked = true;
            }
            else if (radioButton11.Checked == false)
            {
                radioButton10.Enabled = false;
                radioButton9.Enabled = false;
                radioButton10.Checked = false;
                radioButton9.Checked = false;
            }
        }
        private void radioButton13_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton13.Checked == true)
            {
                filter_advanced = 2;
                kernelHighPassAwal();
            }
        }
        private void radioButton12_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton12.Checked == true)
            {
                filter_advanced = 3;
                kernelHighBoostAwal();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int nilai_batas = Convert.ToInt16(numericUpDown1.Value);
            if (filter_standar == 1)
            {
                ((Form1)f).filter_batas(nilai_batas);
                /*
                if (filter_advanced == 1)
                {
                    if (radioButton7.Checked == true)
                    {
                        low_pass_filter_emgu();
                    }
                    else if (radioButton8.Checked == true)
                    {
                        filter_median_emgu();
                    }
                }
                else if (filter_advanced == 2)
                {
                    high_pass_filter_emgu();
                }
                else if (filter_advanced == 3)
                {
                    high_boost_filter_emgu();
                }*/
            }
            else if (filter_standar == 2)
            {
                ((Form1)f).filter_pererataan(nilai_batas);
                /*filter_pererataan_emgu();
                if (filter_advanced == 1)
                {
                    if (radioButton7.Checked == true)
                    {
                        low_pass_filter_emgu();
                    }
                    else if (radioButton8.Checked == true)
                    {
                        filter_median_emgu();
                    }
                }
                else if (filter_advanced == 2)
                {
                    high_pass_filter_emgu();
                }
                else if (filter_advanced == 3)
                {
                    high_boost_filter_emgu();
                }*/
            }
            else if (filter_standar == 3)
            {
                ((Form1)f).filter_median(nilai_batas);
                /*filter_median_emgu();
                if (filter_advanced == 1)
                {
                    if (radioButton7.Checked == true)
                    {
                        low_pass_filter_emgu();
                    }
                    else if (radioButton8.Checked == true)
                    {
                        filter_median_emgu();
                    }
                }
                else if (filter_advanced == 2)
                {
                    high_pass_filter_emgu();
                }
                else if (filter_advanced == 3)
                {
                    high_boost_filter_emgu();
                }*/
            }
            else if (filter_advanced == 1)
            {
                if (radioButton10.Checked == true)
                {
                    ((Form1)f).low_pass_filter(nilai_batas, panjang_kernel, kernel);
                }
                else if (radioButton9.Checked == true)
                {
                    ((Form1)f).filter_median(nilai_batas);
                }
            }
            else if (filter_advanced == 2)
            {
                ((Form1)f).high_pass_filter(nilai_batas, panjang_kernel, kernel);
            }
            else if (filter_advanced == 3)
            {
                ((Form1)f).high_boost_filter(nilai_batas, panjang_kernel, kernel);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();

                int panjang;
                //double tmp;

                panjang_kernel = (int)numericUpDown1.Value;
                panjang = dataGridView1.Width / panjang_kernel;
                dataGridView1.ColumnCount = panjang_kernel;

                for (int i = 0; i < panjang_kernel; i++)
                {
                    var baris = new DataGridViewRow();
                    DataGridViewColumn kolom = dataGridView1.Columns[i];
                    kolom.Width = panjang;
                    dataGridView1.Rows.Add(baris);
                }
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.AllowUserToResizeRows = false;
                dataGridView1.AllowUserToResizeColumns = false;

                if (filter_advanced == 1)
                {
                    kernelLowPassAwal();
                }
                else if (filter_advanced == 2)
                {
                    kernelHighPassAwal();
                }
                else if (filter_advanced == 3)
                {
                    kernelHighBoostAwal();
                }
            }
            catch
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                kernel = new int[panjang_kernel, panjang_kernel];

                for (int i = 0; i < panjang_kernel; i++)
                {
                    for (int j = 0; j < panjang_kernel; j++)
                    {
                        kernel[i, j] = Convert.ToInt16(dataGridView1.Rows[i].Cells[j].Value);
                    }
                }
                ((Form1)f).panjang_kernel = this.panjang_kernel;
                ((Form1)f).kernel = this.kernel;
                MessageBox.Show("Data BERHASIL berhasil dimasukkan !");
            }
            catch
            {
                MessageBox.Show("Data TIDAK berhasil dimasukkan !");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ((Form1)f).apply();
            this.Close();
        }

        private void kernelLowPassAwal()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();

            int panjang;
            //panjang_kernel = 3;
            panjang_kernel = (int)numericUpDown1.Value;
            panjang = dataGridView1.Width / panjang_kernel;
            dataGridView1.ColumnCount = panjang_kernel;
            for (int i = 0; i < panjang_kernel; i++)
            {
                var baris = new DataGridViewRow();
                DataGridViewColumn kolom = dataGridView1.Columns[i];
                kolom.Width = panjang;
                dataGridView1.Rows.Add();
            }

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AllowUserToResizeColumns = false;

            kernel = new int[panjang_kernel, panjang_kernel];
            for (int i = 0; i < panjang_kernel; i++)
            {
                for (int j = 0; j < panjang_kernel; j++)
                {
                    kernel[i, j] = Convert.ToInt16(dataGridView1[i, j].Value = 1);
                }
            }
        }

        private void kernelHighPassAwal()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();

            int panjang;
            //panjang_kernel = 3;
            panjang_kernel = (int)numericUpDown1.Value;
            panjang = dataGridView1.Width / panjang_kernel;
            dataGridView1.ColumnCount = panjang_kernel;
            for (int i = 0; i < panjang_kernel; i++)
            {
                var baris = new DataGridViewRow();
                DataGridViewColumn kolom = dataGridView1.Columns[i];
                kolom.Width = panjang;
                dataGridView1.Rows.Add();
            }

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AllowUserToResizeColumns = false;

            kernel = new int[panjang_kernel, panjang_kernel];
            for (int i = 0; i < panjang_kernel; i++)
            {
                for (int j = 0; j < panjang_kernel; j++)
                {
                    if (i == ((panjang_kernel - 1) / 2) && j == ((panjang_kernel - 1) / 2))
                    {
                        kernel[i, j] = Convert.ToInt16(dataGridView1[i, j].Value = 8);
                    }
                    else
                    {
                        kernel[i, j] = Convert.ToInt16(dataGridView1[i, j].Value = -1);
                    }
                }
            }
        }

        private void kernelHighBoostAwal()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();

            int panjang;
            //panjang_kernel = 3;
            panjang_kernel = (int)numericUpDown1.Value;
            panjang = dataGridView1.Width / panjang_kernel;
            dataGridView1.ColumnCount = panjang_kernel;
            for (int i = 0; i < panjang_kernel; i++)
            {
                var baris = new DataGridViewRow();
                DataGridViewColumn kolom = dataGridView1.Columns[i];
                kolom.Width = panjang;
                dataGridView1.Rows.Add();
            }

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AllowUserToResizeColumns = false;

            kernel = new int[panjang_kernel, panjang_kernel];
            for (int i = 0; i < panjang_kernel; i++)
            {
                for (int j = 0; j < panjang_kernel; j++)
                {
                    if (i == ((panjang_kernel - 1) / 2) && j == ((panjang_kernel - 1) / 2))
                    {
                        kernel[i, j] = Convert.ToInt16(dataGridView1[i, j].Value = ((panjang_kernel * panjang_kernel) - 1) + 1);
                    }
                    else
                    {
                        kernel[i, j] = Convert.ToInt16(dataGridView1[i, j].Value = -1);
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RandomRotate
{
    public partial class Form1 : Form
    {
        private int itr = 50;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox2.Text = saveFileDialog1.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Executer.Run(textBox1.Text, textBox2.Text, itr, m =>
            {
                textBox4.AppendText(m + "\r\n");
            });
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            int newVal;
            if (!Int32.TryParse(textBox3.Text, out newVal) || newVal < 1 || newVal > 100000)
            {
                SystemSounds.Beep.Play();
                textBox3.Text = itr.ToString();
            }
            else
            {
                itr = newVal;
            }
        }
    }
}

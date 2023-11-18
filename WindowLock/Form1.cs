using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowLock
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            this.Bounds = Screen.PrimaryScreen.Bounds;
            this.Location = Screen.AllScreens[0].WorkingArea.Location;

            if(Screen.AllScreens.Length > 1)
            {
                Lock1 f2 = new Lock1();
                f2.FormBorderStyle = FormBorderStyle.None;
                f2.WindowState = FormWindowState.Maximized;
                f2.Bounds = Screen.PrimaryScreen.Bounds;
                f2.Location = Screen.AllScreens[1].WorkingArea.Location;
                f2.Show();
            }  
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            var control = new Control();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sifre = textBox1.Text;
            if (sifre == "3899275")
            {
                Close();
            }
        }
    }
}

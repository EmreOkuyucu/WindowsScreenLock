using System.Windows.Forms;

namespace WindowLock2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            this.Bounds = Screen.PrimaryScreen.Bounds;


            int height = 0;
            int width = 0;
            foreach (var screen in System.Windows.Forms.Screen.AllScreens)
            {
                //take smallest height
                height = (screen.Bounds.Height <= height) ? screen.Bounds.Height : height;
                width += screen.Bounds.Width;
            }

            this.Size = new System.Drawing.Size(width, height);
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
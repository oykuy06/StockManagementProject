using System;
using System.Data.SQLite;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace StockManagement
{
    public partial class Form1 : Form
    {
        private string sqliteDbConstr = "Data Source=path/to/your/database.db;Version=3;";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            using (SQLiteConnection connection = new SQLiteConnection(sqliteDbConstr))
            {
                try
                {
                    connection.Open();
                    MessageBox.Show("Successfully connected to database!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Connection error: {ex.Message}");
                }
            }


        }

        private void button3_Click(object sender, EventArgs e)
        {
            UserPanel userPanel = new UserPanel();
            userPanel.Show();
            this.Hide();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

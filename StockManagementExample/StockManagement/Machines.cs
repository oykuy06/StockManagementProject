using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;
using StokTakip;

namespace StockManagement
{
    public partial class Machines : Form
    {
        private string sqliteDbConstr = "Data Source=path/to/your/database.db;Version=3;";
        public Machines()
        {
            InitializeComponent();
        }

        private void Machines_Load(object sender, EventArgs e)
        {
            LoadData(); 
        }

        
        private void LoadData()
        {
            using (SQLiteConnection connection = new SQLiteConnection(sqliteDbConstr))
            {
                try
                {
                    connection.Open(); 

                    string query = "SELECT MaterialName, MaterialQuantity, Info, MaterialStock FROM Material"; 
                    SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable); 

                    
                    if (dataTable.Rows.Count == 0)
                    {
                        MessageBox.Show("There is no data to display in the database."); 
                    }
                    else
                    {
                        dataGridView1.DataSource = dataTable; 
                    }
                }
                catch (SQLiteException ex) 
                {
                    MessageBox.Show($"Database Error: {ex.Message}"); 
                }
                catch (Exception ex) 
                {
                    MessageBox.Show($"Data loading error: {ex.Message}"); 
                }
            }
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            AddMaterial addMaterial = new AddMaterial();
            addMaterial.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UseMaterial useMaterial = new UseMaterial();
            useMaterial.Show();
            this.Hide();
        }
    }
}

using StockManagement;
using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace StokTakip
{
    public partial class AddMaterial : Form
    {
        private string connectionString = "Data Source=path/to/your/database.db;Version=3;";
        public AddMaterial()
        {
            InitializeComponent();
        }


        private void UpdateMaterialStock(string materialName, int materialStock)
        {
            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                con.Open();
                string query = "UPDATE Material SET MaterialStock = MaterialStock + @MaterialStock WHERE MaterialName = @MaterialName";

                using (SQLiteCommand cmd = new SQLiteCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@MaterialStock", materialStock);
                    cmd.Parameters.AddWithValue("@MaterialName", materialName);
                    cmd.ExecuteNonQuery();
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.CheckedItems.Count == 0)
            {
                MessageBox.Show("Please select at least one material.");
                return;
            }

            if (!int.TryParse(textBox1.Text, out int materialStock) || materialStock <= 0)
            {
                MessageBox.Show("Please enter a valid quantity.");
                return;
            }

            foreach (var selectedItem in checkedListBox1.CheckedItems)
            {
                string materialName = selectedItem.ToString();
                AddToStok(materialName, materialStock);
                UpdateMaterialStock(materialName, materialStock); 
            }

            MessageBox.Show("The selected materials have been added successfully.");
            textBox1.Clear(); 
            checkedListBox1.ClearSelected(); 
        }


        private void AddToStok(string materialName, int materialStock)
        {
            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                con.Open();

                int materialId = GetMaterialId(materialName);

                string query = "INSERT INTO Stock (StockMaterialID, StockPiece, StockTransactionType, StockDate, StockMaterialName) " +
                               "VALUES (@StockMaterialID, @StockPiece, @StockTransactionType, @StockDate, @StockMaterialName)";

                using (SQLiteCommand cmd = new SQLiteCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@StockMaterialID", materialId);
                    cmd.Parameters.AddWithValue("@StockPiece", materialStock);
                    cmd.Parameters.AddWithValue("@StockTransactionType", $"{materialStock} eklendi."); 
                    cmd.Parameters.AddWithValue("@StockDate", DateTime.Now); 
                    cmd.Parameters.AddWithValue("@StockMaterialName", materialName);
                    
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private int GetMaterialId(string materialName)
        {
            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                con.Open();
                string query = "SELECT MaterialID FROM Material WHERE MaterialName = @MaterialName";

                using (SQLiteCommand cmd = new SQLiteCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@MaterialName", materialName);
                    return Convert.ToInt32(cmd.ExecuteScalar()); 
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Machines machines = new Machines();
            machines.Show();
            this.Hide();
        }

        private void AddMaterial_Load(object sender, EventArgs e)
        {

        }
    }
}

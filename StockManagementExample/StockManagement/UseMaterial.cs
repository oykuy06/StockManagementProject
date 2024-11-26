using StockManagement;
using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace StokTakip
{
    public partial class UseMaterial : Form
    {
        private string connectionString = "Data Source=path/to/your/database.db;Version=3;";

        public UseMaterial()
        {
            InitializeComponent();
            checkBox1.CheckedChanged += CheckBoxChooseAll_CheckedChanged;
            checkedListBox1.ItemCheck += checkedListBox1_ItemCheck;
            LoadMaterial();

            label2.Visible = false;
            button1.Visible = false; 
            textBox1.Visible = false; 
            button2.Visible = false; 
        }

        private void LoadMaterial()
        {
            checkedListBox1.Items.Clear();

            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                con.Open();
                string query = "SELECT DISTINCT MaterialName FROM Material";

                using (SQLiteCommand cmd = new SQLiteCommand(query, con))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string materialName = reader["MaterialName"].ToString().Trim();

                            if (checkedListBox1.Items.IndexOf(materialName) == -1) 
                            {
                                checkedListBox1.Items.Add(materialName);
                            }
                        }
                    }
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

            if (!int.TryParse(textBox1.Text, out int InputQuantity) || InputQuantity <= 0) 
            {
                MessageBox.Show("Please enter a valid quantity.");
                return;
            }

            foreach (var selectedItem in checkedListBox1.CheckedItems)
            {
                string materialName = selectedItem.ToString();

                int stockQuantity = GetMaterialStock(materialName);

                // Yeterli stok kontrolü
                if (stockQuantity < InputQuantity)
                {
                    MessageBox.Show($"There is not enough stock for {materialName} Current Stock: {stockQuantity}, Desired Quantity: {InputQuantity}");
                    return;
                }

                UpdateMaterialStock(materialName, InputQuantity);
                AddToStok(materialName, InputQuantity);
            }

            MessageBox.Show("Materials were deducted from stock.");
            ClearSelections();
        }

        private void CheckBoxChooseAll_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, checkBox1.Checked);
            }

            if (checkBox1.Checked)
            {
                button2.Visible = true;  
                button1.Visible = false; 
                textBox1.Visible = false;
                label2.Visible = false;
            }
            else
            {
                button2.Visible = false;
                button1.Visible = checkedListBox1.CheckedItems.Count > 0;
                textBox1.Visible = checkedListBox1.CheckedItems.Count > 0;
                label2.Visible = checkedListBox1.CheckedItems.Count > 0;
            }
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!checkBox1.Checked) 
            {
                int selectedCount = e.NewValue == CheckState.Checked ? checkedListBox1.CheckedItems.Count + 1 : checkedListBox1.CheckedItems.Count - 1;
                label2.Visible = selectedCount > 0;
                button1.Visible = selectedCount > 0;
                textBox1.Visible = selectedCount > 0;
            }
        }

        private void AddToStok(string materialName, int InputQuantity)
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
                    cmd.Parameters.AddWithValue("@StockPiece", InputQuantity);
                    cmd.Parameters.AddWithValue("@StockTransactionType", $"{InputQuantity} eksildi.");
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
                    var result = cmd.ExecuteScalar();

                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }
        }

        private void UpdateMaterialStock(string materialName, int InputQuantity)
        {
            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                con.Open();
                string query = "UPDATE Material SET MaterialStock = MaterialStock - @MaterialQuantity WHERE MaterialName = @MaterialName";

                using (SQLiteCommand cmd = new SQLiteCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@MaterialQuantity", InputQuantity);  
                    cmd.Parameters.AddWithValue("@MaterialName", materialName);
                    cmd.ExecuteNonQuery();
                }
            }
        }



        private void ClearSelections()
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, false);
            }
            textBox1.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.Items.Count == 0)
            {
                MessageBox.Show("No materials to deduct from stock.");
                return;
            }

            foreach (var item in checkedListBox1.Items)
            {
                string materialName = item.ToString();

                int stockQuantity = GetMaterialStock(materialName);
                int materialQuantity = GetMaterialStock(materialName); 

                if (stockQuantity >= materialQuantity)
                {
                    UpdateMaterialStock(materialName, materialQuantity); 
                    AddToStok(materialName, materialQuantity); 
                }
                else
                {
                    MessageBox.Show($" There is not enough stock for {materialName} Current Stock: {stockQuantity}, Required Quantity: {materialQuantity}");
                }
            }

            MessageBox.Show("All available materials have been deducted from stock.");
            ClearSelections();
        }

        private int GetMaterialQuantity(string materialName)
        {
            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                con.Open();
                string query = "SELECT MaterialQuantity FROM Material WHERE MaterialName = @MaterialName";

                using (SQLiteCommand cmd = new SQLiteCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@MaterialName", materialName);
                    var result = cmd.ExecuteScalar();

                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }



        private int GetMaterialStock(string materialName)
        {
            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                con.Open();
                string query = "SELECT MaterialStock FROM Material WHERE MaterialName = @MaterialName";

                using (SQLiteCommand cmd = new SQLiteCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@MaterialName", materialName);
                    var result = cmd.ExecuteScalar();

                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Machines machines = new Machines();
            machines.Show();
            this.Hide();
        }

        private void UseMaterial_Load(object sender, EventArgs e)
        {

        }
    }
}
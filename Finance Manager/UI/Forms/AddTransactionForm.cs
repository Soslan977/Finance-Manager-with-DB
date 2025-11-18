using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Finance_Manager.models;
using Microsoft.Data.Sqlite;
using static Finance_Manager.UI.Forms.Kategories;

namespace Finance_Manager.UI.Forms
{
    public partial class AddTransactionForm : Form
    {
        private SqliteConnection _connection;
        private string _connectionString = "Data Source=DataBase_ForApp.db;Version=3;";
        private List<(int Id, string Name)> categories = new List<(int, string)>();

        public Transaction Transaction { get; private set; }

        public AddTransactionForm()
        {
            InitializeComponent();
            radioIncome.CheckedChanged += radioIncome_CheckedChanged;
            radioExpense.CheckedChanged += radioExpense_CheckedChanged;

            _connection = new SqliteConnection(_connectionString);
            _connection.Open();

            radioIncome.Checked = true;
            LoadCategoriesFromDb();
            UpdateCategoryList();
        }

        private void LoadCategoriesFromDb()
        {
            categories.Clear();

            string query = "SELECT Id, Name FROM Categories WHERE Type = @Type";

            using (var command = new SqliteCommand(query, _connection))
            {
                if (radioIncome.Checked)
                {
                    command.Parameters.AddWithValue("@Type", "Доходы");
                }
                else
                {
                    command.Parameters.AddWithValue("@Type", "Расходы");
                }

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        categories.Add((id, name));
                    }
                }
            }
        }
        private void UpdateCategoryList()
        {
            cmbCategory.Items.Clear();

            foreach (var category in categories)
            {
                cmbCategory.Items.Add(category.Name);
            }

            if (cmbCategory.Items.Count > 0)
            {
                cmbCategory.SelectedIndex = 0;
            }
        }



        private void radioIncome_CheckedChanged(object sender, EventArgs e)
        {
            LoadCategoriesFromDb();
            UpdateCategoryList();
        }

        private void radioExpense_CheckedChanged(object sender, EventArgs e)
        {
            LoadCategoriesFromDb();
            UpdateCategoryList();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateData())
                return;

            int categoryId = categories.FirstOrDefault(c => c.Name == cmbCategory.Text).Id;

            string insertQuery = @"
            INSERT INTO Transactions (CategoryId, Amount, Date, Description)
            VALUES (@CategoryId, @Amount, @Date, @Description)";

            using (var command = new SqliteCommand(insertQuery, _connection))
            {
                command.Parameters.AddWithValue("@CategoryId", categoryId);
                command.Parameters.AddWithValue("@Amount", numAmount.Value);
                command.Parameters.AddWithValue("@Date", dtpDate.Value);
                command.Parameters.AddWithValue("@Description", txtTransactionName.Text);
                command.ExecuteNonQuery();
            }

            Transaction = new Transaction
            {
                Name = txtTransactionName.Text,
                Amount = numAmount.Value,
                CategoryId = (int)cmbCategory.SelectedValue,
                IsIncome = radioIncome.Checked,
                Date = dtpDate.Value
            };

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private bool ValidateData()
        {
            if (string.IsNullOrEmpty(txtTransactionName.Text))
            {
                MessageBox.Show("Введите название транзакции!");
                return false;
            }

            if (cmbCategory.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите категорию!");
                return false;
            }

            if (numAmount.Value <= 0)
            {
                MessageBox.Show("Сумма должна быть больше нуля!");
                return false;
            }

            return true; // Все проверки пройдены успешно
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void numAmount_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}

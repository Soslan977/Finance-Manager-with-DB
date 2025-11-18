using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using static Finance_Manager.UI.Forms.Dizain;
using static Finance_Manager.UI.Forms.Dizain.ThemeManager;

namespace Finance_Manager.UI.Forms
{
    public partial class Kategories : Form, IThemeable
    {
        private SqliteConnection _connection;
        private string _connectionString = "Data Source=DataBase_ForApp.db;Version=3;";
        public Kategories()
        {
            InitializeComponent();
            _connection = new SqliteConnection(_connectionString);
            _connection.Open();

            LoadCategoriesFromDb();
            ThemeManager.ThemeChanged += OnThemeChanged;
            ApplyTheme(ThemeManager.CurrentTheme);
        }

        private void LoadCategoriesFromDb()
        {
            listBoxIncome.Items.Clear();
            listBoxExpense.Items.Clear();

            string query = "SELECT Name, Type FROM Categories";
            using (var command = new SqliteCommand(query, _connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = reader.GetString(0);
                        string type = reader.GetString(1);

                        if (type == "Доходы")
                            listBoxIncome.Items.Add(name);
                        else if (type == "Расходы")
                            listBoxExpense.Items.Add(name);
                    }
                }
            }
        }
        private void OnThemeChanged(object sender, EventArgs e)
        {
            ApplyTheme(ThemeManager.CurrentTheme);
        }


        public void ApplyTheme(ThemeType theme)
        {
            // Логика применения темы к элементам управления данной формы
            this.BackColor = theme == ThemeType.Light ? Color.White : Color.FromArgb(30, 30, 30);
            this.ForeColor = theme == ThemeType.Light ? Color.Black : Color.White;

            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Button button)
                {
                    button.ForeColor = theme == ThemeType.Light ? Color.Black : Color.White;
                    button.BackColor = theme == ThemeType.Light ? Color.White : Color.FromArgb(50, 50, 50);
                }
                else if (ctrl is Label label)
                {
                    label.ForeColor = theme == ThemeType.Light ? Color.Black : Color.White;
                }
            }
        }
       
        private void btnAddIncome_Click(object sender, EventArgs e)
        {
            string newCategory = txtIncomeInput.Text.Trim();

            if (!string.IsNullOrEmpty(newCategory))
            {
                // Проверяем наличие категории в БД
                if (!CategoryExists(newCategory))
                {
                    string insertQuery = @"
            INSERT INTO Categories (Name, Type) 
            VALUES (@Name, @Type)";

                    using (var command = new SqliteCommand(insertQuery, _connection))
                    {
                        command.Parameters.AddWithValue("@Name", newCategory);
                        command.Parameters.AddWithValue("@Type", "Доходы");
                        command.ExecuteNonQuery();
                    }

                    LoadCategoriesFromDb();
                    txtIncomeInput.Clear();
                }
                else
                {
                    MessageBox.Show("Категория уже существует!");
                }
            }
            else
            {
                MessageBox.Show("Категория не заполнена!");
            }
        }


        private void GoBack_Click(object sender, EventArgs e)
        {
            FinanceManagerMain.Instance.Show();
            this.Close();
        }

        private void btnAddExpense_Click(object sender, EventArgs e)
        {
            string newCategory = txtExpenseInput.Text.Trim();

            if (!string.IsNullOrEmpty(newCategory))
            {
                if (!CategoryExists(newCategory))
                {
                    string insertQuery = @"
            INSERT INTO Categories (Name, Type) 
            VALUES (@Name, @Type)";

                    using (var command = new SqliteCommand(insertQuery, _connection))
                    {
                        command.Parameters.AddWithValue("@Name", newCategory);
                        command.Parameters.AddWithValue("@Type", "Расходы");
                        command.ExecuteNonQuery();
                    }

                    LoadCategoriesFromDb();
                    txtExpenseInput.Clear();
                }
                else
                {
                    MessageBox.Show("Категория уже существует!");
                }
            }
            else
            {
                MessageBox.Show("Категория не заполнена!");
            }
        }
        private bool CategoryExists(string name)
        {
            string query = "SELECT COUNT(*) FROM Categories WHERE Name = @Name";
            using (var command = new SqliteCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@Name", name);
                return (int)command.ExecuteScalar() > 0;
            }
        }
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            ThemeManager.ThemeChanged -= OnThemeChanged;
            _connection.Close();
            base.OnFormClosed(e);
        }
    }
}

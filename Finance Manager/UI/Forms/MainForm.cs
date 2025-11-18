using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Finance_Manager.models;
using Finance_Manager.UI.Forms;
using Microsoft.Data.Sqlite;
using static Finance_Manager.UI.Forms.Dizain;
using static Finance_Manager.UI.Forms.Dizain.ThemeManager;

namespace Finance_Manager
{
    public partial class FinanceManagerMain : Form, IThemeable
    {
        private SqliteConnection _connection;
        private string _connectionString = "Data Source=DataBase_ForApp.db;Version=3;";
        private List<string> categories = new List<string>();
        private Dictionary<int, string> categoryMap = new Dictionary<int, string>();

        public static FinanceManagerMain Instance { get; private set; }
        private bool menuIsVisible = false;
        public bool isIncomeMode = true;
        public FinanceManagerMain()
        {
            InitializeComponent();
            AddMenuItems();
            Instance = this;

            _connection = new SqliteConnection(_connectionString);
            _connection.Open();

            CreateTablesIfNotExists();

            chartGraphic.Series.Clear();
            chartGraphic.Series.Add("Транзакции");
            chartGraphic.Series[0].ChartType = SeriesChartType.Pie;
            chartGraphic.Titles.Add("Распределение транзакций");

            LoadCategoriesFromDb();

            ThemeManager.ThemeChanged += OnThemeChanged;
            ApplyTheme(ThemeManager.CurrentTheme);
        }

        private void CreateTablesIfNotExists()
        {
            string createCategoriesTable = @"
    CREATE TABLE IF NOT EXISTS Categories (
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        Name TEXT NOT NULL UNIQUE,
        Type TEXT NOT NULL CHECK (Type IN ('Расходы', 'Доходы'))
    )";

            string createTransactionsTable = @"
    CREATE TABLE IF NOT EXISTS Transactions (
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        CategoryId INTEGER,
        Amount REAL NOT NULL,
        Date DATETIME NOT NULL,
        Description TEXT,
        FOREIGN KEY (CategoryId) REFERENCES Categories(Id)
    )";

            using (var command = new SqliteCommand(createCategoriesTable, _connection))
            {
                command.ExecuteNonQuery();
            }

            using (var command = new SqliteCommand(createTransactionsTable, _connection))
            {
                command.ExecuteNonQuery();
            }
        }
        private void LoadCategoriesFromDb()
        {
            categories.Clear();
            categoryMap.Clear();

            string query = "SELECT Id, Name FROM Categories";
            using (var command = new SqliteCommand(query, _connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        categories.Add(name);
                        categoryMap[id] = name;
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

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            // Отписываемся от события при закрытии формы
            ThemeManager.ThemeChanged -= OnThemeChanged;
            base.OnFormClosed(e);
        }
    

        private void MainForm_Load(object sender, EventArgs e)
        {
            burgerButton.Visible = true;
            menuPanel.Visible = menuIsVisible;
        }
        private void ShowForm(Form newForm)
        {
            this.Hide();

            newForm.Show();

            // При закрытии новой формы - показываем главную
            newForm.FormClosed += (s, args) => this.Show();
        }
        
        private void MenuItem_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;

            switch (button.Text)
            {
                case "Главная":
                    ShowForm(new FinanceManagerMain());
                    break;
                case "Категории":
                    ShowForm(new Kategories());
                    break;
                case "Графики":
                    ShowForm(new Graphicss());
                    break;
                case "Регулярный платеж":
                    break;
                case "Дизайн":
                    ShowForm(new Dizain());
                    break;
                case "Валюта":
                    ShowForm(new ChooseVallet());
                    break;
            }
        }
        private void AddMenuItems()
        {
            var items = new[] { "Категории", "Графики", "Регулярный платеж","Дизайн","Валюта" };

            foreach (var item in items)
            {
                var button = new Button
                {
                    Text = item,
                    Dock = DockStyle.Top,
                    Height = 30,
                    BackColor = Color.FromArgb(224, 224, 224)
                };
                button.Click += MenuItem_Click;
                menuPanel.Controls.Add(button);
            }
        }


        private void burgerButton_Click_1(object sender, EventArgs e)
        {
            Panel menu = menuPanel;

            menu.Visible = !menu.Visible;

            if (menu.Visible)
            {
                burgerButton.Text = "×";
            }
            else
            {
                burgerButton.Text = "☰";
            }
        }

        private void Expensesbutton1_Click(object sender, EventArgs e)
        {
            isIncomeMode = false;
            UpdateChart();
        }

        private void Доходы_Click(object sender, EventArgs e)
        {
            isIncomeMode = true;
            UpdateChart();
        }
        private void UpdateChart()
        {
            chartGraphic.Series[0].Points.Clear();
            string query = @"
        SELECT c.Name, SUM(t.Amount)
        FROM Categories c
        JOIN Transactions t ON c.Id = t.CategoryId
        GROUP BY c.Name";
            using (var command = new SqliteCommand(query, _connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string categoryName = reader.GetString(0);
                        double amount = reader.GetDouble(1);
                        chartGraphic.Series[0].Points.AddXY(categoryName, amount);
                    }
                }
            }

            UpdateBalance();
        }

        // Обновление баланса
        private void UpdateBalance()
        {
            string query = @"
        SELECT SUM(Amount) AS TotalBalance
        FROM Transactions";
            using (var command = new SqliteCommand(query, _connection))
            {
                var result = command.ExecuteScalar();
                double balance = result != null ? Convert.ToDouble(result) : 0;
                // Отображение баланса в элементе управления, например, labelBalance
                lblBalance.Text = $"Баланс: {balance:C}";
            }
        }

        private void AddTransaction_Click(object sender, EventArgs e)
        {
            using (var addForm = new AddTransactionForm())
            {
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    var transaction = addForm.Transaction;

                    // Сохраняем транзакцию в БД
                    string insertQuery = @"
            INSERT INTO Transactions (CategoryId, Amount, Date, Description)
            VALUES (@CategoryId, @Amount, @Date, @Description)";

                    using (var command = new SqliteCommand(insertQuery, _connection))
                    {
                        command.Parameters.AddWithValue("@CategoryId", transaction.CategoryId);
                        command.Parameters.AddWithValue("@Amount", transaction.Amount);
                        command.Parameters.AddWithValue("@Date", transaction.Date);
                        command.Parameters.AddWithValue("@Description", transaction.Description);
                        command.ExecuteNonQuery();
                    }

                    UpdateChart();
                    UpdateBalance();
                    LoadCategoriesFromDb(); // Обновляем список категорий
                }
            }
        }

       
    }
}

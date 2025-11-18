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
    public partial class Graphicss : Form, IThemeable
    {
        public Graphicss()
        {
            InitializeComponent();
            ThemeManager.ThemeChanged += OnThemeChanged;
            ApplyTheme(ThemeManager.CurrentTheme);
        }
        private void OnThemeChanged(object sender, EventArgs e)
        {
            ApplyTheme(ThemeManager.CurrentTheme);
        }
        public void ApplyTheme(ThemeType theme)
        {
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
            ThemeManager.ThemeChanged -= OnThemeChanged;
            base.OnFormClosed(e);
        }
        private void GoBack_Click(object sender, EventArgs e)
        {
            FinanceManagerMain.Instance.Show();
            this.Close();
        }
    }
}

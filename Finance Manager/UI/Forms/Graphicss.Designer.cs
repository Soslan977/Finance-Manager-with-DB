namespace Finance_Manager.UI.Forms
{
    partial class Graphicss
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.GoBack = new System.Windows.Forms.Button();
            this.IncomeGraphic = new System.Windows.Forms.Button();
            this.ExpensesGraphic = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // GoBack
            // 
            this.GoBack.Location = new System.Drawing.Point(3, 2);
            this.GoBack.Name = "GoBack";
            this.GoBack.Size = new System.Drawing.Size(33, 37);
            this.GoBack.TabIndex = 0;
            this.GoBack.Text = "🡸";
            this.GoBack.UseVisualStyleBackColor = true;
            this.GoBack.Click += new System.EventHandler(this.GoBack_Click);
            // 
            // IncomeGraphic
            // 
            this.IncomeGraphic.Location = new System.Drawing.Point(160, 12);
            this.IncomeGraphic.Name = "IncomeGraphic";
            this.IncomeGraphic.Size = new System.Drawing.Size(133, 49);
            this.IncomeGraphic.TabIndex = 1;
            this.IncomeGraphic.Text = "Доходы";
            this.IncomeGraphic.UseVisualStyleBackColor = true;
            // 
            // ExpensesGraphic
            // 
            this.ExpensesGraphic.Location = new System.Drawing.Point(534, 12);
            this.ExpensesGraphic.Name = "ExpensesGraphic";
            this.ExpensesGraphic.Size = new System.Drawing.Size(133, 49);
            this.ExpensesGraphic.TabIndex = 2;
            this.ExpensesGraphic.Text = "Расходы";
            this.ExpensesGraphic.UseVisualStyleBackColor = true;
            // 
            // Graphicss
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1039, 598);
            this.Controls.Add(this.ExpensesGraphic);
            this.Controls.Add(this.IncomeGraphic);
            this.Controls.Add(this.GoBack);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Graphicss";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Graphics";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button GoBack;
        private System.Windows.Forms.Button IncomeGraphic;
        private System.Windows.Forms.Button ExpensesGraphic;
    }
}
namespace FinFormsSample
{
    partial class MainForm
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
            this.label7 = new System.Windows.Forms.Label();
            this.btnGetRealmInfo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 578);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 17);
            this.label7.TabIndex = 15;
            this.label7.Text = "FormDigest";
            // 
            // btnGetRealmInfo
            // 
            this.btnGetRealmInfo.Location = new System.Drawing.Point(94, 27);
            this.btnGetRealmInfo.Name = "btnGetRealmInfo";
            this.btnGetRealmInfo.Size = new System.Drawing.Size(172, 38);
            this.btnGetRealmInfo.TabIndex = 23;
            this.btnGetRealmInfo.Text = "Launch!";
            this.btnGetRealmInfo.UseVisualStyleBackColor = true;
            this.btnGetRealmInfo.Click += new System.EventHandler(this.btnGetRealmInfo_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 121);
            this.Controls.Add(this.btnGetRealmInfo);
            this.Controls.Add(this.label7);
            this.Name = "MainForm";
            this.Text = "Office 365 Auth";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnGetRealmInfo;
    }
}


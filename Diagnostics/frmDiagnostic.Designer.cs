namespace CSide_Library_Diagnostics_Tool
{
   partial class frmDiagnostic
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDiagnostic));
         this.txtServerName = new System.Windows.Forms.TextBox();
         this.label1 = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.txtDatabaseName = new System.Windows.Forms.TextBox();
         this.label3 = new System.Windows.Forms.Label();
         this.txtCompanyName = new System.Windows.Forms.TextBox();
         this.txtServerType = new System.Windows.Forms.TextBox();
         this.label4 = new System.Windows.Forms.Label();
         this.txtCSideVersion = new System.Windows.Forms.TextBox();
         this.label5 = new System.Windows.Forms.Label();
         this.txtAppVersion = new System.Windows.Forms.TextBox();
         this.lstLog = new System.Windows.Forms.ListBox();
         this.btnRefresh = new System.Windows.Forms.Button();
         this.SuspendLayout();
         // 
         // txtServerName
         // 
         this.txtServerName.Location = new System.Drawing.Point(138, 6);
         this.txtServerName.Name = "txtServerName";
         this.txtServerName.ReadOnly = true;
         this.txtServerName.Size = new System.Drawing.Size(203, 20);
         this.txtServerName.TabIndex = 0;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(12, 9);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(38, 13);
         this.label1.TabIndex = 1;
         this.label1.Text = "Server";
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(12, 35);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(53, 13);
         this.label2.TabIndex = 3;
         this.label2.Text = "Database";
         // 
         // txtDatabaseName
         // 
         this.txtDatabaseName.Location = new System.Drawing.Point(69, 32);
         this.txtDatabaseName.Name = "txtDatabaseName";
         this.txtDatabaseName.ReadOnly = true;
         this.txtDatabaseName.Size = new System.Drawing.Size(272, 20);
         this.txtDatabaseName.TabIndex = 2;
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(12, 61);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(51, 13);
         this.label3.TabIndex = 5;
         this.label3.Text = "Company";
         // 
         // txtCompanyName
         // 
         this.txtCompanyName.Location = new System.Drawing.Point(69, 58);
         this.txtCompanyName.Name = "txtCompanyName";
         this.txtCompanyName.ReadOnly = true;
         this.txtCompanyName.Size = new System.Drawing.Size(272, 20);
         this.txtCompanyName.TabIndex = 4;
         // 
         // txtServerType
         // 
         this.txtServerType.Location = new System.Drawing.Point(69, 6);
         this.txtServerType.Name = "txtServerType";
         this.txtServerType.ReadOnly = true;
         this.txtServerType.Size = new System.Drawing.Size(65, 20);
         this.txtServerType.TabIndex = 6;
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(12, 92);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(78, 13);
         this.label4.TabIndex = 8;
         this.label4.Text = "C/Side Version";
         // 
         // txtCSideVersion
         // 
         this.txtCSideVersion.Location = new System.Drawing.Point(115, 89);
         this.txtCSideVersion.Name = "txtCSideVersion";
         this.txtCSideVersion.ReadOnly = true;
         this.txtCSideVersion.Size = new System.Drawing.Size(226, 20);
         this.txtCSideVersion.TabIndex = 7;
         // 
         // label5
         // 
         this.label5.AutoSize = true;
         this.label5.Location = new System.Drawing.Point(12, 118);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(97, 13);
         this.label5.TabIndex = 10;
         this.label5.Text = "Application Version";
         // 
         // txtAppVersion
         // 
         this.txtAppVersion.Location = new System.Drawing.Point(115, 115);
         this.txtAppVersion.Name = "txtAppVersion";
         this.txtAppVersion.ReadOnly = true;
         this.txtAppVersion.Size = new System.Drawing.Size(226, 20);
         this.txtAppVersion.TabIndex = 9;
         // 
         // lstLog
         // 
         this.lstLog.FormattingEnabled = true;
         this.lstLog.Location = new System.Drawing.Point(15, 152);
         this.lstLog.Name = "lstLog";
         this.lstLog.Size = new System.Drawing.Size(326, 303);
         this.lstLog.TabIndex = 11;
         // 
         // btnRefresh
         // 
         this.btnRefresh.Image = global::CSide_Library_Diagnostics_Tool.Properties.Resources.view_refresh_71;
         this.btnRefresh.Location = new System.Drawing.Point(347, 6);
         this.btnRefresh.Name = "btnRefresh";
         this.btnRefresh.Size = new System.Drawing.Size(28, 26);
         this.btnRefresh.TabIndex = 12;
         this.btnRefresh.UseVisualStyleBackColor = true;
         this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
         // 
         // frmDiagnostic
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(382, 464);
         this.Controls.Add(this.btnRefresh);
         this.Controls.Add(this.lstLog);
         this.Controls.Add(this.label5);
         this.Controls.Add(this.txtAppVersion);
         this.Controls.Add(this.label4);
         this.Controls.Add(this.txtCSideVersion);
         this.Controls.Add(this.txtServerType);
         this.Controls.Add(this.label3);
         this.Controls.Add(this.txtCompanyName);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.txtDatabaseName);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.txtServerName);
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.Name = "frmDiagnostic";
         this.Text = "Diagnostic";
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.TextBox txtServerName;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.TextBox txtDatabaseName;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.TextBox txtCompanyName;
      private System.Windows.Forms.TextBox txtServerType;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.TextBox txtCSideVersion;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.TextBox txtAppVersion;
      private System.Windows.Forms.ListBox lstLog;
      private System.Windows.Forms.Button btnRefresh;
   }
}
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
         this.label6 = new System.Windows.Forms.Label();
         this.txtFormToOpen = new System.Windows.Forms.TextBox();
         this.btnFormOpen = new System.Windows.Forms.Button();
         this.label7 = new System.Windows.Forms.Label();
         this.txtFormName = new System.Windows.Forms.TextBox();
         this.txtSourceTable = new System.Windows.Forms.TextBox();
         this.label8 = new System.Windows.Forms.Label();
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
         this.lstLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.lstLog.FormattingEnabled = true;
         this.lstLog.Location = new System.Drawing.Point(15, 152);
         this.lstLog.Name = "lstLog";
         this.lstLog.Size = new System.Drawing.Size(668, 303);
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
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.Location = new System.Drawing.Point(390, 13);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(69, 13);
         this.label6.TabIndex = 13;
         this.label6.Text = "Open Form #";
         // 
         // txtFormToOpen
         // 
         this.txtFormToOpen.Location = new System.Drawing.Point(465, 9);
         this.txtFormToOpen.Name = "txtFormToOpen";
         this.txtFormToOpen.Size = new System.Drawing.Size(90, 20);
         this.txtFormToOpen.TabIndex = 14;
         this.txtFormToOpen.Text = "21";
         // 
         // btnFormOpen
         // 
         this.btnFormOpen.Location = new System.Drawing.Point(564, 7);
         this.btnFormOpen.Name = "btnFormOpen";
         this.btnFormOpen.Size = new System.Drawing.Size(91, 24);
         this.btnFormOpen.TabIndex = 15;
         this.btnFormOpen.Text = "Open";
         this.btnFormOpen.UseVisualStyleBackColor = true;
         this.btnFormOpen.Click += new System.EventHandler(this.btnFormOpen_Click);
         // 
         // label7
         // 
         this.label7.AutoSize = true;
         this.label7.Location = new System.Drawing.Point(365, 39);
         this.label7.Name = "label7";
         this.label7.Size = new System.Drawing.Size(94, 13);
         this.label7.TabIndex = 16;
         this.label7.Text = "Last Form Opened";
         // 
         // txtFormName
         // 
         this.txtFormName.Location = new System.Drawing.Point(468, 35);
         this.txtFormName.Name = "txtFormName";
         this.txtFormName.ReadOnly = true;
         this.txtFormName.Size = new System.Drawing.Size(187, 20);
         this.txtFormName.TabIndex = 17;
         // 
         // txtSourceTable
         // 
         this.txtSourceTable.Location = new System.Drawing.Point(468, 58);
         this.txtSourceTable.Name = "txtSourceTable";
         this.txtSourceTable.ReadOnly = true;
         this.txtSourceTable.Size = new System.Drawing.Size(187, 20);
         this.txtSourceTable.TabIndex = 19;
         // 
         // label8
         // 
         this.label8.AutoSize = true;
         this.label8.Location = new System.Drawing.Point(388, 61);
         this.label8.Name = "label8";
         this.label8.Size = new System.Drawing.Size(71, 13);
         this.label8.TabIndex = 18;
         this.label8.Text = "Source Table";
         // 
         // frmDiagnostic
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(695, 464);
         this.Controls.Add(this.txtSourceTable);
         this.Controls.Add(this.label8);
         this.Controls.Add(this.txtFormName);
         this.Controls.Add(this.label7);
         this.Controls.Add(this.btnFormOpen);
         this.Controls.Add(this.txtFormToOpen);
         this.Controls.Add(this.label6);
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
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.TextBox txtFormToOpen;
      private System.Windows.Forms.Button btnFormOpen;
      private System.Windows.Forms.Label label7;
      private System.Windows.Forms.TextBox txtFormName;
      private System.Windows.Forms.TextBox txtSourceTable;
      private System.Windows.Forms.Label label8;
   }
}
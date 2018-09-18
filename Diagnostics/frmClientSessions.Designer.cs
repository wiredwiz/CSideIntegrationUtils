namespace CSide_Library_Diagnostics_Tool
{
   partial class frmClientSessions
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmClientSessions));
         this.lstClients = new System.Windows.Forms.ListView();
         this.colType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.colServer = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.colDatabase = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.colCompany = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.colBusy = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.ExportObjectsButton = new System.Windows.Forms.Button();
         this.PathTextbox = new System.Windows.Forms.TextBox();
         this.FolderLookupButton = new System.Windows.Forms.Button();
         this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
         this.SuspendLayout();
         // 
         // lstClients
         // 
         this.lstClients.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.lstClients.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colType,
            this.colServer,
            this.colDatabase,
            this.colCompany,
            this.colBusy});
         this.lstClients.FullRowSelect = true;
         this.lstClients.Location = new System.Drawing.Point(12, 12);
         this.lstClients.Name = "lstClients";
         this.lstClients.Size = new System.Drawing.Size(605, 268);
         this.lstClients.TabIndex = 2;
         this.lstClients.UseCompatibleStateImageBehavior = false;
         this.lstClients.View = System.Windows.Forms.View.Details;
         // 
         // colType
         // 
         this.colType.Text = "Type";
         // 
         // colServer
         // 
         this.colServer.Text = "Server";
         this.colServer.Width = 100;
         // 
         // colDatabase
         // 
         this.colDatabase.Text = "Database";
         this.colDatabase.Width = 150;
         // 
         // colCompany
         // 
         this.colCompany.Text = "Company";
         this.colCompany.Width = 200;
         // 
         // colBusy
         // 
         this.colBusy.Text = "Is Busy";
         // 
         // ExportObjectsButton
         // 
         this.ExportObjectsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.ExportObjectsButton.Location = new System.Drawing.Point(512, 286);
         this.ExportObjectsButton.Name = "ExportObjectsButton";
         this.ExportObjectsButton.Size = new System.Drawing.Size(105, 37);
         this.ExportObjectsButton.TabIndex = 3;
         this.ExportObjectsButton.Text = "Export Objects";
         this.ExportObjectsButton.UseVisualStyleBackColor = true;
         this.ExportObjectsButton.Click += new System.EventHandler(this.ExportObjectsButton_Click);
         // 
         // PathTextbox
         // 
         this.PathTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.PathTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.PathTextbox.Location = new System.Drawing.Point(63, 295);
         this.PathTextbox.Name = "PathTextbox";
         this.PathTextbox.Size = new System.Drawing.Size(443, 22);
         this.PathTextbox.TabIndex = 4;
         // 
         // FolderLookupButton
         // 
         this.FolderLookupButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.FolderLookupButton.Image = global::CSide_Library_Diagnostics_Tool.Properties.Resources.document_open_folder;
         this.FolderLookupButton.Location = new System.Drawing.Point(12, 286);
         this.FolderLookupButton.Name = "FolderLookupButton";
         this.FolderLookupButton.Size = new System.Drawing.Size(45, 37);
         this.FolderLookupButton.TabIndex = 5;
         this.FolderLookupButton.UseVisualStyleBackColor = true;
         this.FolderLookupButton.Click += new System.EventHandler(this.FolderLookupButton_Click);
         // 
         // saveFileDialog1
         // 
         this.saveFileDialog1.DefaultExt = "txt";
         this.saveFileDialog1.Filter = "Text Files|*.txt";
         this.saveFileDialog1.Title = "Select save file";
         // 
         // frmClientSessions
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(629, 328);
         this.Controls.Add(this.FolderLookupButton);
         this.Controls.Add(this.PathTextbox);
         this.Controls.Add(this.ExportObjectsButton);
         this.Controls.Add(this.lstClients);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.Name = "frmClientSessions";
         this.Text = "C/Side Clients";
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion
      private System.Windows.Forms.ListView lstClients;
      private System.Windows.Forms.ColumnHeader colServer;
      private System.Windows.Forms.ColumnHeader colDatabase;
      private System.Windows.Forms.ColumnHeader colCompany;
      private System.Windows.Forms.ColumnHeader colType;
      private System.Windows.Forms.ColumnHeader colBusy;
      private System.Windows.Forms.Button ExportObjectsButton;
      private System.Windows.Forms.TextBox PathTextbox;
      private System.Windows.Forms.Button FolderLookupButton;
      private System.Windows.Forms.SaveFileDialog saveFileDialog1;
   }
}


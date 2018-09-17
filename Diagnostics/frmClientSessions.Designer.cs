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
         this.SuspendLayout();
         // 
         // lstClients
         // 
         this.lstClients.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colType,
            this.colServer,
            this.colDatabase,
            this.colCompany,
            this.colBusy});
         this.lstClients.FullRowSelect = true;
         this.lstClients.Location = new System.Drawing.Point(12, 35);
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
         // frmClientSessions
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(629, 315);
         this.Controls.Add(this.lstClients);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.Name = "frmClientSessions";
         this.Text = "C/Side Clients";
         this.ResumeLayout(false);

      }

      #endregion
      private System.Windows.Forms.ListView lstClients;
      private System.Windows.Forms.ColumnHeader colServer;
      private System.Windows.Forms.ColumnHeader colDatabase;
      private System.Windows.Forms.ColumnHeader colCompany;
      private System.Windows.Forms.ColumnHeader colType;
      private System.Windows.Forms.ColumnHeader colBusy;
   }
}


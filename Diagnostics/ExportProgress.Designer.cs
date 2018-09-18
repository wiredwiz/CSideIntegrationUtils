namespace CSide_Library_Diagnostics_Tool
{
   partial class ExportProgress
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
         this.ExportProgressBar = new System.Windows.Forms.ProgressBar();
         this.CancelButton = new System.Windows.Forms.Button();
         this.label1 = new System.Windows.Forms.Label();
         this.ElapsedTimeText = new System.Windows.Forms.TextBox();
         this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
         this.SuspendLayout();
         // 
         // ExportProgressBar
         // 
         this.ExportProgressBar.Location = new System.Drawing.Point(12, 12);
         this.ExportProgressBar.Name = "ExportProgressBar";
         this.ExportProgressBar.Size = new System.Drawing.Size(449, 35);
         this.ExportProgressBar.TabIndex = 0;
         // 
         // CancelButton
         // 
         this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.CancelButton.Location = new System.Drawing.Point(151, 100);
         this.CancelButton.Name = "CancelButton";
         this.CancelButton.Size = new System.Drawing.Size(170, 59);
         this.CancelButton.TabIndex = 1;
         this.CancelButton.Text = "Cancel";
         this.CancelButton.UseVisualStyleBackColor = true;
         this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label1.Location = new System.Drawing.Point(12, 59);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(105, 20);
         this.label1.TabIndex = 2;
         this.label1.Text = "Time Elapsed";
         // 
         // ElapsedTimeText
         // 
         this.ElapsedTimeText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.ElapsedTimeText.Location = new System.Drawing.Point(123, 59);
         this.ElapsedTimeText.Name = "ElapsedTimeText";
         this.ElapsedTimeText.ReadOnly = true;
         this.ElapsedTimeText.Size = new System.Drawing.Size(338, 26);
         this.ElapsedTimeText.TabIndex = 3;
         // 
         // backgroundWorker1
         // 
         this.backgroundWorker1.WorkerReportsProgress = true;
         this.backgroundWorker1.WorkerSupportsCancellation = true;
         this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
         this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
         this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
         // 
         // ExportProgress
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(473, 181);
         this.ControlBox = false;
         this.Controls.Add(this.ElapsedTimeText);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.CancelButton);
         this.Controls.Add(this.ExportProgressBar);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.Name = "ExportProgress";
         this.ShowInTaskbar = false;
         this.Text = "ExportProgress";
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.ProgressBar ExportProgressBar;
      private System.Windows.Forms.Button CancelButton;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.TextBox ElapsedTimeText;
      private System.ComponentModel.BackgroundWorker backgroundWorker1;
   }
}
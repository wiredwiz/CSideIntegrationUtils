using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Org.Edgerunner.Dynamics.Nav.CSide;

using Form = System.Windows.Forms.Form;

namespace CSide_Library_Diagnostics_Tool
{
   public partial class ExportProgress : Form
   {
      private Client Client;

      private string ExportFileName;

      private Stopwatch Watch = new Stopwatch();

      public bool Success;

      public int ObjectCount = 0;

      public ExportProgress()
      {
         InitializeComponent();
      }

      public void SetClient(Client client)
      {
         Client = client;
      }

      public void SetExportFileName(string fileName)
      {
         ExportFileName = fileName;
      }

      private void CancelButton_Click(object sender, EventArgs e)
      {
         if (backgroundWorker1.WorkerSupportsCancellation)
            backgroundWorker1.CancelAsync();
         Close();
      }

      public void StartExport()
      {
         backgroundWorker1.RunWorkerAsync();
      }

      private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
      {
         BackgroundWorker worker = sender as BackgroundWorker;
         if (Client != null)
         {
            var permissionTable = Client.GetTable(2000000044);
            permissionTable.SetFilter(1, ">0");
            permissionTable.SetFilter(10, "=Yes");
            permissionTable.SetFilter(12, "=Yes");
            Watch.Start();
            var records = permissionTable.FetchRecords();
            var recordCount = records.Count;
            var currentIteration = 0;
            using (var masterBuffer = new MemoryStream())
            using (var fileStream = new FileStream(ExportFileName, FileMode.Create))
            {
               foreach (var record in records)
               {
                  if (worker.CancellationPending)
                  {
                     e.Cancel = true;
                     return;
                  }

                  var type = (NavObjectType)int.Parse(record.FieldValues[1].Value);
                  var from = int.Parse(record.FieldValues[3].Value);
                  var to = int.Parse(record.FieldValues[4].Value);
                  var stream = Client.ReadObjectsToStream(type, from, to);
                  if (stream.Length != 0)
                  {
                     var buffer = stream.GetBuffer();
                     masterBuffer.Write(Encoding.Convert(Encoding.GetEncoding(850), Encoding.Default, buffer), 0, buffer.Length);
                     ObjectCount += to - from + 1;
                  }

                  currentIteration++;
                  var progress = (int)Math.Truncate((decimal)currentIteration / recordCount * 100);
                  worker.ReportProgress(Math.Min(100, progress));

                  if (masterBuffer.Length > 50000000)
                  {
                     masterBuffer.WriteTo(fileStream);
                     masterBuffer.SetLength(0);
                  }
               }

               if (masterBuffer.Length != 0)
                  masterBuffer.WriteTo(fileStream);
            }

            Watch.Stop();
            Invoke((MethodInvoker)delegate { Close(); });
         }
      }

      private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
      {
         Success = !e.Cancelled;
         if (!Success)
            File.Delete(ExportFileName);
      }

      private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
      {
         ExportProgressBar.Value = e.ProgressPercentage;
         ElapsedTimeText.Text = Watch.Elapsed.ToString();
      }
   }
}

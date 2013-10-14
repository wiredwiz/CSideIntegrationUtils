using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Org.Edgerunner.Dynamics.Nav.CSide;

namespace CSide_Library_Diagnostics_Tool
{
   public partial class frmClientSessions : global::System.Windows.Forms.Form
   {
      public frmClientSessions()
      {
         InitializeComponent();
      }

      private void timer1_Tick(object sender, EventArgs e)
      {
         var clients = Client.GetClients(false);
         var clientSigs = new List<string>();
         foreach (var client in clients)
         {
            if (!client.IsBusy)
            {
               string signature = string.Format("{0}|{1}|{2}|{3}", client.ServerType.ToString(), client.Server, client.Database, client.Company);
               clientSigs.Add(signature);
               bool found = false;
               foreach (ListViewItem clientItem in lstClients.Items)
               {
                  if (clientItem.Name == signature)
                  {
                     found = true;
                     break;
                  }
               }
               if (!found)
               {
                  var item = new ListViewItem(new string[] { client.ServerType.ToString(), client.Server, client.Database, client.Company });
                  item.Name = signature;
                  lstClients.Items.Add(item);
               }
            }
         }
         foreach (ListViewItem clientItem in lstClients.Items)
         {
            if (!clientSigs.Contains<string>(clientItem.Name))
               lstClients.Items.RemoveByKey(clientItem.Name);
         }
      }

      private void frmClientSessions_Load(object sender, EventArgs e)
      {
         timer1.Enabled = true;
      }

      private void lstClients_DoubleClick(object sender, EventArgs e)
      {
         OpenDiagnosticWindow();
      }

      private void btnOpenDiagnostic_Click(object sender, EventArgs e)
      {
         OpenDiagnosticWindow();
      }

      private void OpenDiagnosticWindow()
      {
         if ((lstClients.SelectedItems == null) || (lstClients.SelectedItems.Count == 0))
         {
            MessageBox.Show("Please select a client session first");
            return;
         }
         string[] id = lstClients.SelectedItems[0].Name.Split(new char[] {'|'});
         Client client;
         if (id[0] == "SQL")
            client = Client.GetClient(ServerType.SQL, id[1], id[2], id[3], true);
         else
            client = Client.GetClient(ServerType.Native, id[1], id[2], id[3], true);
         var diagnostic = new frmDiagnostic();
         diagnostic.SetClient(client);
         diagnostic.Show();
      }
   }
}

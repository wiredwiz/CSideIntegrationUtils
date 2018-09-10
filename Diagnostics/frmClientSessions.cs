//
// Copyright 2014 Thaddeus L Ryker
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// Dynamics Nav is a registered trademark of the Microsoft Corporation
//
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
      private ClientRepository Repository;

      public frmClientSessions()
      {
         InitializeComponent();
         Repository = new ClientRepository();
      }

      private void timer1_Tick(object sender, EventArgs e)
      {
         var clients = Repository.GetClients();
         var clientSigs = new List<long>();
         foreach (var client in clients)
         {
            clientSigs.Add(client.Identifier);
            bool found = false;
            ListViewItem foundItem = null;
            foreach (ListViewItem clientItem in lstClients.Items)
            {
               if ((long)clientItem.Tag == client.Identifier)
               {
                  found = true;
                  foundItem = clientItem;
                  break;
               }
            }
            if (!found)
            {
               var item = new ListViewItem(new string[] { client.ServerType.ToString(), client.Server, client.Database, client.Company, "No" });
               item.Tag = (long)client.Identifier;
               item.Name = client.Identifier.ToString();
               lstClients.Items.Add(item);
            }
            else
            {
               foundItem.SubItems[4].Text = client.IsBusy ? "Yes" : "No";
            }
         }
         foreach (ListViewItem clientItem in lstClients.Items)
         {
            if (!clientSigs.Contains<long>((long)clientItem.Tag))
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
         //if ((lstClients.SelectedItems == null) || (lstClients.SelectedItems.Count == 0))
         //{
         //   MessageBox.Show("Please select a client session first");
         //   return;
         //}
         //string[] id = lstClients.SelectedItems[0].Name.Split(new char[] {'|'});
         //Client client;
         //client = ClientRepository.Default.GetClient(id[0] == "SQL" ? ServerType.SQL : ServerType.Native, id[1], id[2], id[3]);
         //var diagnostic = new frmDiagnostic();
         //diagnostic.SetClient(client);
         //diagnostic.ShowDialog();
         //client.Dispose();
      }
   }
}

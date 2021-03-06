﻿//
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
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Org.Edgerunner.Dynamics.Nav.CSide;

namespace CSide_Library_Diagnostics_Tool
{
   public partial class frmClientSessions : global::System.Windows.Forms.Form
   {
      private Dictionary<long, ListViewItem> ListItems;

      public frmClientSessions()
      {
         InitializeComponent();
         ListItems = new Dictionary<long, ListViewItem>();
         ClientRepository.Default.NewClientDetected += Repository_NewClientDetected;
         ClientRepository.Default.ClientClosed += Repository_ClientClosed;
         PopulateClients();
      }

      private void PopulateClients()
      {
         ListItems.Clear();
         foreach (var client in ClientRepository.Default.GetClients())
         {
            var item = new ListViewItem(new[] { client.ServerType.ToString(), client.Server, client.Database, client.Company, "No" });
            item.Tag = client.Identifier;
            item.Name = client.Identifier.ToString();
            lstClients.Items.Add(item);
            ListItems[client.Identifier] = item;
            client.ServerChanged += Client_ServerChanged;
            client.DatabaseChanged += Client_DatabaseChanged;
            client.CompanyChanged += Client_CompanyChanged;
            client.BusyStatusChanged += Client_BusyStatusChanged;
         }
      }

      private void Repository_ClientClosed(object sender, Client client)
      {
         ListViewItem item;
         if (ListItems.TryGetValue(client.Identifier, out item))
         {
            lstClients.Items.Remove(item);
            ListItems.Remove(client.Identifier);
         }
      }

      private void Repository_NewClientDetected(object sender, Client client)
      {
         client.ServerChanged += Client_ServerChanged;
         client.DatabaseChanged += Client_DatabaseChanged;
         client.CompanyChanged += Client_CompanyChanged;
         client.BusyStatusChanged += Client_BusyStatusChanged;
         var item = new ListViewItem(new[] { client.ServerType.ToString(), client.Server, client.Database, client.Company, "No" });
         item.Tag = client.Identifier;
         item.Name = client.Identifier.ToString();
         lstClients.Items.Add(item);
         ListItems[client.Identifier] = item;
      }

      private void Client_CompanyChanged(object sender, Org.Edgerunner.Dynamics.Nav.CSide.EventArguments.CompanyChangedEventArgs e)
      {
         if (sender is Client client)
         {
            ListViewItem item;
            if (ListItems.TryGetValue(client.Identifier, out item))
            {
               item.SubItems[3].Text = e.New;
            }
         }
      }

      private void Client_DatabaseChanged(object sender, Org.Edgerunner.Dynamics.Nav.CSide.EventArguments.DatabaseChangedEventArgs e)
      {
         if (sender is Client client)
         {
            ListViewItem item;
            if (ListItems.TryGetValue(client.Identifier, out item))
            {
               item.SubItems[2].Text = e.New;
            }
         }
      }

      private void Client_ServerChanged(object sender, Org.Edgerunner.Dynamics.Nav.CSide.EventArguments.ServerChangedEventArgs e)
      {
         if (sender is Client client)
         {
            ListViewItem item;
            if (ListItems.TryGetValue(client.Identifier, out item))
            {
               item.SubItems[1].Text = e.NewServerType.ToString();
               item.SubItems[2].Text = e.NewServerName;
            }
         }
      }

      private void Client_BusyStatusChanged(object sender, bool isBusy)
      {
         if (sender is Client client)
            if (ListItems.TryGetValue(client.Identifier, out var item))
               item.SubItems[4].Text = isBusy ? "Yes" : "No";
      }

      private void ExportObjectsButton_Click(object sender, EventArgs e)
      {
         if (lstClients.SelectedItems.Count == 0)
         {
            MessageBox.Show("Please select a client session first");
            return;
         }

         if (string.IsNullOrEmpty(PathTextbox.Text))
         {
            MessageBox.Show("You must specify a file to export to");
            return;
         }

         var id = (long)lstClients.SelectedItems[0].Tag;
         var client = ClientRepository.Default.GetClientById(id);
         if (client != null)
         {
            var progressWindow = new ExportProgress();
            progressWindow.SetClient(client);
            progressWindow.SetExportFileName(PathTextbox.Text);
            progressWindow.Show(this);
            progressWindow.StartExport();
         }
         else
            MessageBox.Show("Unable to find client");
      }

      private void FolderLookupButton_Click(object sender, EventArgs e)
      {
         if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            PathTextbox.Text = saveFileDialog1.FileName;
      }
   }
}

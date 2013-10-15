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
using CSide = Org.Edgerunner.Dynamics.Nav.CSide;

namespace CSide_Library_Diagnostics_Tool
{
   public partial class frmDiagnostic : Form
   {
      CSide.Client _Client;

      public frmDiagnostic()
      {
         InitializeComponent();
      }

      public void SetClient(CSide.Client client)
      {
         _Client = client;
         Text = string.Format("Diagnostic - {0}", _Client.Company);
         UpdateClientData();
         _Client.Activated += new EventHandler<CSide.CSideEventArgs>(_Client_Activated);
         _Client.Deactivated += new EventHandler<CSide.CSideEventArgs>(_Client_Deactivated);
         _Client.CompanyChanged += new EventHandler<CSide.CSideEventArgs>(_Client_CompanyChanged);
         _Client.DatabaseChanged += new EventHandler<CSide.CSideEventArgs>(_Client_DatabaseChanged);
         _Client.ServerChanged += new EventHandler<CSide.CSideEventArgs>(_Client_ServerChanged);
         _Client.FormOpened += new EventHandler<CSide.CSideEventArgs>(_Client_FormOpened);
      }

      void _Client_FormOpened(object sender, CSide.CSideEventArgs e)
      {
         LogData(string.Format("Form {0} opened", e.Form.ID));
         var currForm = _Client.GetObject(CSide.NavObjectType.Form, e.Form.ID);
         txtFormName.Text = currForm != null ? currForm.Name : string.Empty;
         var table = e.Form.GetTable();
         txtSourceTable.Text = table != null ? table.Name : string.Empty;
      }

      void _Client_Deactivated(object sender, CSide.CSideEventArgs e)
      {
         LogData("Client window deactivated");
      }

      void _Client_ServerChanged(object sender, CSide.CSideEventArgs e)
      {
         LogData(string.Format("Client server changed from {0} to {1}", e.PreviousServer, e.CurrentServer));
      }

      void _Client_DatabaseChanged(object sender, CSide.CSideEventArgs e)
      {
         LogData(string.Format("Client database changed from {0} to {1}", e.PreviousDatabase, e.CurrentDatabase));
      }

      void _Client_CompanyChanged(object sender, CSide.CSideEventArgs e)
      {
         LogData(string.Format("Client company changed from {0} to {1}", e.PreviousCompany, e.CurrentCompany));
      }

      void _Client_Activated(object sender, CSide.CSideEventArgs e)
      {
         LogData("Client window activated");
      }

      private void LogData(string logText)
      {
         if (lstLog.Items.Count > 200)
            lstLog.Items.RemoveAt(0);
         lstLog.Items.Add(string.Format("[{0}] {1}", DateTime.Now.ToShortTimeString(), logText));
      }

      private void UpdateClientData()
      {
         txtServerType.Text = _Client.ServerType.ToString();
         txtServerName.Text = _Client.Server;
         txtDatabaseName.Text = _Client.Database;
         txtCompanyName.Text = _Client.Company;
         txtCSideVersion.Text = _Client.CSideVersion;
         txtAppVersion.Text = _Client.ApplicationVersion;
      }

      private void btnRefresh_Click(object sender, EventArgs e)
      {
         UpdateClientData();
      }

      private void btnFormOpen_Click(object sender, EventArgs e)
      {
         CSide.ClientLink link = new CSide.ClientLink();
         link.ServerType = (_Client.ServerType == CSide.ServerType.SQL) ? CSide.ClientLink.ConnectionServerType.MSSQL : CSide.ClientLink.ConnectionServerType.NAVISION;
         link.Server = _Client.Server;
         link.Database = _Client.Database;
         link.Company = _Client.Company;
         link.NtAuthentication = true;
         link.Target = string.Format("Form {0}", txtFormToOpen.Text);
         link.Open();
      }
   }
}

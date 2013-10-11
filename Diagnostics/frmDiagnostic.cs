using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CSide_Library_Diagnostics_Tool
{
   public partial class frmDiagnostic : Form
   {
      Org.Edgerunner.Dynamics.Nav.CSide.Client _Client;

      public frmDiagnostic()
      {
         InitializeComponent();
      }

      public void SetClient(Org.Edgerunner.Dynamics.Nav.CSide.Client client)
      {
         _Client = client;
         Text = string.Format("Diagnostic - {0}", _Client.Company);
         UpdateClientData();
         _Client.Activated += new EventHandler<Org.Edgerunner.Dynamics.Nav.CSide.CSideEventArgs>(_Client_Activated);
         _Client.Deactivated += new EventHandler<Org.Edgerunner.Dynamics.Nav.CSide.CSideEventArgs>(_Client_Deactivated);
         _Client.CompanyChanged += new EventHandler<Org.Edgerunner.Dynamics.Nav.CSide.CSideEventArgs>(_Client_CompanyChanged);
         _Client.DatabaseChanged += new EventHandler<Org.Edgerunner.Dynamics.Nav.CSide.CSideEventArgs>(_Client_DatabaseChanged);
         _Client.ServerChanged += new EventHandler<Org.Edgerunner.Dynamics.Nav.CSide.CSideEventArgs>(_Client_ServerChanged);
         _Client.FormOpened += new EventHandler<Org.Edgerunner.Dynamics.Nav.CSide.CSideEventArgs>(_Client_FormOpened);
      }

      void _Client_FormOpened(object sender, Org.Edgerunner.Dynamics.Nav.CSide.CSideEventArgs e)
      {
         LogData(string.Format("Form {0} opened", e.Form));
      }

      void _Client_Deactivated(object sender, Org.Edgerunner.Dynamics.Nav.CSide.CSideEventArgs e)
      {
         LogData("Client window deactivated");
      }

      void _Client_ServerChanged(object sender, Org.Edgerunner.Dynamics.Nav.CSide.CSideEventArgs e)
      {
         LogData(string.Format("Client server changed from {0} to {1}", e.PreviousServer, e.CurrentServer));
      }

      void _Client_DatabaseChanged(object sender, Org.Edgerunner.Dynamics.Nav.CSide.CSideEventArgs e)
      {
         LogData(string.Format("Client database changed from {0} to {1}", e.PreviousDatabase, e.CurrentDatabase));
      }

      void _Client_CompanyChanged(object sender, Org.Edgerunner.Dynamics.Nav.CSide.CSideEventArgs e)
      {
         LogData(string.Format("Client company changed from {0} to {1}", e.PreviousCompany, e.CurrentCompany));
      }

      void _Client_Activated(object sender, Org.Edgerunner.Dynamics.Nav.CSide.CSideEventArgs e)
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
   }
}

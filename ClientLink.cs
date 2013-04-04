//
// Copyright 2010 Thaddeus L Ryker
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
using System.Linq;
using System.Text;

namespace Org.Edgerunner.Dynamics.Nav.CSide
{

   /// <summary>A class for parsing or building navision client links in a structured fashion</summary>
   /// <keywords>
   /// 	<keyword>Client</keyword>
   /// 	<keyword flags="">Url</keyword>
   /// 	<keyword>Link</keyword>
   /// </keywords>
   /// <seealso cref="!:http://msdn.microsoft.com/en-us/library/dd338856.aspx">Composing URLs</seealso>
   public class ClientLink
   {
      /// <summary>
      /// Initializes a new instance of the ClientLink class.
      /// </summary>
      public ClientLink()
      {

      }

      /// <summary>
      /// Initializes a new instance of the ClientLink class.
      /// </summary>
      /// <param name="link">String containing a Navision client link</param>
      public ClientLink(string link)
      {
         Parse(link);
      }

      /// <summary>Indicates which server to connect to.</summary>
      public ConnectionServerType ServerType { get; set; }
      /// <summary>Indicates which database server to which the Classic client or Microsoft Dynamics NAV Server connects.</summary>
      public string Server { get; set; }
      /// <summary>Indicates which database file to access.</summary>
      public string Database { get; set; }
      /// <summary>Indicates which company to open.</summary>
      public string Company { get; set; }
      /// <summary>
      /// 	<para>Indicates which object to run. Valid argument values are the following:</para>
      /// 	<list type="bullet">
      /// 		<item>
      /// 			<para>form <em>xxx</em></para>
      /// 		</item>
      /// 		<item>
      /// 			<para>form<em>xxx</em></para>
      /// 		</item>
      /// 		<item>
      /// 			<para>Form <em>xxx</em></para>
      /// 		</item>
      /// 		<item>
      /// 			<para>Form<em>xxx</em></para>
      /// 		</item>
      /// 		<item>
      /// 			<para>report <em>xxx</em></para>
      /// 		</item>
      /// 		<item>
      /// 			<para>report<em>xxx</em></para>
      /// 		</item>
      /// 		<item>
      /// 			<para>Report <em>xxx</em></para>
      /// 		</item>
      /// 		<item>
      /// 			<para>Report<em>xxx</em></para>
      /// 		</item>
      /// 	</list>
      /// 	<para>Where <em>xxx</em> is either the object number or object name.</para>
      /// </summary>
      public string Target { get; set; }
      /// <summary>Indicates which filter to set. The format is the same as for the SourceTableView Property on a form.</summary>
      public string View { get; set; }
      /// <summary>Indicates which record to select. The format is the same as for the SourceTablePlacement Property on a form.</summary>
      public string Position { get; set; }
      /// <summary>Indicates whether a report should display its request form. The default value is no.</summary>
      public bool RequestForm { get; set; }
      /// <summary>Indicates whether a new instance of the Classic client should be started regardless of whether a suitable instance is running. The default value is no.</summary>
      public bool ForceNewInstance { get; set; }
      /// <summary>Indicates whether to use windows login authentication.  The default is database login.</summary>
      public bool NtAuthentication { get; set; }
      /// <summary>Indicates the folder path to be used as the temp folder for the client.</summary>
      public string TempPath { get; set; }
      /// <summary>Indicates the NetType to use for the client instance.</summary>
      public ConnectionNetType NetType { get; set; }
      /// <summary>Indicates the size of the object cache to use.</summary>
      public int ObjectCache { get; set; }
      /// <summary>Indicates whether commit cache should be active.</summary>
      public bool CommitCache { get; set; }

      /// <summary>Attempts to open a Navision client instance using the link data contained within the current ClientLink instance.</summary>
      public void Open()
      {
         Client client = null;
         foreach (var instance in Client.GetClients(false))
         {
            if (!instance.IsBusy &&
               instance.Server.ToUpperInvariant() == Server.ToUpperInvariant() &&
               instance.Database.ToUpperInvariant() == Database.ToUpperInvariant() &&
               instance.Company.ToUpperInvariant() == Company.ToUpperInvariant())
            {
               client = instance;
               break;
            }
         }
         if (client == null)
            try
            {
               System.Diagnostics.Process.Start(ToString());
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
               //System.Windows.Forms.MessageBox.Show(
               //   "Unable to find Navision client\nPlease contact IT to repair your Navision installation",
               //   "Problem opening link",
               //   System.Windows.Forms.MessageBoxButtons.OK,
               //   System.Windows.Forms.MessageBoxIcon.Exclamation);
               throw;
            }
         else
            try
            {
               client.OpenLink(ToString());
            }
            finally
            {
               client.Dispose();
            }
      }

      /// <summary>Returns a constructed Navision client link string containing data from the current ClientLink instance.</summary>
      public override string ToString()
      {
         var link = new StringBuilder();
         if (ServerType == ConnectionServerType.NAVISION && !string.IsNullOrEmpty(Database))
            link.AppendFormat("navision://client/run?servertype=NAVISION&database={0}&company={1}", Database, Company);
         else
            link.AppendFormat("navision://client/run?servertype={0}&servername={1}&database={2}&company={3}", Enum.GetName(typeof(ConnectionServerType), ServerType), Server, Database, Company);
         if (!string.IsNullOrEmpty(Target))
            link.Append(string.Format("&target={0}", Target));
         if (!string.IsNullOrEmpty(View))
            link.Append(string.Format("&view={0}", View));
         if (!string.IsNullOrEmpty(Position))
            link.Append(string.Format("&position={0}", Position));
         if (RequestForm)
            link.Append(string.Format("&requestform={0}", RequestForm ? "Yes" : "no"));
         if (ForceNewInstance)
            link.Append(string.Format("&forcenewinstance={0}", ForceNewInstance ? "Yes" : "no"));
         if ((ServerType == ConnectionServerType.MSSQL) && NtAuthentication)
            link.Append(string.Format("&ntauthentication={0}", NtAuthentication ? "Yes" : "no"));
         if (!string.IsNullOrEmpty(TempPath))
            link.Append(string.Format("&temppath={0}", TempPath));
         if (NetType != ConnectionNetType.@default)
            link.Append(string.Format("nettype={0}", Enum.GetName(typeof(ConnectionNetType), NetType)));
         if (ObjectCache != 0)
            link.Append(string.Format("objectcache={0}", ObjectCache));
         if ((ServerType == ConnectionServerType.NAVISION) && !CommitCache)
            link.Append(string.Format("&commitcache={0}", CommitCache ? "Yes" : "no"));
         return link.ToString();
      }

      /// <summary>
      ///  Attempts to parse the provided Navision client link and populate the current instance with the parameter data
      /// </summary>
      /// <param name="link">A valid Navision client link</param>
      /// <param name="message">A message that describes the reason for a failed parse.</param>
      /// <returns>True if the provided link was valid Navision client link that parsed properly, False otherwise</returns>
      public bool Parse(string link, out string message)
      {
         message = string.Empty;
         if (!link.StartsWith("navision://client/run?"))
            return false;
         if (link.Length < 23)
            return false;
         string candidate = link.Substring(22);
         while (true)
         {
            try
            {
               var nextParameter = GetNextRunParameter(ref candidate);
               if (string.IsNullOrEmpty(nextParameter))
                  break;
               string key;
               string value;
               if (ParseRunParameter(nextParameter, out key, out value))
                  switch (key.ToLowerInvariant())
                  {
                     case "servertype":
                        ServerType = ParseServerTypeParameter(value);
                        break;
                     case "servername":
                        Server = value;
                        break;
                     case "database":
                        Database = value;
                        break;
                     case "company":
                        Company = value;
                        break;
                     case "target":
                        Target = value;
                        break;
                     case "view":
                        View = value;
                        break;
                     case "position":
                        Position = value;
                        break;
                     case "requestform":
                        RequestForm = ParseBooleanParameter(key, value);
                        break;
                     case "forcenewinstance":
                        ForceNewInstance = ParseBooleanParameter(key, value);
                        break;
                     case "ntauthentication":
                        NtAuthentication = ParseBooleanParameter(key, value);
                        break;
                     case "temppath":
                        TempPath = value;
                        break;
                     case "nettype":
                        NetType = ParseNetTypeParameter(value);
                        break;
                     case "objectcache":
                        ObjectCache = ParseIntParameter(key, value);
                        break;
                     case "commitcache":
                        CommitCache = ParseBooleanParameter(key, value);
                        break;
                     default:
                        throw new Exception("'{0}' is not a valid parameter flag");
                  }
            }
            catch (Exception ex)
            {
               message = ex.Message;
            }
         }
         return true;
      }

      /// <summary>
      ///  Attempts to parse the provided Navision client link and populate the current instance with the parameter data
      /// </summary>
      /// <param name="link">A valid Navision client link</param>
      /// <returns>True if the provided link was valid Navision client link that parsed properly, False otherwise</returns>
      public bool Parse(string link)
      {
         string result;
         return Parse(link, out result);
      }

      private bool ParseBooleanParameter(string key, string value)
      {
         switch (value.ToLowerInvariant())
         {
            case "yes":
               return true;
            case "no":
               return false;
            default:
               throw new Exception(string.Format("'{0}' is not a valid option for parameter flag {1}\nValid values are 'Yes' or 'no'", value, key));
         }
      }

      private int ParseIntParameter(string key, string value)
      {
         int result;
         if (!int.TryParse(value, out result))
            throw new Exception(string.Format("'{0}' is not a valid value for parameter flag {1}\n{1} must be a number", value, key));
         return result;
      }

      private ConnectionServerType ParseServerTypeParameter(string value)
      {
         switch (value.ToUpperInvariant())
         {
            case "MSSQL":
               return ConnectionServerType.MSSQL;
            case "NAVISION":
               return ConnectionServerType.NAVISION;
            default:
               throw new Exception(string.Format("'{0}' is not a valid option for parameter flag ServerType\nValid values are 'MSSQL' or 'NAVISION'", value));
         }
      }

      private ConnectionNetType ParseNetTypeParameter(string value)
      {
         switch (value.ToLowerInvariant())
         {
            case "default":
               return ConnectionNetType.@default;
            case "namedpipes":
               return ConnectionNetType.namedpipes;
            case "sockets":
               return ConnectionNetType.sockets;
            case "tcp":
               return ConnectionNetType.tcp;
            case "tcps":
               return ConnectionNetType.tcps;
            case "netb":
               return ConnectionNetType.netb;
            default:
               throw new Exception(string.Format("'{0}' is not a valid option for parameter flag NetType\nValid values are 'default', 'namedpipes', 'sockets', 'tcp', 'tcps', or 'netb'", value));
         }
      }

      private string GetNextRunParameter(ref string candidate)
      {
         const char separator = '&';
         var parameterData = new StringBuilder();
         for (int i = 0; i < candidate.Length; i++)
         {
            if (candidate[i] == separator)
            {
               if (i == (candidate.Length - 1))
               {
                  candidate = string.Empty;
                  return parameterData.ToString();
               }

               if (candidate[i + 1] != separator)
               {
                  candidate = candidate.Substring(i + 1);
                  return parameterData.ToString();
               }
               else
                  i++;
            }
            parameterData.Append(candidate[i]);
         }
         candidate = string.Empty;
         return parameterData.ToString();
      }

      private bool ParseRunParameter(string runParameter, out string key, out string value)
      {
         key = string.Empty;
         value = string.Empty;
         int index = runParameter.IndexOf('=');
         if ((index == -1) || (index == 0))
            return false;
         key = runParameter.Substring(0, index);
         if (index != runParameter.Length - 1)
            value = runParameter.Substring(index + 1);
         return true;
      }

      /// <summary>Represents the different network connection types used for connecting to Microsoft Dynamics Nav.</summary>
      public enum ConnectionNetType
      {
         @default,
         tcp,
         tcps,
         namedpipes,
         netb,
         sockets
      }

      /// <summary>Represents the two Microsoft Dynamics Nav server types used for client connections.</summary>
      public enum ConnectionServerType
      {
         MSSQL,
         NAVISION
      }
   }
}

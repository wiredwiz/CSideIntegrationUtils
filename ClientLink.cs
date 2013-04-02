using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Org.Edgerunner.Dynamics.Nav.CSide
{
   [ComVisible(true)]
   [ClassInterface(ClassInterfaceType.AutoDual)]
   [Guid("948db886-b66f-4f40-bbb2-25f4e23d56bf")]
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

      public ConnectionServerType ServerType { get; set; }
      public string Server { get; set; }
      public string Database { get; set; }
      public string Company { get; set; }
      public string Target { get; set; }
      public string View { get; set; }
      public string Position { get; set; }
      public bool RequestForm { get; set; }
      public bool ForceNewInstance { get; set; }
      public bool NtAuthentication { get; set; }
      public string TempPath { get; set; }
      public ConnectionNetType NetType { get; set; }
      public int ObjectCache { get; set; }
      public bool CommitCache { get; set; }

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

      [ComVisible(false)]
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

      public enum ConnectionNetType
      {
         @default,
         tcp,
         tcps,
         namedpipes,
         netb,
         sockets
      }

      public enum ConnectionServerType
      {
         MSSQL,
         NAVISION
      }
   }
}

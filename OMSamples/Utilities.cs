using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using Microsoft.Win32;
using TCX.Configuration;

namespace OMSamples
{
    public static class Utilities
    {
        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileStringA")]
        static extern int GetKeyValueA(string strSection, string strKeyName, string strNull, StringBuilder RetVal, int nSize, string strFileName);
        static public string GetKeyValue(string Section, string KeyName, string FileName)
        {
            //Reading The KeyValue Method
            try
            {
                StringBuilder JStr = new StringBuilder(255);
                int i = GetKeyValueA(Section, KeyName, String.Empty, JStr, 255, FileName);
                return JStr.ToString();
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }
        //This method creates PBX connection basing on legacy settings taken from ini file
        //must not be used with V14
        public static TCX.PBXAPI.PBXConnection CreatePbxConn_legacy()
        {
            String filePath = @".\3CXPhoneSystem.ini";
            if (!File.Exists(filePath))
            {
                //this code expects 3CXPhoneSystem.ini in current directory
                //you can either copy it from the installation folder (find it in Program Files)
                //or run this sample application with current directory set to location of 3CXPhoneSystem.ini
                //see comments in method OMSamples.Program.Main
                throw new Exception("Cannot find 3CXPhoneSystem.ini");
            }
            String pbxUser = String.Empty,
                   pbxPass = String.Empty,
                   pbxHost = "127.0.0.1";
            Int32 pbxPort = 5482;//default value according to stepan
            String value = GetKeyValue("General", "PBXUser", filePath);
            if (!String.IsNullOrEmpty(value))
                pbxUser = value;
            value = GetKeyValue("General", "PBXPass", filePath);
            if (!String.IsNullOrEmpty(value))
                pbxPass = value;
            value = GetKeyValue("General", "CMHost", filePath);
            if (!String.IsNullOrEmpty(value))
                pbxHost = value;
            value = GetKeyValue("General", "CMPort", filePath);
            if (!String.IsNullOrEmpty(value))
            {
                Int32.TryParse(value, out pbxPort);
            }
            return CreatePbxConn(pbxHost, pbxPort, pbxUser, pbxPass);
        }

        public static TCX.PBXAPI.PBXConnection CreatePbxConn(string host, int port, string user, string password)
        {
             return new TCX.PBXAPI.PBXConnection(host, port, user, password);
        }

        public static TCX.PBXAPI.PBXConnection CreatePbxConn()
        {
            //v14 (cloud and in premiss) - stores pbx API connection parameters in configuration.
            //ini file is not required to obtain this information.
            bool exists = false;
            return CreatePbxConn(
                GetParameterValue("CM_API_IP", "127.0.0.1", ref exists),
                int.Parse(GetParameterValue("CM_API_PORT", "5482", ref exists)),
                GetParameterValue("CM_API_USER", "", ref exists),
                GetParameterValue("CM_API_PASSWORD", "", ref exists));
        }

        public static string GetParameterValue(string name, string DefaultValue, ref bool exists)
        {
            try
            {
                exists = true;
                return TCX.Configuration.PhoneSystem.Root.GetParameterByName(name).Value;
            }
            catch
            {
                exists = false;
                return DefaultValue;
            }
        }
    }
}

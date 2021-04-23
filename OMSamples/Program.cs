using System;
using System.Collections.Generic;
using System.Text;
using TCX.Configuration;
using TCX.PBXAPI;
using System.Threading;
using System.IO;

namespace OMSamples
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                {
                    Random a = new Random(Environment.TickCount);
                    //unique name PhoneSystem.ApplicationName = "TestApi";//any name
                    PhoneSystem.ApplicationName = PhoneSystem.ApplicationName + a.Next().ToString();
                }

                #region phone system initialization(init db server)
                String filePath = @".\3CXPhoneSystem.ini";
                if (!File.Exists(filePath))
                {
                    //this code expects 3CXPhoneSystem.ini in current directory.
                    //it can be taken from the installation folder (find it in Program Files/3CXPhone System/instance1/bin for in premiss installation)
                    //or this application can be run with current directory set to location of 3CXPhoneSystem.ini

                    //v14 (cloud and in premiss) installation has changed folder structure.
                    //3CXPhoneSystem.ini which contains connectio information is located in 
                    //<Program Files>/3CX Phone System/instanceN/Bin folder.
                    //in premiss instance files are located in <Program Files>/3CX Phone System/instance1/Bin
                    throw new Exception("Cannot find 3CXPhoneSystem.ini");
                }
                String value = Utilities.GetKeyValue("ConfService", "ConfPort", filePath);
                Int32 port = 0;
                if (!String.IsNullOrEmpty(value))
                {
                    Int32.TryParse(value.Trim(), out port);
                    PhoneSystem.CfgServerPort = port;
                }
                value = Utilities.GetKeyValue("ConfService", "confUser", filePath);
                if (!String.IsNullOrEmpty(value))
                    PhoneSystem.CfgServerUser = value;
                value = Utilities.GetKeyValue("ConfService", "confPass", filePath);
                if (!String.IsNullOrEmpty(value))
                    PhoneSystem.CfgServerPassword = value;
                #endregion
                DN[] ps = PhoneSystem.Root.GetDN(); //Access PhoneSystem.Root to initialize ObjectModel
                SampleStarter.StartSample(args);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
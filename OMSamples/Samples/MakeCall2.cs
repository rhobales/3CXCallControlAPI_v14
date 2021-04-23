using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCX.PBXAPI;

namespace OMSamples.Samples
{
    [SampleCode("makecall2")]
    [SampleParam("arg1", "Source of the call")]
    [SampleParam("arg2, arg3 and so on", "Additional parameters for the call. in form paramname=paramvalue")]
    [SampleDescription("Shows how to use extended version of PBXAPI.MakeCall(string, Dictionary<string, string>) helper")]
    class MakeCall2Sample : ISample
    {
        public void Run(params string[] args)
        {
            PBXConnection pbx = Utilities.CreatePbxConn();
            Dictionary<String, String> d = new Dictionary<String, String>();
            for (int i = 2; i < args.Length; i++)
            {
                String[] a = args[i].Split(new char[] { '=' });
                d.Add(a[0], a[1]);
            }
            try
            {
                pbx.MakeCall(args[1], d);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.ToString());
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCX.PBXAPI;

namespace OMSamples.Samples
{
    [SampleCode("invoke")]
    [SampleParam("arg1", "command which should be invoked")]
    [SampleParam("arg2, arg3 and so on", "additional parameters for Invoke method - each additional parameter should be set as parameter_name=parameter_value")]
    [SampleDescription("Shows how to use PBXAPI.Invoke()")]
    class InvokeSample : ISample
    {
        public void Run(params string[] args)
        {
            String command_name = args[1];
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            for (int i = 2; i < args.Length; i++)
            {
                String[] a = args[i].Split(new char[] { '=' });
                if (a.Length >= 2)
                {
                    parameters.Add(a[0], String.Join("=", a, 1, a.Length - 1));
                }
                else
                {
                    System.Console.WriteLine(args[i] + " ignored");
                }
            }

            PBXConnection pbx = Utilities.CreatePbxConn();
            try
            {
                pbx.InvokeCommand(command_name, parameters);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.ToString());
            }
        }
    }
}
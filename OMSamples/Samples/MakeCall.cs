using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCX.PBXAPI;

namespace OMSamples.Samples
{
    [SampleCode("makecall")]
    [SampleParam("arg1", "Source of the call")]
    [SampleParam("arg2", "Destination of the call")]
    [SampleDescription("Shows how to use PBXAPI.MakeCall() helper")]
    class MakeCallSample : ISample
    {
        public void Run(params string[] args)
        {
            PBXConnection pbx = Utilities.CreatePbxConn();
            for (; ; )
            {
                try
                {
                    pbx.MakeCall(args[1], args[2]);
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.ToString());
                }
                ConsoleKeyInfo k = System.Console.ReadKey(false);
                if (k.KeyChar == 'e')
                {
                    break;
                }
            }
        }
    }
}

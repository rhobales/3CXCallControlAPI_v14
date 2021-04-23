using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCX.Configuration;
using TCX.PBXAPI;

namespace OMSamples.Samples
{
    [SampleCode("dropcall")]
    [SampleParam("arg1", "Specifies extension number")]
    [SampleDescription("Shows how to drop calls on specific extension using Call Control API")]
    class DropCallSample : ISample
    {
        public void Run(params string[] args)
        {
            DN dn = PhoneSystem.Root.GetDNByNumber(args[1]);
            bool found = false;
            PBXConnection pbx = Utilities.CreatePbxConn();
            foreach (var ac in dn.GetActiveConnections())
            {
                pbx.DropCall(ac);
                found = true;
            }
            if (!found)
                Console.WriteLine(args[2] + " does not participate in call " + args[1]);
        }
    }
}

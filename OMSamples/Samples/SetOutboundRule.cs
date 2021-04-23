using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCX.Configuration;

namespace OMSamples.Samples
{
    [SampleCode("set_outboundrule")]
    [SampleDescription("Shows how to configure outbound rules. Requires at least one gateway configured on PBX.")]
    class SetOutboundRuleSample : ISample
    {
        public void Run(params string[] args)
        {
            PhoneSystem ps = PhoneSystem.Root;
            Tenant t = ps.GetTenants()[0];
            OutboundRule r = t.CreateOutboundRule();

            r.OutboundRoutes[0].Gateway = ps.GetGateways()[0];
            try
            {
                //this line should not throw an exception
                r.OutboundRoutes[5].Gateway = ps.GetGateways()[0];
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Number of available routes is " + r.OutboundRoutes.Length + " by default");
            }
            r.Save();
            Console.WriteLine(r);
        }
    }
}

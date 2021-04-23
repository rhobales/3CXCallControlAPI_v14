using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCX.Configuration;

namespace OMSamples.Samples
{
    [SampleCode("delete_current_profile")]
    [SampleParam("arg1", "Extension number")]
    [SampleWarning("This sample damages configuration of the extension specified by parameter. To check this sample, create new extension run sample with its number and then delete that extension")]
    [SampleDescription("Removes profile which is currently seleted in extension configuration")]
    class DeleteCurrentProfileSample : ISample
    {
        public void Run(params string[] args)
        {
            Extension ext = PhoneSystem.Root.GetDNByNumber(args[1]) as Extension;
            FwdProfile fp = ext.CurrentProfile;
            if (fp != null)
            {
                fp.Delete();
            }
        }
    }
}
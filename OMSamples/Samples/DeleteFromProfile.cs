using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCX.Configuration;

namespace OMSamples.Samples
{
    [SampleCode("delete_from_profile")]
    [SampleParam("arg1", "Extension which will be modified")]
    [SampleWarning("This sample damages configuration of extension 112. 112 should be deleted and recreated again after running this sample")]
    [SampleDescription("Shows how to remove extension rules. The rule, deleted by this samle, will disappear from all \nforwarding profiles where it was assigned to.")]
    class DeleteFromProfileSample : ISample
    {
        public void Run(params string[] args)
        {
            Extension ext = PhoneSystem.Root.GetDNByNumber(args[1]) as Extension;

            List<ExtensionRule> extRuleList = new List<ExtensionRule>(ext.ForwardingRules);
            ExtensionRule ruleToDelete = ext.ForwardingRules[2];
            if (extRuleList.Contains(ruleToDelete))
                extRuleList.Remove(ruleToDelete);
            extRuleList.Clear();
            ext.ForwardingRules = extRuleList.ToArray();
            ext.Save();
        }
    }
}
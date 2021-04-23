using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCX.Configuration;

namespace OMSamples.Samples
{
    [SampleCode("create_fwd_profile")]
    [SampleParam("arg1", "Extension where forwarding profiles will be changed.")]
    [SampleWarning("This sample is does not create valid configuration of an extension, it just show how to work with FwdProfile object.")]
    [SampleDescription("Creates FwdProfile objects for extension 100. Shows how to set current profile for extension and attach rules to the profile.")]
    class CreateFwdProfileSample : ISample
    {
        public void Run(params string[] args)
        {
            PhoneSystem ps = PhoneSystem.Root;
            Extension ex = ps.GetDNByNumber(args[1]) as Extension;
            FwdProfile[] fps = ex.FwdProfiles;
            if (fps.Length > 0)
            {
                ex.CurrentProfile = fps[0];
                ex.Save();
            }
            ExtensionRule er = ex.CreateForwardingRule();
            er.Conditions.Condition = ps.GetRuleConditions()[0];
            er.Conditions.Hours = ps.GetRuleHourTypes()[0];
            er.Conditions.CallType = ps.GetRuleCallTypes()[0];
            er.Data = "1234567";
            er.Forward.To = DestinationType.External;
            er.Forward.External = "987654321";

            HoursRange hr = er.CreateHoursRange();
            System.Collections.Generic.List<HoursRange> arr = new System.Collections.Generic.List<HoursRange>();
            HoursRange r = er.CreateHoursRange();

            r.DayOfWeek = System.DayOfWeek.Wednesday;
            r.StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0);
            r.EndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 17, 0, 0);
            arr.Add(r);
            r = er.CreateHoursRange();
            r.DayOfWeek = System.DayOfWeek.Thursday;
            r.StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0);
            r.EndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 17, 0, 0);
            arr.Add(r);
            er.HoursRanges = arr.ToArray();
            System.Collections.Generic.List<ExtensionRule> arrR = new System.Collections.Generic.List<ExtensionRule>();
            arrR.Add(er);
            ExtensionRule er2 = ex.CreateForwardingRule();
            er2.Conditions.Condition = ps.GetRuleConditions()[1];
            er2.Conditions.Hours = ps.GetRuleHourTypes()[1];
            er2.Conditions.CallType = ps.GetRuleCallTypes()[1];
            er2.Data = "1234567";
            er2.Forward.To = DestinationType.External;
            er2.Forward.External = "1234567890";
            arrR.Add(er2);
            FwdProfile fp = ex.CreateFwdProfile("test_forward_profile1");
            fp.ForwardingRules = arrR.ToArray();
            fp.Save();
            FwdProfile fp1 = ex.CreateFwdProfile("test_forward_profile2");
            arrR.Clear();
            arrR.Add(fp.ForwardingRules[0]);
            fp1.ForwardingRules = arrR.ToArray();
            fp1.Save();
            ex.Save();
        }
    }
}

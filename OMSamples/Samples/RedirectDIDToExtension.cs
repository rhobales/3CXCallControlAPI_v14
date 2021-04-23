using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCX.Configuration;
using System.Threading;

namespace OMSamples.Samples
{
    [SampleCode("redirectdidtoextension")]
    [SampleWarning("this script will modify destination for the existing DID rules. To repair configuration you need to restore configuration or revert changes manually")]
    [SampleDescription("This sample shows how to configure set of DID rules which will transform DID number to extension number.\n It modifies only DIDs rules which are configured to the same destination during office and out of office hours")]
    [SampleParam("arg1", "External line virtual number (DN)")]
    [SampleParam("arg2", "DID number prefix")]
    [SampleParam("arg3", "DID number length")]
    [SampleParam("arg4", "Replace Prefix With")]
    class RedirectDIDToExtension : ISample
    {
        public void Run(params string[] args)
        {
            ExternalLine theLine = PhoneSystem.Root.GetDNByNumber(args[1]) as ExternalLine;
            int DIDLength = Int32.Parse(args[3]);
            string DIDPrefix = args[2];
            ExternalLineRule[] dids = theLine.RoutingRules;
            string retplace_prefix_with = args[4];
            bool changed = false;
            foreach (ExternalLineRule r in dids)
            {
                RuleConditionGroup c = r.Conditions;
                if (c.Hours.Type== RuleHoursType.AllHours&&c.Condition.Type == RuleConditionType.BasedOnDID && r.Data.Length == DIDLength && r.Data.StartsWith(DIDPrefix))
                {
                    string ExtensionNumber = retplace_prefix_with+r.Data.Substring(DIDPrefix.Length);
                    DN dest = PhoneSystem.Root.GetDNByNumber(ExtensionNumber);
                    if (dest!=null &&((dest is Extension)||(dest is RingGroup)||(dest is Queue)||(dest is IVR)||(dest is FaxExtension)))
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("The Line {0}(on {1} {2}): {3}={4}({5})", theLine.Number, theLine.Gateway.Name, (theLine.Gateway is VoipProvider)?"Voip":"PSTN", r.Data, dest.Number, dest.GetType().Name);
                        r.Forward.Internal = dest;
                        r.Forward.To =  (dest is Extension)?DestinationType.Extension:
                            (dest is RingGroup)?DestinationType.RingGroup:
                            (dest is Queue)?DestinationType.Queue:
                            (dest is IVR)?DestinationType.IVR:
                            (dest is FaxExtension)?DestinationType.Fax:DestinationType.Extension;
                        changed = true;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("The Line {0}(on {1} {2}): {3}->{4} Destination is not found. Rule will not be changed", theLine.Number, theLine.Gateway.Name, (theLine.Gateway is VoipProvider) ? "Voip" : "PSTN", r.Data, ExtensionNumber);
                    }
                }
                else
                {
                    if (r.Data.Length != DIDLength || !r.Data.StartsWith(DIDPrefix))
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("The Line {0}(on {1} {2}): {3} Not a {4} digits start with {5}. Skip update.", theLine.Number, theLine.Gateway.Name, (theLine.Gateway is VoipProvider) ? "Voip" : "PSTN", r.Data, DIDLength, DIDPrefix);
                    }
                    else if (c.Condition.Type != RuleConditionType.BasedOnDID)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("The Line {0}(on {1} {2}): {3} Not a DID rule. Skip update.", theLine.Number, theLine.Gateway.Name, (theLine.Gateway is VoipProvider) ? "Voip" : "PSTN", r.Data);
                    }
                    else if (c.Hours.Type != RuleHoursType.AllHours)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("The Line {0}(on {1} {2}): {3} Not an \'All Hours\' rule. Skip update.", theLine.Number, theLine.Gateway.Name, (theLine.Gateway is VoipProvider) ? "Voip" : "PSTN", r.Data);
                    }
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
            if (changed)
            {
                Console.Write("Whould you like to apply the changes?(Y/N)");
                string res = System.Console.ReadLine();
                if (res == "Y" || res == "y")
                {
                    theLine.RoutingRules = dids;
                    theLine.Save();
                    Console.WriteLine("Changes applied");
                }
                else
                {
                    Console.WriteLine("Chnages is not applied");
                }
            }
            else
            {
                Console.WriteLine("There are no changes to apply");
            }
            Thread.Sleep(5000);
        }
    }
}

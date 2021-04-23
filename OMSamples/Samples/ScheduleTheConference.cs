using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCX.PBXAPI;
using TCX.Configuration;

namespace OMSamples.Samples
{
    [SampleCode("schedule_conference")]
    [SampleParam("arg1", "idstat/name")]
    [SampleParam("arg2", "date and time")]
    [SampleParam("arg3", "pin")]
    [SampleParam("arg4", "Name of conference")]
    [SampleParam("arg5", "(optional) Comma separated list of the participant who should be called at the beginning of conference (moderators)")]
    [SampleDescription("Schedule the conference")]
    class ScheduleTheConference : ISample
    {
        public void Run(params string[] args)
        {
            PhoneSystem ps = PhoneSystem.Root;
            var stat = ps.CreateStatistics("S_SCHEDULEDCONF", args[1]);
            if (args.Length == 2 )
            {
                if(stat.GetHashCode() != 0)
                {
                    stat.Delete();
                    System.Console.WriteLine("Schedule {0}.{1} deleted ", args[1], stat.GetHashCode());
                }
                else
                {
                    System.Console.WriteLine("Schedule {0} not found", args[1]);
                }
                return;
            }
            DateTime dt;
            if(!DateTime.TryParseExact(args[2], @"yyyy-MM-dd HH\:mm\:ss", System.Globalization.CultureInfo.InvariantCulture,System.Globalization.DateTimeStyles.AssumeLocal, out dt))
            {
                System.Console.WriteLine("Cannot parse the date: {0} should by yyy-MM-dd HH\\:mm\\:ss format", args[1]);
                return;
            }
            stat.clearall();
            stat["name"] = args[4];
            stat["idstat"] = args[1];
            stat["pin"] = args[3];
            stat["startat"] = dt.ToUniversalTime().ToString(@"yyyy-MM-dd HH\:mm\:ss");
            stat["target"] = args.Length > 5 ? args[5] : "";
            stat["numbertocall"] = "";
            stat["email"] = "";
            stat["description"] = "";
            stat["emailtext"] = "";
            stat.update();
        }
    }
}

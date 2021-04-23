using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TCX.Configuration;

namespace OMSamples.Samples
{
    [SampleCode("receive_stat_notifications")]
    [SampleParam("arg1", "Type of objects")]
    [SampleDescription("Shows notificatins for specific statistics object.")]
    class ReceiveStatNotificationsSample : ISample
    {
        public void Run(params string[] args)
        {
            PhoneSystem ps = PhoneSystem.Root;
            String filter = null;
            if (args.Length > 1)
                filter = args[1];
            MyListener a = new MyListener(filter);
            ps.Updated += new NotificationEventHandler(a.ps_Updated);
            ps.Inserted += new NotificationEventHandler(a.ps_Inserted);
            ps.Deleted += new NotificationEventHandler(a.ps_Deleted);
            Statistics[] myStat;
            myStat = ps.InitializeStatistics(args[1]);
            foreach (Statistics s in myStat)
            {
                Console.WriteLine(s.ToString());
            }

            while (true)
            {
                Thread.Sleep(5000);
            }
        }
    }
}
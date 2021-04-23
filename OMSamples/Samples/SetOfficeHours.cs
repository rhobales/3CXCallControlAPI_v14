using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCX.Configuration;

namespace OMSamples.Samples
{
    [SampleCode("set_office_hours")]
    [SampleWarning("This sample modifies office hours on the PBX and sets them to Monday 8:00 - 17:00 and Tuesday 8:00 - 17:00")]
    [SampleDescription("Shows how to setup 'Office Hours' of PBX")]
    class SetOfficeHoursSample : ISample
    {
        public void Run(params string[] args)
        {
            PhoneSystem ps = PhoneSystem.Root;
            Tenant tadd = ps.GetTenants()[0];
            if (tadd != null)
            {
                System.Collections.Generic.List<HoursRange> arr = new System.Collections.Generic.List<HoursRange>();
                HoursRange r = tadd.CreateHoursRange();
                r.DayOfWeek = System.DayOfWeek.Monday;

                r.StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0);
                r.EndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 17, 0, 0);
                arr.Add(r);
                r = tadd.CreateHoursRange();
                r.DayOfWeek = System.DayOfWeek.Tuesday;
                r.StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0);
                r.EndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 17, 0, 0);
                arr.Add(r);
                tadd.OfficeHoursRanges = arr.ToArray();
                tadd.Save();
            }
        }
    }
}
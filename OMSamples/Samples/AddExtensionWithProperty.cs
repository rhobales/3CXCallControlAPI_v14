using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCX.Configuration;

namespace OMSamples.Samples
{
    [SampleCode("add_extension_with_property")]
    [SampleParam("arg1", "Extension number")]
    [SampleDescription("Creates new extension and sets DNProperty TEST_PROPERTY to string 'justFortest'")]
    class AddExtensionWithPropertySample : ISample
    {
        public void Run(params string[] args)
        {
            PhoneSystem ps = PhoneSystem.Root;
            String data = args[1];
            Extension e = ps.GetTenants()[0].CreateExtension(data);
            e.AuthID = data; //can be the same as extension number
            e.AuthPassword = "Non-trivial-password-here"; //MUST NOT be the same as extension number

            e.SIPID = ""; //here you can specify alias of extension which will be used  to call the
            //extension using "direct SIP"

            e.BusyDetection = BusyDetectionType.UsePBXStatus;
            e.DeliverAudio = false;
            e.EmailAddress = "user@company.com";
            e.Enabled = true;
            e.FirstName = "F_" + data;
            e.HidePresence = false;
            e.Internal = true;
            e.LastName = "L_" + data;
            e.NoAnswerTimeout = 60;
            e.Number = data;
            e.OutboundCallerID = "OUTCID_" + data; //extension may have specific callerId for outbound calls
            e.QueueStatus = QueueStatusType.LoggedOut;
            e.RecordCalls = false;
            e.SupportReinvite = true;
            e.SupportReplaces = true;
            e.UserStatus = UserStatusType.Available;
            e.VMEmailOptions = VMEmailOptionsType.None;
            e.VMEnabled = true;
            e.VMPIN = data;
            e.VMPlayCallerID = true;
            e.VMPlayMsgDateTime = VMPlayMsgDateTimeType.Play24Hr;

            //Adding property to extension:
            e.SetProperty("TEST_PROPERTY", "justFortest", PropertyType.String, "sample description");
			
			e.CurrentProfile = PhoneSystem.Root.GetExtensions()[0].FwdProfiles.FirstOrDefault(p => p.Name == "Available");
			
            e.Save();
            Console.WriteLine("Extension added: " + e);
        }
    }
}
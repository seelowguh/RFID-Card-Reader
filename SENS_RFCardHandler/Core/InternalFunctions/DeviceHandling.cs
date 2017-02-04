using System.Reflection;
using System.Timers;
using clsRFDeviceHandling;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace SENS_RFCardHandler.Core.InternalFunctions
{
    public class DeviceHandling
    {
        /*
        clsPcProx pcProx;

        public DeviceHandling()
        {
            pcProx = new clsPcProx();
            pcProx.pcProxIDDetected += ReadID;
            //Delegate pcProxMethod = clsPcProx.pcProxIDDetectedEventHandler.CreateDelegate(GetType(), ReadID());


            //clsPcProx.pcProxIDDetected += new clsPcProx.pcProxIDDetectedEventHandler(ReadID);
        }

        public string ReadID(ref string _ReturnMessage, ref string _Facilitycode) 
        {
            string scardid = string.Empty;
            try
            {
                //clsPcProx _pcProx = new clsPcProx();
                _ReturnMessage = pcProx.Device_ReadID(ref _Facilitycode, ref scardid);

                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            return scardid;
        }
        */
        private IDevice iDevice;
        private System.Timers.Timer tPollTimer;

        public DeviceHandling()
        {
            StartPollingDevice();

        }

        private void StartPollingDevice()
        {
            if (iDevice.Connect())
            {
                //  set up timer
                double TimerInteval = 800;
                tPollTimer = new Timer(TimerInteval);
                tPollTimer.Enabled = true;
                //Attach timer to sub
                tPollTimer.Elapsed += onPoll;
                //start timer
                tPollTimer.Start();
            }
            else
            {
                if(tPollTimer != null)
                    tPollTimer.Dispose();
            }
        }

        private void onPoll(Object source, System.Timers.ElapsedEventArgs e)
        {
            clsPcProx.CardDetails cardDetails = ReadDeviceID();
            if (cardDetails.CardID != string.Empty)
            {
                RaiseEvent(cardDetails.CardID, cardDetails.FacilityCode);
            }
        }

        private clsPcProx.CardDetails ReadDeviceID()
        {
            return iDevice.ReadDevice();
        }

        public class CardDetailsEventArgs : EventArgs
        {
            public string CardID { get; private set; }
            public string FacilityCode { get; private set; }

            public CardDetailsEventArgs(string _cardid, string _facilitycode)
            {
                CardID = _cardid;
                FacilityCode = _facilitycode;
            }
        }

        public EventHandler<CardDetailsEventArgs> DeviceActivated;

        protected void RaiseEvent(string CardID, string FacilityCode)
        {
            var e = DeviceActivated;
            if (e != null)
                e(this, new CardDetailsEventArgs(CardID, FacilityCode));
        }

    }
}

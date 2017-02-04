using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchraderElectronics.SEL_InterfaceLibrary;

namespace SENS_RFCardHandler.Core.InternalFunctions
{
    public class ClarifiHandling
    {
        public bool SendMessage(string _EventName, string _Installation, string _Execution, string _QueueType, int _QueueNumber, string _Message)
        {
            if (clsClarifi.CheckAgentStatus() == clsClarifi.AgentStatus.Started)
            {
                if (_QueueNumber <=1 || _QueueNumber > 10)
                    clsClarifi.TriggerAgent(_EventName, _Installation, _Message, _Execution, _QueueType);
                else
                    clsClarifi.TriggerAgent(_EventName, _Installation, _Message, _Execution, _QueueType, _QueueNumber);
                return true;
            }
            else
            {
                return false;
            }
        }

        



    }
}

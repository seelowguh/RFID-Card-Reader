using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;


namespace SENS_RFCardHandler.Core.InternalFunctions
{
    public class RegistryHandling
    {
        #region Declarations
        private const string cDeviceType = "DEVICE_TYPE";
        private const string cEventName = "EVENT_NAME";
        private const string cInstallation = "INSTALLATION";
        private const string cExecution = "EXECUTION";
        private const string cQueueType = "QUEUE_TYPE";
        private const string cQueueNumber = "QUEUE_NUMBER";
        private const string cMessage = "XML_MESSAGE";


        public struct RegistryDetails
        {
            public int DeviceType;
            public string EventName;
            public string Installation;
            public string Execution;
            public string QueueType;
            public int QueueNumber;
            public string Message;
        }
        //          private RegistryKey BaseKey = Registry.LocalMachine;
        //          private string SubKey = @"SOFTWARE\SENSATA\RFCARDHANDLER";

        private RegistryKey BaseKey;
        private string SubKey;

        #endregion

        public RegistryHandling(RegistryKey _baseKey, string _subKey)
        {
            BaseKey = _baseKey;
            SubKey = _subKey;
        }
        
        private string ReadKey(string _sKeyName)
        {
            try
            {
                RegistryKey rKey = BaseKey;
                RegistryKey rSubKey = rKey.OpenSubKey(SubKey);

                if (rSubKey == null)
                {
                    return null;
                }
                else
                {
                    return (string) rSubKey.GetValue(_sKeyName.ToUpper());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private bool WriteKey(string _sKeyName, object _oKeyValue)
        {
            try
            {
                RegistryKey rKey = BaseKey;
                RegistryKey rSubKey = rKey.CreateSubKey(SubKey);
                rSubKey.SetValue(_sKeyName.ToUpper(), _oKeyValue);
                return true;
            }
            catch (NullReferenceException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }   
        }

        private bool DeleteKey(string _sKeyName)
        {
            try
            {
                RegistryKey rKey = BaseKey;
                RegistryKey rSubKey = rKey.CreateSubKey(SubKey);

                if (rSubKey != null)
                {
                    rSubKey.DeleteValue(_sKeyName);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool DeleteSubKeyTree()
        {
            try
            {
                RegistryKey rKey = BaseKey;
                RegistryKey rSubKey = rKey.CreateSubKey(SubKey);

                if (rSubKey != null)
                {
                    rKey.DeleteSubKeyTree(SubKey);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int SubKeyCount()
        {
            try
            {
                RegistryKey rKey = BaseKey;
                RegistryKey rSubKey = rKey.CreateSubKey(SubKey);

                if (rSubKey != null)
                    return rSubKey.SubKeyCount;
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int ValueCount()
        {
            try
            {
                RegistryKey rKey = BaseKey;
                RegistryKey rSubKey = rKey.CreateSubKey(SubKey);

                if (rSubKey != null)
                    return rSubKey.ValueCount;
                else
                {
                    return 0;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool WriteRegistry(RegistryDetails rd)
        {
            try
            {
                if (!WriteKey(cDeviceType, rd.DeviceType))
                    return false;

                if (!WriteKey(cEventName, rd.EventName))
                    return false;

                if (!WriteKey(cInstallation, rd.Installation))
                    return false;

                if (!WriteKey(cExecution, rd.Execution))
                    return false;

                if (!WriteKey(cQueueType, rd.QueueType))
                    return false;

                if (rd.QueueNumber < 1 || rd.QueueNumber > 10)
                    rd.QueueNumber = 1;

                if (!WriteKey(cQueueNumber, rd.QueueNumber))
                    return false;

                if (!WriteKey(cMessage, rd.Message))
                    return false;

                return true;

            }
            catch (NullReferenceException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public RegistryDetails ReadDetails()
        {
            try
            {
                RegistryDetails rd = new RegistryDetails();
                rd.DeviceType = Convert.ToInt32(ReadKey(cDeviceType));
                rd.EventName = ReadKey(cEventName);
                rd.Installation = ReadKey(cInstallation);
                rd.Execution = ReadKey(cExecution);
                rd.QueueType = ReadKey(cQueueType);
                rd.QueueNumber = Convert.ToInt32(ReadKey(cQueueNumber));
                rd.Message = ReadKey(cMessage);

                return rd;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static bool CanWriteKey( string key)
        {
            try
            {
                RegistryPermission r = new RegistryPermission(RegistryPermissionAccess.Write, key);
                r.Demand();
                return true;
            }
            catch (SecurityException)
            {
                return false;
            }
        }

        public static bool CanReadKey( string key)
        {
            try
            {
                RegistryPermission r = new RegistryPermission(RegistryPermissionAccess.Read, key);
                r.Demand();
                return true;
            }
            catch (SecurityException)
            {
                return false;
            }
        }
    }
}

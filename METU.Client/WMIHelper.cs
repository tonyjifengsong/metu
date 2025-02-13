using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace METU.Client
{
  public  class WMIHelper
    {
        ManagementScope _managementScope = null;
        public string ServerName { get; set; }
        public string ServerIP { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public ManagementScope GetManagementScope()
      {
          if (_managementScope == null)
          {
              if (Environment.MachineName.ToLower() == ServerName.ToLower())
              {
                  _managementScope = new ManagementScope("\\\\" + ServerIP + "\\root\\cimv2");
              }
              else if (UserName != null && UserName.Length > 0)
             {
                 ConnectionOptions connectionOptions = new ConnectionOptions();
                 connectionOptions.Username = UserName;
                 connectionOptions.Password = Password;
                 _managementScope = new ManagementScope("\\\\" + ServerIP + "\\root\\cimv2", connectionOptions);
             }
             else
             {
                 throw new ManagementException();
             }
         }
         return _managementScope;
     }
        private ManagementObjectSearcher GetManagementObjectSearcher(string wql)
        {
            if (wql == null || wql.ToString().Trim().Length < 10)
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_PerfFormattedData_PerfOS_Processor");
                return searcher;
            }
            return new ManagementObjectSearcher(GetManagementScope(), new SelectQuery(wql));
        }
        public List<ManagementBaseObject> GetManagementObjects(string wql)
        {
            List<ManagementBaseObject> managementObjects = new List<ManagementBaseObject>();
            ManagementObjectCollection collection = GetManagementObjectCollection(wql);
            foreach (ManagementObject managementObject in collection)
            {
                managementObjects.Add(managementObject);
            }
            return managementObjects;
        }
        ManagementObjectCollection GetManagementObjectCollection(string wql)
        {
            ManagementObjectSearcher query = new ManagementObjectSearcher(wql);

            ManagementObjectCollection queryCollection = query.Get();
            return queryCollection;
        }
        public object GetSystemInfo(ManagementBaseObject managementObject, string type)
        {
            return managementObject[type];
        }
        public object GetSystemInfo(string wql, string type)
        {
            return GetSystemInfo(GetManagementObjects(wql)[0], type);
        }
     public   string Name { get; set; }
        private ManagementObject GetServiceObject()
        {
            return GetManagementObjects("SELECT * FROM Win32_Service WHERE Name = '" + Name + "'")[0] as ManagementObject;
        }

        private object GetManagementObjectInfo(string type)
        {
            return GetSystemInfo(GetServiceObject(), type);
        }
        public bool Start()
        {
            try
            {
                if (Status() == "Stopped")
                {
                    GetServiceObject().InvokeMethod("StartService", null);
                    while (Status() == "Start Pending") ;
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                throw new Exception(Name + "服务启动失败", e);
            }
        }
        public string Status()
     {
         return GetManagementObjectInfo("State") as string;
     }
        public bool AcceptStop()
        {
            return (bool)GetManagementObjectInfo("AcceptStop");
        }

        public bool Stop()
        {
            try
            {
                if (AcceptStop())
                {
                    GetServiceObject().InvokeMethod("StopService", null);
                    while (Status() == "Stop Pending") ;
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                throw new Exception(Name + "服务停止失败", e);
            }
        }
        public int AverageLoadPercentage()
        {
            int loadPercentage = 0;
            List<ManagementBaseObject> collection = GetManagementObjects("SELECT * FROM Win32_Processor");
            foreach (ManagementObject managementObject in collection)
            {
                object load = GetSystemInfo(managementObject, "LoadPercentage");
                if (load == null)
                {
                    load = 0;
                }
                loadPercentage += Int32.Parse(load.ToString());
            }
            return loadPercentage / collection.Count;
        }
        public int Number()
      {
         return GetManagementObjects("SELECT * FROM Win32_Processor").Count;
      }
        public string Model()
        {
            return GetSystemInfo("SELECT * FROM Win32_Processor", "Name").ToString().Trim();
        }
        public string TotalSize()
        {
            float size = 0;
            List<ManagementBaseObject> collection = GetManagementObjects("SELECT * FROM Win32_OperatingSystem");
            foreach (ManagementObject managementObject in collection)
            {
                size += long.Parse(GetSystemInfo(managementObject, "TotalVisibleMemorySize").ToString());
            }
            return (size / 1024).ToString("0.00") + "MB";
        }
        public string FreeSize()
        {
            float size = 0;
            List<ManagementBaseObject> collection = GetManagementObjects("SELECT * FROM Win32_OperatingSystem");
            foreach (ManagementObject managementObject in collection)
            {
                size += float.Parse(GetSystemInfo(managementObject, "FreePhysicalMemory").ToString());
            }
            return (size / 1024).ToString("0.00") + "MB";
        }
        private void GetLogicalDisk()
        {
            List<ManagementBaseObject> collection = GetManagementObjects("SELECT * FROM Win32_LogicalDisk WHERE DriveType = " + (int)System.IO.DriveType.Fixed);

            foreach (ManagementObject managementObject in collection)
            {
                long size, freeSize;
                object managementObjectInfo;
                managementObjectInfo = GetSystemInfo(managementObject, "Size");
                if (managementObjectInfo == null)
                {
                    continue;
                }
                else
                {
                    size = long.Parse(GetSystemInfo(managementObject, "Size").ToString());
                }
                managementObjectInfo = GetSystemInfo(managementObject, "FreeSpace");
                if (managementObjectInfo == null)
                {
                    freeSize = 0;
                }
                else
                {
                    freeSize = long.Parse(GetSystemInfo(managementObject, "FreeSpace").ToString());
                }
                string deviceID = GetSystemInfo(managementObject, "deviceid").ToString();
                _logicalDisks.Add(new LogicalDisk(deviceID, size, freeSize));
            }
        }
        List<LogicalDisk> _logicalDisks = new List<LogicalDisk>();
        public struct LogicalDisk
        {
            private string _deviceID;
            private long _size;
            private long _freeSize;

            public string Size
            {
                get
                {
                    return ((float)_size / 1024 / 1024 / 1024).ToString("0.00") + "GB";
                }
            }

            public string FreeSize
            {
                get
                {
                    return ((float)_freeSize / 1024 / 1024 / 1024).ToString("0.00") + "GB";
                }
            }

            public LogicalDisk(string deviceID, long size, long freeSize)
            {
                _deviceID = deviceID;
                _size = size;
                _freeSize = freeSize;
            }
        }
    }
}

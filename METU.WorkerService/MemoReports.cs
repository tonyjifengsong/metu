using System;
using System.Management;

namespace METU.WorkerService
{
    public  class MemoReports
    {
     
        #region 获取内存使用率

        #region 可用内存

        /// <summary>
        ///     获取可用内存
        /// </summary>
        internal static long? GetMemoryAvailable()
        {
            const int MbDiv = 1024 * 1024;
            long availablebytes = 0;
            var managementClassOs = new ManagementClass("Win32_OperatingSystem");
            foreach (var managementBaseObject in managementClassOs.GetInstances())
                if (managementBaseObject["FreePhysicalMemory"] != null)
                    availablebytes = 1024 * long.Parse(managementBaseObject["FreePhysicalMemory"].ToString());
            return availablebytes / MbDiv;
        }

        #endregion

        internal static double? GetMemoryUsed()
        {
            float? PhysicalMemory = GetPhysicalMemory();
            float? MemoryAvailable = GetMemoryAvailable();
            double? MemoryUsed = (double?)(PhysicalMemory - MemoryAvailable);
            double currentMemoryUsed = (double)MemoryUsed;
            return currentMemoryUsed;
        }

        private static long? GetPhysicalMemory()
        {
            //获得物理内存
            const int MbDiv = 1024 * 1024;
            var managementClass = new ManagementClass("Win32_ComputerSystem");
            var managementObjectCollection = managementClass.GetInstances();
            foreach (var managementBaseObject in managementObjectCollection)
                if (managementBaseObject["TotalPhysicalMemory"] != null)
                    return long.Parse(managementBaseObject["TotalPhysicalMemory"].ToString()) / MbDiv;
            return null;
        }

        public static double? GetMemoryUsedRate()
        {
            float? PhysicalMemory = GetPhysicalMemory();
            float? MemoryAvailable = GetMemoryAvailable();
            double? MemoryUsedRate = (double?)(PhysicalMemory - MemoryAvailable) / PhysicalMemory;
            return MemoryUsedRate.HasValue ? Convert.ToDouble(MemoryUsedRate * 100) : 0;
        }

        #endregion
        public static void GetLogicalDrives()
        {

            ManagementClass diskClass = new ManagementClass("Win32_LogicalDisk");
            ManagementObjectCollection disks = diskClass.GetInstances();
            double usedDisk = 0;
            double totalDisk = 0;
            double freeDisk = 0;


            foreach (ManagementObject disk in disks)
            {
                double pusedDisk = double.Parse(disk["Size"].ToString())- double.Parse(disk["FreeSpace"].ToString());
                double ptotalDisk = double.Parse(disk["Size"].ToString());
                double pfreeDisk = double.Parse(disk["FreeSpace"].ToString());
                usedDisk += pusedDisk;
                freeDisk += pfreeDisk;
                totalDisk += ptotalDisk;
                Console.WriteLine(disk["Name"].ToString()+"-----"+ long.Parse(disk["Size"].ToString()) + "-----" + long.Parse(disk["FreeSpace"].ToString())+"-----"+(pusedDisk*100/ptotalDisk));
               
            }

        }
    }
}

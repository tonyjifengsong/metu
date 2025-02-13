using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace METU.Client
{
    public class RegLocalHelper
    {/// <summary>
     /// 判断项是否存在
     /// </summary>
     /// <param name="RegBoot"></param>
     /// <param name="ItemName"></param>
     /// <returns></returns>
        public static bool IsRegeditItemExist(Microsoft.Win32.RegistryKey RegBoot, string ItemName)
        {
            if (ItemName.IndexOf("\\") <= -1)
            {
                string[] subkeyNames;
                subkeyNames = RegBoot.GetValueNames();
                foreach (string ikeyName in subkeyNames)   // 遍历整个数组
                {
                    if (ikeyName == ItemName)  // 判断子项的名称
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                string[] strkeyNames = ItemName.Split('\\');
                Microsoft.Win32.RegistryKey _newsubRegKey = RegBoot.OpenSubKey(strkeyNames[0]);
                string _newRegKeyName = "";
                int i;
                for (i = 1; i < strkeyNames.Length; i++)
                {
                    _newRegKeyName = _newRegKeyName + strkeyNames[i];
                    if (i != strkeyNames.Length - 1)
                    {
                        _newRegKeyName = _newRegKeyName + "\\";
                    }
                }
                return IsRegeditItemExist(_newsubRegKey, _newRegKeyName);
            }
        }
        /// <summary>
        /// 判断键值是否存在
        /// </summary>
        /// <param name="RegBoot"></param>
        /// <param name="RegKeyName"></param>
        /// <returns></returns>
        public static bool IsRegeditKeyExist(Microsoft.Win32.RegistryKey RegBoot, string RegKeyName)
        {
            string[] subkeyNames;
            subkeyNames = RegBoot.GetValueNames();
            foreach (string keyName in subkeyNames)
            {

                if (keyName == RegKeyName)   // 判断键值的名称
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 读取注册表指定键值
        /// </summary>
        /// <param name="RegKeyName"></param>
        /// <returns></returns>
        public static string ReadRegedit(string RegKeyName)
        {
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine;


            if (!IsRegeditItemExist(key.OpenSubKey("software\\ThirdParty\\"), "Register.INI"))
            {
                // 创建
                Microsoft.Win32.RegistryKey software = key.CreateSubKey("software\\ThirdParty");
                // 打开(true表示可以写入)
                software = key.OpenSubKey("software\\ThirdParty", true);
                Microsoft.Win32.RegistryKey ThirdPartyReg = software.CreateSubKey("Register.INI");
                // 记得关闭,两个都要关
                key.Close();
                ThirdPartyReg.Close();
            }
            if (!IsRegeditKeyExist(key.OpenSubKey("software\\ThirdParty\\Register.INI"), RegKeyName))
            {
                Microsoft.Win32.RegistryKey Register = key.OpenSubKey("SOFTWARE\\ThirdParty\\Register.INI", true);
                // 写入
                Register.SetValue(RegKeyName, "");
                // 关闭
                Register.Close();
                key.Close();
            }
            //判断软件是否注册
            Microsoft.Win32.RegistryKey retkey = key.OpenSubKey("SOFTWARE\\ThirdParty\\Register.INI", true);
            string str = retkey.GetValue(RegKeyName).ToString();
            retkey.Close();
            key.Close();
            return str;

        }
        /// <summary>
        /// 写入指定键值
        /// </summary>
        /// <param name="RegKeyName"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static bool SetRegedit(string RegKeyName, string Value)
        {
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine;


            if (!IsRegeditItemExist(key.OpenSubKey("software\\ThirdParty\\"), "Register.INI"))
            {
                // 创建
                Microsoft.Win32.RegistryKey software = key.CreateSubKey("software\\ThirdParty");
                // 打开(true表示可以写入)
                software = key.OpenSubKey("software\\ThirdParty", true);
                Microsoft.Win32.RegistryKey ThirdPartyReg = software.CreateSubKey("Register.INI");

                // 记得关闭,两个都要关
                key.Close();
                ThirdPartyReg.Close();
            }
            if (!IsRegeditKeyExist(key.OpenSubKey("software\\ThirdParty\\Register.INI"), RegKeyName))
            {
                Microsoft.Win32.RegistryKey Register = key.OpenSubKey("SOFTWARE\\ThirdParty\\Register.INI", true);
                // 写入
                Register.SetValue(RegKeyName, "");
                // 关闭
                Register.Close();
                key.Close();
            }
            //判断软件是否注册
            Microsoft.Win32.RegistryKey retkey = key.OpenSubKey("SOFTWARE\\ThirdParty\\Register.INI", true);
            // 写入
            retkey.SetValue(RegKeyName, Value);
            retkey.Close();
            key.Close();
            return true;

        }
        /// <summary>
        /// 检查OS
        /// </summary>
        /// <returns></returns>
        public static bool CheckIsWindows()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.WriteLine("This application can only run on windows.");
                // Environment.Exit(-1);
                return false;
            }
            return true;

        }
        /// <summary>
        /// 判断程序是否具有管理员权限
        /// </summary>
        /// <returns></returns>
        public static bool CheckAdmin()
        {
            using var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            var isElevated = principal.IsInRole(WindowsBuiltInRole.Administrator);

            if (!isElevated)
            {
                Console.WriteLine("Administrator permission is required to running.");
                // Environment.Exit(-1);
                return false;
            }

            return true;

        }


        /// <summary>
        /// 获取所有子项名
        /// </summary>
        /// <param name="subkeyName"></param>
        /// <returns></returns>
        public static List<string> ReadKeyNames(string subkeyName)
        {

            List<string> rs = new List<string>();
            var key = Registry.LocalMachine.OpenSubKey(subkeyName, true);
            //先获取ValueName
            var keyNames = key.GetValueNames();
            if (!keyNames.Any())
            {
                Console.WriteLine("No record found, no need to clear.");
                return rs;
            }
            foreach (var name in keyNames)
            {
                //获取值
                rs.Add(name);
                //删除值
                // key.DeleteValue(name);
            }
            return rs;
        }

        /// <summary>
        /// 获取指定子项目下键值
        /// </summary>
        /// <param name="subkeyName"></param>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public static string ReadKeyValue(string subkeyName, string keyName)
        {
            if (subkeyName == null) return null;
            if (keyName == null) return null;
            if (subkeyName.Trim().Length < 3) return null;
            if (keyName.Trim().Length < 1) return null;

            List<string> rs = new List<string>();
            var key = Registry.LocalMachine.OpenSubKey(subkeyName, true);

            return key.GetValue(keyName).ToString();
        }

        /// <summary>
        /// 指定子项目下添加子项
        /// </summary>
        /// <param name="subkeyName"></param>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public static bool SetKeyValue(string subkeyName, string keyName, string keyValue)
        {
            if (subkeyName == null) return false;
            if (keyName == null) return false;
            if (subkeyName.Trim().Length < 3) return false;
            if (keyName.Trim().Length < 1) return false;

            List<string> rs = new List<string>();
            var key = Registry.LocalMachine.OpenSubKey(subkeyName, true);

            key.SetValue(keyName, keyValue);
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="subkeyName"></param>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public static bool CreateSubKey(string subkeyName, string keyName)
        {
            if (subkeyName == null) return false;
            if (keyName == null) return false;
            if (subkeyName.Trim().Length < 3) return false;
            if (keyName.Trim().Length < 1) return false;

            List<string> rs = new List<string>();
            var key = Registry.LocalMachine.OpenSubKey(subkeyName, true);

            key.CreateSubKey(keyName);
            return true;
        }
        public static bool DeleteSubKey(string subkeyName, string keyName)
        {
            if (subkeyName == null) return false;
            if (keyName == null) return false;
            if (subkeyName.Trim().Length < 3) return false;
            if (keyName.Trim().Length < 1) return false;

            List<string> rs = new List<string>();
            var key = Registry.LocalMachine.OpenSubKey(subkeyName, true);

            key.DeleteSubKey(keyName);
            return true;
        }
        /// <summary>
        /// 删除指定子项下的指定项
        /// </summary>
        /// <param name="subkeyName"></param>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public static bool DeleteSubValue(string subkeyName, string keyName)
        {
            if (subkeyName == null) return false;
            if (keyName == null) return false;
            if (subkeyName.Trim().Length < 3) return false;
            if (keyName.Trim().Length < 1) return false;

            List<string> rs = new List<string>();
            var key = Registry.LocalMachine.OpenSubKey(subkeyName, true);

            key.DeleteValue(subkeyName);
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="subkeyName"></param>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public static bool DeleteSubKeyTree(string subkeyName, string keyName)
        {
            if (subkeyName == null) return false;
            if (keyName == null) return false;
            if (subkeyName.Trim().Length < 3) return false;
            if (keyName.Trim().Length < 1) return false;

            List<string> rs = new List<string>();
            var key = Registry.LocalMachine.OpenSubKey(subkeyName, true);

            key.DeleteSubKeyTree(keyName);
            return true;
        }
        /// <summary>
        /// CreateProtocol
        /// </summary>
        public static void CreateProtocol()
        {
            string nodeName = AppDomain.CurrentDomain.FriendlyName;
            string nodeNames = nodeName + ".exe";
            string programFullPath = string.Format("{0}\\{1}", Environment.CurrentDirectory, nodeNames);
            string protocolName = nodeName.Replace(".", "").Replace("_", "").Replace("-", "");

            RegistryKey key = Registry.ClassesRoot;

            if (!key.Name.Contains(protocolName))
            {

                RegistryKey software = key.CreateSubKey(protocolName);
                software.SetValue("URL Protocol", programFullPath);
                software.SetValue("", protocolName);

                RegistryKey softwareDefaultIcon = software.CreateSubKey("DefaultIcon");
                softwareDefaultIcon.SetValue("", string.Format("{0},{1}", programFullPath, 1));

                RegistryKey softwareShell = software.CreateSubKey("shell");
                softwareShell = softwareShell.CreateSubKey("open");
                softwareShell = softwareShell.CreateSubKey("command");
                softwareShell.SetValue("", string.Format("\"{0}\" \"%{1}\"", programFullPath, 1));
            }


        }

        /// <summary>
        /// CreateProtocol
        /// </summary>
        public static void CreateProtocolByAppSetting()
        {
            string fl = "";
            try
            {
                fl = AppSettingsHelper.Configuration["enabledprotocolconfig"].ToString();
            }
            catch (Exception ex)
            {
                fl = "true";
            }
            string nodeName = AppSettingsHelper.Configuration["appname"].ToString(); ;

            string nodeNames = nodeName + ".exe";
            string programFullPath = string.Format("{0}\\{1}", Environment.CurrentDirectory, nodeNames);
            string protocolName = "";
            if (fl == "")
            {
                protocolName = nodeName.Replace(".", "").Replace("_", "").Replace("-", "");
            }
            else
            {
                protocolName = null;
            }
            if (protocolName == null)
            {
                protocolName = AppSettingsHelper.Configuration["protocol"].ToString();
            }
            if (protocolName.Trim().ToString().Length < 1)
            {
                protocolName = AppSettingsHelper.Configuration["protocol"].ToString();

            }
            RegistryKey key = Registry.ClassesRoot;

            if (!key.Name.Contains(protocolName))
            {

                RegistryKey software = key.CreateSubKey(protocolName);
                software.SetValue("URL Protocol", programFullPath);
                software.SetValue("", protocolName);

                RegistryKey softwareDefaultIcon = software.CreateSubKey("DefaultIcon");
                softwareDefaultIcon.SetValue("", string.Format("{0},{1}", programFullPath, 1));

                RegistryKey softwareShell = software.CreateSubKey("shell");
                softwareShell = softwareShell.CreateSubKey("open");
                softwareShell = softwareShell.CreateSubKey("command");
                softwareShell.SetValue("", string.Format("\"{0}\" \"%{1}\"", programFullPath, 1));
            }


        }

    }
}

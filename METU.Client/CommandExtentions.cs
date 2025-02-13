using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.Client
{
    /// <summary>
    /// 
    /// </summary>
 public  static class CommandExtentions
    {
        public static string ExecuteShell(this string fileName, string args = null)
        {
            #region NETCore调用Shell
            fileName = AppDomain.CurrentDomain.BaseDirectory + fileName;
            //创建一个ProcessStartInfo对象 使用系统shell 指定命令和参数 设置标准输出
            var psi = new ProcessStartInfo(fileName, args) { RedirectStandardOutput = true };
            //启动
            var proc = Process.Start(psi);
            if (proc == null)
            {
                return ("Can not exec.");
            }
            else
            {
                string result = "";
                result = proc.StandardOutput.ReadToEnd();
                if (!proc.HasExited)
                {
                    proc.Kill();
                }

                return result;

            }
       
            #endregion

         }
        public static List<string> ExecutesShell(this string fileName, string args = null)
        {
            List<string> rs = new List<string>();
            #region NETCore调用Shell
            fileName = AppDomain.CurrentDomain.BaseDirectory + fileName;
            //创建一个ProcessStartInfo对象 使用系统shell 指定命令和参数 设置标准输出
            var psi = new ProcessStartInfo(fileName, args) { RedirectStandardOutput = true };
            //启动
            var proc = Process.Start(psi);
            if (proc == null)
            {
                return rs;
            }
            else
            {
                //开始读取
                using (var sr = proc.StandardOutput)
                {
                    while (!sr.EndOfStream)
                    {
                        rs.Add(sr.ReadLine());
                    }

                    if (!proc.HasExited)
                    {
                        proc.Kill();
                    }
                }
                proc.WaitForExit();//等待程序执行完退出进程  
                proc.Close();//结束 
                return rs;

            }

            #endregion

        }
        public static string ExecuteBatShell(this string fileName)
        {
            #region NETCore调用Shell
            fileName = AppDomain.CurrentDomain.BaseDirectory + fileName;
            var psi = new ProcessStartInfo(fileName) { RedirectStandardOutput = true };
            //启动
            var proc = Process.Start(psi);
            if (proc == null)
            {
                return ("Can not exec.");
            }
            else
            {
            string result = "";
            result = proc.StandardOutput.ReadToEnd();
            if (!proc.HasExited)
            {
                proc.Kill();
            }

            return result;
        }
            #endregion

        }
        public static List<string> ExecuteBatsShell(this string fileName)
        {
            List<string> rs = new List<string>();
            #region NETCore调用Shell
            fileName = AppDomain.CurrentDomain.BaseDirectory + fileName;
            var psi = new ProcessStartInfo(fileName) { RedirectStandardOutput = true };
            //启动
            var proc = Process.Start(psi);
            if (proc == null)
            {
                return rs;
            }
            else
            {
                //开始读取
                using (var sr = proc.StandardOutput)
                {
                    while (!sr.EndOfStream)
                    {
                        rs.Add(sr.ReadLine());
                    }

                    if (!proc.HasExited)
                    {
                        proc.Kill();
                    }
                }
                proc.WaitForExit();//等待程序执行完退出进程  
                proc.Close();//结束 
                return rs;
            }
            #endregion

        }
        public static string ExecuteCmd(this string cmdStr)
        {
            #region NETCore调用Shell
            Process CmdProcess = new Process();
            CmdProcess.StartInfo.FileName = "cmd.exe";

            CmdProcess.StartInfo.CreateNoWindow = true;         // 不创建新窗口    
            CmdProcess.StartInfo.UseShellExecute = false;       //不启用shell启动进程  
            CmdProcess.StartInfo.RedirectStandardInput = true;  // 重定向输入    
            CmdProcess.StartInfo.RedirectStandardOutput = true; // 重定向标准输出    
            CmdProcess.StartInfo.RedirectStandardError = true;  // 重定向错误输出  
            CmdProcess.Start();//执行  
            CmdProcess.StandardInput.WriteLine(cmdStr);
            CmdProcess.StandardInput.WriteLine("exit");
           string result = CmdProcess.StandardOutput.ReadToEnd();//获取返回值  
             CmdProcess.WaitForExit();//等待程序执行完退出进程  
            CmdProcess.Close();//结束 
           return  result;
            #endregion

        }

        public static List<string> ExecutesCmd(this string cmdStr)
        {
            List<string> rs =new  List<string>();
            #region NETCore调用Shell
            Process CmdProcess = new Process();
            CmdProcess.StartInfo.FileName = "cmd.exe";

            CmdProcess.StartInfo.CreateNoWindow = true;         // 不创建新窗口    
            CmdProcess.StartInfo.UseShellExecute = false;       //不启用shell启动进程  
            CmdProcess.StartInfo.RedirectStandardInput = true;  // 重定向输入    
            CmdProcess.StartInfo.RedirectStandardOutput = true; // 重定向标准输出    
            CmdProcess.StartInfo.RedirectStandardError = true;  // 重定向错误输出  
            CmdProcess.Start();//执行  
            CmdProcess.StandardInput.WriteLine(cmdStr);
            CmdProcess.StandardInput.WriteLine("exit");
            //开始读取
            using (var sr = CmdProcess.StandardOutput)
            {
                while (!sr.EndOfStream)
                {
                   rs.Add(sr.ReadLine());
                }

                if (!CmdProcess.HasExited)
                {
                    CmdProcess.Kill();
                }
            }
            CmdProcess.WaitForExit();//等待程序执行完退出进程  
            CmdProcess.Close();//结束 
            return rs;
            #endregion

        }

        public static List<string> GetMACSExecuteCmd(this string cmdStr)
        {
            cmdStr = "ipconfig/all";
            List<string> rs = new List<string>();
            #region NETCore调用Shell
            Process CmdProcess = new Process();
            CmdProcess.StartInfo.FileName = "cmd.exe";

            CmdProcess.StartInfo.CreateNoWindow = true;         // 不创建新窗口    
            CmdProcess.StartInfo.UseShellExecute = false;       //不启用shell启动进程  
            CmdProcess.StartInfo.RedirectStandardInput = true;  // 重定向输入    
            CmdProcess.StartInfo.RedirectStandardOutput = true; // 重定向标准输出    
            CmdProcess.StartInfo.RedirectStandardError = true;  // 重定向错误输出  
            CmdProcess.Start();//执行  
            CmdProcess.StandardInput.WriteLine(cmdStr);
            CmdProcess.StandardInput.WriteLine("exit");
            //开始读取
            using (var sr = CmdProcess.StandardOutput)
            {
                while (!sr.EndOfStream)
                {
                    string mac = sr.ReadLine();

                    string macs = mac;
                    if (mac == null) continue;
                    if (mac.Trim().Length == 0) continue;
                    if (mac.IndexOf(":") < 1) continue;
                    if (mac.IndexOf("-") < 1) continue;
                    mac = mac.Split(":")[1];
                    if (mac == null)
                    {
                        rs.Add(macs);
                        continue;
                    }
                    if (mac.Trim().Length == 0)
                    {
                        rs.Add(macs);
                        continue;
                    }
                    if (mac.Split("-").Length == 6)
                    {
                        rs.Add(macs);
                    }
                   
                }

                if (!CmdProcess.HasExited)
                {
                    CmdProcess.Kill();
                }
            }
            CmdProcess.WaitForExit();//等待程序执行完退出进程  
            CmdProcess.Close();//结束 
            return rs;
            #endregion

        }

        public static List<string> GetIPv4sExecuteCmd(this string cmdStr)
        {
            cmdStr = "ipconfig/all";
            List<string> rs = new List<string>();
            #region NETCore调用Shell
            Process CmdProcess = new Process();
            CmdProcess.StartInfo.FileName = "cmd.exe";

            CmdProcess.StartInfo.CreateNoWindow = true;         // 不创建新窗口    
            CmdProcess.StartInfo.UseShellExecute = false;       //不启用shell启动进程  
            CmdProcess.StartInfo.RedirectStandardInput = true;  // 重定向输入    
            CmdProcess.StartInfo.RedirectStandardOutput = true; // 重定向标准输出    
            CmdProcess.StartInfo.RedirectStandardError = true;  // 重定向错误输出  
            CmdProcess.Start();//执行  
            CmdProcess.StandardInput.WriteLine(cmdStr);
            CmdProcess.StandardInput.WriteLine("exit");
            //开始读取
            using (var sr = CmdProcess.StandardOutput)
            {
                while (!sr.EndOfStream)
                {
                    string mac = sr.ReadLine();

                    string macs = mac;
                    if (mac == null) continue;
                    if (mac.Trim().Length == 0) continue;
                    if (mac.IndexOf(":") < 1) continue;
                    if (mac.IndexOf(".") < 1) continue;
                    if (mac.ToLower().IndexOf("ipv4") < 1) continue;
                    mac = mac.Split(":")[1];
                    if(mac==null)
                    {
                        rs.Add(macs);
                        continue;
                    }
                    if (mac.Trim().Length == 0)
                    {
                        rs.Add(macs);
                        continue;
                    }
                    if (mac.Split(".").Length == 4)
                    {
                        rs.Add(macs);
                    }

                }

                if (!CmdProcess.HasExited)
                {
                    CmdProcess.Kill();
                }
            }
            CmdProcess.WaitForExit();//等待程序执行完退出进程  
            CmdProcess.Close();//结束 
            return rs;
            #endregion

        }
        public static List<string> GetIPv6sExecuteCmd(this string cmdStr)
        {
            cmdStr = "ipconfig/all";
            List<string> rs = new List<string>();
            #region NETCore调用Shell
            Process CmdProcess = new Process();
            CmdProcess.StartInfo.FileName = "cmd.exe";

            CmdProcess.StartInfo.CreateNoWindow = true;         // 不创建新窗口    
            CmdProcess.StartInfo.UseShellExecute = false;       //不启用shell启动进程  
            CmdProcess.StartInfo.RedirectStandardInput = true;  // 重定向输入    
            CmdProcess.StartInfo.RedirectStandardOutput = true; // 重定向标准输出    
            CmdProcess.StartInfo.RedirectStandardError = true;  // 重定向错误输出  
            CmdProcess.Start();//执行  
            CmdProcess.StandardInput.WriteLine(cmdStr);
            CmdProcess.StandardInput.WriteLine("exit");
            //开始读取
            using (var sr = CmdProcess.StandardOutput)
            {
                while (!sr.EndOfStream)
                {
                    string mac = sr.ReadLine();

                    string macs = mac;
                    if (mac == null) continue;
                    if (mac.Trim().Length == 0) continue;
                    if (mac.IndexOf(":") < 1) continue;
                    if (mac.IndexOf(".") < 1) continue;
                    if (mac.ToLower().IndexOf("ipv6") < 1) continue;
                    mac = mac.Split(":")[1];
                    if (mac == null)
                    {
                        rs.Add(macs);
                        continue;
                    }
                    if (mac.Trim().Length == 0)
                    {
                        rs.Add(macs);
                        continue;
                    }
                    if (mac.Split(".").Length == 4)
                    {
                        rs.Add(macs);
                    }

                }

                if (!CmdProcess.HasExited)
                {
                    CmdProcess.Kill();
                }
            }
            CmdProcess.WaitForExit();//等待程序执行完退出进程  
            CmdProcess.Close();//结束 
            return rs;
            #endregion

        }
        public static string getIP(this string cmdResultStr)
        {
            if (cmdResultStr == null) return "";
            if (cmdResultStr.Trim().Length==0) return "";
            if (cmdResultStr.IndexOf(":") > 0)
            {
                string ip = cmdResultStr.Split(":")[1];

                if (ip.Split(".").Length == 4)
                {
                    return ip.Split("(")[0];
                }
                else
                {
                    return "";
                }
            }
            else
            {
                if (cmdResultStr.Split(".").Length == 4)
                {
                    return cmdResultStr.Split("(")[0];
                }
                else
                {
                    return "";
                }
            }
        }
       
    }
}

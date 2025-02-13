using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Threading.Tasks;

namespace METU.Client
{
    class Program
    {
        
        static async Task   Main(string[] args)
        {  
            RegClassesRootHelper.CreateProtocol();
            while (true)
            {
                if (args.Length > 0)
                {
                    for (int i = 0; i < args.Length; i++)
                    {
                        Console.WriteLine(args[0]);
                    }
                }

                string cmdStr = "ipconfig/all".ExecuteCmd();
                Console.WriteLine(cmdStr);
                Console.WriteLine("Hello World!");
            }
        }
         
    }
   
}

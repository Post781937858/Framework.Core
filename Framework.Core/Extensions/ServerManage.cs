using Framework.Core.Common;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

namespace Framework.Core
{
    public class ServerManage
    {
        private readonly IWebHostEnvironment environment;

        public ServerManage(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        /// <summary>
        /// 核心数
        /// </summary>
        public int ProcessorCount { get { return Environment.ProcessorCount; } }

        /// <summary>
        /// 服务器名称
        /// </summary>
        public string MachineName { get { return Environment.MachineName; } }

        /// <summary>
        /// 操作系统
        /// </summary>
        public string OSDescription { get { return System.Runtime.InteropServices.RuntimeInformation.OSDescription; } }

        /// <summary>
        /// 局域网IP
        /// </summary>
        public string LanIp
        {
            get
            {
                foreach (var hostAddress in Dns.GetHostAddresses(Dns.GetHostName()))
                {
                    if (hostAddress.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return hostAddress.ToString();
                    }
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 系统架构
        /// </summary>
        public string OSArchitecture
        {
            get
            {
                return System.Runtime.InteropServices.RuntimeInformation.OSArchitecture.ToString();
            }
        }

        /// <summary>
        /// 内存占用
        /// </summary>
        public string MemoryUsage { get { return ((Double)System.Diagnostics.Process.GetCurrentProcess().WorkingSet64 / 1048576).ToString("N2") + " MB"; } }

        /// <summary>
        /// 环境变量
        /// </summary>
        public string EnvironmentName { get { return environment.EnvironmentName; } }


        /// <summary>
        /// ContentRootPath
        /// </summary>
        public string ContentRootPath { get { return environment.ContentRootPath; } }


        /// <summary>
        /// WebRootPath
        /// </summary>
        public string WebRootPath { get { return environment.WebRootPath; } }


        /// <summary>
        /// .NET Core版本
        /// </summary>
        public string FrameworkDescription { get { return System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription; } }

        /// <summary>
        /// 启动时间
        /// </summary>
        public string StartTime { get { return System.Diagnostics.Process.GetCurrentProcess().StartTime.ToString("yyyy-MM-dd HH:mm:ss"); } }

    }

    public class ServerStatus
    {
        /// <summary>
        /// CPU使用率
        /// </summary>
        public string CPURate { get { return ComputerHelper.GetComputerInfo().CPURate; } }

        /// <summary>
        /// 总内存
        /// </summary>
        public string TotalRAM { get { return ComputerHelper.GetComputerInfo().TotalRAM; } }

        /// <summary>
        /// 内存使用率
        /// </summary>
        public string RAMRate { get { return ComputerHelper.GetComputerInfo().RAMRate; } }

        /// <summary>
        /// 系统运行时间
        /// </summary>
        public string RunTime { get { return ComputerHelper.GetComputerInfo().RunTime; } }
    }


    public class ComputerHelper
    {
        public static ComputerInfo GetComputerInfo()
        {
            ComputerInfo computerInfo = new ComputerInfo();
            try
            {
                MemoryMetricsClient client = new MemoryMetricsClient();
                MemoryMetrics memoryMetrics = client.GetMetrics();
                computerInfo.TotalRAM = Math.Ceiling(memoryMetrics.Total / 1024).ToString() + " GB";
                computerInfo.RAMRate = Math.Ceiling(100 * memoryMetrics.Used / memoryMetrics.Total).ToString() + " %";
                computerInfo.CPURate = Math.Ceiling(GetCPURate().ToDouble()) + " %";
                computerInfo.RunTime = GetRunTime();
            }
            catch (Exception) { }
            return computerInfo;
        }

        public static bool IsUnix()
        {
            var isUnix = RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            return isUnix;
        }

        public static string GetCPURate()
        {
            string cpuRate = string.Empty;
            if (IsUnix())
            {
                string output = ShellHelper.Bash("top -b -n1 | grep \"Cpu(s)\" | awk '{print $2 + $4}'");
                cpuRate = output.Trim();
            }
            else
            {
                string output = ShellHelper.Cmd("wmic", "cpu get LoadPercentage");
                cpuRate = output.Replace("LoadPercentage", string.Empty).Trim();
            }
            return cpuRate;
        }

        public static string GetRunTime()
        {
            string runTime = string.Empty;
            try
            {
                if (IsUnix())
                {
                    string output = ShellHelper.Bash("uptime -s");
                    output = output.Trim();
                    runTime = DateTimeHelper.FormatTime(long.Parse((DateTime.Now - ParseToDateTime(output)).TotalMilliseconds.ToString().Split('.')[0]));
                }
                else
                {
                    string output = ShellHelper.Cmd("wmic", "OS get LastBootUpTime/Value");
                    string[] outputArr = output.Split("=", StringSplitOptions.RemoveEmptyEntries);
                    if (outputArr.Length == 2)
                    {
                        runTime = DateTimeHelper.FormatTime(long.Parse((DateTime.Now - ParseToDateTime(outputArr[1].Split('.')[0])).TotalMilliseconds.ToString().Split('.')[0]));
                    }
                }
            }
            catch (Exception) { }
            return runTime;
        }

        /// <summary>
        /// 将string转换为DateTime，若转换失败，则返回日期最小值。不抛出异常。  
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime ParseToDateTime(string str)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    return DateTime.MinValue;
                }
                if (str.Contains("-") || str.Contains("/"))
                {
                    return DateTime.Parse(str);
                }
                else
                {
                    int length = str.Length;
                    switch (length)
                    {
                        case 4:
                            return DateTime.ParseExact(str, "yyyy", System.Globalization.CultureInfo.CurrentCulture);
                        case 6:
                            return DateTime.ParseExact(str, "yyyyMM", System.Globalization.CultureInfo.CurrentCulture);
                        case 8:
                            return DateTime.ParseExact(str, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                        case 10:
                            return DateTime.ParseExact(str, "yyyyMMddHH", System.Globalization.CultureInfo.CurrentCulture);
                        case 12:
                            return DateTime.ParseExact(str, "yyyyMMddHHmm", System.Globalization.CultureInfo.CurrentCulture);
                        case 14:
                            return DateTime.ParseExact(str, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                        default:
                            return DateTime.ParseExact(str, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                    }
                }
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
    }


    public class MemoryMetrics
    {
        public double Total { get; set; }
        public double Used { get; set; }
        public double Free { get; set; }
    }

    public class MemoryMetricsClient
    {
        public MemoryMetrics GetMetrics()
        {
            if (ComputerHelper.IsUnix())
            {
                return GetUnixMetrics();
            }
            return GetWindowsMetrics();
        }

        private MemoryMetrics GetWindowsMetrics()
        {
            string output = ShellHelper.Cmd("wmic", "OS get FreePhysicalMemory,TotalVisibleMemorySize /Value");

            var lines = output.Trim().Split("\n");
            var freeMemoryParts = lines[0].Split("=", StringSplitOptions.RemoveEmptyEntries);
            var totalMemoryParts = lines[1].Split("=", StringSplitOptions.RemoveEmptyEntries);

            var metrics = new MemoryMetrics();
            metrics.Total = Math.Round(double.Parse(totalMemoryParts[1]) / 1024, 0);
            metrics.Free = Math.Round(double.Parse(freeMemoryParts[1]) / 1024, 0);
            metrics.Used = metrics.Total - metrics.Free;

            return metrics;
        }

        private MemoryMetrics GetUnixMetrics()
        {
            string output = ShellHelper.Bash("free -m");

            var lines = output.Split("\n");
            var memory = lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);

            var metrics = new MemoryMetrics();
            metrics.Total = double.Parse(memory[1]);
            metrics.Used = double.Parse(memory[2]);
            metrics.Free = double.Parse(memory[3]);

            return metrics;
        }
    }

    public class ComputerInfo
    {
        /// <summary>
        /// CPU使用率
        /// </summary>
        public string CPURate { get; set; }
        /// <summary>
        /// 总内存
        /// </summary>
        public string TotalRAM { get; set; }
        /// <summary>
        /// 内存使用率
        /// </summary>
        public string RAMRate { get; set; }
        /// <summary>
        /// 系统运行时间
        /// </summary>
        public string RunTime { get; set; }
    }

}

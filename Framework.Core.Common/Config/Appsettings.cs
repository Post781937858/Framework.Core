
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace Framework.Core.Common
{
    /// <summary>
    /// appsettings.json操作类
    /// </summary>
    public class Appsettings
    {
        private static IConfiguration Configuration { get; set; }

        public Appsettings(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        /// <summary>
        /// 封装要操作的字符
        /// </summary>
        /// <param name="sections">节点配置</param>
        /// <returns></returns>
        public static string app(params string[] sections)
        {
            try
            {
                if (sections.Any())
                {
                    return Configuration[string.Join(":", sections)];
                }
            }
            catch (Exception) { }

            return "";
        }
    }
}

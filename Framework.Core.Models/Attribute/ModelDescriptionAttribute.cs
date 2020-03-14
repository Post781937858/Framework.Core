using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Core.Models
{
    /// <summary>
    /// 模型注释
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class ModelDescriptionAttribute:Attribute
    {
        public string Description { get; set; }
    }
}

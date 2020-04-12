using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Core.Models
{
    [ModelDescription(Description = "API请求日志模型")]
    public class ApiRequestLog : RootEntity
    {
        [SugarColumn(IsNullable = true, ColumnDescription = "请求路径")]
        public string path { get; set; }


        [SugarColumn(IsNullable = true, ColumnDescription = "请求路径")]
        public string method { get; set; }
        
        [SugarColumn(IsNullable = true, ColumnDescription = "请求时间")]
        public DateTime requestTime { get; set; }


        [SugarColumn(IsNullable = true, ColumnDescription = "URL参数")]
        public string Urlparameter { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "请求参数")]
        public string FormDataparameter { get; set; }


        [SugarColumn(IsNullable = true, ColumnDescription = "请求状态")]
        public Requeststate state { get; set; }


        [SugarColumn(Length = 20000, IsNullable = true, ColumnDescription = "响应数据")]
        public string ResponseData { get; set; }
        

        [SugarColumn(IsNullable = true, ColumnDescription = "耗时")]
        public long consumingTime { get; set; }


        [SugarColumn(IsNullable = true, ColumnDescription = "请求用户")]
        public string userName { get; set; }
    }


    public enum Requeststate
    {
        succeed = 1,
        error = 2
    }
}

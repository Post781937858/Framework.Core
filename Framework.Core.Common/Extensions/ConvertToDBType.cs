using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Core.Common
{
    public class ConvertToDBType
    {
        public static string SqlServerChangeDBTypeToCSharpType(string type)
        {
            string reval = string.Empty;
            switch (type.ToLower())
            {
                case "int":
                    reval = "int";
                    break;
                case "text":
                    reval = "string";
                    break;
                case "bigint":
                    reval = "long";
                    break;
                case "binary":
                    reval = "object";
                    break;
                case "bit":
                    reval = "bool";
                    break;
                case "char":
                    reval = "string";
                    break;
                case "datetime":
                    reval = "DateTime";
                    break;
                case "decimal":
                    reval = "decimal";
                    break;
                case "float":
                    reval = "double";
                    break;
                case "image":
                    reval = "byte[]";
                    break;
                case "money":
                    reval = "decimal";
                    break;
                case "nchar":
                    reval = "string";
                    break;
                case "ntext":
                    reval = "string";
                    break;
                case "numeric":
                    reval = "decimal";
                    break;
                case "nvarchar":
                    reval = "string";
                    break;
                case "real":
                    reval = "float";
                    break;
                case "smalldatetime":
                    reval = "DateTime";
                    break;
                case "smallint":
                    reval = "short";
                    break;
                case "smallmoney":
                    reval = "decimal";
                    break;
                case "timestamp":
                    reval = "System.DateTime";
                    break;
                case "tinyint":
                    reval = "byte[]";
                    break;
                case "uniqueidentifier":
                    reval = "Guid";
                    break;
                case "varbinary":
                    reval = "byte[]";
                    break;
                case "varchar":
                    reval = "string";
                    break;
                case "Variant":
                    reval = "object";
                    break;
                default:
                    reval = "string";
                    break;
            }
            return reval;
        }


        public static string MysqlChangeDBTypeToCSharpType(string type)
        {
            string reval = string.Empty;
            switch (type.ToLower())
            {
                case "int"://
                    reval = "int";
                    break;
                case "text": //
                    reval = "string";
                    break;
                case "bigint": //
                    reval = "long";
                    break;
                case "binary": //
                    reval = "object";
                    break;
                case "tinyint": //
                    reval = "bool";
                    break;
                case "char": //
                    reval = "string";
                    break;
                case "datetime": //
                    reval = "DateTime";
                    break;
                case "decimal": //
                    reval = "decimal";
                    break;
                case "float": //
                    reval = "double";
                    break;
                case "varchar": //
                    reval = "string";
                    break;
                case "time": //
                    reval = "DateTime";
                    break;
                case "smallint": //
                    reval = "short";
                    break;
                case "timestamp":  //
                    reval = "System.DateTime";
                    break;
                case "uniqueidentifier":
                    reval = "Guid";
                    break;
                case "varbinary": //
                    reval = "byte[]";
                    break;
                default:
                    reval = "string";
                    break;
            }
            return reval;
        }
    }
}

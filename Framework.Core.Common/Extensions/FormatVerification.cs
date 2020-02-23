using System;
using System.Text.RegularExpressions;

namespace Framework.Core.Common
{
    public static class FormatVerification
    {
        /// <summary>
        /// 匹配是否为数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsFloat(string str)
        {
            string regextext = @"^(-?\d+)(\.\d+)?$";
            Regex regex = new Regex(regextext, RegexOptions.None);
            return regex.IsMatch(str.Trim());
        }

        /// <summary>
        /// 判断字符串中是否包含中文
        /// </summary>
        /// <param name="str">需要判断的字符串</param>
        /// <returns>判断结果</returns>
        public static bool HasChinese(string str)
        {
            return Regex.IsMatch(str, @"[\u4e00-\u9fa5]|[a-zA-Z]");
        }

        /// <summary>
        /// 匹配是否为IP
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool HasIP(string str)
        {
            return Regex.IsMatch(str, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
        /// <summary>
        /// 匹配是否为小数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool Strfloat(string str)
        {
            return Regex.IsMatch(str, "^([0-9]{1,}[.][0-9]*)$");
        }

        /// <summary>
        /// string转int
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int TransformInt(this string obj)
        {
            if (string.IsNullOrWhiteSpace(obj) || !IsFloat(obj))
                return 0;
            else
                return Convert.ToInt32(obj);
        }


        /// <summary>
        /// 对象值复制到另一个对象
        /// </summary>
        public static T ObjectAssignment<T>(this object thisobj) where T : class
        {
            Type type = typeof(T);
            Type tagType = thisobj.GetType();
            object obj = Activator.CreateInstance(type);
            foreach (var item in type.GetProperties())
            {
                item.SetValue(obj, tagType.GetProperty(item.Name).GetValue(thisobj));
            }
            return (T)obj;
        }


        /// <summary>
        /// 通过枚举，获得其枚举值
        /// </summary>
        /// <param name="enumInstance"></param>
        /// <returns></returns>
        public static string GetValue<T>(T enumInstance)
        {
            return enumInstance.ToString();
        }
    }
}

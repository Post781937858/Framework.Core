using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Framework.Core.Common
{
    public class TemplateEngine
    {
        public string m_noMarkers = "", m_error = "";
        public bool m_ifEndErr = false;
        public Dictionary<string, string> m_valueList = new Dictionary<string, string>();
        public Dictionary<string, string> m_blockList = new Dictionary<string, string>();

        /// <summary>
        /// 设置处理区块
        /// </summary>
        /// <param name="p_templateName">模板名称</param>
        /// <param name="p_blockTag">区块标签</param>
        /// <param name="p_blockName">区块名称</param>
        /// <param name="p_pattern"><!--\\s+BEGIN " + p_blockTag + "\\s+-->([\\s\\S.]*)<!--\\s+END " + p_blockTag + "\\s+--></param>
        public void SetBlock(string p_templateName, string p_blockTag, string p_blockName, string p_pattern)
        {
            MatchCollection mc = Regex.Matches(m_blockList[p_templateName], p_pattern, RegexOptions.IgnoreCase);
            foreach (Match m in mc)
            {
                m_blockList.Add(p_blockTag, m.Groups[1].Value);
                m_blockList[p_templateName] = m_blockList[p_templateName].Replace(m.Value, "{" + p_blockName + "}");
            }
           
        }

        /// <summary>
        /// 设置模板文件
        /// </summary>
        /// <param name="p_templateName">模板名称</param>
        /// <param name="p_file">模板路径</param>
        public void SetFile(string p_templateName, string p_file)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            if (System.IO.File.Exists(p_file))
            {
                m_blockList.Add(p_templateName, System.IO.File.ReadAllText(p_file, Encoding.GetEncoding("gb2312")));
            }
            else
            {
                m_blockList.Add(p_templateName, System.IO.File.ReadAllText(p_file, Encoding.GetEncoding("gb2312")));
            }
        }

        /// <summary>
        /// 设置单项值
        /// </summary>
        /// <param name="p_tags">单项标签</param>
        /// <param name="p_tagsValue">单项值</param>
        /// <param name="p_append">追加或替换</param>
        public void SetVal(string p_tags, string p_tagsValue, bool p_append)
        {
            if (string.IsNullOrEmpty(p_tagsValue))
                p_tagsValue = "";

            if (m_valueList.ContainsKey(p_tags))
            {
                if (p_append)
                    m_valueList[p_tags] = m_valueList[p_tags] + p_tagsValue;
                else
                    m_valueList[p_tags] = p_tagsValue;
            }
            else
                m_valueList.Add(p_tags, p_tagsValue);
        }

        /// <summary>
        /// 移除单项值
        /// </summary>
        /// <param name="p_tags">单项标签</param>
        public void RemoveVal(string p_tags)
        {
            if (m_valueList.ContainsKey(p_tags))
                m_valueList.Remove(p_tags);
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="p_tags">单项标签</param>
        /// <param name="p_blockTags"></param>
        /// <param name="p_append"></param>
        public void Parse(string p_tags, string p_blockTags, bool p_append)
        {
            if (!m_blockList.ContainsKey(p_blockTags))
                ShowError("未指定的块标记");
            if (m_valueList.ContainsKey(p_tags))
            {
                if (p_append)
                    m_valueList[p_tags] = m_valueList[p_tags] + InstanceValue(p_blockTags);
                else
                    m_valueList[p_tags] = InstanceValue(p_blockTags);
            }
            else
                m_valueList.Add(p_tags, InstanceValue(p_blockTags));
        }

        /// <summary>
        /// 实例
        /// </summary>
        /// <param name="p_blockTags"></param>
        public string InstanceValue(string p_blockTags)
        {
            string instanceValue = m_blockList[p_blockTags];
            Dictionary<string, string>.KeyCollection keys = m_valueList.Keys;
            foreach (string tags in keys)
                instanceValue = instanceValue.Replace("{" + tags + "}", m_valueList[tags]);
            return instanceValue;
        }

        /// <summary>
        /// 去除块后缀
        /// </summary>
        /// <param name="p_tags"></param>
        /// <param name="p_str"></param>
        public void RemoveSuffix(string p_tags, string p_str)
        {
            string str = "";
            if (m_valueList.ContainsKey(p_tags))
            {
                str = m_valueList[p_tags];

                if (str.Substring(str.Length - p_str.Length) == p_str)
                {
                    str = str.Substring(0, str.Length - p_str.Length);
                }

                m_valueList[p_tags] = str;
            }
        }

        /// <summary>
        /// 输出页面代码
        /// </summary>
        /// <param name="p_tags"></param>
        /// <returns></returns>
        public string PutOutPageCode(string p_tags)
        {
            if (!m_valueList.ContainsKey(p_tags))
                ShowError("不存在的标记" + p_tags);
            return Finish(m_valueList[p_tags]);
        }

        /// <summary>
        /// 处理错误信息
        /// </summary>
        /// <param name="p_msg"></param>
        public void ShowError(string p_msg)
        {
            m_error = p_msg;
            //MessageBox.Write("<font color=\"red\" style=\"font-size;14px\"><b>模板错误：" + m_error + "</b></font><br>");
            //if (m_ifEndErr)
            //    HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 设置对未指定的标记的处理方式，有keep、remove、comment三种
        /// </summary>
        /// <param name="p_noMarkers"></param>
        public void SetNoMarkers(string p_noMarkers)
        {
            m_noMarkers = p_noMarkers;
        }

        /// <summary>
        /// 移除全部
        /// </summary>
        public void RemoveAll()
        {
            m_valueList.Clear();
            m_blockList.Clear();
        }

        /// <summary>
        /// 完成
        /// </summary>
        /// <param name="p_content"></param>
        /// <returns></returns>
        public string Finish(string p_content)
        {
            string strFinish = "";
            switch (m_noMarkers)
            {
                case "keep":
                    strFinish = p_content;
                    break;
                case "remove":
                    strFinish = RegexReplace2(p_content, @"{[^ \t\r\n}]+}", "");//右括号前面不是换行，tab的进行替换。
                    break;
                case "comment":
                    strFinish = RegexReplace2(p_content, @"{([^ \t\r\n}]+)}", "<!-- Template Variable $1 undefined -->");//右括号前面不是换行，tab的进行替换，变量未定义。
                    break;
                default:
                    strFinish = p_content;
                    break;
            }
            return strFinish;
        }

        /// <summary>
        /// 正则表达式进行忽略大小写的字符串替换2
        /// </summary>
        /// <param name="p_cont"></param>
        /// <param name="p_old"></param>
        /// <param name="p_new"></param>
        /// <returns></returns>
        public static string RegexReplace2(string p_cont, string p_old, string p_new)
        {
            try
            {
                return System.Text.RegularExpressions.Regex.Replace(p_cont, p_old, p_new);
            }
            catch (Exception exc) { throw new Exception(exc.Message + "[ND.Tool.Common.RegexReplace2]"); }
        }
    }
}
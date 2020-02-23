using LZ4;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Core.Common
{
    public static class LZ4Compress
    {  
        /// <summary>
       /// 压缩文本
       /// </summary>
       /// <param name="text">文本内容</param>
       /// <returns></returns>
        public static string CompressBuffer(this string text)
        {
            var compressed = Convert.ToBase64String(
                LZ4Codec.Wrap(Encoding.UTF8.GetBytes(text)));
            return compressed;
        }

        /// <summary>
        /// 解压文本
        /// </summary>
        /// <param name="compressed">压缩的文本</param>
        /// <returns></returns>
        public static string DecompressBuffer(this string compressed)
        {
            var lorems =
                Encoding.UTF8.GetString(
                    LZ4Codec.Unwrap(Convert.FromBase64String(compressed)));
            return lorems;
        }
    }
}

/*----------------------------------------------------------------
    Daniel.Zhang
    
    文件名：PrefixForCharToHex.cs
    文件功能描述：字符转换成十六进制时前缀的类型

----------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wl.WordsAnalyzer
{
    public enum PrefixForCharToHex
    {
        /// <summary>
        /// 不加前缀
        /// </summary>
        None = 0,

        /// <summary>
        /// 常规，比如 0x3f
        /// </summary>
        Normal = 10,

        /// <summary>
        /// 用于转码 比如\x3f
        /// </summary>
        Escape = 20,

        /// <summary>
        /// 用于lucene 分词中的单词转义 
        /// 前缀加symbolx
        /// </summary>
        LuceneNormal = 30,

        /// <summary>
        /// 用于lucene 分词中的单词转义 
        /// 前缀加symbolx
        /// </summary>
        LuceneEscape = 40
    }
}

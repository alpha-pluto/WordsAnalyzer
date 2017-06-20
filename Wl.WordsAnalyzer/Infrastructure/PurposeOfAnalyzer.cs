/*----------------------------------------------------------------
    Weilan
    
    文件名：PurposeOfAnalyzer.cs
    文件功能描述：分析器 目的 归类 

----------------------------------------------------------------*/
using System;

namespace Wl.WordsAnalyzer
{
    public enum PurposeOfAnalyzer
    {
        /// <summary>
        /// 标准分析器
        /// </summary>
        Standard = 1,

        /// <summary>
        /// 简单分析器
        /// </summary>
        Simple = 2,

        /// <summary>
        /// 停止词分析器
        /// </summary>
        Stop = 4,

        /// <summary>
        /// 空格分析器
        /// </summary>
        Whitespace = 8,

        /// <summary>
        /// 单数分析器
        /// </summary>
        Singular = 16,

        /// <summary>
        /// 同义词分析器
        /// </summary>
        Synonym = 32,

        /// <summary>
        /// 盘古分析器
        /// </summary>
        Pangu = 64
    }
}

/*----------------------------------------------------------------
    Daniel.Zhang
    
    文件名：IAnalysis.cs
    文件功能描述：各分析器 在要实现的接口

----------------------------------------------------------------*/
using System;
using System.Collections.Generic;


namespace Wl.WordsAnalyzer
{
    public interface IAnalysis
    {
        /// <summary>
        /// 通过输入的文本，获取分析后的文本
        /// </summary>
        /// <param name="inString"></param>
        /// <returns></returns>
        IEnumerable<string> GetAnalyzedWords(string inString);
    }
}

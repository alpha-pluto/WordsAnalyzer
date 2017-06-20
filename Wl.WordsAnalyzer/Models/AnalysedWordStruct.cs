﻿/*----------------------------------------------------------------
    Weilan
    
    文件名：AnalysedWordStruct.cs
    文件功能描述：分析结果的实体

----------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
namespace Wl.WordsAnalyzer.Models
{
    [Serializable]
    [XmlRoot("AnalysedWordStruct")]
    public class AnalysedWordStruct
    {
        /// <summary>
        /// 源词语
        /// </summary>
        public string OriginalWord { get; set; }

        /// <summary>
        /// 变成单数后的单词
        /// </summary>
        public string SingularizedWord { get; set; }

        /// <summary>
        /// 用源单词查出的同义词集合
        /// </summary>
        public IEnumerable<string> SynonymsOfOriginalWord { get; set; }

        /// <summary>
        /// 用单数化后的单词查出的同义词集合
        /// </summary>
        public IEnumerable<string> SynonymsOfSingularizedWord { get; set; }

        public AnalysedWordStruct()
        {
            OriginalWord = string.Empty;
            SingularizedWord = string.Empty;

            SynonymsOfOriginalWord = new List<string>();
            SynonymsOfSingularizedWord = new List<string>();
        }

    }
}

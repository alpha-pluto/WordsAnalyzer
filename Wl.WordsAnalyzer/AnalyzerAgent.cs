using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wl.WordsAnalyzer.Extension;

namespace Wl.WordsAnalyzer
{
    public class AnalyzerAgent
    {
        #region parameter

        private Wl.WordsAnalyzer.AnalyzerContainer<Lucene.Net.Analysis.WhitespaceAnalyzer> analyzerWords;

        private Wl.WordsAnalyzer.AnalyzerContainer<Wl.WordsAnalyzer.SingularAnalyzer> analyzerSingular;

        private Wl.WordsAnalyzer.AnalyzerContainer<Wl.WordsAnalyzer.SynonymAnalyzer> analyzerSynonym;

        #endregion

        #region constructor

        public AnalyzerAgent()
        {
            analyzerWords = new AnalyzerContainer<Lucene.Net.Analysis.WhitespaceAnalyzer>();

            analyzerSingular = new AnalyzerContainer<SingularAnalyzer>(true);

            analyzerSynonym = new Wl.WordsAnalyzer.AnalyzerContainer<Wl.WordsAnalyzer.SynonymAnalyzer>(true);
        }

        #endregion

        #region method

        /// <summary>
        /// 分析句子 并 将结果 以 实体格式返回 
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public IList<Wl.WordsAnalyzer.Models.AnalysedWordStruct> SentenceAnalysis(string title)
        {
            var origin = analyzerWords.GetAnalyzedWords(title);

            IList<Wl.WordsAnalyzer.Models.AnalysedWordStruct> lsRetWords = new List<Wl.WordsAnalyzer.Models.AnalysedWordStruct>();

            foreach (string word in origin)
            {
                var singular = analyzerSingular.GetAnalyzedWords(word);
                var synFromWord = new List<string>();
                if (!word.IsSymbolMark())
                {
                    synFromWord = analyzerSynonym.GetAnalyzedWords(word).ToList();
                }
                var synFromSingular = new List<string>();
                if (singular != null && singular.Any())
                {
                    if (!singular.First().IsSymbolMark())
                        synFromSingular = analyzerSynonym.GetAnalyzedWords(singular.First()).ToList();
                }
                lsRetWords.Add(new Wl.WordsAnalyzer.Models.AnalysedWordStruct
                {
                    OriginalWord = word.IsSymbolMark() ? word.ConvertHexToChar().ToString() : word,
                    SingularizedWord = singular.First().IsSymbolMark() ? singular.First().ConvertHexToChar().ToString() : singular.First(),
                    SynonymsOfOriginalWord = synFromWord,
                    SynonymsOfSingularizedWord = synFromSingular
                });

            }

            return lsRetWords;
        }

        /// <summary>
        /// 分析 句子 并 将结果 以 json 格式 返回 
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public string SentenceAnalysisJson(string title)
        {
            var lsRetWords = SentenceAnalysis(title);
            var jsonData = JsonConvert.SerializeObject(lsRetWords);
            return jsonData;
        }

        /// <summary>
        /// 分析 句子 并 将结果 以 json 格式 返回 ,在返回之前 用 自定义的 转码 器 转码 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public string SentenceAnalysisJsonProcessBeforeReturn(string title, Func<string, string> filter = null)
        {
            var lsRetWords = SentenceAnalysis(title);
            var jsonData = JsonConvert.SerializeObject(lsRetWords.AnalysedWordStructProcessBeforeReturn(filter));
            return jsonData;
        }

        /// <summary>
        /// 分析 句子并将结果以简化实体返回 
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public IList<Wl.WordsAnalyzer.Models.AnalysedWordStructSimplified> SentenceAnalysisSimplified(string title)
        {
            var lsRetWords = SentenceAnalysis(title);
            return lsRetWords.SimplifyAnalysedWordStruct();
        }

        /// <summary>
        /// 分析 句子并将结果以简化实体json返回 
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public string SentenceAnalysisSimplifiedJson(string title)
        {
            var lsRetWords = SentenceAnalysisSimplified(title);
            var jsonData = JsonConvert.SerializeObject(lsRetWords);
            return jsonData;
        }

        /// <summary>
        /// 分析 句子并将结果以简化实体json返回,关在输出结果前用自定义的转码器转码
        /// </summary>
        /// <param name="title"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public string SentenceAnalysisSimplifiedJsonProcessBeforeReturn(string title, Func<string, string> filter = null)
        {
            var lsRetWords = SentenceAnalysisSimplified(title);
            var jsonData = JsonConvert.SerializeObject(lsRetWords.SimplifiedWordStructProcessBeforeReturn(filter));
            return jsonData;
        }

        /// <summary>
        /// 分析关键字或是标签（以符号相隔） 找出同义词
        /// </summary>
        /// <param name="keywordsOrTag"></param>
        /// <param name="splitor"></param>
        /// <returns></returns>
        public IList<Wl.WordsAnalyzer.Models.AnalysedWordStructSimplified> KeywordsOrTagsAnalysis(string keywordsOrTag, string splitor = ",")
        {
            string[] keywordsOrTagSplit = System.Text.RegularExpressions.Regex.Split(keywordsOrTag ?? "", splitor);

            IList<Wl.WordsAnalyzer.Models.AnalysedWordStructSimplified> lsRetWords = new List<Wl.WordsAnalyzer.Models.AnalysedWordStructSimplified>();

            foreach (string word in keywordsOrTagSplit)
            {
                var synFromWord = new List<string>();
                if (!word.IsSymbolMark())
                {
                    synFromWord = analyzerSynonym.GetAnalyzedWords(word).ToList();
                }


                lsRetWords.Add(new Wl.WordsAnalyzer.Models.AnalysedWordStructSimplified
                {
                    Word = word,
                    Synonyms = synFromWord
                });

            }

            return lsRetWords;
        }

        /// <summary>
        /// 分析关键字或是标签（以符号相隔） 找出同义词
        /// </summary>
        /// <param name="keywordsOrTag"></param>
        /// <param name="splitor"></param>
        /// <returns></returns>
        public string KeywordsOrTagsAnalysisJson(string keywordsOrTags, string splitor = ",")
        {
            var lsRetWords = KeywordsOrTagsAnalysis(keywordsOrTags, splitor);
            var jsonData = JsonConvert.SerializeObject(lsRetWords);
            return jsonData;
        }

        public string KeywordsOrTagsAnalysisJsonProcessBeforeReturn(string keywordsOrTags, string splitor = ",", Func<string, string> filter = null)
        {
            var lsRetWords = KeywordsOrTagsAnalysis(keywordsOrTags, splitor);
            var jsonData = JsonConvert.SerializeObject(lsRetWords.SimplifiedWordStructProcessBeforeReturn(filter));
            return jsonData;
        }

        #endregion
    }
}

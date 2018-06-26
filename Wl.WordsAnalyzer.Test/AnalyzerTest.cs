using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Newtonsoft.Json;
using Wl.WordsAnalyzer.Extension;
namespace Wl.WordsAnalyzer.Test
{
    [TestClass]
    public class AnalyzerTest
    {
        [TestMethod]
        public void TestAnalyzerStandard()
        {
            var title = @"Anti-broken 360° Full Protection PC Case Cover Tempered Glass Gifts For iPhone 5/5S/SE/6/6S/7 Plus and Android 4.0 work ? very hard or Symbian 3.0 -q ";

            var analyzer = new Wl.WordsAnalyzer.AnalyzerContainer();

            var retTitle = analyzer.GetAnalyzedWords(title);

            Console.WriteLine(string.Join(",", retTitle.ToArray()));
        }

        [TestMethod]
        public void TestAnalyzerGeneral()
        {
            var retDict = new List<Wl.WordsAnalyzer.Models.AnalysedWordStruct>();

            var title = @"Anti-broken 360° Full Protection PC Case Cover Tempered Glass Gifts For iPhone 5/5S/SE/6/6S/7 + Plus and * Android 4.0 work ? very hard or Symbian 3.0 -q ";

            var analyzerWords = new Wl.WordsAnalyzer.AnalyzerContainer<Lucene.Net.Analysis.WhitespaceAnalyzer>();

            var analyzerSingular = new Wl.WordsAnalyzer.AnalyzerContainer<Wl.WordsAnalyzer.SingularAnalyzer>(true);

            var analyzerSynonym = new Wl.WordsAnalyzer.AnalyzerContainer<Wl.WordsAnalyzer.SynonymAnalyzer>(true);

            var origin = analyzerWords.GetAnalyzedWords(title);

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
                retDict.Add(new Wl.WordsAnalyzer.Models.AnalysedWordStruct
                {
                    OriginalWord = word.IsSymbolMark() ? word.ConvertHexToChar().ToString() : word,
                    SingularizedWord = singular.First().IsSymbolMark() ? singular.First().ConvertHexToChar().ToString() : singular.First(),
                    SynonymsOfOriginalWord = synFromWord,
                    SynonymsOfSingularizedWord = synFromSingular
                });

            }

            var jc = JsonConvert.SerializeObject(retDict);

            Console.Write(jc);
        }

        [TestMethod]
        public void TestAnalyzerAgent()
        {
            var aa = new Wl.WordsAnalyzer.AnalyzerAgent();
            var title = @"Anti-broken 360° Full Protection PC Case Cover Tempered Glass Gifts For iPhone 5/5S/SE/6/6S/7 + Plus and * Android 4.0 work ? very hard or Symbian 3.0 -q ";
            var jsd = aa.SentenceAnalysisSimplifiedJson(title);

            Console.WriteLine(jsd);
        }
    }
}

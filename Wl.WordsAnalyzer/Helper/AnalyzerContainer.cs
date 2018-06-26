using Lucene.Net.Analysis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wl.WordsAnalyzer.Extension;
namespace Wl.WordsAnalyzer
{
    public class AnalyzerContainer<TAnalyzer> : AnalyzerContainer where TAnalyzer : Analyzer
    {
        #region parameter

        #endregion

        #region Constructor

        public AnalyzerContainer()
        {
            _analyzer = Activator.CreateInstance<TAnalyzer>();
        }

        public AnalyzerContainer(bool para)
        {
            var luceneVersionStr = ConfigurationManager.AppSettings["LuceneVersion"] ?? "LUCENE_CURRENT";

            _luceneVersion = luceneVersionStr.ParseLuceneVersion();

            _analyzer = (TAnalyzer)Activator.CreateInstance(typeof(TAnalyzer), _luceneVersion);
        }

        #endregion

        #region Interface Implementation

        public override IEnumerable<string> GetAnalyzedWords(string inString)
        {
            return base.GetAnalyzedWords(inString);
        }

        #endregion

    }


    public class AnalyzerContainer : IAnalysis, IDisposable
    {
        #region parameter

        protected Analyzer _analyzer;

        protected Lucene.Net.Util.Version _luceneVersion;

        protected readonly char[] escapeSymbols = new char[] { '+', '-', '*', '?', '(', ')', '{', '}', '[', ']', '!', '^' };

        #endregion

        #region Constructor

        public AnalyzerContainer()
        {
            var luceneVersionStr = ConfigurationManager.AppSettings["LuceneVersion"] ?? "LUCENE_CURRENT";

            _luceneVersion = luceneVersionStr.ParseLuceneVersion();

            _analyzer = new Lucene.Net.Analysis.Standard.StandardAnalyzer(_luceneVersion);
        }

        #endregion

        #region Interface Implementation

        public virtual IEnumerable<string> GetAnalyzedWords(string inString)
        {
            IList<string> retWords = new List<String>();
            var escapedInputString = inString.AnalyzerInputStringAdaptSymbolMark(escapeSymbols);
            System.IO.StringReader reader = new System.IO.StringReader(escapedInputString);
            Lucene.Net.Analysis.TokenStream ts = _analyzer.TokenStream(escapedInputString, reader);
            bool hasNext = ts.IncrementToken();
            Lucene.Net.Analysis.Tokenattributes.ITermAttribute ita;
            while (hasNext)
            {
                ita = ts.GetAttribute<Lucene.Net.Analysis.Tokenattributes.ITermAttribute>();
                retWords.Add(ita.Term);
                hasNext = ts.IncrementToken();
            }
            ts.CloneAttributes();
            reader.Close();
            _analyzer.Close();
            return retWords;
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            if (_analyzer != null)
                ((IDisposable)_analyzer).Dispose();
        }

        #endregion
    }


}

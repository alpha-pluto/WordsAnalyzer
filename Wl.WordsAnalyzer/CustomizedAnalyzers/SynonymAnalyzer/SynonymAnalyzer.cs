/*----------------------------------------------------------------
    Daniel.Zhang
    
    文件名：SynonymAnalyzer.cs
    文件功能描述：同义词分析器

----------------------------------------------------------------*/
using System;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Version = Lucene.Net.Util.Version;
using System.Configuration;

namespace Wl.WordsAnalyzer
{
    public class SynonymAnalyzer : Analyzer, IAnalyzer
    {
        #region parameter

        private Version CURRENT_VERSION;

        public ISynonymEngine SynonymEngine { get; private set; }

        #endregion

        #region constructor

        public SynonymAnalyzer(Version currentVersion)
        {
            var pathSynonymDict = ConfigurationManager.AppSettings["SynonymDictionaryPath"] ?? System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @"/lucene_dict/syn_index");
            CURRENT_VERSION = currentVersion;
            SynonymEngine = new Wl.WordsAnalyzer.WordNetSynonymEngine(pathSynonymDict);
        }

        public SynonymAnalyzer(Version currentVersion, ISynonymEngine engine)
        {
            CURRENT_VERSION = currentVersion;
            SynonymEngine = engine;
        }

        #endregion

        #region override

        public override TokenStream TokenStream(string fieldName, System.IO.TextReader reader)
        {
            //create the tokenizer  
            TokenStream result = new StandardTokenizer(CURRENT_VERSION, reader);

            //add in filters
            //*result = new Lucene.Net.Analysis.Snowball.SnowballFilter(result, new EnglishStemmer());

            //add in filters  
            // first normalize the StandardTokenizer  
            result = new StandardFilter(result);

            // makes sure everything is lower case  
            result = new LowerCaseFilter(result);

            result = new ASCIIFoldingFilter(result);

            // use the default list of Stop Words, provided by the StopAnalyzer class.  
            //-result = new StopFilter(true, result, StopAnalyzer.ENGLISH_STOP_WORDS_SET);
            result = new StopFilter(true, result, StopAnalyzer.ENGLISH_STOP_WORDS_SET);

            // injects the synonyms.   
            result = new SynonymFilter(result, SynonymEngine);

            //return the built token stream.  
            return result;
        }

        #endregion
    }
}

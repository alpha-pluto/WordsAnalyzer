/*----------------------------------------------------------------
    Weilan
    
    文件名：SingularAnalyzer.cs
    文件功能描述：单数化 分析器

----------------------------------------------------------------*/
using System;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using SF.Snowball.Ext;
using System.Collections.Generic;
using Version = Lucene.Net.Util.Version;
namespace Wl.WordsAnalyzer
{
    public class SingularAnalyzer : Analyzer
    {
        #region parameter

        private const Version CURRENT_VERSION = Version.LUCENE_30;

        #endregion

        #region override

        public override TokenStream TokenStream(string fieldName, System.IO.TextReader reader)
        {
            //create the tokenizer  
            TokenStream result = new StandardTokenizer(CURRENT_VERSION, reader);

            //add in filters
            result = new Lucene.Net.Analysis.Snowball.SnowballFilter(result, new EnglishStemmer());

            //add in filters  
            // first normalize the StandardTokenizer  
            //-result = new StandardFilter(result);

            // makes sure everything is lower case  
            result = new LowerCaseFilter(result);

            result = new ASCIIFoldingFilter(result);

            // use the default list of Stop Words, provided by the StopAnalyzer class.  
            //-result = new StopFilter(true, result, StopAnalyzer.ENGLISH_STOP_WORDS_SET);
            result = new StopFilter(true, result, new HashSet<string>());


            //return the built token stream.  
            return result;
        }

        #endregion
    }
}

/*----------------------------------------------------------------
    Daniel.Zhang
    
    文件名：SynonymFilter.cs
    文件功能描述：同义词过滤器

----------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Tokenattributes;

namespace Wl.WordsAnalyzer
{
    public class SynonymFilter : TokenFilter
    {
        #region parameter

        //private Queue<Token> synonymTokenQueue = new Queue<Token>();

        private State currentState;

        private Queue<string> splittedQueue = new Queue<string>();

        private readonly ITermAttribute termAtt;

        private readonly IPositionIncrementAttribute posAtt;


        #endregion

        #region properties

        public ISynonymEngine SynonymEngine { get; private set; }

        #endregion

        #region constructor

        public SynonymFilter(TokenStream input, ISynonymEngine synonymEngine)
            : base(input)
        {
            if (synonymEngine == null)
                throw new ArgumentNullException("synonymEngine");

            SynonymEngine = synonymEngine;
            termAtt = AddAttribute<ITermAttribute>();
            posAtt = AddAttribute<IPositionIncrementAttribute>();
        }

        #endregion

        #region override

        public override bool IncrementToken()
        {
            if (splittedQueue.Count > 0)
            {
                string splitted = splittedQueue.Dequeue();
                RestoreState(currentState);
                termAtt.SetTermBuffer(splitted);
                posAtt.PositionIncrement = 0;
                return true;
            }

            if (!input.IncrementToken())
                return false;

            string currentTerm = termAtt.Term;
            if (currentTerm != null)
            {
                var sb = new StringBuilder();
                var synonyms = SynonymEngine.GetSynonyms(currentTerm);

                if (synonyms == null || synonyms.Any() == false) return true;
                foreach (var synonym in synonyms)
                {
                    splittedQueue.Enqueue(synonym.ToLower());
                }

            }

            currentState = CaptureState();
            return true;
        }

        #endregion

    }
}

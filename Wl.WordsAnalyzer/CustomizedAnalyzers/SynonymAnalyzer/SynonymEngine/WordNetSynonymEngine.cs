/*----------------------------------------------------------------
    Daniel.Zhang
    
    文件名：WordNetSynonymEngine.cs
    文件功能描述：word net 同义词引擎

----------------------------------------------------------------*/
using System;
using System.IO;
using System.Collections.Generic;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Wl.WordsAnalyzer.Extension;
using LuceneDirectory = Lucene.Net.Store.Directory;
using Version = Lucene.Net.Util.Version;
using System.Configuration;

namespace Wl.WordsAnalyzer
{
    public class WordNetSynonymEngine : ISynonymEngine
    {
        #region parameter

        private const int TOP_NUMBER = 1000;

        private Version CURRENT_VERSION;

        private IndexSearcher searcher;

        private Analyzer analyzer;

        #endregion

        #region constructore

        /// <summary>
        /// syn_index_directory 为前面用 Syns2Index 生成的同义词索引目录
        /// </summary>
        /// <param name="syn_index_directory"></param>
        public WordNetSynonymEngine(string syn_index_directory)
        {
            var luceneVersionStr = ConfigurationManager.AppSettings["LuceneVersion"] ?? "LUCENE_CURRENT";
            CURRENT_VERSION = luceneVersionStr.ParseLuceneVersion();
            analyzer = new StandardAnalyzer(CURRENT_VERSION);
            LuceneDirectory indexDir = FSDirectory.Open(new DirectoryInfo(syn_index_directory));
            searcher = new IndexSearcher(indexDir, true);
        }

        #endregion

        #region interface implementation
        public IEnumerable<string> GetSynonyms(string word)
        {
            //exMessage = new StringBuilder();
            QueryParser parser = new QueryParser(CURRENT_VERSION, "word", analyzer);
            Query query = parser.Parse(word);

            TopDocs docs = searcher.Search(query, (Filter)null, TOP_NUMBER);

            if (docs == null || docs.TotalHits == 0)
            {
                return null;
            }
            else
            {

                List<string> synonyms = new List<string>();
                int counter = 1;
                foreach (ScoreDoc sd in docs.ScoreDocs)//遍历搜索到的结果  
                {
                    try
                    {
                        Document doc = searcher.Doc(sd.Doc);
                        if (doc != null)
                        {

                            Field[] fields = doc.GetFields("syn");
                            foreach (Field f in fields)
                            {
                                synonyms.Add(f.StringValue);
                            }

                        }

                    }
                    catch (Exception ex)
                    {
                        //exMessage.AppendLine(ex.Message);
                    }
                    counter++;
                }
                return synonyms;
            }
        }

        #endregion
    }
}

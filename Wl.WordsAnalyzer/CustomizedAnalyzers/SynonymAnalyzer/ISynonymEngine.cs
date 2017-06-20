using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wl.WordsAnalyzer
{
    public interface ISynonymEngine
    {
        /// <summary>
        /// 获取同义词接口
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        IEnumerable<string> GetSynonyms(string word);
    }
}

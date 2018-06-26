/*----------------------------------------------------------------
    Daniel.Zhang
    
    文件名：       AnalyzerExtension.cs
    文件功能描述：扩展方法

----------------------------------------------------------------*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Wl.WordsAnalyzer.Extension
{
    public static class AnalyzerExtension
    {
        #region 符号相关

        /// <summary>
        /// 将前缀的枚举类型转换成对应的正则表达式部分
        /// </summary>
        /// <param name="prefixForCharToHex"></param>
        /// <returns></returns>
        private static string PrefixForCharToHexForRegexPttnpartial(this PrefixForCharToHex prefixForCharToHex)
        {
            string retString = "";
            switch (prefixForCharToHex)
            {
                case PrefixForCharToHex.LuceneEscape:
                    retString = @"symbolx";
                    break;
                case PrefixForCharToHex.LuceneNormal:
                    retString = @"symbolx";
                    break;
                case PrefixForCharToHex.Escape:
                    retString = @"\\x";
                    break;
                case PrefixForCharToHex.Normal:
                    retString = @"0x";
                    break;
                default:
                    break;
            }
            return retString;
        }

        /// <summary>
        /// 字符是否符合符号的格式
        /// </summary>
        /// <param name="inString"></param>
        /// <returns></returns>
        public static bool IsSymbolMark(this string inString, PrefixForCharToHex prefixForCharToHex = PrefixForCharToHex.LuceneEscape)
        {
            string retString = inString ?? "";

            string prefix = prefixForCharToHex.PrefixForCharToHexForRegexPttnpartial();

            string pttnSymbolMark = @"(?i)" + prefix + @"([\da-f]{2,})";

            return System.Text.RegularExpressions.Regex.IsMatch(retString, pttnSymbolMark);
        }

        /// <summary>
        /// 字符是否符合符号的格式
        /// </summary>
        /// <param name="inString"></param>
        /// <param name="hexNumber"></param>
        /// <returns></returns>
        public static bool IsSymbolMark(this string inString, out string hexNumber, PrefixForCharToHex prefixForCharToHex = PrefixForCharToHex.LuceneEscape)
        {
            hexNumber = string.Empty;

            string retString = inString ?? "";

            string prefix = prefixForCharToHex.PrefixForCharToHexForRegexPttnpartial();

            string pttnSymbolMark = @"(?i)" + prefix + @"(?<hex>[\da-f]{2,})";

            Match mHex = System.Text.RegularExpressions.Regex.Match(retString, pttnSymbolMark);

            if (mHex.Success)
            {
                hexNumber = mHex.Groups["hex"].Value;
            }

            return mHex.Success;
        }

        /// <summary>
        /// 将十六进制的
        /// </summary>
        /// <param name="inString"></param>
        /// <returns></returns>
        public static decimal ParseHexString(this string hexNumber)
        {
            var hexString = string.Empty;
            var isSymbolMark = hexNumber.IsSymbolMark(out hexString);
            if (isSymbolMark)
            {
                long result = 0;
                long.TryParse(hexString, System.Globalization.NumberStyles.HexNumber, null, out result);
                return result;
            }
            return 0;
        }

        /// <summary>
        /// 将十六进制的字符串转化成字符
        /// </summary>
        /// <param name="hexNumber"></param>
        /// <param name="prefixForCharToHex"></param>
        /// <returns></returns>
        public static char ConvertHexToChar(this string hexNumber, PrefixForCharToHex prefixForCharToHex = PrefixForCharToHex.LuceneEscape)
        {
            var hexString = string.Empty;
            var isSymbolMark = hexNumber.IsSymbolMark(out hexString, prefixForCharToHex);
            if (isSymbolMark)
            {
                long result = 0;
                long.TryParse(hexString, System.Globalization.NumberStyles.HexNumber, null, out result);
                return (char)result;
            }
            return (char)0;
        }

        /// <summary>
        /// 将字符 转换成 十六进制 
        /// </summary>
        /// <param name="inChar"></param>
        /// <param name="needPrefix">是否加\x前缀</param>
        /// <returns></returns>
        public static string ConvertCharToHex(this char inChar, PrefixForCharToHex prefixForCharToHex = PrefixForCharToHex.None)
        {
            string hex = Convert.ToByte(inChar).ToString("x2");
            switch (prefixForCharToHex)
            {
                case PrefixForCharToHex.LuceneEscape:
                    hex = @"symbolx" + hex;
                    break;
                case PrefixForCharToHex.LuceneNormal:
                    hex = @"symbolx" + hex;
                    break;
                case PrefixForCharToHex.Escape:
                    hex = @"\x" + hex;
                    break;
                case PrefixForCharToHex.Normal:
                    hex = @"0x" + hex;
                    break;
                default:
                    break;
            }
            return hex;
        }

        #endregion

        #region 符号转化 相关

        /// <summary>
        /// 将输入的句子中的问号作适应性修改 
        /// </summary>
        /// <param name="inString"></param>
        /// <returns></returns>
        public static string AnalyzerInputStringAdaptQuestionMark(this string inString)
        {
            string retString = inString ?? "";

            retString = retString.AnalyzerInputStringAdaptSymbolMark('?');

            return retString;
        }

        /// <summary>
        /// 将输入的句子中的符号作适应性修改
        /// </summary>
        /// <param name="inString"></param>
        /// <param name="hexOfSymbol"></param>
        /// <returns></returns>
        public static string AnalyzerInputStringAdaptSymbolMark(this string inString, char symbol)
        {
            string retString = inString ?? "";

            string hexOfSymbol = symbol.ConvertCharToHex();

            #region 转义单独的符号和结尾的符号


            string pttnContainsWithQuestionMark = @"(?<!\x" + hexOfSymbol + @"|\s)\x" + hexOfSymbol + @"(?!\x" + hexOfSymbol + @"|\s)";
            retString = System.Text.RegularExpressions.Regex.Replace(retString, pttnContainsWithQuestionMark, " " + symbol + " ");

            string pttnStartsWithQuestionMark = @"\x" + hexOfSymbol + @"(?!\x" + hexOfSymbol + @"|\s)";
            retString = System.Text.RegularExpressions.Regex.Replace(retString, pttnStartsWithQuestionMark, symbol + " ");

            string pttnEndWithQuestionMark = @"(?<!\x" + hexOfSymbol + @"|\s)\x" + hexOfSymbol;
            retString = System.Text.RegularExpressions.Regex.Replace(retString, pttnEndWithQuestionMark, " " + symbol);


            string pttnQuestionMark = @"\x" + hexOfSymbol;
            retString = System.Text.RegularExpressions.Regex.Replace(retString, pttnQuestionMark, symbol.ConvertCharToHex(PrefixForCharToHex.LuceneEscape));

            #endregion

            return retString;
        }

        /// <summary>
        /// 将输入的句子中的符号作适应性修改
        /// </summary>
        /// <param name="inString"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static string AnalyzerInputStringAdaptSymbolMark(this string inString, char[] symbol)
        {
            var retString = inString ?? "";
            if (symbol != null && symbol.Length > 0)
            {
                foreach (char ch in symbol)
                {
                    retString = retString.AnalyzerInputStringAdaptSymbolMark(ch);
                }

            }
            return retString;
        }

        #endregion

        #region Lucene 基础

        /// <summary>
        /// 解析 lucene 的版本设置
        /// </summary>
        /// <param name="inString"></param>
        /// <returns></returns>
        public static Lucene.Net.Util.Version ParseLuceneVersion(this string inString)
        {
            var retVersion = Lucene.Net.Util.Version.LUCENE_30;
            var type = typeof(Lucene.Net.Util.Version);
            string tmpString = inString ?? "LUCENE_CURRENT";
            try
            {
                retVersion = (Lucene.Net.Util.Version)Enum.Parse(type, tmpString);
            }
            catch (Exception)
            {
                retVersion = Lucene.Net.Util.Version.LUCENE_30;
            }
            return retVersion;
        }

        #endregion

        #region 结果集转化

        /// <summary>
        /// 将结果集简化
        /// </summary>
        /// <param name="analysedWordStruct"></param>
        /// <returns></returns>
        public static IList<Wl.WordsAnalyzer.Models.AnalysedWordStructSimplified> SimplifyAnalysedWordStruct(this IList<Wl.WordsAnalyzer.Models.AnalysedWordStruct> analysedWordStruct)
        {
            var retAnalysedWordStructSimplified = new List<Wl.WordsAnalyzer.Models.AnalysedWordStructSimplified>();

            if (analysedWordStruct != null && analysedWordStruct.Any())
            {
                foreach (var analysedWordSet in analysedWordStruct)
                {
                    var curAnalysedWordStructSimplified = new Wl.WordsAnalyzer.Models.AnalysedWordStructSimplified();
                    if (analysedWordSet.SynonymsOfSingularizedWord.Count() > analysedWordSet.SynonymsOfOriginalWord.Count())
                    {
                        curAnalysedWordStructSimplified.Synonyms = analysedWordSet.SynonymsOfSingularizedWord;
                        curAnalysedWordStructSimplified.Word = analysedWordSet.SingularizedWord;
                    }
                    else
                    {
                        curAnalysedWordStructSimplified.Synonyms = analysedWordSet.SynonymsOfOriginalWord;
                        curAnalysedWordStructSimplified.Word = analysedWordSet.OriginalWord;
                    }

                    retAnalysedWordStructSimplified.Add(curAnalysedWordStructSimplified);
                }
            }

            return retAnalysedWordStructSimplified;
        }

        /// <summary>
        /// 将结果集简化 并 在输出前 用定义的转码器 转码 
        /// </summary>
        /// <param name="analysedWordStruct"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IList<Wl.WordsAnalyzer.Models.AnalysedWordStructSimplified> SimplifyAnalysedWordStructProcessBeforeReturn(this IList<Wl.WordsAnalyzer.Models.AnalysedWordStruct> analysedWordStruct, Func<string, string> filter = null)
        {
            var retAnalysedWordStructSimplified = new List<Wl.WordsAnalyzer.Models.AnalysedWordStructSimplified>();

            if (analysedWordStruct != null && analysedWordStruct.Any())
            {
                foreach (var analysedWordSet in analysedWordStruct)
                {
                    var curAnalysedWordStructSimplified = new Wl.WordsAnalyzer.Models.AnalysedWordStructSimplified();
                    if (analysedWordSet.SynonymsOfSingularizedWord.Count() > analysedWordSet.SynonymsOfOriginalWord.Count())
                    {
                        if (filter != null)
                        {
                            curAnalysedWordStructSimplified.Synonyms = analysedWordSet.SynonymsOfSingularizedWord.Select(x => filter(x)).ToList();
                            curAnalysedWordStructSimplified.Word = filter(analysedWordSet.SingularizedWord);
                        }
                        else
                        {

                            curAnalysedWordStructSimplified.Synonyms = analysedWordSet.SynonymsOfSingularizedWord;
                            curAnalysedWordStructSimplified.Word = analysedWordSet.SingularizedWord;
                        }

                    }
                    else
                    {
                        if (filter != null)
                        {
                            curAnalysedWordStructSimplified.Synonyms = analysedWordSet.SynonymsOfOriginalWord.Select(x => filter(x)).ToList();
                            curAnalysedWordStructSimplified.Word = filter(analysedWordSet.OriginalWord);
                        }
                        else
                        {
                            curAnalysedWordStructSimplified.Synonyms = analysedWordSet.SynonymsOfOriginalWord;
                            curAnalysedWordStructSimplified.Word = analysedWordSet.OriginalWord;
                        }

                    }

                    retAnalysedWordStructSimplified.Add(curAnalysedWordStructSimplified);
                }
            }

            return retAnalysedWordStructSimplified;
        }

        #endregion

        #region 结果集转化，是否转码

        /// <summary>
        /// 将分析 后的词 输出前 加入 自定义的 转换器
        /// </summary>
        /// <param name="analysedWordStruct"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IList<Wl.WordsAnalyzer.Models.AnalysedWordStruct> AnalysedWordStructProcessBeforeReturn(this IList<Wl.WordsAnalyzer.Models.AnalysedWordStruct> analysedWordStruct, Func<string, string> filter = null)
        {
            var retAnalysedWordStruct = new List<Wl.WordsAnalyzer.Models.AnalysedWordStruct>();

            if (analysedWordStruct != null && analysedWordStruct.Any())
            {
                if (filter != null)
                {
                    foreach (var wordSet in analysedWordStruct)
                    {
                        var curWordSet = new Wl.WordsAnalyzer.Models.AnalysedWordStruct();
                        curWordSet.OriginalWord = filter(wordSet.OriginalWord);
                        curWordSet.SingularizedWord = filter(wordSet.SingularizedWord);

                        if (wordSet.SynonymsOfOriginalWord != null && wordSet.SynonymsOfOriginalWord.Any())
                        {
                            curWordSet.SynonymsOfOriginalWord = wordSet.SynonymsOfOriginalWord.Select(x => filter(x)).ToList();
                        }

                        if (wordSet.SynonymsOfSingularizedWord != null && wordSet.SynonymsOfSingularizedWord.Any())
                        {
                            curWordSet.SynonymsOfSingularizedWord = wordSet.SynonymsOfSingularizedWord.Select(x => filter(x)).ToList();
                        }

                        retAnalysedWordStruct.Add(curWordSet);
                    }
                }
                else
                {
                    retAnalysedWordStruct.AddRange(analysedWordStruct);
                }
            }

            return retAnalysedWordStruct;
        }

        /// <summary>
        /// 将分析后的 词输出，并在输出前用自定义的转码器转码
        /// </summary>
        /// <param name="simplifiedAnalysedWordStruct"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IList<Wl.WordsAnalyzer.Models.AnalysedWordStructSimplified> SimplifiedWordStructProcessBeforeReturn(this IList<Wl.WordsAnalyzer.Models.AnalysedWordStructSimplified> simplifiedAnalysedWordStruct, Func<string, string> filter = null)
        {
            var retSimplifiedAnalysedWordStruct = new List<Wl.WordsAnalyzer.Models.AnalysedWordStructSimplified>();
            if (simplifiedAnalysedWordStruct != null && simplifiedAnalysedWordStruct.Any())
            {
                if (filter != null)
                {
                    foreach (var simplifiedWord in simplifiedAnalysedWordStruct)
                    {
                        var curSimplifiedWord = new Wl.WordsAnalyzer.Models.AnalysedWordStructSimplified();

                        curSimplifiedWord.Word = filter(simplifiedWord.Word);

                        curSimplifiedWord.Synonyms = simplifiedWord.Synonyms.Select(x => filter(x)).ToList();

                        retSimplifiedAnalysedWordStruct.Add(curSimplifiedWord);
                    }
                }
                else
                {
                    retSimplifiedAnalysedWordStruct.AddRange(simplifiedAnalysedWordStruct);
                }
            }

            return retSimplifiedAnalysedWordStruct;
        }

        #endregion

    }
}

using System;
using Wl.WordsAnalyzer.Extension;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Wl.WordsAnalyzer.Test
{
    [TestClass]
    public class AnalyzerExtensionTest
    {
        [TestMethod]
        public void TestAnalyzerInputStringAdaptQuestionMark()
        {
            var title = "Anti-broken 360° Full Protection PC Case Cover Tempered Glass Gifts For iPhone 5/5S/SE/6/6S/7 Plus and Android 4.0 work?very hard or Symbian 3.0 -q ";

            Console.WriteLine(title.AnalyzerInputStringAdaptQuestionMark());
        }

        [TestMethod]
        public void TestAnalyzerInputStringAdaptMarks()
        {
            var chars = new char[] { '+', '-', '*', '?', '(', ')', '{', '}', '[', ']', '!', '^' };

            var title = "Anti-broken 360° Full Protection PC Case Cover Tempered Glass Gifts For iPhone 5/5S/SE/6/6S/7 Plus and Android 4.0 [work]?very hard or Symbian 3.0 -q ";

            Console.WriteLine(title.AnalyzerInputStringAdaptSymbolMark(chars));
        }

        [TestMethod]
        public void TestParseHexString()
        {
            var hexString = @"\x3f";

            var hexCode = hexString.ParseHexString();

            string hex = Convert.ToByte('?').ToString("x2");

            Console.WriteLine(hex);
        }

        [TestMethod]
        public void TestConvertHexToChar()
        {
            var c = @"\x3f";
            Console.WriteLine(c.ConvertHexToChar());
        }
    }
}

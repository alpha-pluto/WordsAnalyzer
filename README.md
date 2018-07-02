# WordsAnalyzer
Get synonyms for each word in a sentence
给定一句语句（英文），分词并给出每个词的同义词

调用的类为 Wl.WordsAnalyzer.AnalyzerContainer

  SentenceAnalysisSimplifiedJson 返回 json 结果

调用范例

var agent = new Wl.WordsAnalyzer.AnalyzerAgent();
var title = @"Anti-broken 360° Full Protection PC Case Cover Tempered Glass Gifts For iPhone 5/5S/SE/6/6S/7 + Plus and * Android 4.0 work ? very hard or Symbian 3.0 -q ";
var jsonString = agent.SentenceAnalysisSimplifiedJson(title);

json 的结果如下 
 
 <img src="http://m.qpic.cn/psb?/71ab6d5a-79d4-4d4e-9470-3d08984aad4d/LimPkieFoeWqovuFSTphpnQ2fNrWxOSxSmJKcaEdJsw!/b/dEQBAAAAAAAA&bo=ugIYAroCGAIDCSw!&rf=viewer_4" />

项目中 lucene_dict 目录 为字典目录 ，组件运行 需要 拷贝到ｅ盘根目录下。

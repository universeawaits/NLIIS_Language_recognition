using System.Collections.Generic;

namespace NLIIS_Language_recognizer.Service
{
    public interface ILanguageRecognizer
    {
        string MethodName { get; }
        string Recognize(string text);
        IDictionary<string, double> GetWords(string text);
    }
}
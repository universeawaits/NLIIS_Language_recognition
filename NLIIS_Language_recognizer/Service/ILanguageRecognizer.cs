using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NLIIS_Language_recognizer.Models;

namespace NLIIS_Language_recognizer.Service
{
    public interface ILanguageRecognizer
    {
        static string MethodName { get; }
        string Recognize(string text);
        IDictionary<string, double> GetWords(string text);
    }
}
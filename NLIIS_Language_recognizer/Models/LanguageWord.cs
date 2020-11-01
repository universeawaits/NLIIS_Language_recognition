using System;
using System.Collections.Generic;

namespace NLIIS_Language_recognizer.Models
{
    public class LanguageWord
    {
        public string Word { get; set; }
        
        public string Language { get; set; }
        
        public string Method { get; set; }
        
        public double Probability { get; set; }

        public static List<LanguageWord> Words = new List<LanguageWord>();
        
        public class Comparer : IEqualityComparer<LanguageWord> {
            public bool Equals(LanguageWord x, LanguageWord y) {
                return x.Word.Equals(y.Word) && x.Language.Equals(y.Language);
            }

            public int GetHashCode(LanguageWord obj) {
                return Int32.Parse(obj).ToString().GetHashCode();
            }
        }
    }
}

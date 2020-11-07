using System;
using System.Collections.Generic;
using System.Globalization;

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
                if (x == null || y == null)
                {
                    return false;
                }
                
                return x.Word.Equals(y.Word) && x.Language.Equals(y.Language) && x.Method.Equals(y.Method);
            }

            public int GetHashCode(LanguageWord obj) {
                return Int32.Parse(
                    (obj.Probability * obj.Language.Length * obj.Method.Length / obj.Word.Length).ToString(CultureInfo.InvariantCulture))
                    .ToString()
                    .GetHashCode();
            }
        }
    }
}

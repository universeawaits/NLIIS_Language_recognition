using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using BitMiracle.Docotic.Pdf;

namespace NLIIS_Language_recognizer.Service
{
    public static class DocumentService
    {
        public static string FromPDF(string path)
        {
            using var pdf = new PdfDocument(path);
            
            return pdf.GetText();
        }
        
        public static IEnumerable<string> GetAtoms(string text)
        {
            var words = GetCleanText(text).Split(" ");

            return new HashSet<string>(words);
        }

        private static string GetCleanText(string text)
        {
            return 
                Regex.Replace(text, "[–,.;:!?]", string.Empty)
                    .Replace("\n", " ")
                    .ToLower();
            //.replaceAll("  ( )*", " ").toLowerCase();
        }

        public static IEnumerable<string> GetSplitWords(string text){
            return GetCleanText(text).Split(" ");
        }

        public static IDictionary<string, int> GetWordsOccurrences(IEnumerable<string> words) {

            var initialForms = new Dictionary<string, int>();

            foreach (var word in words) {

                if (initialForms.ContainsKey(word))
                {
                    initialForms.TryGetValue(word, out var prevValue);
                    initialForms.Remove(word);
                    initialForms.Add(word, prevValue + 1);
                } else {
                    initialForms.Add(word, 1);
                }
            }

            return initialForms;
        }
    }
}

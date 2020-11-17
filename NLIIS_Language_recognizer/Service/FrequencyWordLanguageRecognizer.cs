using System.Collections.Generic;
using System.Linq;
using NLIIS_Language_recognizer.Models;

namespace NLIIS_Language_recognizer.Service
{
    public class FrequencyWordLanguageRecognizer : ILanguageRecognizer
    {
        public string MethodName => "FrequencyWord";

        public string Recognize(string text)
        {
            var wordsOccurrences = Language.GetAllLanguages()
                .ToDictionary(language => language, language => 0d);

            var atoms = DocumentService.GetAtoms(text);

            foreach (var atom in atoms)
            {
                var founds = LanguageWord.Words
                    .Where(word => word.Word.Equals(atom) && word.Method.Equals(MethodName));
                
                foreach (var found in founds)
                {
                    var currentProbability = wordsOccurrences[found.Language];
                    wordsOccurrences.Remove(found.Language);
                    currentProbability += found.Probability;
                    wordsOccurrences.Add(found.Language, currentProbability);
                }
            }

            return wordsOccurrences.FirstOrDefault(pair => pair.Value == wordsOccurrences.Values.Max() && pair.Value != 0).Key
                   ?? "Undefined";
        }

        public IDictionary<string, double> GetWords(string text)
        {
            var allWords = DocumentService.GetSplitWords(text);
            var wordsOccurrences = DocumentService.GetWordsOccurrences(allWords);

            return wordsOccurrences.ToDictionary(
                pair => pair.Key,
                pair => (double) pair.Value / (double) allWords.Count());
        }
    }
}

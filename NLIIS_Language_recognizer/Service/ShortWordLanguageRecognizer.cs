using System;
using System.Collections.Generic;
using System.Linq;
using NLIIS_Language_recognizer.Models;

namespace NLIIS_Language_recognizer.Service
{
    public class ShortWordLanguageRecognizer : ILanguageRecognizer
    {
        public static string MethodName => "ShortWord";

        public string Recognize(string text)
        {
            var termsOccurrences = Language.GetAllLanguages()
                .ToDictionary(language => language, language => Math.Pow(100, 150D));

            var atoms = DocumentService.GetAtoms(text);

            foreach (var atom in atoms)
            {
                foreach (var language in Language.GetAllLanguages())
                {
                    var currentProbability = termsOccurrences[language];
                    termsOccurrences.Remove(language);
                    currentProbability *= GetProbability(atom, MethodName, language);
                    termsOccurrences.Add(language, currentProbability);
                }
            }

            return termsOccurrences.First(pair => pair.Value == termsOccurrences.Values.Max()).Key ?? "Undefined";
        }

        public IDictionary<string, double> GetWords(string text)
        {
            var allWords = DocumentService.GetSplitWords(text);
            var shortWords = DocumentService.GetWordsOccurrences(allWords)
                .Where(pair => pair.Key.Length < 5 && pair.Value > 3)
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            var sumFrequencies = shortWords.Sum(pair => pair.Value);

            return shortWords.ToDictionary(
                pair => pair.Key,
                pair => (double) (pair.Value / sumFrequencies));
        }

        private double GetProbability(string atom, string method, string language){
            LanguageWord word = wordRepository.findByWordAndMethodAndLanguage(atom, method, language);

            return word?.Probability ?? 0.01;
        }
    }
}
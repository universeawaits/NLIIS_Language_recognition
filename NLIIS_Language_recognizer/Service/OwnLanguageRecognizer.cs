using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NLIIS_Language_recognizer.Models;

namespace NLIIS_Language_recognizer.Service
{
    public class OwnLanguageRecognizer : ILanguageRecognizer
    {
        public static string MethodName => "Own";

        public string Recognize(string text)
        {
            var termsOccurrences = Language.GetAllLanguages()
                .ToDictionary(language => language, language => 0);

            var atoms = DocumentService.GetAtoms(text);

            foreach (var atom in atoms)
            {
                foreach (var language in Language.GetAllLanguages())
                {
                    var currentProbability = termsOccurrences[language];
                    termsOccurrences.Remove(language);
                    currentProbability += IsLanguageWord(atom, language);
                    termsOccurrences.Add(language, currentProbability);
                }
            }

            return termsOccurrences.FirstOrDefault(pair => pair.Value == termsOccurrences.Values.Max() && pair.Value != 0).Key
                   ?? "Undefined";
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
                pair => (double)pair.Value / (double)sumFrequencies);
        }

        private int IsLanguageWord(string atom, string language)
        {
            var isMatch = false;

            switch (language)
            {
                case "Russian":
                {
                    isMatch = Regex.IsMatch(atom, "^[а-яА-Я]+$");
                    break;
                }
                case "Italian":
                {
                    isMatch = Regex.IsMatch(atom, "^[a-zA-ZÀÈÉÌÒÙàèéìòù]+$");
                    break;
                }
            }

            return isMatch ? 1 : 0;
        }
    }
}

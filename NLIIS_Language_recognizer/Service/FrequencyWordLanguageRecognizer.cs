using System;
using System.Collections.Generic;
using System.Linq;
using NLIIS_Language_recognizer.Models;

namespace NLIIS_Language_recognizer.Service
{
    public class FrequencyWordLanguageRecognizer : ILanguageRecognizer
    {
        public static string MethodName => "FrequencyWord";

        public string Recognize(string text)
        {
            var wordsOccurrences = Language.GetAllLanguages()
                .ToDictionary(language => language, language => 0);

            var atoms = DocumentService.GetAtoms(text);

            foreach (var atom in atoms)
            {
                var founds = wordRepository.findAllByWordAndMethod(atom, MethodName);
                
                foreach (var found in founds)
                {
                    var currentProbability = wordsOccurrences[found.Language];
                    wordsOccurrences.Remove(found.Language);
                    currentProbability += found.Frequency;
                    wordsOccurrences.Add(found.Language, currentProbability);
                }
            }

            return wordsOccurrences.First(pair => pair.Value == wordsOccurrences.Values.Max()).Key ?? "Undefined";
        }

        public IDictionary<string, double> GetWords(string text)
        {
            var allWords = DocumentService.GetSplitWords(text);
            var wordsOccurrences = DocumentService.GetWordsOccurrences(allWords);

            return wordsOccurrences.ToDictionary(
                pair => pair.Key,
                pair => (double) (pair.Value / allWords.Count()));
        }
    }
}
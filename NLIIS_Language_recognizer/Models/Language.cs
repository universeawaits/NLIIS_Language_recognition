using System.Collections.Generic;

namespace NLIIS_Language_recognizer.Models
{
    public static class Language
    {
        public static IEnumerable<string> GetAllLanguages()
        {
            return new[] { "Russian", "Italian" };
        }
    }
}
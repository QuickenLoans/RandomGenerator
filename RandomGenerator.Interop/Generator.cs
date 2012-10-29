using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.FSharp.Collections;

namespace RandomGenerator.Interop
{
    public static class Generator
    {
        public static string SpecialCharacters { get; set; }

        public static string Single(int length, bool allowAlpha = true, bool allowNumeric = true, bool allowSpecial = false)
        {
            return GenerateSingleString(length, allowAlpha, allowNumeric, allowSpecial);
        }

        public static List<string> Multiple(int count, int length, bool allowAlpha = true, bool allowNumeric = true, bool allowSpecial = false)
        {
            return GenerateSingleMultiple(count, length, allowAlpha, allowNumeric, allowSpecial).ToList<string>();
        }

        private static string[] GenerateSingleMultiple(int count, int length, bool allowAlpha = true, bool allowNumeric = true, bool allowSpecial = false)
        {
            if (!string.IsNullOrEmpty(SpecialCharacters))
            {
                RandomGenerator.Lib.SpecialChars = ListModule.OfSeq(SpecialCharacters.ToArray());
            }

            return RandomGenerator.Lib.GenerateMultiple(allowAlpha, allowNumeric, allowSpecial, length, count);
        }

        private static string GenerateSingleString(int length, bool allowAlpha = true, bool allowNumeric = true, bool allowSpecial = false)
        {
            if (!string.IsNullOrEmpty(SpecialCharacters))
            {
                RandomGenerator.Lib.SpecialChars = ListModule.OfSeq(SpecialCharacters.ToArray());
            }
            return RandomGenerator.Lib.Generate(allowAlpha, allowNumeric, allowSpecial, length);
        }
    }
}

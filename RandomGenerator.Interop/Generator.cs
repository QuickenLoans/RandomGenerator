using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.FSharp.Collections;

namespace RandomGenerator.Interop
{
    public static class Generator
    {
        public static string Letters { get; set; }
        public static string Numbers { get; set; }
        public static string SpecialCharacters { get; set; }

        public static string Single(int length, bool allowAlpha = true, bool allowNumeric = true, bool allowSpecial = false)
        {
            return GenerateSingleString(length);
        }

        public static List<string> Multiple(int count, int length, bool allowAlpha = true, bool allowNumeric = true, bool allowSpecial = false)
        {
            return GenerateSingleMultiple(count, length).ToList<string>();
        }

        private static string[] GenerateSingleMultiple(int count, int length)
        {
            return RandomGenerator.Lib.generateMultiple(count, length, BuildCharacterTypes());
        }

        private static string GenerateSingleString(int length)
        {
            return RandomGenerator.Lib.generate(length, BuildCharacterTypes());
        }

        private static Lib.CharacterTypes BuildCharacterTypes()
        {
            if (!string.IsNullOrEmpty(SpecialCharacters))
            {
                RandomGenerator.Lib.SpecialChars = ListModule.OfSeq(SpecialCharacters.ToArray());
            }
        }
    }
}

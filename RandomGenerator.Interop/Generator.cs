using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.FSharp.Collections;

namespace RandomGenerator.Interop
{
    public class Generator
    {
        private readonly bool _allowAlpha = false;
        private readonly bool _allowNumeric = false;
        private readonly bool _allowSpecial = false;
        private string _letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private string _numbers = "0123456789";
        private string _special = @"_()[]{}<>!?;:=*-+/\%.,$£&#@";

        /// <summary>
        /// List of Letters to use. Default is A-Z (upper case)
        /// </summary>
        public string Letters
        {
            get { return _letters; }
            set { _letters = value; }
        }
        /// <summary>
        /// List of Numbers to use. Default is 0-9
        /// </summary>
        public string Numbers
        {
            get { return _numbers; }
            set { _numbers = value; }
        }
        /// <summary>
        /// List of Special Characters to use. Default is _()[]{}<>!?;:=*-+/\\%.,$£&#@
        /// </summary>
        public string SpecialCharacters
        {
            get { return _special; }
            set { _special = value; }
        }

        public Generator(bool allowAlpha = true, bool allowNumeric = true, bool allowSpecial = false)
        {
            _allowAlpha = allowAlpha;
            _allowNumeric = allowNumeric;
            _allowSpecial = allowSpecial;
        }

        public string Single(int length)
        {
            return GenerateSingleString(length);
        }

        public List<string> Multiple(int count, int length)
        {
            return GenerateMultiple(count, length).ToList<string>();
        }

        private string[] GenerateMultiple(int count, int length)
        {
            //var gen = new RandomGenerator.Lib.Generator();
            return RandomGenerator.Lib.GenerateMultiple(count, length, BuildCharacterTypes());
        }

        private string GenerateSingleString(int length)
        {
            //var gen = new RandomGenerator.Lib.Generator();
            return RandomGenerator.Lib.Generate(length, BuildCharacterTypes());
        }

        private Lib.CharacterTypes BuildCharacterTypes()
        {
            Lib.CharacterTypes chars = null;

            if (_allowAlpha)
                chars = Lib.CharacterTypes.NewChars(ListModule.OfSeq(_letters.ToArray()));
            if (_allowNumeric)
                chars = chars == null ? 
                    Lib.CharacterTypes.NewChars(ListModule.OfSeq(_numbers.ToArray())) : 
                    Lib.CharacterTypes.NewCharSet(chars, Lib.CharacterTypes.NewChars(ListModule.OfSeq(_numbers.ToArray())));
            if (_allowSpecial)
                chars = chars == null ?
                    Lib.CharacterTypes.NewChars(ListModule.OfSeq(_special.ToArray())) : 
                    Lib.CharacterTypes.NewCharSet(chars, Lib.CharacterTypes.NewChars(ListModule.OfSeq(_special.ToArray())));

            return chars;
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace RSA.Implementation.RSA
{
    public class Encoder : Encryption
    {
        public Encoder(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                this.DigitList.Add(GetIndexByLetter(text[i]));
            }
        }

        private BigInteger GetIndexByLetter(char letter)
        {
            return AlphabetMap.Alphabet.ContainsKey(letter)
                ? AlphabetMap.Alphabet[letter]
                : AlphabetMap.Alphabet.Last().Value;
        }

        public override object PerformOperation()
        {
            var sequenceNumbers = new List<BigInteger>();
            foreach (var digit in this.DigitList)
            {
                sequenceNumbers.Add(ProcessDigit(digit, PublicKey, SecretDigitMultiplication));
            }

            return sequenceNumbers;
        }
    }
}
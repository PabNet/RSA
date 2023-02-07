using System.Collections.Generic;
using System.Numerics;

namespace RSA.Implementation.RSA
{
    public class Decoder : Encryption
    {
        public Decoder(object sequenceNumbers)
        {
            this.DigitList = (List<BigInteger>)sequenceNumbers;
        }


        public override object PerformOperation()
        {
            var text = "";
            foreach (var digit in this.DigitList)
            {
                text += GetLetterByIndex(ProcessDigit(digit, PrivateKey, SecretDigitMultiplication));
            }

            return text;
        }
        
        private char GetLetterByIndex(BigInteger index)
        {
            char letter = '\0';
            foreach (var letterPair in AlphabetMap.Alphabet)
            {
                if (letterPair.Value == index)
                {
                    letter = letterPair.Key;
                    break;
                }
            }

            return letter;
        }
    }
}
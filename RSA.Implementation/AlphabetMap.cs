using System;
using System.Collections.Generic;
using System.Linq;

namespace RSA.Implementation
{
    public static class AlphabetMap
    {
        public static Dictionary<char, byte> Alphabet { get; } = new Dictionary<char, byte>();

        static AlphabetMap()
        {
            List<char> letterList = Enumerable.Range(0, 32).Select((i) => (char)('а' + i)).ToList();
            letterList.Insert(letterList.IndexOf('ж'), 'ё');
            letterList.AddRange(Enumerable.Range(0, 32).Select((i) => (char)('А' + i)));
            letterList.Insert(letterList.IndexOf('Ж'), 'Ё');
            letterList.Add(Convert.ToChar(" "));
        

            foreach (var letter in letterList)
            {
                Alphabet[letter] = (byte)(letterList.IndexOf(letter) + 2);
            }
            
        }
    }
}
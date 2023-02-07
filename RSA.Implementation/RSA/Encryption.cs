using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;

namespace RSA.Implementation.RSA
{
    public abstract class Encryption : IEncryption
    {
        private protected List<BigInteger> DigitList = new ();
        private protected static BigInteger PublicKey, PrivateKey, SecretDigitMultiplication;

        static Encryption()
        {
            /*Первое секретное число*/
            GetSecretDigit(out var firstDigit);
            /*Второе секретное число*/
            GetSecretDigit(out var secondDigit);

            /*r*/
            SecretDigitMultiplication = firstDigit*secondDigit;
            /*функция эйлера*/
            var eulerFunction = (firstDigit - 1) * (secondDigit - 1);
            /*открытая экспонента*/
            GetPublicKey(in eulerFunction, out PublicKey);
            /*закрытая экспонента*/
            GetPrivateKey(in eulerFunction, in PublicKey, out PrivateKey);
        }

        public abstract object PerformOperation();
        
        /*Алгоритм быстрого возведения в степень*/
        private protected static BigInteger ProcessDigit(BigInteger a, BigInteger z, BigInteger n)
        {
            BigInteger a1 = a, z1 = z, x = 1;
            while (z1 != 0)
            {
                while (z1 % 2 == 0)
                {
                    z1 = z1 / 2;
                    a1 = (a1 * a1) % n;
                }
                z1 = z1 - 1;
                x = (x * a1) % n;
            }

            return x;
        }

        private static void GetPublicKey(in BigInteger eulerFunction, out BigInteger publicKey)
        {
            var big = new BigInteger();
            do
            {
                publicKey = new BigInteger(GetBytes());
            } while (publicKey <= 0 || publicKey >= eulerFunction ||
                     ExecuteExtendedEuclideanAlgorithm(publicKey, eulerFunction, out big) != 1);
        }

        /*расширенный алгоритм евклида*/
        private static BigInteger ExecuteExtendedEuclideanAlgorithm(BigInteger a, BigInteger b, out BigInteger y)
        {
            BigInteger d0 = a,  d1 = b, x0 = 1, x1 = 0, y0 = 0, y1 = 1;
            while (d1 > 1)
            {
                BigInteger q = d0 / d1, d2 = d0 % d1, x2 = x0 - q * x1, y2 = y0 - q * y1;
                d0 = d1;
                d1 = d2;
                x0 = x1;
                x1 = x2;
                y0 = y1;
                y1 = y2;
            }
            y = y1;
            return d1;
        }
        
        private static void GetPrivateKey(in BigInteger eulerFunction, in BigInteger publicKey, 
            out BigInteger privateKey)
        {
            ExecuteExtendedEuclideanAlgorithm(eulerFunction, publicKey, out privateKey);
            if (privateKey < 0)
            {
                privateKey += eulerFunction;
            }
        }

        /*Получение рандомного числа размером от 64-128 байт*/
        private static byte[] GetBytes()
        {
            var bytes = new byte[IEncryption.MinByteValue 
                          + new Random().Next()
                          %(IEncryption.MaxByteValue-IEncryption.MinByteValue)];
            new RNGCryptoServiceProvider().GetBytes(bytes);
            return bytes;
        }

        private static void GetSecretDigit(out BigInteger bigInteger)
        {
            do
            {
                bigInteger = new BigInteger(GetBytes());
            } while (!IsPrime(bigInteger)
                     || bigInteger < 0);
        }
        
        /*Проверка на простое число*/
        private static bool IsPrime(BigInteger digit)
        {
            if (digit == 2 || digit == 3)
            {
                return true;
            }

            if (digit < 2 || digit % 2 == 0)
            {
                return false;
            }
   
            BigInteger t = digit - 1;

            int s = 0;

            while (t % 2 == 0)
            {
                t /= 2;
                s += 1;
            }
            
            for (byte i = 0; i < 10; i++)
            {
                RNGCryptoServiceProvider rng = new ();

                byte[] byteArray = new byte[digit.ToByteArray().LongLength];

                BigInteger a;

                do
                {
                    rng.GetBytes(byteArray);
                    a = new BigInteger(byteArray);
                } while (a < 2 || a >= digit - 2);
                
                BigInteger x = BigInteger.ModPow(a, t, digit);

                if (x == 1 || x == digit - 1)
                {
                    continue;
                }

                for (int r = 1; r < s; r++)
                {
                    x = BigInteger.ModPow(x, 2, digit);

                    if (x == 1)
                    {
                        return false;
                    }
                    
                    if (x == digit - 1)
                    {
                        break;
                    }
                }

                if (x != digit - 1)
                {
                    return false;
                }
                
            }
            
            return true;
            
        }
        
        
    }
}
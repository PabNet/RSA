using System;
using RSA.Implementation.RSA;

namespace RSA.Implementation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Введите текст: ");
            Encoder encoder = new Encoder(Console.ReadLine());
            Decoder decoder = new Decoder(encoder.PerformOperation());
            Console.WriteLine($"Результат расшифрования: {decoder.PerformOperation()}");
        }
        
    }
}
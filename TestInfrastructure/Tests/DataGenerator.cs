using System;

namespace SKBKontur.Treller.Tests.Tests
{
    public static class DataGenerator
    {
        private const string RussianAlphabet = "ЁЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮёйцукенгшщзхъфывапролджэячсмитьбю-";
        private const string EnglishAlphabet = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm";
        private static readonly Random Random = new Random();

        public static string GenRussainString(int length)
        {
            return GenString(RussianAlphabet, length);
        }

        public static string GenEnglishString(int length)
        {
            return GenString(EnglishAlphabet, length);
        }

        private static string GenString(string alphabet, int length)
        {
            string result = "";
            for (int i = 0; i < length; i++)
                result += alphabet[Random.Next(0, alphabet.Length - 1)];
            return result;
        }

        public static string GenDigitString(int length)
        {
            string result = "";
            for (int i = 0; i < length; i++)
                result += Random.Next(9);
            return result;
        }

        public static int GenInt()
        {
            return Random.Next();
        }

        public static string GenEmail()
        {
            return $"{GenEnglishString(10)}@test.ru";
        }
    }
}
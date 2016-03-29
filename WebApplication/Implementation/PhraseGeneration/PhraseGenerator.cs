using System;

namespace SKBKontur.Treller.WebApplication.Implementation.PhraseGeneration
{
    public class PhraseGenerator : IPhraseGenerator
    {
        private readonly string[] adjectives;
        private readonly string[] nouns;
        private readonly string[] verbs;
        private readonly string[] adverbs;

        public PhraseGenerator()
        {
            adjectives = new[]
                             {
                                 "общительных", "беззаботных", "непринужденных", "непринужденных", "искренних", "честных", "робких", "застенчивых", "сдержанных", "чувствительных",
                                 "творческих", "эмоциональных", "нерешительных", "волевых", "энергичных", "воспитанных", "независимых", "инертных", "вялых", "медленных",
                                 // TODO: Добавить еще 11 строк по 10 элементов
                             };

            nouns = new[]
                        {
                            "сельдей", "анчоусов", "белух", "горбуш", "тунцов", "ершей", "сардин", "окуней", "карасей", "карпов",
                            "кижучей", "лососей", "лещей", "мойв", "щук", "угорей", "налимов", "палтусов", "форелей", "скумбрий",
                            // TODO: Добавить еще 11 строк по 10 элементов
                        };

            verbs = new[]
                        {
                            "ускоряются", "разгоняются", "скучиваются", "собираются", "действуют", "приспосабливаются", "адаптируются", "советуются", "консультируются", "выстраиваются",
                            "реагируют", "спорят", "оценивают", "помогают", "следят", "заботятся", "запоминают", "успокаиваются", "объединяются", "творят",
                            // TODO: Добавить еще 11 строк по 10 элементов
                        };

            adverbs = new[]
                          {
                              "нежно", "бдительно", "безапеляционно", "беззаветно", "безропотно", "безупречно", "горячо", "пристально", "нагло", "необыкновенно",
                              "основательно", "остро", "плотно", "свято", "страстно", "фанатично", "хорошо", "нормально", "широко", "ярко",
                              // TODO: Добавить еще 11 строк по 10 элементов
                          };
        }

        public string GenerateRandomPhrase()
        {
            var random = new Random();
            var number = random.Next(5, 20);

            var adjective = adjectives[random.Next(adjectives.Length)];
            var noun = nouns[random.Next(nouns.Length)];
            var verb = verbs[random.Next(verbs.Length)];
            var adverb = adverbs[random.Next(adverbs.Length)];

            return string.Join(" ", number, adjective, noun, verb, adverb);
        }
    }
}
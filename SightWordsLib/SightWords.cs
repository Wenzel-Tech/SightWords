using Newtonsoft.Json;
using SimpleNLG;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SightWordsLib
{
    
    public class SightWords
    {
        private static Random Random = new Random();
        public Dictionary<string, List<WordElement>> WordsByDolchClass { get; set; }
        public Dictionary<LexicalCategoryEnum, List<WordElement>> WordsByCategory{get;set;}
        public SightWords()
        {
            GetAndProcessWordsAsync().Wait();
        }

        public List<WordElement> Verbs
        {
            get
            {
                return WordsByCategory[LexicalCategoryEnum.VERB];
            }
        }
        public List<WordElement> Nouns
        {
            get
            {
                return WordsByCategory[LexicalCategoryEnum.NOUN];
            }
        }
        public List<WordElement> Adjectives
        {
            get
            {
                return WordsByCategory[LexicalCategoryEnum.ADJECTIVE];
            }
        }
        public List<WordElement> Adverbs
        {
            get
            {
                return WordsByCategory[LexicalCategoryEnum.ADVERB];
            }
        }
        public List<WordElement> Pronouns
        {
            get
            {
                return WordsByCategory[LexicalCategoryEnum.PRONOUN];
            }
        }
        public List<WordElement> Prepositions
        {
            get
            {
                return WordsByCategory[LexicalCategoryEnum.PREPOSITION];
            }
        }
        public List<WordElement> Determiners
        {
            get
            {
                return WordsByCategory[LexicalCategoryEnum.DETERMINER];
            }
        }


        public WordElement Verb
        {
            get
            {
                return GetRandomWord(LexicalCategoryEnum.VERB);
            }
        }
        public WordElement Noun
        {
            get
            {
                return GetRandomWord(LexicalCategoryEnum.NOUN);
            }
        }
        public WordElement Adjective
        {
            get
            {
                return GetRandomWord(LexicalCategoryEnum.ADJECTIVE);
            }
        }
        public WordElement Adverb
        {
            get
            {
                return GetRandomWord(LexicalCategoryEnum.ADVERB);
            }
        }
        public WordElement Pronoun
        {
            get
            {
                return GetRandomWord(LexicalCategoryEnum.PRONOUN);
            }
        }
        public WordElement Preposition
        {
            get
            {
                return GetRandomWord(LexicalCategoryEnum.PREPOSITION);
            }
        }
        public WordElement Determiner
        {
            get
            {
                return GetRandomWord(LexicalCategoryEnum.DETERMINER);
            }
        }

        public Phrase GetRandomSentence( WordElement word = null )
        {
            if (word == null)
                word = GetRandomWord();

            return new Phrase(this, word);
        }
        public WordElement GetRandomWord( string dolchClass = null )
        {
            if( string.IsNullOrEmpty(dolchClass) )
            {
                dolchClass = PickRandomDolchClass();
            }
            var list = WordsByDolchClass[dolchClass];
            return list[Random.Next(0, list.Count - 1)];
        }

        public WordElement GetRandomWord( LexicalCategoryEnum category)
        {
            
            var list = WordsByCategory[category];
            return list[Random.Next(0, list.Count - 1)];
        }

        private async Task GetAndProcessWordsAsync()
        {
            ProcessWords(await GetRawWordsAsync());
        }
        private string PickRandomDolchClass()
        {
          
            return WordsByDolchClass.Keys.ToArray()[ Random.Next( 0, WordsByDolchClass.Keys.Count()-1 ) ];
        }
        private async Task<Dictionary<string, List<string>>> GetRawWordsAsync()
        {
            using (var reader = new StreamReader(@"Data/Words.json"))
            {
                var jsonText = await reader.ReadToEndAsync();
                return await JsonConvert.DeserializeObjectAsync<Dictionary<string, List<string>>>(jsonText);
            }
        }
        private void ProcessWords( Dictionary<string, List<string>> rawWords )
        {
            WordsByDolchClass = new Dictionary<string, List<WordElement>>();
            WordsByCategory = new Dictionary<LexicalCategoryEnum, List<WordElement>>();

            var lexicon = Lexicon.getDefaultLexicon();

            foreach (var key in rawWords.Keys)
            {
                var list = new List<WordElement>();
                WordsByDolchClass[key] = list;

                foreach (var rawWord in rawWords[key])
                {
                    foreach (var word in lexicon.getWords(rawWord))
                    {
                        list.Add(word);
                        var category = (LexicalCategoryEnum)word.getCategory().enumType;

                        if (!WordsByCategory.ContainsKey(category))
                            WordsByCategory[category] = new List<WordElement>();

                        WordsByCategory[category].Add(word);
                    }
                }

            }
        }
    }
}

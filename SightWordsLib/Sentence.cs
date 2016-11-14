using SimpleNLG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SightWordsLib
{
    public class Phrase
    {
        private static Random Random = new Random((int)DateTime.Now.Ticks);
        private static Lexicon Lexicon = Lexicon.getDefaultLexicon();
        private SPhraseSpec SentenceSpec { get; set; }
        public Phrase(SightWords words, WordElement seed)
        {
            var factory = new NLGFactory(Lexicon);

            SentenceSpec = factory.createClause();

            var category = (LexicalCategoryEnum)seed.getCategory().enumType;

            NPPhraseSpec subject = null;
            VPPhraseSpec verb    = null;
            PPPhraseSpec complement = null;
            switch( category )
            {
                case LexicalCategoryEnum.PRONOUN:
                    var v = words.Verb;
                    Console.Out.WriteLine("PRONOUN: {0}, {1} VERB: {2}, {3}", seed.getBaseForm(), seed.id, v.getBaseForm(), v.id);
                    subject = factory.createNounPhrase( seed );
                    
                    verb = factory.createVerbPhrase( v );
                    break;
                case LexicalCategoryEnum.NOUN:
                    
                    subject = factory.createNounPhrase( words.Determiner, seed);
                    //verb = words.Verb;
                    break;
                case LexicalCategoryEnum.VERB:

                    verb = factory.createVerbPhrase( seed );
                    subject = factory.createNounPhrase( words.Pronoun );
 
                    break;

                case LexicalCategoryEnum.ADJECTIVE:
                    subject = factory.createNounPhrase(words.Determiner, words.Noun);
                    //verb = words.Verb;
                    ((NPPhraseSpec)subject).addModifier(seed);
                   

                    break;

                case LexicalCategoryEnum.DETERMINER:
                    
                    subject = factory.createNounPhrase( seed, words.Noun );
                    //verb = words.Verb;
                    break;
                
                case LexicalCategoryEnum.ADVERB:
                    subject = factory.createNounPhrase( words.Pronoun );
                    verb = factory.createVerbPhrase(words.Verb);
                    ((VPPhraseSpec)verb).addPreModifier(seed);
                    break;

                case LexicalCategoryEnum.PREPOSITION:
                  
                    complement = factory.createPrepositionPhrase();
                    complement.setComplement(factory.createNounPhrase(words.Determiner, words.Noun));
                    complement.setPreposition(seed);

                    break;
                 
                case LexicalCategoryEnum.CONJUNCTION:

                    var subject1 = factory.createNounPhrase(words.Determiner, words.Noun);
                    var subject2 = factory.createNounPhrase(words.Determiner, words.Noun);
                    var coord = factory.createCoordinatedPhrase(subject1, subject2);
                    coord.setFeature(Feature.CONJUNCTION.ToString(), seed);

                     break;
                case LexicalCategoryEnum.MODAL:
                     break;
                default:
                    
                break;

            }

            if (subject != null)
                SentenceSpec.setSubject(subject);

            if (verb != null)
                SentenceSpec.setVerb(verb);
 
            //if (complement != null)
            //    SentenceSpec.setComplement(complement);

            //SentenceSpec.setSubject("me");
            //SentenceSpec.setVerb("ride");
           // var tense = (Tense)Random.Next(0, 2);
            //SentenceSpec.setFeature(Feature.TENSE.ToString(), tense);
        }

        public override string ToString()
        {
            Realiser realizer = new Realiser();
            return realizer.realiseSentence(SentenceSpec);
        }
    }
}

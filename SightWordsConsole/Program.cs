using SightWordsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SightWordsConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                var sightWords = new SightWords();
                var sentence = sightWords.GetRandomSentence();
                Console.Out.WriteLine(sentence);

            }
            while (Console.ReadLine() != "exit");
           

            
        }
    }
}

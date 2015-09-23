using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Threading;
using System.IO;



namespace ConsoleApplication
{
    /*
     * Test console application program
     * 
     * @category C#
     * @author Ekaterina Bizimova
     * 
     */

    public class DataProcess
    {

        private static SortedDictionary<string, Dictionary<string, int>> sourceDictionaryWords;
        private static string[] sourceUserWords;

        /*
         * Get source dictionary words
         * 
         * @return SortedDictionary<string, Dictionary<string, int>> Source Dictionary Words
         */

        public static SortedDictionary<string, Dictionary<string, int>> SourceDictionaryWords
        {
            get
            {
                int wordCount = Convert.ToInt32(Console.ReadLine());
                try
                {
                    ValidateNewNumericValue(wordCount, 1, Math.Pow(10, 5));
                }
                catch (Exception e)
                {
                    ErrorProcess(Convert.ToString(e));
                } //end try

                var words = new SortedDictionary<string, int>();

                for (var i = 0; i < (int)wordCount; i++)
                {
                    try
                    {
                        string word = Console.ReadLine();
                        string[] parsedWord = ParseDictionaryData(word);

                        words.Add(parsedWord[0], Convert.ToInt32(parsedWord[1]));
                    }
                    catch (Exception error)
                    {
                        ErrorProcess(Convert.ToString(error));
                    }
                } //end for

                SortedDictionary<string, Dictionary<string, int>> sortedWords = SortDictionaryData((int)wordCount, words);

                return sortedWords;
            } //end get
        } //end SortedDictionary


        /*
         * Get source user words
         * 
         * @return string[] Source user words
         */

        public static string[] SourceUserWords
        {
            get
            {
                int userStringWordCount = Convert.ToInt32(Console.ReadLine());
                try
                {
                    ValidateNewNumericValue(userStringWordCount, 1, 15000);
                }
                catch (Exception e)
                {
                    ErrorProcess(Convert.ToString(e));
                } //end try

                var userStringWords = new string[userStringWordCount];
                try
                {
                    for (int i = 0; i < userStringWordCount; i++)
                    {
                        string userString = Console.ReadLine();
                        string checkedUserString = CheckUserData(userString);
                        userStringWords[i] = checkedUserString;
                    }
                }
                catch (Exception e)
                {
                    ErrorProcess(Convert.ToString(e));
                } //end try

                return userStringWords;
            } //end get
        } //end SourceUserWords


        /*
        * Parse dictionary data
        *  
        * @param string[] dictionaryString Dictionary string
        * 
        * @return string[] Array of parsed dictionary words
        */

        public static string[] ParseDictionaryData(string dictionaryString)
        {
            Match ReqMatch = Regex.Match(dictionaryString, @"^[a-z]{1,15} \d");
            if (ReqMatch == Match.Empty)
            {
                throw new Exception("Bad string");
            }

            string[] ReadValue = Regex.Split(dictionaryString, " ");
            ValidateNewNumericValue(Convert.ToInt32(ReadValue[1]), 1, Math.Pow(10, 6));
            var result = new string[2];
            return ReadValue;
        } //end ParseDictionaryData()


        /*
        * Sort dictionary data
        *  
        * @param int                           wordCount Word count
        * @param SortedDictionary<string, int> words     Parsed dictionary words
        * 
        * @return SortedDictionary<string, Dictionary<string, int>> Array of sorted dictinary words
        */

        public static SortedDictionary<string, Dictionary<string, int>> SortDictionaryData(int wordCount, SortedDictionary<string, int> words)
        {
            var items = from pair in words
                        orderby pair.Value descending
                        select pair;

            var sortedDictionary = new SortedDictionary<string, Dictionary<string, int>>();

            foreach (var kvp in items)
            {
                for (var i = 1; i <= kvp.Key.Length; i++)
                {
                    string subString = kvp.Key.Substring(0, i);
                    if (sortedDictionary.ContainsKey(subString) == false)
                    {
                        sortedDictionary.Add(subString, new Dictionary<string, int>());
                    }
                    else if (sortedDictionary[subString].Count == 10)
                    {
                        break;
                    }
                    sortedDictionary[subString].Add(kvp.Key, kvp.Value);
                } //end for
            } //end foreach
            return sortedDictionary;
        }//end SortDictionaryData()


        /*
        * Check user data
        *  
        * @param string userString
        * 
        * @return string Checked user string
        */

        public static string CheckUserData(string userString)
        {
            Match ReqMatch = Regex.Match(userString, @"^[a-z]{1,15}$");
            if (ReqMatch == Match.Empty)
            {
                throw new Exception("Bad string");
            }
            return userString;
        } //end CheckUserData()


        /*
        * Validate new numeric value
        *  
        * @param int    value Value
        * @param int    low   Low limit
        * @param double top   Top limit
        * 
        * @return void
        */

        private static void ValidateNewNumericValue(Int32 value, Int32 low, double top)
        {
            if (value < low || value > top)
            {
                throw new Exception(string.Format("Value in range [{0}-{1}] expected", low, top));
            }
        } //end ValidateNewNumericValue()


        /*
         * Start application
         * 
         * @param string[] args Arguments for console application
         * 
         * @return void
         */

        static void Main(string[] args)
        {
            try
            {
                sourceDictionaryWords = SourceDictionaryWords;
                sourceUserWords = SourceUserWords;

                foreach (var userString in sourceUserWords)
                {
                    foreach (var res in Process(sourceDictionaryWords, userString))
                    {
                        Console.WriteLine(res);
                    }
                    Console.WriteLine();
                }
                Console.Read(); 
            }
            catch (Exception e)
            {
                ErrorProcess(Convert.ToString(e));
            }
        } //end Main()


        /*
         * Data process function
         * 
         * @param SortedDictionary<string, Dictionary<string, int>> sourceDictionaryWords Source dictionary words
         * @param string                                            userString            Source user string
         * 
         * @return string[]
         */

        public static string[] Process(SortedDictionary<string, Dictionary<string, int>> sourceDictionaryWords, string userString)
        {
            //       var tr = new StreamWriter("file.out");
            var result = new string[10];

            if (sourceDictionaryWords.ContainsKey(userString) == true)
            {
                var i = 0;
                foreach (var res in sourceDictionaryWords[userString])
                {
                    result[i] = res.Key;
                    i++;
                }
            }
            return result;
        } //end Process()


        /*
         * Error process
         * 
         * @param string error Error text
         * 
         * @return void
         */

        public static void ErrorProcess(string error)
        {
            Console.WriteLine(error);
            Console.WriteLine("There is an error in source data. Please check the input data.");
            Console.WriteLine("Press the Escape (Esc) key to quit: \n");
            ConsoleKeyInfo cki;
            do
            {
                cki = Console.ReadKey();
                Console.Write(" --- You pressed ");
                if ((cki.Modifiers & ConsoleModifiers.Alt) != 0) Console.Write("ALT+");
                if ((cki.Modifiers & ConsoleModifiers.Shift) != 0) Console.Write("SHIFT+");
                if ((cki.Modifiers & ConsoleModifiers.Control) != 0) Console.Write("CTL+");
                Console.WriteLine(cki.Key.ToString());
            }
            while (cki.Key != ConsoleKey.Escape);
            return;
        } //end ErrorProcess() 
    } //end class
} //end namespace


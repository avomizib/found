using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConsoleApplication;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Diagnostics;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            TextReader tr = new StreamReader("test.in");
            int wordCount = Convert.ToInt32(tr.ReadLine());
            SortedDictionary<string, int> words = new SortedDictionary<string, int>();

            for (int i = 0; i < (int)wordCount; i++)
            {
                string word = tr.ReadLine();
                string[] readValue = DataProcess.ParseDictionaryData(word);

                words.Add(readValue[0], Convert.ToInt32(readValue[1]));
            }

            SortedDictionary<string, Dictionary<string, int>> result = DataProcess.SortDictionaryData(wordCount, words);

            int userStringWordCount = Convert.ToInt32(tr.ReadLine());

            string[] userStringWords = new string[userStringWordCount];

            for (int i = 0; i < userStringWordCount; i++)
            {
                string userString = tr.ReadLine();
                string checkedUserString = DataProcess.CheckUserData(userString);
                userStringWords[i] = checkedUserString;
            }
            foreach (var userString in userStringWords)
            {
                DataProcess.Process(result, userString);
            }

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Assert.AreEqual("00:00:01.00", elapsedTime);

        }
    } //end class
}

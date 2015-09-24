using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.IO;
using ConsoleApplication;
using System.Text.RegularExpressions;


namespace Tcpserver
{
    /*
      * Test console application program for tcp server
      * 
      * @category C#
      * @author Ekaterina Bizimova
      * 
      */

    class Client
    {

        /*
         * Client process function
         * 
         * @param TcpClient Client Tcp client
         * 
         * @return SortedDictionary<string, SortedList<string, int>> Source Dictionary Words
         */

        public Client(TcpClient Client)
        {
            string request;
            byte[] buffer = new byte[1024];
            int count;

            while ((count = Client.GetStream().Read(buffer, 0, buffer.Length)) > 0)
            {
                try
                {
                    request = Encoding.ASCII.GetString(buffer, 0, count);
                    Console.WriteLine("User request:");

                    char[] charsToTrim = { ' ' };
                    Match ReqMatch = Regex.Match(request, @"^get \w");
                    if (ReqMatch == Match.Empty)
                    {
                        throw new Exception("Bad request");
                    }
                    string processedRequest = request.Substring(4, request.Length - 4).Trim(charsToTrim);

                    Console.WriteLine(processedRequest);
                    string result = GetData(processedRequest);
                    byte[] replyBuffer = Encoding.ASCII.GetBytes(result);
                    Client.GetStream().Write(replyBuffer, 0, replyBuffer.Length);
                    Console.WriteLine("Reply sent");
                }
                catch (Exception e)
                {
                    byte[] ErrorString = Encoding.ASCII.GetBytes("Internal server error.Connection closed");
                    Console.WriteLine(e.Message);
                    Client.GetStream().Write(ErrorString, 0, ErrorString.Length);
                    break;
                } //end try
            } //end while
            Client.Close();
        } //end Client()


        /*
         * Data process function
         * 
         * @param string userRequest User request
         * 
         * @return string Dictionary words for user request
         */

        private string GetData(string userRequest)
        {
            try
            {
                TextReader tr = new StreamReader(Server.DictionaryPath);
                int wordCount = Convert.ToInt32(tr.ReadLine());

                SortedDictionary<string, int> words = new SortedDictionary<string, int>();

                for (int i = 0; i < (int)wordCount; i++)
                {
                    string word = tr.ReadLine();
                    string[] readValue = DataProcess.ParseDictionaryData(word);

                    words.Add(readValue[0], Convert.ToInt32(readValue[1]));
                } //end for

                SortedDictionary<string, Dictionary<string, int>> result = DataProcess.SortDictionaryData(wordCount, words);
                string resultString = "";

                foreach (string s in DataProcess.Process(result, userRequest))
                {
                    resultString = resultString + s + "\n";
                } //end foreach

                return resultString;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            } //end try
        } //end getData()


    } //end class


    class Server
    {

        public static string DictionaryPath;
        TcpListener Listener;

        /*
         * Server process function
         * 
         * @param int Port Port for connection
         * 
         * @return void
         */

        public Server(int port)
        {
            Listener = new TcpListener(IPAddress.Any, port);
            Listener.Start();

            while (true)
            {
                Console.WriteLine("Success");
                TcpClient Client = Listener.AcceptTcpClient();
                var Thread = new Thread(new ParameterizedThreadStart(ClientThread));
                Thread.Start(Client);
            } //end while
        } //end Server()


        /*
         * Function called in each client thread
         * 
         * @param Object StateInfo
         * 
         * @return void
         */

        static void ClientThread(Object StateInfo)
        {
            new Client((TcpClient)StateInfo);
        } //end ClientThread()

        ~Server()
        {
            if (Listener != null)
            {
                Listener.Stop();
            }
        }


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
                Console.Write("Port to listen: ");
                int port = Convert.ToInt32(Console.ReadLine());
                Console.Write("Path to dictionary: ");
                string path = Console.ReadLine();
                DictionaryPath = path;
                Console.WriteLine("Creating server...");
                new Server(port);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            } //end try
        } //end Main()
    }
}


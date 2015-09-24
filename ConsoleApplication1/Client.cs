using System;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Text.RegularExpressions;


namespace Tcpclient
{
    /*
       * Test console application program for client
       * 
       * @category C#
       * @author Ekaterina Bizimova
       * 
       */

    class Client
    {

        /*
         * Start application
         * 
         * @param string[] args Arguments for console application
         * 
         * @return void
        */

        static void Main(string[] args)
        {
            string data;

            TcpClient client = new TcpClient();

            Console.Write("IP or host to connect to: ");
            string hostname = Console.ReadLine();
            IPHostEntry host;
            host = Dns.GetHostEntry(hostname);

            Console.Write("\r\nPort: ");
            int port = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Connecting to server...");

            try
            {
                client.Connect(host.AddressList[1], port);
            }
            catch
            {
                Console.WriteLine("Cannot connect to remote host!");
                return;
            }

            Console.Write("done\r\nFor client request type: get <prefix>\r\nTo end, type 'END'");
            Socket Sock = client.Client;

            while (true)
            {
                try
                {
                    Console.Write("\r\n>");
                    data = Console.ReadLine();

                    if (data == "END")
                    {
                        break;
                    }

                    Match ReqMatch = Regex.Match(data, @"^get \w");

                    if (ReqMatch == Match.Empty)
                    {
                        throw new Exception("Bad request");
                    }

                    byte[] HeadersBuffer = Encoding.ASCII.GetBytes(data);
                    client.GetStream().Write(HeadersBuffer, 0, HeadersBuffer.Length);

                    byte[] Buffer = new byte[1024];
                    int count;
                    string reply;
                    count = client.GetStream().Read(Buffer, 0, Buffer.Length);
                    Console.WriteLine("Server reply:");
                    reply = Encoding.ASCII.GetString(Buffer, 0, count);
                    Console.WriteLine(reply);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Connection closed");
                    Console.Read();
                    break;
                } //end try
            } //end while
            Sock.Close();
            client.Close();
        } //end Main()
    } //end class
} //end namespace


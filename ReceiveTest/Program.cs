using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ReceiveTest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Test the receive API method
                var request = WebRequest.Create("http://climbing.pororeplays.com:23122/message");
                request.Method = "POST";
                using(var stream = request.GetRequestStream())
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write("test");
                }
                var response = request.GetResponse();
                Console.WriteLine("Sent request to server.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
        }
    }
}

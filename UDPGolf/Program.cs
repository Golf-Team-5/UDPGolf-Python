using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UDPGolf
{
    class Program
    {

        static string ServerURI = "http://localhost:52549/api/swingdata/";

        static void Main(string[] args)
        {
            //opsætning af UDP socket
            //OBS: husk at ændre IP hvis nødvendigt!
            UdpClient udpGolfServer = new UdpClient(6789);
            IPAddress ip = IPAddress.Parse("192.168.104.124");
            IPEndPoint golfIpEndPoint = new IPEndPoint(ip, 6789);
            
            try
            { Console.WriteLine("Server is waiting for input");

                while (true)
                {
                    //modtagelse af data fra Raspberry Pi'en, som så printes ud i konsollen. 
                    Byte[] receivedData = udpGolfServer.Receive(ref golfIpEndPoint);
                    string receivedSwingData = Encoding.ASCII.GetString(receivedData);
                    Console.WriteLine("Received Swing Data is: " + receivedSwingData + " km/t.");


                    //her kalder vi vores metode der sender SwingDataen til RestServicen
                    PostSwingData(new SwingData(Convert.ToDouble(receivedSwingData)));
                    
                }
            }
            catch (Exception e)
            {
                //hvis noget går galt udskrives exceptionen her
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// This method takes our SwingData and sends it to our RestService as a JSON-string, and returns the response message.
        /// </summary>
        /// <param name="swingData"></param>
        /// <returns></returns>
        public static async Task<string> PostSwingData(SwingData swingData)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    //Metoden tager vores swingData og konverterer objektet til en Json-String, og pakker den derefter ind i en httpContent pakke,
                    //som den sender til vores RestService, og derefter returnerer en response message som en string.
                    var jsonString = JsonConvert.SerializeObject(swingData);
                    StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(ServerURI, content);
                    string str = await response.Content.ReadAsStringAsync();
                    return str;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    
                }

                return null;
            }
        }

    }
}

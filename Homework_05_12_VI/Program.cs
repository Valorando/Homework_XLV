using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Homework_05_12_VI
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //3 задание - клиент

            IPAddress serverIP = IPAddress.Parse("127.0.0.1");
            int serverPort = 8888;

            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                await client.ConnectAsync(new IPEndPoint(serverIP, serverPort));
                Console.WriteLine("Подключено к серверу");

                string message = "Привет, сервер!";
                byte[] data = Encoding.UTF8.GetBytes(message);
                await client.SendAsync(new ArraySegment<byte>(data), SocketFlags.None);

                data = new byte[256];
                int bytesReceived = await client.ReceiveAsync(new ArraySegment<byte>(data), SocketFlags.None);
                string response = Encoding.UTF8.GetString(data, 0, bytesReceived);
                DateTime dt = DateTime.Now;
                TimeSpan ts = dt.TimeOfDay;
                Console.WriteLine($"В {ts} от [{((IPEndPoint)client.RemoteEndPoint).Address}] получена строка: {response} ");

                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            finally
            {
                client.Shutdown(SocketShutdown.Both);
                client.Close();
                Console.ReadLine();
            }
        }
    }
}

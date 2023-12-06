using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Homework_05_12_VIII
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //Задание 4 - клиент

            IPAddress serverIP = IPAddress.Parse("127.0.0.1");
            int serverPort = 8888;

            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                await client.ConnectAsync(new IPEndPoint(serverIP, serverPort));
                Console.WriteLine("Подключено к серверу");

                Console.WriteLine("/time - показать текущее время /date - показать текущую дату");
                Console.Write("Введите команду: ");
                string command = Console.ReadLine();
                byte[] data = Encoding.UTF8.GetBytes(command);
                await client.SendAsync(new ArraySegment<byte>(data), SocketFlags.None);

                data = new byte[256];
                int bytesReceived = await client.ReceiveAsync(new ArraySegment<byte>(data), SocketFlags.None);
                string response = Encoding.UTF8.GetString(data, 0, bytesReceived);
                Console.WriteLine(response);
                client.Shutdown(SocketShutdown.Both);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            finally
            {
                
                client.Close();
                Console.ReadLine();
            }
        }
    }
}

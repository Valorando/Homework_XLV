using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Homework_05_12_IV
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //2 задание клиент
            IPAddress serverIP = IPAddress.Parse("127.0.0.1");
            int serverPort = 8888;

            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                client.Connect(new IPEndPoint(serverIP, serverPort));
                Console.WriteLine("Подключено к серверу");
                Console.WriteLine("/time - текущее время, /date - текущая дата");
                Console.Write("Введите команду: ");
                string command = Console.ReadLine();

                byte[] data = Encoding.UTF8.GetBytes(command);
                client.Send(data);

                data = new byte[256];
                int bytes = client.Receive(data);
                command = Encoding.UTF8.GetString(data, 0, bytes);
                Console.WriteLine(command);

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

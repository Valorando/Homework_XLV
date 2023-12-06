using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;


namespace Homework_05_12_III
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //2 задание - сервер
            try
            {
                IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
                int port = 8888;

                Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                listener.Bind(new IPEndPoint(ipAddress, port));
                listener.Listen(10);
                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                Socket client = listener.Accept();

                byte[] data = new byte[256];
                int bytes = client.Receive(data);
                string message = Encoding.UTF8.GetString(data, 0, bytes);

                if (message == "/date")
                {
                    byte[] response = Encoding.UTF8.GetBytes($"Текущая дата: {DateTime.Today}");
                    client.Send(response);
                }

                if (message == "/time")
                {
                    DateTime dt = DateTime.Now;
                    TimeSpan ts = dt.TimeOfDay;
                    byte[] response = Encoding.UTF8.GetBytes($"Текущее время: {ts}");
                    client.Send(response);
                }

                else
                {
                    byte[] response = Encoding.UTF8.GetBytes("Команда не распознана.");
                    client.Send(response);
                }

                client.Shutdown(SocketShutdown.Both);
                client.Close();
                listener.Close();
                Console.ReadLine();
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}

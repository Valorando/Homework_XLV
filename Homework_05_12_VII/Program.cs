using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Homework_05_12_VII
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //Задание 4 - сервер
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            int port = 8888;

            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(ipAddress, port));
            listener.Listen(10);
            Console.WriteLine("Сервер запущен. Ожидание подключений...");

            while (true)
            {
                Socket client = await listener.AcceptAsync();
                _ = Task.Run(() => ProcessClient(client));
            }
        }

        static async Task ProcessClient(Socket client)
        {
            
            byte[] data = new byte[256];
            int bytes = await client.ReceiveAsync(new ArraySegment<byte>(data), SocketFlags.None);
            string message = Encoding.UTF8.GetString(data, 0, bytes);

            if (message == "/time")
            {

                DateTime dt = DateTime.Now;
                TimeSpan ts = dt.TimeOfDay;
                byte[] response = Encoding.UTF8.GetBytes($"Текущее время: {ts}");
                await client.SendAsync(new ArraySegment<byte>(response), SocketFlags.None);
            }

            if (message == "/date")
            {

                byte[] response = Encoding.UTF8.GetBytes($"Текущая дата: {DateTime.Today}");
                await client.SendAsync(new ArraySegment<byte>(response), SocketFlags.None);
            }

            else
            {
                byte[] response = Encoding.UTF8.GetBytes("Команда не распознана");
                await client.SendAsync(new ArraySegment<byte>(response), SocketFlags.None);
            }

            client.Shutdown(SocketShutdown.Both);
            client.Close();
        }
    }
}

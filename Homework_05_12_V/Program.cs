using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Homework_05_12_V
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //3 Задание - сервер
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

            DateTime dt = DateTime.Now;
            TimeSpan ts = dt.TimeOfDay;
            Console.WriteLine($"В {ts} от [{((IPEndPoint)client.RemoteEndPoint).Address}] получена строка: {message} ");


            byte[] response = Encoding.UTF8.GetBytes("Привет, клиент!");
            await client.SendAsync(new ArraySegment<byte>(response), SocketFlags.None);

            client.Shutdown(SocketShutdown.Both);
            client.Close();
            Console.ReadLine();
        }
    }
}

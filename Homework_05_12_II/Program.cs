﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;


namespace Homework_05_12_II
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //1 задание - клиент
            IPAddress serverIP = IPAddress.Parse("127.0.0.1");
            int serverPort = 8888;

            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                
                client.Connect(new IPEndPoint(serverIP, serverPort));
                Console.WriteLine("Подключено к серверу");

                string message = "Привет, сервер!";
                byte[] data = Encoding.UTF8.GetBytes(message);
                client.Send(data);

                data = new byte[256];
                int bytes = client.Receive(data);
                message = Encoding.UTF8.GetString(data, 0, bytes);

                DateTime dt = DateTime.Now;
                TimeSpan ts = dt.TimeOfDay;
                Console.WriteLine($"В {ts} от [{((IPEndPoint)client.RemoteEndPoint).Address}] получена строка: {message} ");
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

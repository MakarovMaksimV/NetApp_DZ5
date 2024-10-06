using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Seminar5;
using Seminar5.Models;

namespace Seminar5
{
    public class Server
    {
        // Словарь для хранения адресов клиентов по их именам
        Dictionary<String, IPEndPoint> clients = new Dictionary<string, IPEndPoint>(); // Объект для работы с UDP-сокетом
        UdpClient udpClient;
        // Метод для обработки регистрации нового клиента
        void Register(MessagesUDP message, IPEndPoint fromep)
        {
            Console.WriteLine("Message Register, name = " + message.FromName);
            // Добавляем клиента в словарь
            clients.Add(message.FromName, fromep);
            // Добавляем пользователя в базу данных, если его еще нет
            using (var ctx = new Context())
            {
                if (ctx.Users.FirstOrDefault(x => x.Name == message.FromName) != null) return;
                ctx.Add(new User { Name = message.FromName });
                ctx.SaveChanges();
            }
        }
        // Метод для подтверждения получения сообщения
        void ConfirmMessageReceived(int? id)
        {
            Console.WriteLine("Message confirmation id=" + id);
            // Изменяем статус получения сообщения в базе данных
            using (var ctx = new Context())
            {
                var msg = ctx.Massages.FirstOrDefault(x => x.Id == id);
                if (msg != null)
                {
                    msg.Received = true;
                    ctx.SaveChanges();
                }
            }
        }
        // Метод для пересылки сообщения
        void RelyMessage(MessagesUDP message)
        {
            int? id = null;
            if (clients.TryGetValue(message.ToName, out IPEndPoint ep))
            {
                // Добавляем сообщение в базу данных
                using (var ctx = new Context())
                {
                    var fromUser = ctx.Users.First(x => x.Name == message.FromName);
                    var toUser = ctx.Users.First(x => x.Name == message.ToName);
                    var msg = new Seminar5.Models.Messages
                    {
                        FromUser = fromUser,
                        ToUser = toUser,
                        Received = false,
                        Text = message.Text
                    }; ctx.Massages.Add(msg);
                    ctx.SaveChanges();
                    id = msg.Id;
                }
                // Подготавливаем сообщение для пересылки
                var forwardMessageJson = new MessagesUDP() { Id = id, Command = Command.Message, ToName = message.ToName, FromName = message.FromName, Text = message.Text }.ToJson();
                byte[] forwardBytes = Encoding.ASCII.GetBytes(forwardMessageJson);
                // Отправляем сообщение клиенту
                udpClient.Send(forwardBytes, forwardBytes.Length, ep);
                Console.WriteLine($"Message Relied, from = {message.FromName} to = {message.ToName}");
            }
            else
            {
                Console.WriteLine("Пользователь не найден.");
            }
        }
        // Метод для обработки полученного сообщения
        void ProcessMessage(MessagesUDP message, IPEndPoint fromep)
        {
            Console.WriteLine($"Получено сообщение от {message.FromName} для {message.ToName} с командой " +
                $"{message.Command}:");
        Console.WriteLine(message.Text);
            // Обработка в зависимости от команды сообщения
            if (message.Command == Command.Register)
            {
                Register(message, new IPEndPoint(fromep.Address, fromep.Port));
            }
            if (message.Command == Command.Confirmation)
            {
                Console.WriteLine("Confirmation receiver");
                ConfirmMessageReceived(message.Id);
            }
            if (message.Command == Command.Message)
            {
                RelyMessage(message);
            }
        }

        // Метод для запуска работы сервера
        public void Work()
        {
            // Инициализация объекта для приема данных по UDP
            IPEndPoint remoteEndPoint;
            udpClient = new UdpClient(12345);
            remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            Console.WriteLine("UDP Клиент ожидает сообщений...");
            // Бесконечный цикл приема сообщений
            while (true)
            {
                byte[] receiveBytes = udpClient.Receive(ref remoteEndPoint); string receivedData = Encoding.ASCII.GetString(receiveBytes);
                Console.WriteLine(receivedData);
                try
                {
                    // Десериализация полученного сообщения
                    var message = MessagesUDP.FromJson(receivedData);
                    // Обработка сообщения
                    ProcessMessage(message, remoteEndPoint);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка при обработке сообщения: " + ex.Message);
                }
            }
        }
    }
}


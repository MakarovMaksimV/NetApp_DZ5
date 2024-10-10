using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Seminar5.Abstraction;

namespace Seminar5
{
	public class MessageSource : IMessageSource
    {
        private readonly UdpClient udpClient;

		public MessageSource(int port)
		{
            udpClient = new UdpClient(port);
		}

        public MessagesUDP Resive(ref IPEndPoint iPEnd)
        {
            byte[] buffer = udpClient.Receive(ref iPEnd);
            string messageJson = Encoding.Default.GetString(buffer);
            return MessagesUDP.FromJson(messageJson);
        }

        public void Send(MessagesUDP messages, IPEndPoint iPEnd)
        {
            string ms = messages.ToJson();
            byte[] bytes = Encoding.Default.GetBytes(ms);
            udpClient.Send(bytes, iPEnd);
        }
    }
}


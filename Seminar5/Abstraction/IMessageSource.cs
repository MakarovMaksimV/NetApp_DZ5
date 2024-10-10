using System;
using System.Net;

namespace Seminar5.Abstraction
{
	public interface IMessageSource
	{
		public void Send(MessagesUDP messages, IPEndPoint iPEnd);
		public MessagesUDP Resive(ref IPEndPoint iPEnd);
    }

	
}


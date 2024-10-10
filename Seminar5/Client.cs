using System;
using System.Net;
using Seminar5.Abstraction;

namespace Seminar5
{
	public class Client
	{
		private readonly string name;

		private readonly IMessageSource messageSource;
		private readonly IPEndPoint iPEnd;

        public Client(IMessageSource messageSource, IPEndPoint iPEnd, string name)
		{
			this.messageSource = messageSource;
			this.iPEnd = iPEnd;
			this.name = name;
		}

		private void Register()
		{
			var messagejson = new MessagesUDP()
			{
				Command = Command.Register,
				FromName = name
			};

			messageSource.Send(messagejson,iPEnd);
		}

		public void ClientSendler()
		{
			Register();
			while(true)
			{
				Console.Write("Кому: ");
                string? toName = Console.ReadLine();
                Console.Write("Сообщение: ");
				string? massage = Console.ReadLine();

				MessagesUDP messagesUDP = new MessagesUDP()
				{
                    FromName = name,
					ToName = toName,
					Text = massage
                };
                messageSource.Send(messagesUDP, iPEnd);
            }

		}

        public void ClientListener()
        {
            Register();
			IPEndPoint iP = new IPEndPoint(iPEnd.Address,iPEnd.Port);

            while (true)
			{
				Console.WriteLine(messageSource.Resive(ref iP).ToString());
				

			}
        }

    }
}


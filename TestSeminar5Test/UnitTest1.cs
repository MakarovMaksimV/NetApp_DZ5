using System.Net;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Seminar5;
using Seminar5.Abstraction;

namespace TestSeminar5Test;

public class Tests
{

    IMessageSource source;
    IPEndPoint iP;

    [SetUp]
    public void Setup()
    {
        iP = new IPEndPoint(IPAddress.Any, 0);
    }

    [Test]
    public void TestResiveMessage()
    {
        source = new MockMessageSource();
        var result = source.Resive(ref iP);

        Assert.IsNotNull(result);
        Assert.IsNull(result.Text);
        Assert.IsNotNull(result.FromName);
        Assert.That("Вася",Is.EqualTo(result.FromName));
        Assert.That(Command.Register, Is.EqualTo(result.Command));

    }
}
public class MockMessageSource : IMessageSource
{

    private Queue<MessagesUDP> messages = new();
    public MockMessageSource()
    {
        messages.Enqueue(new MessagesUDP { Command = Command.Register, FromName = "Вася" });
        messages.Enqueue(new MessagesUDP { Command = Command.Register, FromName = "Юля" });
        messages.Enqueue(new MessagesUDP { Command = Command.Message, FromName = "Юля", ToName = "Вася", Text = "От Юли" });
        messages.Enqueue(new MessagesUDP { Command = Command.Message, FromName = "Вася", ToName = "Юля", Text = "От Васи" });
    }

    public void Send(MessagesUDP messagesUDP, IPEndPoint iPEnd)
    {
        messages.Enqueue(messagesUDP);
    }

    MessagesUDP IMessageSource.Resive(ref IPEndPoint iPEnd)
    {
        return messages.Peek();
    }
}





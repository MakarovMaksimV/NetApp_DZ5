namespace TestSeminar5TestClient;

public class UnitTest1
{
    [Fact]
    public void CanConnectToServer()
    {
        Client client = new Client();
        bool isConnected = client.ConnectToServer("192.168.1.1");
        Assert.True(isConnected);
    }

    [Fact]
    public void CanSendDataToServer()
    {
        Client client = new Client();
        bool isDataSent = client.SendDataToServer("Hello, server!");
        Assert.True(isDataSent);
    }

}

public class Client
{
    public bool ConnectToServer(string serverIP)
    {
        return true;
    }

    public bool SendDataToServer(string data)
    {
        return true;
    }
}

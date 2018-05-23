using System;

public class UDPClient : IConnectionClient
{
    private readonly UDPReceive UDPReceive;
    private readonly UDPSend UDPSend;

    public UDPClient(string ip, int sendPort, int receivePort, IReceiver receiver)
    {
        UDPSend = new UDPSend(ip, sendPort);
        UDPReceive = new UDPReceive(ip, receivePort, receiver);
    }

    public void send(string message)
    {
        UDPSend.sendString(message);
    }

    public void destroy()
    {
        UDPSend.destroy();
        UDPReceive.destroy();
    }

    public void receive()
    {
        throw new NotImplementedException();
    }
}
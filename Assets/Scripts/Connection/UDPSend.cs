using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class UDPSend
{
    private readonly UdpClient client;

    private readonly string IP;
    private readonly int port;

    private readonly IPEndPoint remoteEndPoint;

    public UDPSend(string ip, int port)
    {
        IP = ip;
        this.port = port;
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), this.port);
        client = new UdpClient();
        client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.NoDelay, true);
    }

    public void sendString(string message)
    {
        try
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            client.Send(data, data.Length, remoteEndPoint);
        }
        catch (Exception err)
        {
            Debug.Log(err);
        }
    }

    public void destroy()
    {
        client.Close();
    }
}
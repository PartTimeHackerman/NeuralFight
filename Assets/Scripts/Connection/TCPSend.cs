using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class TCPSend
{
    private readonly TcpClient client;

    private readonly string IP;
    private readonly int port;

    private IPEndPoint remoteEndPoint;

    public TCPSend(string ip, int port)
    {
        IP = ip;
        this.port = port;
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), this.port);
        client = new TcpClient();
    }

    public void sendString(string message)
    {
        try
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            //client.Send(data, data.Length, remoteEndPoint);
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
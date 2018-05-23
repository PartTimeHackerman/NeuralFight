using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class UDPReceive
{
    private UdpClient client;

    private readonly string IP;
    private readonly int port;

    private readonly IReceiver receiver;

    private readonly Thread receiveThread;

    private bool receiving = true;

    public UDPReceive(string ip, int port, IReceiver receiver)
    {
        IP = ip;
        this.port = port;
        this.receiver = receiver;
        receiveThread = new Thread(receive);
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    private void receive()
    {
        client = new UdpClient(port);
        while (receiving)
            try
            {
                var ipEndPoint = new IPEndPoint(IPAddress.Parse(IP), 0);
                byte[] data = client.Receive(ref ipEndPoint);

                var text = Encoding.UTF8.GetString(data);
                UnityMainThreadDispatcher.Instance().Enqueue(() => receiver.receive(text));
            }
            catch (Exception err)
            {
                Debug.Log(err);
            }
    }

    public void destroy()
    {
        receiving = false;
        client.Close();
        receiveThread.Abort();
    }
}
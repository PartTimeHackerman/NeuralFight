using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class TCPClient : IConnectionClient
{
    private TcpClient client;
    public bool established;
    private bool flushing = false;
    public string ip;

    public string name = "Localhost";
    private NetworkStream networkStream;
    public int port;
    private readonly IReceiver receiver;
    private readonly Thread receiveThread;
    private bool receiving = true;

    public bool socketReady;
    private StreamReader streamReader;
    private StreamWriter streamWriter;

    public TCPClient(string ip, int port, IReceiver receiver)
    {
        this.ip = ip;
        this.port = port;
        this.receiver = receiver;
        setupSocket();
        receiveThread = new Thread(receive);
        receiveThread.IsBackground = true;
        receiveThread.Start();
        var establishThread = new Thread(establish);
        establishThread.IsBackground = true;
        establishThread.Start();
    }

    public void send(string message)
    {
        if (!socketReady)
            return;

        while (!established) Thread.Sleep(1000);
        //Debug.Log(message);
        streamWriter.Write(message + "\n");
        streamWriter.Flush();
    }


    public void receive()
    {
        while (receiving)
            if (networkStream.DataAvailable)
            {
                byte[] inStream = new byte[client.SendBufferSize];
                networkStream.Read(inStream, 0, inStream.Length);
                var result = Encoding.UTF8.GetString(inStream);

                if (!established)
                {
                    established = true;
                    Debug.Log("EST");
                }
                else
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(() => receiver.receive(result));
                }
            }
    }

    public void destroy()
    {
        if (!socketReady)
            return;
        streamWriter.Close();
        streamReader.Close();
        receiving = false;
        client.Close();
        receiveThread.Abort();
        client.Close();
        socketReady = false;
    }

    public void setupSocket()
    {
        try
        {
            client = new TcpClient(ip, port);
            client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.NoDelay, true);
            client.Client.DontFragment = true;
            networkStream = client.GetStream();
            streamWriter = new StreamWriter(networkStream);
            streamReader = new StreamReader(networkStream);
            socketReady = true;
        }
        catch (Exception e)
        {
            Debug.Log("Socket error:" + e);
        }
    }

    private void establish()
    {
        streamWriter.Write("");
        streamWriter.Flush();
    }

    public void maintainConnection()
    {
        if (!networkStream.CanRead) setupSocket();
    }
}
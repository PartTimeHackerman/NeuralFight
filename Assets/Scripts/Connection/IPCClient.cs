using System.Threading;
using NetMQ;
using NetMQ.Sockets;
using UnityEngine;

public class IPCClient : IConnectionClient
{
    private readonly RequestSocket client;
    private readonly IReceiver receiver;
    private readonly Thread receiveThread;
    private readonly bool receiving = true;
    private readonly ResponseSocket server;

    public IPCClient(IReceiver receiver)
    {
        this.receiver = receiver;
        server = new ResponseSocket();
        client = new RequestSocket("ipc:///tmp/sumoserverpy");
        server.Bind("ipc:///tmp/sumoservercs");
        receiveThread = new Thread(receive);
        receiveThread.IsBackground = true;
        //receiveThread.Start();
    }


    public void send(string message)
    {
        var m = server.ReceiveFrameString();
        Debug.Log(m);
        server.SendFrame(message);
    }


    public void receive()
    {
        while (receiving)
        {
            var data = client.ReceiveFrameString();
            receiver.receive(data);
        }
    }

    public void destroy()
    {
        client.Close();
        client.Dispose();
    }
}
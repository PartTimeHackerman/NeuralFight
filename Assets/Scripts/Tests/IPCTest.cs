using System;
using System.Threading;
using UnityEngine;

public class IPCTest : Test, IReceiver
{
    private long calcStart;

    private IConnectionClient connection;
    private readonly Epoch epoch = Epoch.get();


    public void receive(string data)
    {
        Debug.Log("Received: " + data);
        var calcTime = epoch.getEpoch() - calcStart;
        Debug.Log("Time: " + calcTime);
    }

    private void Awake()
    {
        base.Awake();
        connection = new IPCClient(this);
        InvokeRepeating("Send", 0.0f, 1.0f);
    }

    public void Create()
    {
        connection = new IPCClient(this);
        while (true)
        {
            Send();
            Thread.Sleep(1000);
        }
    }

    public void Send()
    {
        calcStart = epoch.getEpoch();
        var epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var currentEpochTime = (DateTime.UtcNow - epochStart).TotalMilliseconds;
        connection.send(currentEpochTime.ToString());
        Debug.Log("Sent: " + currentEpochTime);
    }

    private void OnDestroy()
    {
        base.OnDestroy();
        connection.destroy();
    }
}
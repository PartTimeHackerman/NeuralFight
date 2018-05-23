using UnityEngine;

public class UDPTest : Test, IReceiver
{
    private long calcStart;

    private IConnectionClient connection;
    private readonly Epoch epoch = Epoch.get();


    public void receive(string data)
    {
        var calcTime = epoch.getEpoch() - calcStart;
        Debug.Log("Received: " + data + " " + calcTime);
    }

    public override void StartTest()
    {
        connection = new UDPClient("127.0.0.1", 9000, 9001, this);
        InvokeRepeating("Send", 0.0f, 1.0f);
    }

    public void Send()
    {
        var s = str(10000);
        calcStart = epoch.getEpoch();
        connection.send(calcStart + "|" + s);
        Debug.Log("Sent: " + calcStart);
    }

    private void OnDestroy()
    {
        base.OnDestroy();
        connection.destroy();
    }

    private string str(int s)
    {
        var ss = "";
        for (var i = 0; i < s; i++) ss += "a";
        return ss;
    }
}
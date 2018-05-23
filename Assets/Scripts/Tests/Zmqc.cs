using System;
using NetMQ;
using NetMQ.Sockets;

public class Zmqc : Test
{
    private readonly Epoch epoch = Epoch.get();

    public override void StartTest()
    {
        using (var client = new RequestSocket())
        {
            client.Connect("tcp://localhost:5555");

            for (var i = 0; i < 10; i++)
            {
                Console.WriteLine("Sending Hello");
                var s = str(10000);
                client.SendFrame(epoch.getEpoch() + "|" + s);

                var message = client.ReceiveFrameString();
                Console.WriteLine("Received {0}", message);
            }

            client.Close();
            client.Dispose();
            NetMQConfig.Cleanup();
        }
    }

    private string str(int s)
    {
        var ss = "";
        for (var i = 0; i < s; i++) ss += "a";
        return ss;
    }
}
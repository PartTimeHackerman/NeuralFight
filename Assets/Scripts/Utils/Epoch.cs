using System;

public class Epoch
{
    private static readonly object syncLock = new object();
    private static Epoch epoch;
    private static readonly DateTime beginning = new DateTime(1970, 1, 1);


    public static Epoch get()
    {
        if (epoch == null)
            lock (syncLock)
            {
                epoch = new Epoch();
            }

        return epoch;
    }

    public long getEpoch()
    {
        return (long) (DateTime.UtcNow - beginning).TotalMilliseconds;
    }
}
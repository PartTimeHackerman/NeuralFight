using System.Collections.Generic;

public interface IEnvironment
{
    //void sendObservations(IAgent agent, List<float> observations);

    //void sendReward(IAgent agent, float reward, float terminal);
}

public static class IEnvironmentHelper
{
    public static string observationsToString(this IEnvironment environment, List<float> observations)
    {
        var data = "";
        for (int i = 0, s = observations.Count; i < s; i++)
        {
            data += observations[i].ToString();
            if (i != s - 1) data += " ";
        }

        return data;
    }

    public static string rewardToString(this IEnvironment environment, float reward, float terminated)
    {
        var data = reward + " " + terminated;
        return data;
    }

    public static List<float> actionsToData(this IEnvironment environment, string data)
    {
        List<string> floatsString = new List<string>(data.Split(' '));
        ;

        List<float> floatData = new List<float>();
        foreach (var stringFloat in floatsString)
        {
            var action = float.Parse(stringFloat);
            floatData.Add(action);
        }

        return floatData;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

class RewardFnsGraphSpawner : MonoBehaviour
{

    public float low = 0f;
    public float up = 0f;
    public float mar = 0f;
    public float valAt1 = 1f;
    public int points = 10;

    void Start()
    {
        float x = 0f;

        /*spawnGraph(x, RewardFunction.GAUSSIAN);
        for (int i = 1; i <= 11; i++)
        {
            double y = RewardFunctions.gaussian(i/10f, .1f);
            Debug.Log(y);
            spawnPoint(x + i-1, (float)y*10);
        }
        x += 11;*/
        foreach (RewardFunction rewardFunction in Enum.GetValues(typeof(RewardFunction)))
        {
            spawnGraph(x, rewardFunction);
            spawnPoints(x, rewardFunction);
            x += 11;
        }
    }

    void spawnGraph(float x, RewardFunction rewardFunction)
    {
        string name = Enum.GetName(typeof(RewardFunction), rewardFunction);
        GameObject graph = GameObject.Instantiate((GameObject)Resources.Load("graph"));
        graph.transform.position = new Vector3(x, 0, 0);
        graph.GetComponentInChildren<TextMesh>().text = name;
    }

    void spawnPoints(float x, RewardFunction rewardFunction)
    {
        for (int i = 0; i <= points; i++)
        {
            double y = RewardFunctions.tolerance(i / (float)points, low, up, mar, valAt1, rewardFunction);
            spawnPoint(x + (i/((float)points/10)), (float)y*10, Color.red);
            y = RewardFunctions.toleranceInv(i / (float)points, low, up, mar, valAt1, rewardFunction);
            spawnPoint(x + (i / ((float)points / 10)), (float)y * 10, Color.blue);
        }
    }

    void spawnPoint(float x, float y, Color color)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Destroy(sphere.GetComponent<SphereCollider>());
        sphere.transform.localScale = new Vector3(.1f, .1f, .1f);
        sphere.GetComponent<MeshRenderer>().material.color = color;
        sphere.transform.position = new Vector3(x, y, -0.5f);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

class RewardFnsGraphSpawner : MonoBehaviour
{

    public bool reset = false;
    public bool autoRun = false;
    public float low = 0f;
    public float up = 0f;
    public float mar = 0f;
    public float valAt1 = 1f;
    public int points = 10;
    private bool updateMar = false;

    private List<GameObject> objects = new List<GameObject>();
    private float nextUpdate = .1f;

    void Start()
    {
        resetPoints();

        if (autoRun)
        {
            mar = 0;
            valAt1 = -1;
        }
    }

    void Update()
    {
        if (reset)
        {
            resetPoints();
            reset = false;
        }

        if (Time.time >= nextUpdate && autoRun)
        {
            nextUpdate = Time.time + .1f;
            auto();
        }
    }

    void spawnGraph(float x, RewardFunction rewardFunction)
    {
        string name = Enum.GetName(typeof(RewardFunction), rewardFunction);
        GameObject graph = GameObject.Instantiate((GameObject)Resources.Load("graph"));
        objects.Add(graph);
        graph.transform.position = new Vector3(x, 0, 0);
        graph.GetComponentInChildren<TextMesh>().text = name;
    }

    void spawnPoints(float x, RewardFunction rewardFunction)
    {
        for (int i = 0; i <= points; i++)
        {
            double y = RewardFunctions.tolerance(i / (float)points, low, up, mar, valAt1, rewardFunction);
            spawnPoint(x + (i / ((float)points / 10)), (float)y * 10, Color.red);
            y = RewardFunctions.toleranceInv(i / (float)points, low, up, mar, valAt1, rewardFunction);
            spawnPoint(x + (i / ((float)points / 10)), (float)y * 10, Color.blue);
        }
    }

    void spawnPoint(float x, float y, Color color)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        objects.Add(sphere);
        Destroy(sphere.GetComponent<SphereCollider>());
        sphere.transform.localScale = new Vector3(.1f, .1f, .1f);
        sphere.GetComponent<MeshRenderer>().material.color = color;
        sphere.transform.position = new Vector3(x, y, -0.5f);
    }

    void resetPoints()
    {
        foreach (GameObject g in objects)
        {
            Destroy(g);
        }
        objects.Clear();
        float x = 0f;
        foreach (RewardFunction rewardFunction in Enum.GetValues(typeof(RewardFunction)))
        {
            spawnGraph(x, rewardFunction);
            spawnPoints(x, rewardFunction);
            x += 11;
        }
    }

    void auto()
    {
        valAt1 = valAt1 + .1f;
        if (valAt1 >= 1)
        {
            updateMar = true;
        }
        if (updateMar)
        {
            mar = mar + .1f;
            valAt1 = - 1;
            updateMar = false;
        }

        resetPoints();
    }
}

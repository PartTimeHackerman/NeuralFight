using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class InternalEnv : MonoBehaviour, IReceiver, IEnvironment
{
    private static InternalEnv instance;
    private List<ActionsDTO> actions = new List<ActionsDTO>();
    private readonly int actionTime = 10;

    private readonly Dictionary<int, IAgent> agents = new Dictionary<int, IAgent>();
    private ApplicationSettings applicationSettings;
    private long calcStart;

    public long calcTime;

    private IConnectionClient connectionClient;

    private readonly Epoch epoch = Epoch.get();
    private long lastTime;

    private float longestStanding;
    private readonly List<ObservationsDTO> observations = new List<ObservationsDTO>();
    private bool pause;


    public void receive(string data)
    {
        actions.Clear();

        actions = JsonConvert.DeserializeObject<List<ActionsDTO>>(data);
        foreach (var action in actions)
        {
            var agent = agents[action.hash];
            agent.pauseAgent(false);
            agent.receiveActions(action);
        }

        calcTime = epoch.getEpoch() - calcStart;
        pause = false;
    }

    internal static InternalEnv get(IAgent agent)
    {
        if (instance == null) throw new Exception("No StandingAI instance");
        instance.agents[agent.getHash()] = agent;
        return instance;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        if (Environment.UserName != "Y")
            connectionClient = new UDPClient("127.0.0.1", 9000, 9001, this);
        else
            connectionClient = new UDPClient("127.0.0.1", 9000, 9001, this);
        //applicationSettings = ApplicationSettings.Instance();

        //actionTime = actionTime / (int)applicationSettings.timeScale;

        //InvokeRepeating("receiveTest", 0.0f, 1f);
    }

    private void Start()
    {
        getAllAgents();
    }

    private void Update()
    {
        if (!pause && epoch.getEpoch() - lastTime > actionTime + calcTime)
        {
            sendObservations();
            lastTime = epoch.getEpoch();
        }
    }

    public void sendObservations()
    {
        calcStart = epoch.getEpoch();
        pause = true;
        observations.Clear();

        foreach (KeyValuePair<int, IAgent> agent in agents)
        {
            observations.Add(agent.Value.getObservations());
            agent.Value.pauseAgent(true);
        }

        var data = JsonConvert.SerializeObject(observations);
        connectionClient.send(data);
    }

    private void receiveTest()
    {
        foreach (KeyValuePair<int, IAgent> agent in agents) agent.Value.pauseAgent(false);
        calcTime = epoch.getEpoch() - calcStart;
        pause = false;
    }

    public void setLongestStanding(float time)
    {
        if (time > longestStanding)
        {
            longestStanding = time;
            Debug.Log("Longest standing: " + longestStanding);
        }
    }

    private void getAllAgents()
    {
        StandingAgentSmall[] agents = (StandingAgentSmall[]) FindObjectsOfType(typeof(StandingAgentSmall));
        foreach (var agent in agents) this.agents[agent.GetHashCode()] = agent;
    }

    private void OnDestroy()
    {
        connectionClient.destroy();
    }
}
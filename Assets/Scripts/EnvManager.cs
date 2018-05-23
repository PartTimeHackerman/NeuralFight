using System;
using System.Collections.Generic;
using UnityEngine;

public class EnvManager : MonoBehaviour, IReceiver
{
    private static EnvManager instance;
    private Dictionary<int, IEnvironment> enviroments = new Dictionary<int, IEnvironment>();
    private UDPReceive UDPReceive;
    private UDPSend UDPSend;

    public void receive(string data)
    {
        throw new NotImplementedException();
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        //UDPSend = new UDPSend("127.0.0.1", 9998);
        //UDPReceive = new UDPReceive("127.0.0.1", 9999, this);
    }

    private void Update()
    {
    }

    public static EnvManager Instance()
    {
        if (!Exists()) throw new Exception("No EnvManager found");
        return instance;
    }

    public static bool Exists()
    {
        return instance != null;
    }

    private void OnDestroy()
    {
        instance = null;
    }

    public void create()
    {
    }

    public void close(int hash)
    {
    }

    public void reset(int hash)
    {
    }

    public void execute(int hash, List<float> actions)
    {
    }

    public void states(int hash)
    {
    }

    public void actions(int hash)
    {
    }
}
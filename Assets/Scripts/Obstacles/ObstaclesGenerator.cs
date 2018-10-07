using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;


class ObstaclesGenerator : MonoBehaviour
{
    public Pool<Ramp> rampPool;
    public Pool<Step> stepPool;
    public Pool<Gap> gapPool;
    public Pool<Booster> boostPool;
    public Pool<Cannon> cannonPool;

    public Rigidbody playerRoot;

    public float minDist = -100f;
    public float maxDist = 100f;

    private Queue<Obstacle> activeObstacles = new Queue<Obstacle>();
    private float lastObstacleXPos = 0f;
    private Obstacle lastObstacle;
    private bool running = false;


    void Start()
    {
        rampPool = ObjectsPool.getPool<Ramp>();
        stepPool = ObjectsPool.getPool<Step>();
        gapPool = ObjectsPool.getPool<Gap>();
        boostPool = ObjectsPool.getPool<Booster>();
        cannonPool = ObjectsPool.getPool<Cannon>();
        resetRun();
    }

    void FixedUpdate()
    {
        if (running)
        {
            Obstacle firstObstacle = activeObstacles.Peek();
            if (playerRoot.transform.position.x - firstObstacle.transform.position.x > maxDist)
            {
                activeObstacles.Dequeue();
                firstObstacle.push();
                /*PooledObstacle newObstacle = getRandomObstacle();
                newObstacle.obstacle.setRandom();
                newObstacle.obstacle.setXPosition(lastObstacleXPos);
                lastObstacleXPos = newObstacle.obstacle.transform.position.x + newObstacle.obstacle.totalWidth;
                activeObstacles.Enqueue(newObstacle);*/
            }

            if (lastObstacleXPos - playerRoot.transform.position.x < maxDist)
            {
                Obstacle newObstacle = getRandomObstacle();
                newObstacle.setRandom();
                newObstacle.setXPosition(lastObstacleXPos);
                lastObstacleXPos = newObstacle.transform.position.x + newObstacle.totalWidth;
                //ColorShifterManager newColorShifterManager = newObstacle.colorShifterManager;
                //ColorShifterManager lastColorShifterManager = lastObstacle.colorShifterManager;
                lastObstacle = newObstacle;
                //newColorShifterManager.setColors(lastColorShifterManager.color);
                activeObstacles.Enqueue(newObstacle);

            }

        }

    }

    public void resetRun()
    {
        running = false;
        dequeueAllObstacles();
        Obstacle boost = boostPool.Pop();
        boost.setRandom();
        boost.setXPosition(0f);
        lastObstacleXPos = boost.transform.position.x + boost.totalWidth;
        lastObstacle = boost;
        activeObstacles.Enqueue(boost);
        running = true;
    }

    private void dequeueAllObstacles()
    {
        foreach (Obstacle activeObstacle in activeObstacles)
        {
            activeObstacle.push();
        }
        activeObstacles.Clear();
    }

    private Obstacle getRandomObstacle()
    {
        ObstacleType type = getRandomTypeWeighted();
        switch (type)
        {
            case ObstacleType.RAMP: return rampPool.Pop();
            case ObstacleType.STEP: return stepPool.Pop();
            case ObstacleType.GAP: return gapPool.Pop();
            case ObstacleType.BOOST: return boostPool.Pop();
            case ObstacleType.CANNON:
                {
                    Obstacle cannon = cannonPool.Pop();
                    ((Cannon) cannon).target = playerRoot;
                    return cannon;
                }
            default: return rampPool.Pop();
        }
    }

    private ObstacleType getRandomType()
    {
        Array values = Enum.GetValues(typeof(ObstacleType));
        List<ObstacleType> types = values.OfType<ObstacleType>().ToList();
        ObstacleType type = (ObstacleType)values.GetValue(Random.Range(0, types.Count));
        if (type == lastObstacle.type)
        {
            type = getRandomType();
        }
        return type;
    }
    private ObstacleType getRandomTypeWeighted()
    {
        var weights = new Dictionary<ObstacleType, float>();
        weights.Add(ObstacleType.RAMP, 100f);
        weights.Add(ObstacleType.STEP, 100f);
        weights.Add(ObstacleType.GAP, 100f);
        weights.Add(ObstacleType.BOOST, 100f);
        weights.Add(ObstacleType.CANNON, 10f);
        ObstacleType type = WeightedRandomizer.From(weights).TakeOne();
        if (type == lastObstacle.type)
        {
            type = getRandomType();
        }
        return type;
    }


}

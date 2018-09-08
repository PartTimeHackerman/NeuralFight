using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;


class ObstaclesGenerator : MonoBehaviour
{
    public ObstaclesPool rampPool;
    public ObstaclesPool stepPool;
    public ObstaclesPool gapPool;
    public ObstaclesPool boostPool;
    public GameObject playerRoot;

    public float minDist = -100f;
    public float maxDist = 100f;

    private Queue<PooledObstacle> activeObstacles = new Queue<PooledObstacle>();
    private float lastObstacleXPos = 0f;
    private PooledObstacle lastObstacle;
    private bool running = false;


    void Start()
    {
        resetRun();
    }

    void FixedUpdate()
    {
        if (running)
        {
            PooledObstacle firstObstacle = activeObstacles.Peek();
            if (playerRoot.transform.position.x - firstObstacle.obstacle.transform.position.x > maxDist)
            {
                activeObstacles.Dequeue();
                firstObstacle.poolObstacle();
                /*PooledObstacle newObstacle = getRandomObstacle();
                newObstacle.obstacle.setRandom();
                newObstacle.obstacle.setXPosition(lastObstacleXPos);
                lastObstacleXPos = newObstacle.obstacle.transform.position.x + newObstacle.obstacle.totalWidth;
                activeObstacles.Enqueue(newObstacle);*/
            }

            if (lastObstacleXPos - playerRoot.transform.position.x < maxDist)
            {
                PooledObstacle newObstacle = getRandomObstacle();
                newObstacle.obstacle.setRandom();
                newObstacle.obstacle.setXPosition(lastObstacleXPos);
                lastObstacleXPos = newObstacle.obstacle.transform.position.x + newObstacle.obstacle.totalWidth;
                lastObstacle = firstObstacle;
                ColorShifterManager newColorShifterManager = newObstacle.colorShifterManager;
                ColorShifterManager lastColorShifterManager = lastObstacle.colorShifterManager;
                newColorShifterManager.setColors(lastColorShifterManager.color);
                activeObstacles.Enqueue(newObstacle);

            }

        }

    }

    public void resetRun()
    {
        running = false;
        dequeueAllObstacles();
        PooledObstacle boost = boostPool.getObstacle();
        boost.obstacle.setRandom();
        boost.obstacle.setXPosition(0f);
        lastObstacleXPos = boost.obstacle.transform.position.x + boost.obstacle.totalWidth;
        activeObstacles.Enqueue(boost);
        running = true;
    }

    private void dequeueAllObstacles()
    {
        foreach (PooledObstacle activeObstacle in activeObstacles)
        {
            activeObstacle.poolObstacle();
        }
        activeObstacles.Clear();
    }

    private PooledObstacle getRandomObstacle()
    {
        switch (Random.Range(0, 4))
        {
            case 0: return rampPool.getObstacle();
            case 1: return stepPool.getObstacle();
            case 2: return gapPool.getObstacle();
            case 3: return boostPool.getObstacle();
            default: return rampPool.getObstacle();
        }
    }


}

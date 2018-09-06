using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class ObstaclesPool : MonoBehaviour
{
    public Obstacle obstacle;
    public int amount;
    private Queue<PooledObstacle> obstaclesQueue = new Queue<PooledObstacle>();

    void OnEnable()
    {
        for (int i = 0; i < amount; i++)
        {
            addObstacle();
        }
    }

    private void addObstacle()
    {
        Obstacle obj = (Obstacle)Instantiate(obstacle);
        obj.gameObject.SetActive(false);
        PooledObstacle pooledObstacle = new PooledObstacle(this, obj);
        obstaclesQueue.Enqueue(pooledObstacle);

    }

    public PooledObstacle getObstacle()
    {
        if (!obstaclesQueue.Any())
            addObstacle();
        PooledObstacle pooledObstacle = obstaclesQueue.Dequeue();
        pooledObstacle.obstacle.gameObject.SetActive(true);
        return pooledObstacle;
    }

    public void poolObstacle(PooledObstacle pooledObstacle)
    {
        pooledObstacle.obstacle.gameObject.SetActive(false);
        obstaclesQueue.Enqueue(pooledObstacle);
    }


}

class PooledObstacle
{
    public ObstaclesPool pool;
    public Obstacle obstacle;

    public PooledObstacle(ObstaclesPool pool, Obstacle obstacle) {
        this.pool = pool;
        this.obstacle = obstacle;
    }

    public void poolObstacle()
    {
        obstacle.gameObject.SetActive(false);
        pool.poolObstacle(this);
    }
}


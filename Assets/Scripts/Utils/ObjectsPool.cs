using System;
using UnityEngine;

class ObjectsPool : MonoBehaviour
{

    public static Pool<T> getPool<T>() where T : Poolable
    {
        Type type = typeof(T);
        
        Pool<T> pool;
        if (PoolHelper.ContainsPool<T>(type))
        {
            pool = PoolHelper.GetPool<T>(type);
        }
        else
        {
            pool = PoolHelper.GetPool<T>(type);
            pool.OnPush = (item) => item.gameObject.SetActive(false);
            pool.OnPop = (item) => item.gameObject.SetActive(true);
            pool.CreateFunction = (template) =>
            {
                var item = Instantiate(Resources.Load<T>("Poolable/" + type.Name));
                item.OnPush = () => pool.Push(item);
                DontDestroyOnLoad(item);
                return item;
            };

            pool.Populate(100);
        }
        return pool;
    }
    
    public static Pool<T> getPool<T>( Action<T> OnPush, Action<T> OnPop) where T : Poolable
    {
        Type type = typeof(T);
        
        Pool<T> pool;
        if (PoolHelper.ContainsPool<T>(type))
        {
            pool = PoolHelper.GetPool<T>(type);
        }
        else
        {
            pool = PoolHelper.GetPool<T>(type);
            pool.OnPush = OnPush;
            pool.OnPop = OnPop;
            pool.CreateFunction = (template) =>
            {
                var item = Instantiate(Resources.Load<T>("Poolable/" + type.Name));
                item.OnPush = () => pool.Push(item);
                DontDestroyOnLoad(item);
                return item;
            };

            pool.Populate(100);
        }
        return pool;
    }
    public Pool<T> getPool<T>(Type type) where T : Component
    {
        return PoolHelper.GetPool<T>(type);
    }

}


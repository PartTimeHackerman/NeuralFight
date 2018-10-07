using System;
using System.Collections.Generic;
using UnityEngine;

public static class PoolHelper
{
    // Pool container for given type
    private static class PoolsOfType<T> where T : class 
    {
        // Pool with poolName = null
        private static Pool<T> defaultPool = null;

        // Other pools
        private static Dictionary<Type, Pool<T>> namedPools = new Dictionary<Type, Pool<T>>();

        public static Pool<T> GetPool(Type poolType)
        {
            if (poolType == null)
            {
                if (defaultPool == null)
                    defaultPool = new Pool<T>();

                return defaultPool;
            }
            else
            {
                Pool<T> result;

                if (namedPools == null)
                {
                    result = new Pool<T>();
                    namedPools.Add(poolType, result);
                }
                else if (!namedPools.TryGetValue(poolType, out result))
                {
                    result = new Pool<T>();
                    namedPools.Add(poolType, result);
                }
                
                return result;
            }
        }

        public static bool ContainsPool(Type type)
        {
            return namedPools.ContainsKey(type);
        }
    }

    // NOTE: if you don't need two or more pools of same type,
    // leave poolName as null while calling any of these functions
    // for better performance

    public static Pool<T> GetPool<T>(Type poolType) where T : class
    {
        Pool<T> pool =  PoolsOfType<T>.GetPool(poolType);
        return pool;
    }

    public static void Push<T>(T obj, Type poolType) where T : class
    {
        PoolsOfType<T>.GetPool(poolType).Push(obj);
    }

    public static T Pop<T>(Type poolType) where T : class
    {
        return PoolsOfType<T>.GetPool(poolType).Pop();
    }

    // Extension method as a shorthand for Push function
    public static void Pool<T>(this T obj, Type poolType) where T : class
    {
        PoolsOfType<T>.GetPool(poolType).Push(obj);
    }

    public static bool ContainsPool<T>(Type type) where T : class
    {
        return PoolsOfType<T>.ContainsPool(type);
    }
}
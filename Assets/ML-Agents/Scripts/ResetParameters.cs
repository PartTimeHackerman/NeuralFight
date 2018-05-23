using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ResetParameters : Dictionary<string, float>, ISerializationCallbackReceiver
{
    [SerializeField] private readonly List<ResetParameter> resetParameters = new List<ResetParameter>();

    public void OnBeforeSerialize()
    {
        resetParameters.Clear();

        foreach (KeyValuePair<string, float> pair in this)
        {
            var rp = new ResetParameter();
            rp.key = pair.Key;

            rp.value = pair.Value;
            resetParameters.Add(rp);
        }
    }

    public void OnAfterDeserialize()
    {
        Clear();


        for (var i = 0; i < resetParameters.Count; i++)
            if (ContainsKey(resetParameters[i].key))
                Debug.LogError("The ResetParameters contains the same key twice");
            else
                Add(resetParameters[i].key, resetParameters[i].value);
    }

    [Serializable]
    public struct ResetParameter
    {
        public string key;
        public float value;
    }
}
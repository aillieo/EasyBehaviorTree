using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    Dictionary<string, Queue<MonoBehaviour>> dict = new Dictionary<string, Queue<MonoBehaviour>>();

    public T Get<T>(T template, Transform parent = null) where T: MonoBehaviour
    {
        string key = template.name;
        if (!dict.ContainsKey(key))
        {
            dict.Add(key, new Queue<MonoBehaviour>());
        }

        var queue = dict[key];
        T obj;

        if (queue.Count > 0 )
        {
            obj = queue.Dequeue() as T;
        }
        else
        {
            obj = Instantiate(template);
            obj.name = key;
        }

        if (parent != null)
        {
            obj.transform.SetParent(parent, false);
        }
        return obj;
    }

    public void Recycle<T>(T obj) where T : MonoBehaviour
    {
        obj.transform.SetParent(transform);

        string key = obj.name;
        if (!dict.ContainsKey(key))
        {
            dict.Add(key, new Queue<MonoBehaviour>());
        }
        var queue = dict[key];
        queue.Enqueue(obj);
    }
}

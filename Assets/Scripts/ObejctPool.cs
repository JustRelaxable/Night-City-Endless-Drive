using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObejctPool
{
    List<int> availableIndexes = new List<int>();
    List<GameObject> instances = new List<GameObject>();

    public ObejctPool(GameObject gameObject, int instanceCount)
    {
        for (int i = 0; i < instanceCount; i++)
        {
            var go = GameObject.Instantiate(gameObject);
            go.GetComponent<StreetManager>().roadManager.AssignObjectPool(this);
            go.SetActive(false);
            instances.Add(go);
            availableIndexes.Add(i);
        }
    }

    public GameObject GetAvailableInstance()
    {
        if (availableIndexes.Count>0)
        {
            var go = instances[availableIndexes[0]];
            //instances.RemoveAt(availableIndexes[0]);
            availableIndexes.RemoveAt(0);
            return go;
        }
        else
        {
            return null;
        }
    }

    public void RegisterNewInstance(GameObject go)
    {
        availableIndexes.Add(instances.FindIndex(x => x == go));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public Transform spawnPoint;
    public Transform startPoint;
    public Transform endPoint;
    [HideInInspector]public ObejctPool spawnedObjectPool;

    public int roadIndex = -1;
    

    public void Notify(int doorTypeIndex)
    {
        ObjectPoolManager.instance.Notify(roadIndex, doorTypeIndex);
    }

    public void AssignObjectPool(ObejctPool op)
    {
        spawnedObjectPool = op;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadColliderManager : MonoBehaviour
{
    public RoadCollider roadCollider;
    public RoadManager roadManager;
    public int doorTypeIndex;

    private void OnTriggerEnter(Collider other)
    {
        if (roadCollider.hasDoorTouched)
        {
            roadManager.Notify(doorTypeIndex);
        }
    }


}

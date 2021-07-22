using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public int instanceCount = 3;
    public GameObject car;

    public GameObject[] roadPrefabs;
    private List<ObejctPool> objectPools = new List<ObejctPool>();
    private RoadManager[] roadManagers = new RoadManager[5];

    public static ObjectPoolManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        for (int i = 0; i < roadPrefabs.Length; i++)
        {
            objectPools.Add(new ObejctPool(roadPrefabs[i], instanceCount));
        }


        Vector3 spawnPoint = new Vector3(0, 0, 0);
        Quaternion spawnRotation = Quaternion.identity;
        bool firstLoopFinished = false;
        for (int i = 0; i < 5; i++)
        {
            var go = GetRandomRoad();
            go.SetActive(true);
            var roadManager = go.GetComponent<StreetManager>().roadManager;
            roadManagers[i] = roadManager;
            roadManager.roadIndex = i;
            go.transform.position = spawnPoint;
            


            if (firstLoopFinished)
            {
                //go.transform.rotation = Quaternion.LookRotation(spawnPoint);

                go.transform.rotation = spawnRotation;
            }

            spawnPoint = roadManager.endPoint.position;
            spawnRotation = roadManager.endPoint.rotation;


            if (i == 0)
            {
                firstLoopFinished = true;
            }

            if(i == 2)
            {
                car.transform.position = roadManager.spawnPoint.position;
                car.transform.rotation = roadManager.spawnPoint.rotation;
            }
        }
    }

    public GameObject GetRandomRoad()
    {
        var randomNumber = Random.Range(0, roadPrefabs.Length);
        return objectPools[randomNumber].GetAvailableInstance();
    }

    public void Notify(int roadIndex,int doorIndex)
    {
        //Road Creation Process
        Debug.Log(roadIndex + " " + doorIndex);
        switch (doorIndex)
        {
            case 0:
                break;
            case 1:
                roadManagers[roadIndex - 2].spawnedObjectPool.RegisterNewInstance(roadManagers[roadIndex - 2].transform.parent.parent.gameObject);
                roadManagers[roadIndex - 2].gameObject.transform.parent.parent.gameObject.SetActive(false);


                for (int i = 0; i < 4; i++)
                {
                    roadManagers[i] = roadManagers[i + 1];
                    roadManagers[i].roadIndex = i;
                }
                /*
                roadManagers[roadIndex - 1] = roadManagers[roadIndex];
                roadManagers[roadIndex - 1].roadIndex = 0;
                roadManagers[roadIndex] = roadManagers[roadIndex + 1];
                roadManagers[roadIndex].roadIndex = 1;
                */
                var go = GetRandomRoad();
                go.SetActive(true);
                go.GetComponent<StreetManager>().roadManager.roadIndex = roadIndex + 2;
                roadManagers[roadIndex + 2] = go.GetComponent<StreetManager>().roadManager;
                go.transform.position = roadManagers[roadIndex+1].endPoint.position;
                go.transform.rotation = roadManagers[roadIndex+1].endPoint.rotation;
                break;
            default:
                break;
        }
    }
}

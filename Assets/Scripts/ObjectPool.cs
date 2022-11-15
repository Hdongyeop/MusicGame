using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectInfo
{
    public GameObject goPrefab;
    public int count;
    public Transform tfPoolParent;
}

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private ObjectInfo[] objectInfo;
    
    public static ObjectPool Instance;
    public Queue<GameObject> NoteQueue = new();

    private void Start()
    {
        Instance = this;
        NoteQueue = InsertQueue(objectInfo[0]);
    }

    Queue<GameObject> InsertQueue(ObjectInfo pObjectInfo)
    {
        Queue<GameObject> tQueue = new();

        for (int i = 0; i < pObjectInfo.count; i++)
        {
            GameObject tClone;
            if (pObjectInfo.tfPoolParent != null)
                tClone = Instantiate(pObjectInfo.goPrefab, transform.position, Quaternion.identity, pObjectInfo.tfPoolParent);
            else
                tClone = Instantiate(pObjectInfo.goPrefab, transform.position, Quaternion.identity, this.transform);
            tClone.SetActive(false);
            
            tQueue.Enqueue(tClone);
        }

        return tQueue;
    }
}

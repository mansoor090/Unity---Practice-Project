using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{

    public static ObjectPooler Instance;
    [System.Serializable]
    public class Pool
    {
        public string pooltag;
        public GameObject Prefab;
        public int size;
    }



    
    // Pool Variables
    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

    //PoolSpawnning

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            
        }
    }

    private void OnDisable()
    {
        Instance = null;
    }

    private void Start()
    {

        
        foreach (Pool data in pools)
        {
            Queue<GameObject> objectpool = new Queue<GameObject>();
            for (int i = 0; i < data.size; i++)
            {
                GameObject obj = Instantiate(data.Prefab);
                obj.SetActive(false);
                objectpool.Enqueue(obj);
            }
            poolDictionary.Add(data.pooltag,objectpool);
        }
    }

    // public void MakeAPool(string ftag, int fsize, GameObject fobj)
    // {
    //  
    //     Queue<GameObject> objectpool = new Queue<GameObject>();
    //     for (int i = 0; i < fsize; i++)
    //     {
    //         GameObject obj = Instantiate(fobj);
    //         obj.SetActive(false);
    //         objectpool.Enqueue(obj);
    //     }
    //     poolDictionary.Add(ftag,objectpool);
    //     
    // }

    //PoolAccess
    public GameObject SpawnFromPool(string pTag)
    {
        if (!poolDictionary.ContainsKey(pTag))
        {
            Debug.Log("TAG NOT FOUND");
            return null;
        }

        GameObject Temp = null;
        //SpawnFromPool
        if (poolDictionary[pTag].Count > 0)
        {
            Temp = poolDictionary[pTag].Dequeue();
        }
        else
        {

            foreach (Pool data in pools)
            {
                if (data.pooltag == pTag)
                {
                    Temp = Instantiate(data.Prefab);
                }
            }

            
        }
        Temp.SetActive(true);
        return Temp;

    }


    //BackToPool
    public void EnqueuePool(GameObject refObj,string etag)
    {
        if (!poolDictionary.ContainsKey(etag))
        {
            /// There is no such tag available
            Debug.Log("TAG NOT FOUND");
        }
        
        poolDictionary[etag].Enqueue(refObj);
        refObj.SetActive(false);

    }

}

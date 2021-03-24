using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public static ObjectPooler SharedInstance;
    public List<Pool> pools;
    public bool willGrow = true;

    private Dictionary<string, List<GameObject>> poolDictionary;

    void Awake()
    {
        SharedInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        poolDictionary = new Dictionary<string, List<GameObject>>();

        foreach (Pool pool in pools)
        {
            List<GameObject> objectPool = new List<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                InstantiatePoolObject(pool.prefab, objectPool); 
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    private GameObject InstantiatePoolObject(GameObject prefab, List<GameObject> objectPool)
    {
        GameObject obj = (GameObject)Instantiate(prefab);
        obj.SetActive(false);
        objectPool.Add(obj);
        obj.transform.SetParent(this.transform); // set as children of Spawn Manager
        return obj;
    }

    private GameObject GetPrefabByTag(string tag)
    {
        for (int i = 0; i < pools.Count; i++)
        {
            if (pools[i].tag == tag)
            {
                return pools[i].prefab;
            }
        }
        return null;
    }

    public GameObject GetPooledObject(string tag, Vector3 postion, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        // For as many objects as are in the pooledObjects list
        for (int i = 0; i < poolDictionary[tag].Count; i++)
        {
            // if the pooled objects is NOT active, return that object 
            if (!poolDictionary[tag][i].activeInHierarchy)
            {
                GameObject currObj = poolDictionary[tag][i];
                currObj.transform.position = postion;
                currObj.transform.rotation = rotation;
                currObj.SetActive(true);
                return currObj;
            }
        }

        if (willGrow)
        {
            GameObject prefab = GetPrefabByTag(tag);
            if (prefab != null)
            {
                GameObject obj = InstantiatePoolObject(prefab, poolDictionary[tag]);
                obj.transform.position = postion;
                obj.transform.rotation = rotation;
                obj.SetActive(true);
                return obj;
            }
            return null;
        }
        // otherwise, return null   
        return null;
    }

    public void SetObjectToPool(GameObject obj)
    {
        obj.SetActive(false);
    }
}

using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameObjectAssets
{
    public string assetsName;
    public int count;
    public GameObject prefab;

}

public class GameObjectPool : SingletonBase<GameObjectPool>
{

    [SerializeField] [Header("Ô¤ÖÆÌå")] private List<GameObjectAssets> assetsList = new List<GameObjectAssets>();
    [SerializeField] private Transform poolObjectParent;
    private Dictionary<string, Queue<GameObject>> pools = new Dictionary<string, Queue<GameObject>>();


    private void Awake()
    {
        InitPool();
    }

    private void InitPool()
    {
        if (assetsList.Count == 0)
            return;

        for (int i = 0; i < assetsList.Count; i++)
        {
            if(!pools.ContainsKey(assetsList[i].assetsName))
            {
                if(assetsList[i].prefab != null)
                {
                    pools.Add(assetsList[i].assetsName, new Queue<GameObject>());

                    for (int j = 0; j < assetsList[i].count; j++)
                    {
                        GameObject temp = Instantiate(assetsList[i].prefab);
                        temp.transform.SetParent(poolObjectParent);
                        temp.transform.position = Vector3.zero;
                        temp.transform.rotation = Quaternion.identity;
                        pools[assetsList[i].assetsName].Enqueue(temp);
                        temp.SetActive(false);
                        
                    }
                }
            }
        }
    }

    public GameObject SpawnObject(string name)
    {
        if(pools.ContainsKey(name))
        {
            GameObject pool = pools[name].Dequeue();
            
            pools[name].Enqueue(pool);
            pool.SetActive(true);
            return pool;
        }
        return null;
    }

    public void RecycleObject(GameObject gameObject)
    {
        gameObject.transform.position = Vector3.zero;
        gameObject.transform.rotation = Quaternion.identity;
        gameObject.SetActive(false);

    }


}

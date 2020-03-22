using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject[] items;
    public int illegalItemsPerSuitcase;

    //private Dictionary<GameManager.ITEMS, GameObject> prefabsWithNames = new Dictionary<GameManager.ITEMS, GameObject>();
    //private Dictionary<GameManager.ITEMS, bool> itemsLegality = new Dictionary<GameManager.ITEMS, bool>();

    private Dictionary<GameManager.ITEMS, (bool legality, GameObject prefab)> allPrefabs = new Dictionary<GameManager.ITEMS, (bool legality, GameObject prefab)>();

    private Dictionary<GameManager.ITEMS, GameObject> legalInGame = new Dictionary<GameManager.ITEMS, GameObject>();
    private Dictionary<GameManager.ITEMS, GameObject> legalNotInGame = new Dictionary<GameManager.ITEMS, GameObject>();
    private Dictionary<GameManager.ITEMS, GameObject> forbiddenInGame = new Dictionary<GameManager.ITEMS, GameObject>();
    private Dictionary<GameManager.ITEMS, GameObject> forbiddenNotInGame = new Dictionary<GameManager.ITEMS, GameObject>();

    private Forbidden forbiddenSc;

    void Awake()
    {
        forbiddenSc = FindObjectOfType<Forbidden>();
        #region Fill dictionaries
        foreach (var item in items)
        {
            Item sc = item.GetComponent<Item>();

            allPrefabs.Add(sc.type, (sc.isLegalOnStart, item));

            if (sc.isLegalOnStart) legalNotInGame.Add(sc.type, item);
            else forbiddenNotInGame.Add(sc.type, item);

            //prefabsWithNames.Add(sc.type, item);
            //itemsLegality.Add(sc.type, sc.isLegalOnStart);
        }
        #endregion
    }

    public void LevelUp(LevelUp level)
    {
        for(int i=0;i<level.newLegalItems;i++)
        {
            int r = UnityEngine.Random.Range(0, legalNotInGame.Count);
            var item = legalNotInGame.ElementAt(r);
            legalNotInGame.Remove(item.Key);
            legalInGame.Add(item.Key,item.Value);
        }
        for (int i = 0; i < level.newForbiddenItems; i++)
        {
            int r = UnityEngine.Random.Range(0, forbiddenNotInGame.Count);
            var item = forbiddenNotInGame.ElementAt(r);
            forbiddenNotInGame.Remove(item.Key);
            forbiddenInGame.Add(item.Key, item.Value);
            forbiddenSc.AddItem(item.Key);
        }
    }

    public Sprite GetLatestObjectSprite()
    {
        return forbiddenInGame.ElementAt(forbiddenInGame.Count-1).Value.GetComponent<SpriteRenderer>().sprite;
    }

    public List<GameObject> GetPrefabs(int max, int itemsCount, int forbiddenCount)
    {
        illegalItemsPerSuitcase = forbiddenCount;

        List<GameObject> prefabs = new List<GameObject>();
        List<GameManager.ITEMS> prefabNames = new List<GameManager.ITEMS>();
        //int legalAdded = 0;
        //int illegalAdded = 0;

        for(int i=0;i< forbiddenCount; i++)
        {
            int r = UnityEngine.Random.Range(0, forbiddenInGame.Count);
            var item = forbiddenInGame.ElementAt(r);
            if(prefabNames.Contains(item.Key))
            {
                r = UnityEngine.Random.Range(0, forbiddenInGame.Count);
                item = forbiddenInGame.ElementAt(r);
            }
            prefabs.Add(item.Value);
            prefabNames.Add(item.Key);
        }
        for (int i = 0; i < itemsCount-forbiddenCount; i++)
        {
            int r = UnityEngine.Random.Range(0, legalInGame.Count);
            var item = legalInGame.ElementAt(r);

            if (prefabNames.Contains(item.Key))
            {
                r = UnityEngine.Random.Range(0, legalInGame.Count);
                item = legalInGame.ElementAt(r);
            }

            prefabs.Add(item.Value);
            prefabNames.Add(item.Key);
        }
        for (int i = 0; i < max-itemsCount; i++)
        {
            prefabs.Add(null);
        }

        //do
        //{
        //    foreach (var item in allPrefabs.OrderBy(x => UnityEngine.Random.value))
        //    {
        //        if (item.Value.legality && legalAdded < prefabsCount - illegalItemsPerSuitcase)
        //        {
        //            prefabs.Add(item.Value.prefab);
        //            legalAdded++;
        //        }
        //        else if (!item.Value.legality && illegalAdded < illegalItemsPerSuitcase)
        //        {
        //            prefabs.Add(item.Value.prefab);
        //            illegalAdded++;
        //        }

        //        if (prefabs.Count == prefabsCount) break;
        //    }
        //} while (prefabs.Count < prefabsCount);

        //do
        //{
        //    foreach (var item in itemsLegality.OrderBy(x => UnityEngine.Random.value))
        //    {
        //        if (item.Value && legalAdded < prefabsCount - illegalItemsPerSuitcase)
        //        {
        //            prefabs.Add(GetPrefabByName(item.Key));
        //            legalAdded++;
        //        }
        //        else if (!item.Value && illegalAdded < illegalItemsPerSuitcase)
        //        {
        //            prefabs.Add(GetPrefabByName(item.Key));
        //            illegalAdded++;
        //        }

        //        if (prefabs.Count == prefabsCount) break;
        //    }
        //} while (prefabs.Count < prefabsCount);

        prefabs = prefabs.OrderBy(x => UnityEngine.Random.value).ToList();
        return prefabs;
    }

    //private GameObject GetPrefabByName(GameManager.ITEMS name)
    //{
    //    return prefabsWithNames[name];
    //}

    public bool IsLegal(GameManager.ITEMS name)
    {
        return allPrefabs[name].legality;
    }
}

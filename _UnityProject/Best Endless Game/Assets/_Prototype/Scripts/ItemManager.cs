using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject[] items;
    public int illegalItemsPerSuitcase = 2;
    public int totalItemsInCurrentSuitcase = 0;

    //private Dictionary<GameManager.ITEMS, GameObject> prefabsWithNames = new Dictionary<GameManager.ITEMS, GameObject>();
    //private Dictionary<GameManager.ITEMS, bool> itemsLegality = new Dictionary<GameManager.ITEMS, bool>();

    private Dictionary<GameManager.ITEMS, (bool legality, GameObject prefab)> allPrefabs = new Dictionary<GameManager.ITEMS, (bool legality, GameObject prefab)>();

    void Awake()
    {
        #region Fill dictionaries
        foreach (var item in items)
        {
            Item sc = item.GetComponent<Item>();

            allPrefabs.Add(sc.type, (sc.isLegalOnStart, item));

            //prefabsWithNames.Add(sc.type, item);
            //itemsLegality.Add(sc.type, sc.isLegalOnStart);
        }
        #endregion
    }

    public List<GameObject> GetPrefabs(int prefabsCount)
    {
        totalItemsInCurrentSuitcase = prefabsCount;

        List<GameObject> prefabs = new List<GameObject>();
        int legalAdded = 0;
        int illegalAdded = 0;

        do
        {
            foreach (var item in allPrefabs.OrderBy(x => UnityEngine.Random.value))
            {
                if (item.Value.legality && legalAdded < prefabsCount - illegalItemsPerSuitcase)
                {
                    prefabs.Add(item.Value.prefab);
                    legalAdded++;
                }
                else if (!item.Value.legality && illegalAdded < illegalItemsPerSuitcase)
                {
                    prefabs.Add(item.Value.prefab);
                    illegalAdded++;
                }

                if (prefabs.Count == prefabsCount) break;
            }
        } while (prefabs.Count < prefabsCount);

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

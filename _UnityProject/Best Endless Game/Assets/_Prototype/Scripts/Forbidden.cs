using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forbidden : MonoBehaviour
{
    public List<GameObject> forbiddenItems;

    private Dictionary<GameManager.ITEMS, GameObject> labelsDictionary = new Dictionary<GameManager.ITEMS, GameObject>();
    private int emptyIndex=0;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < forbiddenItems.Count; i++)
        {
            ItemLabel script = forbiddenItems[i].GetComponent<ItemLabel>();
            labelsDictionary.Add(script.type,forbiddenItems[i]);
        }
    }

    public void AddItem(GameManager.ITEMS name)
    {
        Instantiate(labelsDictionary[name], transform.GetChild(emptyIndex));
        emptyIndex++;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suitcase : MonoBehaviour
{
    public Transform slots;
    public int slotCount;

    private ItemManager itemManager;

    // Start is called before the first frame update
    void Start()
    {
        slotCount = slots.childCount;
        itemManager = FindObjectOfType<ItemManager>();

        #region Fill slots with items
        var prefabs = itemManager.GetPrefabs(slotCount);
        for (int i = 0; i < slotCount; i++)
        {
            var item = Instantiate(prefabs[i], slots.GetChild(i));
        } 
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}

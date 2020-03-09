using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suitcase : MonoBehaviour
{
    public List<Transform> allSlots;
    public int totalItemsMin;
    public int totalItemsMax;
    public int forbiddenMin;
    public int forbiddenMax;

    private int slotCount;
    private ItemManager itemManager;

    // Start is called before the first frame update
    void Start()
    {
        Transform slots = allSlots[Random.Range(0,3)];
        slots.gameObject.SetActive(true);
        slotCount = slots.childCount;
        itemManager = FindObjectOfType<ItemManager>();

        int itemsCount = Random.Range(totalItemsMin,totalItemsMax+1);
        int forbiddenItemsCount = Random.Range(forbiddenMin,forbiddenMax+1);

        #region Fill slots with items
        var prefabs = itemManager.GetPrefabs(slotCount,itemsCount,forbiddenItemsCount);
        for (int i = 0; i < slotCount; i++)
        {
            if (prefabs[i] == null) continue;
            float rotation = Random.Range(0f, 360f);
            var item = Instantiate(prefabs[i], slots.GetChild(i).position, Quaternion.Euler(new Vector3(0, 0, rotation)), slots.GetChild(i));
            //slots.GetChild(i).rotation = Quaternion.Euler(new Vector3(0, 0, 90));
        }
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}

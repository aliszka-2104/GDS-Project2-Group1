using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSuitcase : MonoBehaviour
{
    public Transform slots;

    private int slotCount;
    private ItemManager itemManager;

    // Start is called before the first frame update
    void Start()
    {
        slots.gameObject.SetActive(true);
        slotCount = slots.childCount;
        itemManager = FindObjectOfType<ItemManager>();

        #region Fill slots with items
        var prefabs = itemManager.GetPrefabs(3,3,1,false);
        for (int i = 0; i < slotCount; i++)
        {
            if (prefabs[i] == null) continue;
            float rotation = Random.Range(0f, 360f);

            var item = Instantiate(prefabs[i], slots.GetChild(i).position, Quaternion.Euler(new Vector3(0, 0, rotation)), slots.GetChild(i));

            var prefabScript = item.GetComponent<Item>();
            if (prefabScript.isLegalOnStart) prefabScript.isActive = false;
            
            //slots.GetChild(i).rotation = Quaternion.Euler(new Vector3(0, 0, 90));
        }
        #endregion
    }
}

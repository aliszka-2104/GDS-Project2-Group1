using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forbidden : MonoBehaviour
{
    public Sprite[] forbiddenItems;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < forbiddenItems.Length; i++)
        {
            transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = forbiddenItems[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

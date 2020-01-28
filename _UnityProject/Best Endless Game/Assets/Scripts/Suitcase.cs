using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suitcase : MonoBehaviour
{
    public GameObject[] items;
    public Transform slots;

    // Start is called before the first frame update
    void Start()
    {
        for(int i=0;i<slots.childCount;i++)
        {
            int rand = Random.Range(0, items.Length - 1);
            var item = Instantiate(items[rand], slots.GetChild(i));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

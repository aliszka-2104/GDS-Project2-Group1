using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForbiddenItemScreen : MonoBehaviour
{
    public GameObject item;
    private Image image;
    public Animator animator;
    // Start is called before the first frame update
    void Awake()
    {
        image = item.GetComponent<Image>();
        //animator.GetComponent<Animator>();
    }

    public void Show(Sprite sprite)
    {
        gameObject.SetActive(true);
        image.sprite = sprite;
        image.SetNativeSize();
        animator.SetTrigger("Play");
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

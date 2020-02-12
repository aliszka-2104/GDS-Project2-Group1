using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public GameManager.ITEMS type;
    public bool isLegalOnStart;

    private Rigidbody2D rb;
    private Collider2D myCollider;
    private GameManager gameManager;
    private ItemManager itemManager;

    private Vector2 dragStart = new Vector2();
    private Vector2 dragEnd = new Vector2();
    private Vector2 direction = new Vector2();
    public float speed = 10f;
    private bool isSomeoneTouchingMe = false;
    private bool isActive = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
        gameManager = FindObjectOfType<GameManager>();
        itemManager = FindObjectOfType<ItemManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && isActive && gameManager.canInteractWithToys)
        {
            Touch touch = Input.touches[0];
            var pos = Camera.main.ScreenToWorldPoint(touch.position);
            if (touch.phase == TouchPhase.Began)
            {
                Collider2D foundPropCollider = Physics2D.OverlapPoint(pos, LayerMask.GetMask("Prop"));
                if (foundPropCollider == myCollider)
                {
                    dragStart = pos;
                    isSomeoneTouchingMe = true;
                }
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                if (isSomeoneTouchingMe)
                {
                    dragEnd = pos;
                    //transform.position = new Vector3(pos.x,pos.y,transform.position.z);
                }
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                if (isSomeoneTouchingMe)
                {
                    dragEnd = pos;
                    direction = dragEnd - dragStart;
                    if(direction!=Vector2.zero)ItemSwiped();
                }
            }
        }
    }

    private void ItemSwiped()
    {
        rb.velocity = direction.normalized * speed;
        isSomeoneTouchingMe = false;
        gameManager.ItemSwiped(this);
        isActive = false;
        transform.SetParent(gameManager.transform, true);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}

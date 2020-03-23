using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public GameManager.ITEMS type;
    public bool isLegalOnStart;
    public AnimationCurve curve;
    public Sprite[] variants;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Collider2D myCollider;
    private GameManager gameManager;
    private ItemManager itemManager;
    private AudioManager audioManager;

    private Vector2 dragStart = new Vector2();
    private Vector2 dragEnd = new Vector2();
    private Vector2 direction = new Vector2();
    public float speed = 10f;
    private bool isSomeoneTouchingMe = false;
    public bool isActive = true;

    private const float gravity = 9.81f;
    private Vector2 myStartPosition;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
        gameManager = FindObjectOfType<GameManager>();
        itemManager = FindObjectOfType<ItemManager>();
        audioManager = FindObjectOfType<AudioManager>();
        sr = GetComponent<SpriteRenderer>();

        myStartPosition = transform.position;
        if (variants.Length>0)
        {
            sr.sprite = variants[Random.Range(0, variants.Length)];
        }
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
                    sr.sortingLayerName = "Props";
                    audioManager.PlayItemGrabbed();
                }
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                if (isSomeoneTouchingMe)
                {
                    Collider2D foundPropCollider = Physics2D.OverlapPoint(pos, LayerMask.GetMask("Bag"));
                    if (foundPropCollider == null)
                    {
                        dragEnd = pos;
                        direction = dragEnd - dragStart;
                        if (direction != Vector2.zero) ItemSwiped();
                    }
                    else
                    {
                        dragEnd = pos;
                        transform.position = new Vector3(pos.x, pos.y, transform.position.z);
                    }
                }
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                if (isSomeoneTouchingMe)
                {
                    //dragEnd = transform.position;
                    //direction = dragEnd - dragStart;
                    //if (Vector2.Distance(dragStart, dragEnd) > 0)
                    //{
                    //    ItemSwiped();
                    //}
                    isSomeoneTouchingMe = false;

                    Collider2D foundPropCollider = Physics2D.OverlapPoint(pos, LayerMask.GetMask("Bag"));
                    if (foundPropCollider != null)
                    {
                        transform.position = myStartPosition;
                        sr.sortingLayerName = "Baggage";
                    }
                    else
                    {
                        dragEnd = pos;
                        direction = dragEnd - dragStart;
                        if (direction != Vector2.zero) ItemSwiped();
                    }
                }
            }
        }
    }

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.gameObject.layer == LayerMask.NameToLayer("Bag"))
    //    {
    //        dragEnd = transform.position;
    //        direction = dragEnd - dragStart;
    //        if (direction != Vector2.zero) ItemSwiped();
    //        Debug.Log("out of the bag!");
    //    }
    //}

    private void ItemSwiped()
    {
        Move();
        isSomeoneTouchingMe = false;
        gameManager.ItemSwiped(this);
        isActive = false;
        transform.SetParent(gameManager.transform, true);
    }

    private void Move()
    {
        //rb.velocity = direction.normalized * speed;

        //StartCoroutine(SimulateMovement());

        rb.WakeUp();
        rb.gravityScale = 1;
        rb.AddForce(direction * speed);
        gameObject.layer = 11;
    }
    
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public GameManager.ITEMS type;
    public bool isLegalOnStart;
    public AnimationCurve curve;

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

    private const float gravity = 9.81f;
    private Vector2 myStartPosition;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
        gameManager = FindObjectOfType<GameManager>();
        itemManager = FindObjectOfType<ItemManager>();

        myStartPosition = transform.position;
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
        rb.AddForce(direction * speed);
        gameObject.layer = 11;
    }

    IEnumerator SimulateMovement()
    {
        //Vector2 end = myStartPosition+direction*speed;

        //var yOffset = 6 - myStartPosition.y;
        //var xOffset = direction.x / direction.y * yOffset;

        //Vector2 end = new Vector2(myStartPosition.x + xOffset, 6);

        Vector2 end = new Vector2(15 * Mathf.Sign(direction.x)+myStartPosition.x, myStartPosition.y+2);
        var offset = 10.0f*direction.y;
        var duration = 2f/Vector2.SqrMagnitude(direction);
        duration = Mathf.Clamp(duration, 0.5f, 2f);

        float time = 0;
        while (time <= duration)
        {
            time += Time.deltaTime;

            float linearT = time / duration;
            float heightT = curve.Evaluate(linearT);
            float height = Mathf.Lerp(0f, offset, heightT);

            transform.position = Vector2.Lerp(myStartPosition, end, linearT) + new Vector2(0f, height);

            yield return null;
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}

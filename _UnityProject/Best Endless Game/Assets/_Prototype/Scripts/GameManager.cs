using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum ITEMS
    {
        _NONE_,
        GUN,
        GRANADE,
        TEDDY,
        SHAMPOO,
        BOOK,
        IDCARD,
        BRUSH,
        HEADPHONES,
        KEYS
    }

    public ItemManager itemManager;
    public GameObject suitcasePrefab;
    public bool canInteractWithToys = true;


    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text timeText;
    [SerializeField]
    private GameObject gameOver;
    [SerializeField]
    private GameObject okText;
    private int score = 0;
    private int timeToResolveSuitcase = 3;
    private int itemsToBeRemovedCount;

    private Suitcase currentSuitcase;
    private float countDownLeft = 0f;

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "Score: " +Environment.NewLine+ score;

        CreateSuitcase();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CreateSuitcase()
    {
        StartCoroutine(StartCountdown(timeToResolveSuitcase));
        var suitcase = Instantiate(suitcasePrefab);
        currentSuitcase = suitcase.GetComponent<Suitcase>();

        itemsToBeRemovedCount = itemManager.illegalItemsPerSuitcase;
        canInteractWithToys = true;
        timeText.text = timeToResolveSuitcase.ToString();
    }

    public void ItemSwiped(Item item)
    {
        if (itemManager.IsLegal(item.type))
        {
            //dupa
            //game over
            GameOver();
        }
        else
        {
            score++;
            if (score%15==0 && itemManager.illegalItemsPerSuitcase<3) itemManager.illegalItemsPerSuitcase++;
            itemsToBeRemovedCount--;

            if (itemsToBeRemovedCount == 0)
            {
                StopAllCoroutines();
                canInteractWithToys = false;
                okText.SetActive(true);
                Invoke("SuitcaseResolved", 0);
            }
        }

        scoreText.text = "Score: " +Environment.NewLine+ score;
    }

    private void GameOver()
    {
        StopAllCoroutines();
        gameOver.SetActive(true);
        canInteractWithToys = false;
        Invoke("ReloadGame", 2);
    }

    private void ReloadGame()
    {
        gameOver.SetActive(false);
        okText.SetActive(false);

        SceneManager.LoadScene(0);
    }

    private void SuitcaseResolved()
    {
        okText.SetActive(false);
        Destroy(currentSuitcase.gameObject);
        CreateSuitcase();
    }

    private IEnumerator StartCountdown(float countdownValue)
    {
        countDownLeft = countdownValue;
        while (countDownLeft > -1)
        {
            timeText.text = countDownLeft.ToString();
            yield return new WaitForSeconds(1.0f);
            countDownLeft--;
        }
        GameOver();
    }
}

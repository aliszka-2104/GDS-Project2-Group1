using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class LevelUp
{
    public int score;
    public int newLegalItems;
    public int newForbiddenItems;
    [Range(0, 100)]
    public float smallBagChance;
    [Range(0, 100)]
    public float mediumBagChance;
    [Range(0, 100)]
    public float largeBagChance;
}

public class GameManager : MonoBehaviour
{
    public enum ITEMS
    {
        _NONE_,

        GUN,
        GRANADE,
        PARROT,
        POISON,
        BOMB,

        CYLINDER,
        JAM,
        BOOK,
        PILLOW,
        LAPTOP,
        HEADPHONES,
        PERFUME,
        SUNGLASSES,
    }

    public List<LevelUp> levels = new List<LevelUp>();
    private int currentLevel = 0;

    public ItemManager itemManager;
    public GameObject largeBagPrefab;
    public GameObject mediumBagPrefab;
    public GameObject smallBagPrefab;
    public bool canInteractWithToys = true;
    public Animator bagAnimator;
    public Animator gameOverAnimator;
    public Animator gameOverHighScoreAnimator;

    [SerializeField] private Text scoreText;
    [SerializeField] private Text highscoreText;
    [SerializeField] private Text gameOverScoreText;
    [SerializeField] private Text timeText;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject gameOverHighScore;
    [SerializeField] private GameObject okText;
    [SerializeField] private BlinkingLight light;
    private int score = 0;
    private int timeToResolveSuitcase = 5;
    private int itemsRemovedByFar;

    private Suitcase currentSuitcase;
    private float countDownLeft = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Save.LoadGame();
        gameOver.SetActive(false);
        gameOverHighScore.SetActive(false);
        okText.SetActive(false);
        scoreText.text = score.ToString();

        itemManager.LevelUp(levels[currentLevel]);
        CreateSuitcase();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private GameObject GetRandomBagPrefab()
    {
        float r = UnityEngine.Random.Range(0f, 1f);
        float chancesSum = levels[currentLevel].smallBagChance + levels[currentLevel].mediumBagChance + levels[currentLevel].largeBagChance;

        if (r * chancesSum < levels[currentLevel].smallBagChance)
        {
            return smallBagPrefab;
        }
        if (r * chancesSum < levels[currentLevel].smallBagChance + levels[currentLevel].mediumBagChance)
        {
            return mediumBagPrefab;
        }
        return largeBagPrefab;
    }

    private void CreateSuitcase()
    {
        bagAnimator.SetTrigger("ReloadBag");
        //bagAnimator.SetFloat("RepeatTime", 5f);

        StartCoroutine(StartCountdown(timeToResolveSuitcase));
        var suitcase = Instantiate(GetRandomBagPrefab());
        currentSuitcase = suitcase.GetComponent<Suitcase>();

        itemsRemovedByFar = 0;
        canInteractWithToys = true;
        //timeText.text = timeToResolveSuitcase.ToString();
        light.StartBlinking(timeToResolveSuitcase);
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
            //if (score%15==0 && itemManager.illegalItemsPerSuitcase<3) itemManager.illegalItemsPerSuitcase++;
            itemsRemovedByFar++;

            if (itemsRemovedByFar == itemManager.illegalItemsPerSuitcase)
            {
                StopAllCoroutines();
                canInteractWithToys = false;
                okText.SetActive(true);
                Invoke("SuitcaseResolved", 0);
            }
        }

        scoreText.text = score.ToString();
    }

    private void LevelUpIfYouShould()
    {
        if (currentLevel == levels.Count - 1) return;

        if (score < levels[currentLevel + 1].score) return;

        currentLevel++;
        itemManager.LevelUp(levels[currentLevel]);
    }

    private void GameOver()
    {
        StopAllCoroutines();
        canInteractWithToys = false;

        if (Save.HighScore > score)
        {
            gameOver.SetActive(true);
            gameOverAnimator.SetTrigger("GameOver");
            gameOverScoreText.text = score.ToString();
        }
        else
        {
            gameOverHighScore.SetActive(true);
            Save.HighScore = score;
            gameOverHighScoreAnimator.SetTrigger("HighScore");
            highscoreText.text = score.ToString();
        }

        Save.SaveGame();

        //Invoke("ReloadGame", 2);
    }

    public void ReloadGame()
    {
        
        SceneManager.LoadScene(0);
    }

    private void SuitcaseResolved()
    {
        light.StopBlinking();
        LevelUpIfYouShould();
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

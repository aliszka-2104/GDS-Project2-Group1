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
    public float timeToResolveSuitcase;
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
        CZEKAN,
        FETUS,
        FISH,
        FROG,
        HASH,
        HASH_BAG,
        HOLY_GRANADE,
        KASTET,
        KNIFE,
        KOKA,
        REVOLVER,
        SATAN,
        TEDDY_HASH,
        URAN,
        VIRUS,

        CYLINDER,
        JAM,
        BOOK,
        PILLOW,
        LAPTOP,
        HEADPHONES,
        PERFUME,
        SUNGLASSES,
        BALL,
        BRUSH,
        CLOCK,
        DWARF,
        FLOWER,
        LAMP,
        MASK,
        PIZZA,
        TEDDY,
        TOWEL,
        SHIRT,
        BIKINI
    }

    public List<LevelUp> levels = new List<LevelUp>();
    private int currentLevel = 0;
    private float timeToResolveSuitcase = 5;

    public ItemManager itemManager;
    public AudioManager audioManager;
    public AchievementManager achievementManager;

    public GameObject largeBagPrefab;
    public GameObject mediumBagPrefab;
    public GameObject smallBagPrefab;
    public GameObject tutorialBagPrefab;
    public bool canInteractWithToys = true;
    public Animator bagAnimator;
    public Animator gameOverAnimator;
    public Animator gameOverHighScoreAnimator;
    public GameObject tutorialScreen;

    public GameObject pause;

    [SerializeField] private Text scoreText;
    [SerializeField] private Text highscoreText;
    [SerializeField] private Text gameOverScoreText;
    [SerializeField] private Text timeText;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject gameOverHighScore;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private BlinkingLight light;
    //[SerializeField] private GameObject newForbiddenScreen;
    //[SerializeField] private GameObject newForbiddenImage;
    [SerializeField] private ForbiddenItemScreen forbiddenItemScreen;
    private int score = 0;
    private int itemsRemovedByFar;

    private GameObject currentSuitcase;
    private float countDownLeft = 0f;

    private bool paused = false;
    private bool tutorialPassed = false;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        Save.SetAchievementNames(achievementManager.achievements);
        Save.LoadGame();
        audioManager.AdjustSound();

        DisactivateWindows();
        Time.timeScale = 1;
        scoreText.text = score.ToString();

        //timeToResolveSuitcase = levels[currentLevel].timeToResolveSuitcase;
        //itemManager.LevelUp(levels[currentLevel]);
        LevelUp();
        CreateTutorialSuitcase();
        audioManager.PlayBackgroundMusic();
        pause.gameObject.SetActive(false);
    }

    private void DisactivateWindows()
    {
        gameOver.SetActive(false);
        gameOverHighScore.SetActive(false);
        pauseMenu.SetActive(false);
        forbiddenItemScreen.Hide();
    }

    public void PauseGame()
    {
        if (!tutorialPassed) return;
        Time.timeScale = 0;
        canInteractWithToys = false;
        pauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        canInteractWithToys = true;
    }

    public void PauseGameplay()
    {
        canInteractWithToys = false;
        light.PauseBlinking();
        paused = true;
    }

    public void ResumeGameplay()
    {
        canInteractWithToys = true;
        light.ResumeBlinking();
        paused = false;
        DisactivateWindows();
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
        StartCoroutine(StartCountdown(timeToResolveSuitcase));
        var suitcase = Instantiate(GetRandomBagPrefab());
        currentSuitcase = suitcase;

        itemsRemovedByFar = 0;
        light.StartBlinking(timeToResolveSuitcase);
        StartCoroutine(PlayNewBagEffects());
    }

    private void CreateTutorialSuitcase()
    {
        var suitcase = Instantiate(tutorialBagPrefab);
        currentSuitcase = suitcase;

        itemsRemovedByFar = 0;
        light.TutorialBlink();
        StartCoroutine(PlayNewBagEffects());
    }

    private IEnumerator PlayNewBagEffects()
    {
        while (paused)
        {
            yield return null;
        }
        
        canInteractWithToys = true;
        audioManager.PlayNewBag();
        bagAnimator.SetTrigger("ReloadBag");

        if (!tutorialPassed)
        {
            yield return new WaitForSeconds(.3f);
            tutorialScreen.SetActive(true);
        }
    }

    public void ItemSwiped(Item item)
    {
        if (itemManager.IsLegal(item.type))
        {
            GameOver();
        }
        else
        {
            score++;
            Save.TotalScore++;
            itemsRemovedByFar++;
            if (achievementManager.CheckAchievement())
            {
                audioManager.PlayTrophy();
            }

            if (itemsRemovedByFar == itemManager.illegalItemsPerSuitcase)
            {
                StopAllCoroutines();
                canInteractWithToys = false;
                Invoke("SuitcaseResolved", 0);
            }
        }

        scoreText.text = score.ToString();
        audioManager.PlayItemThrown();
    }

    private void LevelUpIfYouShould()
    {
        if (currentLevel == levels.Count - 1) return;

        if (score < levels[currentLevel + 1].score) return;

        currentLevel++;
        LevelUp();
    }

    private void LevelUp()
    {
        itemManager.LevelUp(levels[currentLevel]);
        timeToResolveSuitcase = levels[currentLevel].timeToResolveSuitcase;
        if (levels[currentLevel].newForbiddenItems == 1)
        {
            bagAnimator.SetTrigger("ReloadBag");
            forbiddenItemScreen.Show(itemManager.GetLatestObjectSprite());
            PauseGameplay();
        }
    }

    private void GameOver()
    {
        StopAllCoroutines();
        canInteractWithToys = false;

        if (Save.HighScore >= score)
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

        audioManager.PlayGameOverSounds();

        //Invoke("ReloadGame", 2);
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene(1);
    }

    private void SuitcaseResolved()
    {
        if (!tutorialPassed)
        {
            tutorialPassed = true;
            tutorialScreen.SetActive(false);
            pause.gameObject.SetActive(true);
        }
        light.StopBlinking();
        LevelUpIfYouShould();
        Destroy(currentSuitcase);
        CreateSuitcase();
    }

    private IEnumerator StartCountdown(float countdownValue)
    {
        while (paused || !tutorialPassed)
        {
            yield return null;
        }

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

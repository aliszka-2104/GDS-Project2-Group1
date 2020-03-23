using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;

[Serializable]
public class Achievement
{
    public int score;
    public string name;
    public Sprite image;
}

public class AchievementManager : MonoBehaviour
{
    public Achievement[] achievements;
    public GameObject achievementScreen;
    public float time;
    private Image trophyImageComponent;

    void Start()
    {
        trophyImageComponent = achievementScreen.GetComponent<Image>();
        achievementScreen.SetActive(false);
    }

    public bool CheckAchievement()
    {
        int score = Save.TotalScore;
        if (!achievements.Any(a => a.score == score)) return false;

        var gained = achievements.First(a => a.score == score);
        if (Save.GetAchievementGained(gained.name)) return false;

        Save.SetAchievementGained(gained.name);
        Save.SaveGame();
        StartCoroutine(ShowAchievement(gained));
        return true;
    }

    private IEnumerator ShowAchievement(Achievement gained)
    {
        Debug.Log("Achievement unlocked! " + gained.name);
        achievementScreen.SetActive(true);
        trophyImageComponent.sprite = gained.image;
        trophyImageComponent.SetNativeSize();
        yield return new WaitForSecondsRealtime(time);
        achievementScreen.SetActive(false);
    }
}

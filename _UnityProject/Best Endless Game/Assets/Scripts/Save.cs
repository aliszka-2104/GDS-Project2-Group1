using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Save
{
    private static int highScore;
    private static bool sound;

    public static int HighScore
    {
        get { return highScore; }
        set { highScore = value; }
    }

    public static bool Sound { get => sound; set => sound = value; }

    public static Dictionary<string, bool> Achievements = new Dictionary<string, bool>();

    private static List<string> achievemntsNames = new List<string>();

    public static void SaveGame()
    {
        PlayerPrefs.SetInt("HighScore", highScore);
        if (Sound) PlayerPrefs.SetInt("Sound", 1);
        else PlayerPrefs.SetInt("Sound", 0);

        foreach (var achievement in Achievements)
        {
            if (Achievements[achievement.Key])
            {
                PlayerPrefs.SetInt(achievement.Key, 1);
            }
            else
            {
                PlayerPrefs.SetInt(achievement.Key, 0);
            }
        }
    }

    public static void LoadGame()
    {
        HighScore = PlayerPrefs.GetInt("HighScore") > 0 ? PlayerPrefs.GetInt("HighScore") : 0;
        Sound = PlayerPrefs.GetInt("Sound") > 0;
        foreach (var achievementName in achievemntsNames)
        {
            Achievements[achievementName] = PlayerPrefs.GetInt(achievementName) > 0;
        }
    }

    public static void SetAchievementNames(Achievement[] achievements)
    {
        foreach (var achievement in achievements)
        {
            if (!achievemntsNames.Contains(achievement.name))
            {
                achievemntsNames.Add(achievement.name);
            }
        }
    }

    public static bool GetAchievementGained(string name)
    {
        return Achievements[name];
    }

    public static void SetAchievementGained(string name)
    {
        Achievements[name] = true;
    }

    public static void ToggleSound()
    {
        Sound = !Sound;
    }
}
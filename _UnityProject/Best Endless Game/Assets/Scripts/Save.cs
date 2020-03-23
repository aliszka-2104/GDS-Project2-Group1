using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Save
{
    private static int highScore;
    private static bool sound;
    private static bool music;
    private static int totalScore;

    public static int HighScore
    {
        get { return highScore; }
        set { highScore = value; }
    }

    public static int TotalScore { get => totalScore; set => totalScore = value; }

    public static bool Sound { get => sound; set => sound = value; }
    public static bool Music { get => music; set => music = value; }

    public static Dictionary<string, bool> Achievements = new Dictionary<string, bool>();

    private static List<string> achievemntsNames = new List<string>();

    public static void SaveGame()
    {
        PlayerPrefs.SetInt("HighScore", HighScore);
        PlayerPrefs.SetInt("TotalScore", TotalScore);
        if (Sound) PlayerPrefs.SetInt("Sound", 1);
        else PlayerPrefs.SetInt("Sound", -1);

        if (Music) PlayerPrefs.SetInt("Music", 1);
        else PlayerPrefs.SetInt("Music", -1);

        if (Achievements["a10"])
        {
            PlayerPrefs.SetInt("a10", 1);
        }
        else
        {
            PlayerPrefs.SetInt("a10", 0);
        }

        if (Achievements["a100"])
        {
            PlayerPrefs.SetInt("a100", 1);
        }
        else
        {
            PlayerPrefs.SetInt("a100", 0);
        }

        if (Achievements["a250"])
        {
            PlayerPrefs.SetInt("a250", 1);
        }
        else
        {
            PlayerPrefs.SetInt("a250", 0);
        }

        //foreach (var achievement in Achievements)
        //{
        //    if (Achievements[achievement.Key])
        //    {
        //        PlayerPrefs.SetInt(achievement.Key, 1);
        //    }
        //    else
        //    {
        //        PlayerPrefs.SetInt(achievement.Key, 0);
        //    }
        //}
    }

    public static void LoadGame()
    {
        if (!Achievements.ContainsKey("a10")) Achievements.Add("a10", false);
        if (!Achievements.ContainsKey("a100")) Achievements.Add("a100", false);
        if (!Achievements.ContainsKey("a250")) Achievements.Add("a250", false);

        HighScore = PlayerPrefs.GetInt("HighScore") > 0 ? PlayerPrefs.GetInt("HighScore") : 0;
        TotalScore = PlayerPrefs.GetInt("TotalScore") > 0 ? PlayerPrefs.GetInt("TotalScore") : 0;
        Sound = PlayerPrefs.GetInt("Sound") >= 0;
        Music = PlayerPrefs.GetInt("Music") >= 0;

        Achievements["a10"] = PlayerPrefs.GetInt("a10") > 0;
        Achievements["a100"] = PlayerPrefs.GetInt("a100") > 0;
        Achievements["a250"] = PlayerPrefs.GetInt("a250") > 0;

        //foreach (var achievementName in achievemntsNames)
        //{
        //    Achievements[achievementName] = PlayerPrefs.GetInt(achievementName) > 0;
        //}
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
        SaveGame();
    }

    public static void ToggleMusic()
    {
        Music = !Music;
        SaveGame();
    }
    
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Save
{
    private static int highScore;

    public static int HighScore
    {
        get { return highScore; }
        set { highScore = value; }
    }

    public static void SaveGame()
    {
        PlayerPrefs.SetInt("HighScore", highScore);
    }

    public static void LoadGame()
    {
        HighScore = PlayerPrefs.GetInt("HighScore") > 0 ? PlayerPrefs.GetInt("HighScore") : 0;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject creditsPrefab;
    public GameObject achievementsPrefab;
    public AudioManager audioManager;
    private GameObject credits;
    private GameObject achievements;
    private GameObject canvas;

    public void SetCanvas()
    {
        canvas = GameObject.Find("Canvas");
        if (credits == null) credits= Instantiate(creditsPrefab,canvas.transform);
        credits.SetActive(false);
        if (achievements == null) achievements = Instantiate(achievementsPrefab, canvas.transform);
        achievements.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
        audioManager.PlayMenuMusic();
        Save.SaveGame();
    }

    public void ShowCredits()
    {
        SetCanvas();
        //credits = GameObject.Find("CreditsScreen");
        credits.SetActive(true);
    }

    public void HideCredits()
    {
        credits.SetActive(false);
    }

    public void ShowAchievements()
    {
        SetCanvas();
        //credits = GameObject.Find("CreditsScreen");
        achievements.SetActive(true);
        achievements.SendMessage("UpdateAchievements");
    }

    public void HideAchievements()
    {
        achievements.SetActive(false);
    }

    public void ToggleSound()
    {
        Save.ToggleSound();
    }

    public void ToggleMusic()
    {
        Save.ToggleMusic();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject creditsPrefab;
    private GameObject credits;
    private GameObject canvas;

    public void SetCanvas()
    {
        canvas = GameObject.Find("Canvas");
        credits= Instantiate(creditsPrefab,canvas.transform);
        credits.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
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

    public void ToggleSound()
    {
        Save.ToggleSound();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    
}

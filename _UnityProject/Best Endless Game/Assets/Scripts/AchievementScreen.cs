using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementScreen : MonoBehaviour
{
    public Image a10;
    public Image a100;
    public Image a250;

    public Sprite a10_On;
    public Sprite a10_Off;
    public Sprite a100_On;
    public Sprite a100_Off;
    public Sprite a250_On;
    public Sprite a250_Off;

    // Start is called before the first frame update
    void Start()
    {
        Save.LoadGame();
    }
    
    public void UpdateAchievements()
    {
        if (Save.Achievements["a10"]) a10.sprite = a10_On;
        else a10.sprite = a10_Off;

        if (Save.Achievements["a100"]) a100.sprite = a100_On;
        else a100.sprite = a100_Off;

        if (Save.Achievements["a250"]) a250.sprite = a250_On;
        else a250.sprite = a250_Off;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

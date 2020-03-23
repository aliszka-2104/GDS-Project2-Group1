using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioButton : MonoBehaviour
{
    public Menu menu;
    public Sprite spriteOn;
    public Sprite spriteOff;
    public bool isMusic;

    public bool isOn;
    private Image image;
    // Start is called before the first frame update
    void Start()
    {
        Save.LoadGame();
        isOn = isMusic?Save.Music:Save.Sound;
        image = GetComponent<Image>();

        SetSprite();
    }

    public void Toggle()
    {

        isOn = !isOn;

        if (isMusic)
        {
            menu.ToggleMusic();
        }
        else
        {
            menu.ToggleSound();
        }
        SetSprite();
    }

    private void SetSprite()
    {
        if (isOn)
        {
            image.sprite = spriteOn;
        }
        else
        {
            image.sprite = spriteOff;
        }
    }
}

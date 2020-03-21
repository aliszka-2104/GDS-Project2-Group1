using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingLight : MonoBehaviour
{
    public Material material;
    public Sprite lightOn;
    public Sprite lightOff;
    public float maxBlink;
    public float minBlink;

    public float startTimeOn;
    public float endTimeOn;
    public float timeCut;

    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        material.SetFloat("_TintAmount", 0f);
        sr = GetComponent<SpriteRenderer>();
        //StartCoroutine(Blink(startTimeOn, startTimeOff));
    }
    
    public void StartBlinking(float time)
    {
        material.SetFloat("_TintAmount", 0f);

        float sum = endTimeOn;
        int count = 0;
        while (sum<time)
        {
            count++;
            sum += startTimeOn + timeCut * count;
        }

        StartCoroutine(Blink(startTimeOn, timeCut*count));
    }

    public void StopBlinking()
    {
        StopAllCoroutines();
    }

    IEnumerator Blink(float timeOn, float timeOff)
    {

        material.SetFloat("_TintAmount", maxBlink);
        sr.sprite = lightOn;
       yield return new WaitForSeconds(timeOn);
        if (timeOff>0)
        {
            material.SetFloat("_TintAmount", minBlink);
            sr.sprite = lightOff;
            yield return new WaitForSeconds(timeOff); 
        }

        timeOff = Mathf.Clamp(timeOff, 0, timeOff - timeCut);
        StartCoroutine(Blink(timeOn,timeOff));
    }
}

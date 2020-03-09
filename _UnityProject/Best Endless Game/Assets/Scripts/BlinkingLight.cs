using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingLight : MonoBehaviour
{
    public Material material;
    public float maxBlink;
    public float minBlink;
    public float timeInSeconds;

    public float startTimeOn;
    public float startTimeOff;
    public float endTimeOn;
    public float timeCut= 0.02f;

    // Start is called before the first frame update
    void Start()
    {
        material.SetFloat("_TintAmount", 0f);
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
       yield return new WaitForSeconds(timeOn);
        if (timeOff>0)
        {
            material.SetFloat("_TintAmount", minBlink);
            yield return new WaitForSeconds(timeOff); 
        }

        timeOff = Mathf.Clamp(timeOff, 0, timeOff - timeCut);
        StartCoroutine(Blink(timeOn,timeOff));
    }
}

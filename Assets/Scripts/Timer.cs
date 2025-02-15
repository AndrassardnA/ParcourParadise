using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] FinishLine finishLine;
    [SerializeField] Text timeText;
    float time;
    void Start()
    {
        time = 0;
        timeText.text = Mathf.Round(time * 100) * 0.01 + "";
    }

    void Update()
    {
        if (!finishLine.isStopped)
        {
            time += Time.deltaTime;
        }
        timeText.text = Mathf.Round(time * 100) * 0.01 + "";

    }
    public float getTime()
    {
        return time;
    }
}

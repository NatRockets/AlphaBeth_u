using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeCounter : MonoBehaviour
{
    [SerializeField] private float initialTime;
    [SerializeField] private TextMeshProUGUI timerUI;

    private float timeLeft;
    private IEnumerator timerCoroutine;

    private Action TimeoutCallback;

    private IEnumerator StartTimer()
    {
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            UpdateTimer(timeLeft);
            yield return null;
        }
        if (timeLeft <= 0)
        {
            timerUI.enabled = false;
            TimeoutCallback?.Invoke();
        }
    }

    private void UpdateTimer(float timeLeft)
    {
        if (timeLeft < 0)
        {
            timeLeft = 0;
        }
        timerUI.text = Mathf.FloorToInt(timeLeft % 60).ToString();
    }

    public void Activate()
    {
        timerUI.enabled = true;
        timerCoroutine = StartTimer();
        StartCoroutine(timerCoroutine);
    }

    public void Deactivate()
    {
        if(timerCoroutine != null)
        {
            timerUI.enabled = false;
            StopCoroutine(timerCoroutine);
        }
    }

    public void Refresh()
    {
        timeLeft = initialTime;
        UpdateTimer(timeLeft) ;
    }

    public void SetCallback(Action callback)
    {
        TimeoutCallback = callback;
    }
}

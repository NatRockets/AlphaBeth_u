using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TestButton : MonoBehaviour
{
    private Button target;
    private Text value;

    public void InitAnswer()
    {
        target = GetComponent<Button>();
        value = transform.GetChild(1).GetComponent<Text>();
    }
    
    public void ResetAnswer()
    {
        target.onClick.RemoveAllListeners();
        target.interactable = false;
        value.text = "";
    }

    public void SetAnswer(string labelT, Action callback)
    {
        target.onClick.AddListener(() =>
        {
            target.interactable = false;
            callback();
        });
        value.DOText(labelT, 1f)
            .OnComplete(() => target.interactable = true);
    }
}

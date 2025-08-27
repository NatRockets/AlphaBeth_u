using System;
using System.Collections.Generic;
using System.Text;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class WordBuilder : MonoBehaviour
{
    [SerializeField] private GameObject startObject;
    [SerializeField] private Button startButton;
    [SerializeField] private GameObject finishObject;
    [SerializeField] private Button finishButton;
    [SerializeField] private Text resText;
    [SerializeField] private TimeCounter timer;
    [SerializeField] private SampleProvider provider;

    [Header("Fields")] 
    [SerializeField] private GameObject source;
    [SerializeField] private Text questionAsset;
    [SerializeField] private string[] questionVariants;
    [SerializeField] private TestButton[] answerButtons;

    private int rightAnswers;
    private int questionCounter;
    private string answer;

    private List<string> currentAssets = new();

    private void Awake()
    {
        foreach (TestButton answerButton in answerButtons)
        {
            answerButton.InitAnswer();
        }

        timer.SetCallback(() => OnAnswerPicked(false));

        startButton.onClick.AddListener(StartTest);
        finishButton.onClick.AddListener(FinishTest);
    }

    private void OnEnable()
    {
        //provider.GetAnagrams(10);
        
        startObject.SetActive(true);
        finishObject.SetActive(false);
        source.SetActive(false);
    }

    private void OnDisable()
    {
        timer.Deactivate();
    }

    private void StartTest()
    {
        startObject.SetActive(false);

        rightAnswers = 0;
        currentAssets = provider.GetAnagrams(10);
        questionCounter = 0;
        source.SetActive(true);
        InitQuestion();
    }

    private void OnLetterPicked(char letter)
    {
        var sb = new StringBuilder(answer);
        sb.Append(letter);
        answer = sb.ToString();

        if (answer.Length > 3)
        {
            OnAnswerPicked(string.Equals(answer, currentAssets[questionCounter], StringComparison.OrdinalIgnoreCase));
        }
    }
    
    private void OnAnswerPicked(bool right)
    {
        timer.Deactivate();
        foreach (var but in answerButtons)
        {
            but.ResetAnswer();
        }
        
        if (right)
        {
            rightAnswers++;
        }
        
        questionCounter++;
        if (questionCounter >= currentAssets.Count)
        {
            OnTestFinished();
        }
        else
        {
            InitQuestion();
        }
    }

    private void InitQuestion()
    {
        answer = "";
        questionAsset.text = "";
        questionAsset.DOText(questionVariants[Random.Range(0, questionVariants.Length)], 1f)
            .OnComplete(() =>
            {
                SampleProvider.Shuffle(answerButtons);
                answerButtons[0].SetAnswer(currentAssets[questionCounter][0].ToString(), () => OnLetterPicked(currentAssets[questionCounter][0]));
                answerButtons[1].SetAnswer(currentAssets[questionCounter][1].ToString(), () => OnLetterPicked(currentAssets[questionCounter][1]));
                answerButtons[2].SetAnswer(currentAssets[questionCounter][2].ToString(), () => OnLetterPicked(currentAssets[questionCounter][2]));
                answerButtons[3].SetAnswer(currentAssets[questionCounter][3].ToString(), () => OnLetterPicked(currentAssets[questionCounter][3]));
            });
        
        timer.Refresh();
        timer.Activate();
    }

    private void OnTestFinished()
    {
        foreach (var but in answerButtons)
        {
            but.ResetAnswer();
        }
        
        source.SetActive(false);
        
        resText.text = $"You correctly formed {rightAnswers} words";
        finishObject.SetActive(true);
    }

    private void FinishTest()
    {
        finishObject.SetActive(false);
        startObject.SetActive(true);
    }
}

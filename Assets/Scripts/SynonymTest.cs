using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SynonymTest : MonoBehaviour
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
    [SerializeField] private TextMeshProUGUI questionAsset;
    [SerializeField] private TestButton[] answerButtons;

    private int rightAnswers;
    private int questionCounter;

    private List<List<string>> currentAssets = new();

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
        //provider.GetSynonyms(10);
        
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
        currentAssets = provider.GetSynonyms(10);
        questionCounter = 0;
        source.SetActive(true);
        InitQuestion();
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
        questionAsset.text = currentAssets[questionCounter][0];
        
        SampleProvider.Shuffle(answerButtons);
        answerButtons[0].SetAnswer(currentAssets[questionCounter][1], () => OnAnswerPicked(true));
        answerButtons[1].SetAnswer(currentAssets[questionCounter][2], () => OnAnswerPicked(false));
        answerButtons[2].SetAnswer(currentAssets[questionCounter][3], () => OnAnswerPicked(false));
        answerButtons[3].SetAnswer(currentAssets[questionCounter][4], () => OnAnswerPicked(false));
        
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
        
        resText.text = $"You correctly chose {rightAnswers} synonyms";
        finishObject.SetActive(true);
    }

    private void FinishTest()
    {
        finishObject.SetActive(false);
        startObject.SetActive(true);
    }
}

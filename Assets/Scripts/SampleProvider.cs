using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;

[Serializable]
public class SynonymGroup
{
    public List<string> words;
}

public class AlphabethSample
{
    public string question;
    public List<char> answers;//1st answer is right
}
public class SampleProvider : MonoBehaviour
{
    [SerializeField] private string[] anagramTemplates;
    [SerializeField] private SynonymGroup[] synonymTemplates;

    private string alphabethString = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public List<List<string>> GetSynonyms(int number)
    {
        var allWords = synonymTemplates.SelectMany(g => g.words).Distinct().ToList();
        var synonymMap = synonymTemplates
            .SelectMany(g => g.words.Select(w => (w, group: g.words)))
            .GroupBy(t => t.w)
            .ToDictionary(g => g.Key, g => g.First().group);

        var rnd = new System.Random();
        var assets = new List<List<string>>(number);

        for (int i = 0; i < number; i++)
        {
            // 2. Выбираем случайную группу и слово из неё
            var group = synonymTemplates[rnd.Next(synonymTemplates.Length)];
            string word = group.words[rnd.Next(group.words.Count)];

            // 3. Синоним, отличный от слова
            string synonym = group.words.Count == 1
                ? word                                    // нет альтернативы
                : group.words.Where(w => w != word)
                    .OrderBy(_ => rnd.Next())   // псевдо-перемешать
                    .First();

            // 4. Случайные слова, НЕ синонимы к выбранному слову
            string[] nonSynonyms = Enumerable.Range(0, 3)
                .Select(_ =>
                {
                    string candidate;
                    do { candidate = allWords[rnd.Next(allWords.Count)]; }
                    while (synonymMap[candidate] == synonymMap[word]); // пока не найдём не-синоним
                    return candidate;
                })
                .ToArray();

            assets.Add(new List<string> { word, synonym, nonSynonyms[0], nonSynonyms[1], nonSynonyms[2] });
        }

        /*foreach (var asset in assets)
        {
            Debug.Log($"{asset[0]} : {asset[1]}, {asset[2]}, {asset[3]}, {asset[4]}");
        }*/
        
        return assets;
    }

    private AlphabethSample AlphaRule1()
    {
        //before
        List<int> indexes = new List<int>();
        int tempIndex = Random.Range(1, alphabethString.Length);
        indexes.Add(tempIndex);
        indexes.Add(tempIndex - 1);
        
        AlphabethSample target = new AlphabethSample();
        target.answers = new List<char>();
        target.answers.Add(alphabethString[tempIndex - 1]);
        target.question = $"Which letter comes before {alphabethString[tempIndex]}?";
        
        for (int i = 0; i < 3; i++)
        {
            do
            {
                tempIndex = Random.Range(1, alphabethString.Length);
            } 
            while (indexes.Contains(tempIndex));
            indexes.Add(tempIndex);
            target.answers.Add(alphabethString[tempIndex]);
        }
        return target;
    }
    
    private AlphabethSample AlphaRule2()
    {
        //after
        List<int> indexes = new List<int>();
        int tempIndex = Random.Range(0, alphabethString.Length - 1);
        indexes.Add(tempIndex);
        indexes.Add(tempIndex + 1);
        
        AlphabethSample target = new AlphabethSample();
        target.answers = new List<char>();
        target.answers.Add(alphabethString[tempIndex + 1]);
        target.question = $"Which letter comes after {alphabethString[tempIndex]}?";
        
        for (int i = 0; i < 3; i++)
        {
            do
            {
                tempIndex = Random.Range(0, alphabethString.Length - 1);
            } 
            while (indexes.Contains(tempIndex));
            indexes.Add(tempIndex);
            target.answers.Add(alphabethString[tempIndex]);
        }
        return target;
    }
    
    private AlphabethSample AlphaRule3()
    {
        //closer
        List<int> indexes = new List<int>();
        int tempIndex = Random.Range(0, alphabethString.Length);
        indexes.Add(tempIndex);
        
        AlphabethSample target = new AlphabethSample();
        target.answers = new List<char>();
        target.question = $"Which letter is closer to {alphabethString[tempIndex]}?";
        
        for (int i = 0; i < 4; i++)
        {
            do
            {
                tempIndex = Random.Range(0, alphabethString.Length);
            } 
            while (indexes.Contains(tempIndex));
            indexes.Add(tempIndex);
        }
        tempIndex = indexes[0];
        indexes.RemoveAt(0);
        indexes.Sort((a, b) => Math.Abs(a - tempIndex).CompareTo(Math.Abs(b - tempIndex)));
        foreach (int i in indexes)
        {
            target.answers.Add(alphabethString[i]);
        }
        return target;
    }

    private delegate AlphabethSample AlphabethRuleDelegate();

    public List<AlphabethSample> GetAlphabethSamples(int number)
    {
        List<AlphabethSample> samples = new List<AlphabethSample>();
        
        
        for (int i = 0; i < number; i++)
        {
            AlphabethRuleDelegate rule = Random.Range(0, 3) switch
            {
                0 => AlphaRule1,
                1 => AlphaRule2,
                2 => AlphaRule3,
                _ => throw new ArgumentOutOfRangeException()
            };
            samples.Add(rule());
        }
        
        /*foreach (var asset in samples)
        {
            Debug.Log($"{asset.question} : {asset.answers[0]}, {asset.answers[1]}, {asset.answers[2]}, {asset.answers[3]}");
        }*/
        
        return samples;
    }
    
    public List<string> GetAnagrams(int number)
    {
        List<string> anagrams = new List<string>();
        string temp;
        for (int i = 0; i < number; i++)
        {
            do
            {
                temp = anagramTemplates[Random.Range(0, anagramTemplates.Length)];
            } 
            while (anagrams.Contains(temp));
            anagrams.Add(temp);
        }
        return anagrams;
    }
    
    public static void Shuffle<T> (T[] array)
    {
        int n = array.Length;
        while (n > 1) 
        {
            int k = Random.Range(0, n--);
            (array[n], array[k]) = (array[k], array[n]);
        }
    }
}

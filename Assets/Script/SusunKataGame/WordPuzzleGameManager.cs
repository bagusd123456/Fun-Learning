using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class WordPuzzleGameManager : MonoBehaviour
{
    public static WordPuzzleGameManager Instance; // Singleton instance

    public static Action<string> OnWordDrop;

    private bool isDragging = false; // Flag to track if an object is being dragged
    public string correctWord; // Correct word to be formed
    public List<string> currentWords; // List of words that have been formed

    public List<WordObject> choiceWordObjectsList; // List of word objects
    public List<WordContainer> wordContainersList; // List of word containers

    public GameObject panelWin; // Panel that shows the win condition

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
        InitCorrectWord();
        RandomizeWordObjects();
    }

    private void OnEnable()
    {
        OnWordDrop += RegisterCorrectWords;
    }

    private void OnDisable()
    {
        OnWordDrop -= RegisterCorrectWords;
    }

    #region Game Initialization

    // Assign word containers to the word correct word
    public void InitCorrectWord()
    {
        for (int i = 0; i < correctWord.Length; i++)
        {
            wordContainersList[i].SetWord(correctWord[i].ToString());
        }
    }

    [ContextMenu("Randomize Word Objects")]
    public void RandomizeWordObjects()
    {
        List<char> usedCorrectWord = new List<char>();
        //Reset all word objects
        foreach (var wordObject in choiceWordObjectsList)
        {
            wordObject.Initialize("");
        }

        // Randomize the correct word placement in the word objects
        for (int i = 0; i < correctWord.Length; i++)
        {
            int randomIndex = Random.Range(0, choiceWordObjectsList.Count);

            while (choiceWordObjectsList[randomIndex].word != "")
            {
                randomIndex = Random.Range(0, choiceWordObjectsList.Count);
            }
            choiceWordObjectsList[randomIndex].Initialize(correctWord[i].ToString());
            usedCorrectWord.Add(correctWord[i]);
        }

        // Fill the remaining word objects with random characters
        foreach (var wordObject in choiceWordObjectsList)
        {
            if (wordObject.word == "")
            {
                char randomChar = GetRandomChar();
                wordObject.Initialize(randomChar.ToString());
            }
        }
    }

    public void WinCondition()
    {
        panelWin.SetActive(true);
    }

    private char GetRandomChar()
    {
        // Generate a random character between A-Z
        return (char)Random.Range('A', 'Z' + 1);
    }
    #endregion

    #region Input Handling
    // Method to set isDragging flag
    public void SetDragging(bool value)
    {
        isDragging = value;
    }

    // Method to check if an object is currently being dragged
    public bool IsDragging()
    {
        return isDragging;
    }
    #endregion

    #region Word Checking

    //Register the word drop event to current words list
    public void RegisterCorrectWords(string word)
    {
        currentWords.Add(word);
        if (CurrentWordsFinished())
        {
            WinCondition();
            Debug.Log("All words are correct!");
        }
    }

    //Check if the current words list contains all the correct words
    public bool CurrentWordsFinished()
    {
        List<int> correctWordsIndex = new List<int>();
        // Check if the current words list has the same length as the correct word
        if (currentWords.Count != correctWord.Length)
        {
            return false;
        }

        // Check if the current words list contains all the correct words
        for (int i = 0; i < correctWord.Length; i++)
        {
            char currentWord = correctWord[i];
            for (int j = 0; j < currentWords.Count; j++)
            {
                if (correctWordsIndex.Contains(j))
                {
                    continue;
                }

                if (currentWord == currentWords[j][0])
                {
                    correctWordsIndex.Add(j);
                    break;
                }
            }
        }

        if (correctWordsIndex.Count-1 == correctWord.Length)
        {
            return true;
        }

        //// Check if the current words list contains all the correct words
        //for (int i = 0; i < correctWord.Length; i++)
        //{
        //    for (int j = 0; j < currentWords.Count; j++)
        //    {
        //        //if correct word index already added, skip
        //        if (correctWordsIndex.Contains(j))
        //        {
        //            continue;
        //        }

        //        // if the current word is equal to the correct word
        //        if (correctWord[i].ToString() == currentWords[j])
        //        {
        //            correctWordsIndex.Add(j);
        //            Debug.Log($"Current word: {currentWords[i]}" +
        //                      $"is Equal to correctWord: {correctWord[i]}");
        //            continue;
        //        }

        //        if (j == currentWords.Count - 1)
        //        {
        //            return false;
        //        }
        //    }
        //}

        return true;
    }

    #endregion
}

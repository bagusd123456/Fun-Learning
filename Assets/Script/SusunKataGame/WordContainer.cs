using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WordContainer : MonoBehaviour
{
    private TMP_Text textComponent;

    public string word;
    public bool isCorrect = false;
    public bool isFilled = false;

    public void Awake()
    {
        textComponent = GetComponentInChildren<TMP_Text>();
    }

    public void SetWord(string newWord)
    {
        word = newWord;

        if (textComponent != null)
        {
            textComponent.text = newWord;
        }
    }
}

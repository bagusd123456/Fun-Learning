using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WordObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 initialPosition;
    private TMP_Text textComponent;

    public string word;
    public bool isCorrect = false;
    void Awake()
    {
        textComponent = GetComponentInChildren<TMP_Text>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        initialPosition = rectTransform.localPosition;
    }

    public void Initialize(string newWord)
    {
        word = newWord;

        if (textComponent != null)
        {
            textComponent.text = newWord;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        if (WordPuzzleGameManager.Instance.IsDragging() && !isCorrect)
        {
            return;
        }
        WordPuzzleGameManager.Instance.SetDragging(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / GetComponentInParent<Canvas>().scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // Check if dropped on a valid drop zone with WordContainer script
        if (eventData.pointerEnter != null)
        {
            // Get the drop zone
            GameObject dropZone = eventData.pointerEnter.gameObject;

            // Check if the drop zone has the WordContainer script
            WordContainer wordContainer = dropZone.GetComponent<WordContainer>();

            Debug.Log($"Dropped on: {dropZone.name}");

            if (wordContainer != null && !wordContainer.isFilled)
            {
                Debug.Log("Dropped on a drop zone!");
                // Get the text component of the drop zone
                Text dropZoneText = dropZone.GetComponentInChildren<Text>();

                // Get the text of the dragged object
                Text draggedText = GetComponentInChildren<Text>();

                // Check if the dropped word matches the expected word in the drop zone
                if (wordContainer.word == word)
                {
                    WordPuzzleGameManager.OnWordDrop?.Invoke(word);
                    Debug.Log("Word placed correctly!");

                    // You can add more logic here such as scoring, completing levels, etc.

                    // Set the position of the dragged object to the drop zone
                    rectTransform.position = dropZone.transform.position;
                    isCorrect = true;
                    wordContainer.isFilled = true;
                    wordContainer.isCorrect = true;
                }
                else
                {
                    // Reset the position if the word is not placed correctly
                    transform.localPosition = initialPosition;
                }
            }
            else
            {
                // Reset the position if the drop zone doesn't have WordContainer script
                transform.localPosition = initialPosition;
            }
        }
        else
        {
            // Reset the position if not dropped on any object
            transform.localPosition = initialPosition;
        }

        WordPuzzleGameManager.Instance.SetDragging(false);
    }
}
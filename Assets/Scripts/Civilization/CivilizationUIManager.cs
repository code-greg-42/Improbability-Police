using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class CivilizationUIManager : MonoBehaviour
{
    private readonly float wordDisplayDelay = 0.1f; // delay between each word
    private readonly float infoWarningTimer = 5.0f; // total duration for info warning

    // references
    [SerializeField] private TMP_Text mainText;
    [SerializeField] private TMP_Text startText;
    [SerializeField] private TMP_Text happinessScore;
    [SerializeField] private TMP_Text uniquenessScore;
    [SerializeField] private TMP_Text characteristic;
    [SerializeField] private RawImage image;
    [SerializeField] private TMP_InputField userInput;
    [SerializeField] private Button submitButton;
    [SerializeField] private TMP_Text informationWarning;
    [SerializeField] private Button backToMenuButton;

    public void ActivateBackToMenuButton()
    {
        backToMenuButton.gameObject.SetActive(true);
    }

    public void ActivateImage()
    {
        image.gameObject.SetActive(true);
    }

    public void ActivateUserInput()
    {
        userInput.gameObject.SetActive(true);
        submitButton.gameObject.SetActive(true);
    }

    public void DeactivateBackToMenuButton()
    {
        backToMenuButton.gameObject.SetActive(false);
    }

    public void DeactivateStartText()
    {
        startText.gameObject.SetActive(false);
    }

    public void DeactivateUserInput()
    {
        userInput.gameObject.SetActive(false);
        submitButton.gameObject.SetActive(false);
    }

    public string GetUserInput()
    {
        // save user input to variable
        string input = userInput.text;

        // clear user input
        userInput.text = "";

        return input;
    }

    public void SetCharacteristic(string description)
    {
        characteristic.text = "Leadership Personality: " + description;
    }

    public void SetHappinessScore(string score)
    {
        happinessScore.text = "Happiness: " + score;
    }

    public void SetImage(Texture2D texture)
    {
        image.texture = texture;
    }

    public void SetUniquenessScore(string score)
    {
        uniquenessScore.text = "Uniqueness: " + score;
    }

    public IEnumerator DisplayStartText(string text)
    {
        string[] words = text.Split(' '); // Split the text into an array of words
        foreach (string word in words)
        {
            startText.text += word + " "; // Add the next word to the text
            yield return new WaitForSeconds(wordDisplayDelay); // Wait for the specified delay
        }
    }

    public IEnumerator DisplayMainText(string text, bool initialText = false)
    {
        // clear existing text
        mainText.text = "";

        // set position based on if it is the initial text, or a typical feedback round
        RectTransform rectTransform = mainText.gameObject.GetComponent<RectTransform>();

        Vector2 anchoredPosition = rectTransform.anchoredPosition;

        if (initialText)
        {
            anchoredPosition.y = 50;
        }
        else
        {
            anchoredPosition.y = -50;
        }

        // assign correct position back to the transform
        rectTransform.anchoredPosition = anchoredPosition;

        string[] words = text.Split(' '); // Split the text into an array of words
        foreach (string word in words)
        {
            mainText.text += word + " "; // Add the next word to the text
            yield return new WaitForSeconds(wordDisplayDelay); // Wait for the specified delay
        }
    }

    public IEnumerator RunInformationWarning(string warningText, bool isError = false)
    {
        if (isError)
        {
            informationWarning.color = Color.red;
        }
        else
        {
            informationWarning.color = Color.green;
        }

        informationWarning.text = warningText;

        yield return new WaitForSeconds(infoWarningTimer);

        informationWarning.text = "";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class CivilizationUIManager : MonoBehaviour
{
    private readonly float wordDisplayDelay = 0.1f; // delay between each word

    // references
    [SerializeField] private TMP_Text mainText;
    [SerializeField] private TMP_Text startText;
    [SerializeField] private TMP_Text happinessScore;
    [SerializeField] private TMP_Text uniquenessScore;
    [SerializeField] private TMP_Text characteristic;
    [SerializeField] private RawImage image;
    [SerializeField] private TMP_InputField userInput;
    [SerializeField] private Button submitButton;

    public void ActivateImage()
    {
        image.gameObject.SetActive(true);
    }

    public void ActivateUserInput()
    {
        userInput.gameObject.SetActive(true);
        submitButton.gameObject.SetActive(true);
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

    public void SetCharacteristic(string description)
    {
        characteristic.text = "Leadership Personality: " + description;
    }

    public void SetHappinessScore(float score)
    {
        happinessScore.text = "Happiness: " + score;
    }

    public void SetImage(Texture2D texture)
    {
        image.texture = texture;
    }

    public void SetUniquenessScore(float score)
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

    public IEnumerator DisplayMainText(string text)
    {
        // clear existing text
        mainText.text = "";

        string[] words = text.Split(' '); // Split the text into an array of words
        foreach (string word in words)
        {
            mainText.text += word + " "; // Add the next word to the text
            yield return new WaitForSeconds(wordDisplayDelay); // Wait for the specified delay
        }
    }
}

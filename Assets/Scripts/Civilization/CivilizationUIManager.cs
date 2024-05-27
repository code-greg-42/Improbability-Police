using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CivilizationUIManager : MonoBehaviour
{
    public static CivilizationUIManager Instance { get; private set; }

    private readonly float wordDisplayDelay = 0.1f; // delay between each word

    // references
    [SerializeField] private TMP_Text mainText;
    [SerializeField] private TMP_Text startText;
    [SerializeField] private RawImage image;

    private void Awake()
    {
        Instance = this;
    }

    public void ActivateImage()
    {
        image.gameObject.SetActive(true);
    }

    public void DeactivateStartText()
    {
        startText.gameObject.SetActive(false);
    }

    public void SetImage(Texture2D texture)
    {
        image.texture = texture;
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

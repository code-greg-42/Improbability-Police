using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuGameManager : MonoBehaviour
{
    [SerializeField] private Button backstoryButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button newCivilizationButton;
    [SerializeField] private Button exitButton;

    [SerializeField] private TMP_Text happinessScoreText;
    [SerializeField] private TMP_Text uniquenessScoreText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text characteristicText;

    void Start()
    {
        if (DataManager.Instance.HasUserActed())
        {
            continueButton.gameObject.SetActive(true);
            continueButton.onClick.AddListener(OnContinueButton);
            LoadData();
        }

        backstoryButton.onClick.AddListener(LoadBackstory);
        newCivilizationButton.onClick.AddListener(OnNewButton);
        exitButton.onClick.AddListener(OnExitButton);
    }

    private void LoadData()
    {
        // get data values
        string happinessScore = DataManager.Instance.GetHappinessScore();
        string uniquenessScore = DataManager.Instance.GetUniquenessScore();
        string characteristic = DataManager.Instance.GetMostCommonCharacteristic();

        // calculate score
        int? score = CalculateScore(happinessScore, uniquenessScore);

        if (score.HasValue)
        {
            // set values to UI
            happinessScoreText.text = "Happiness: " + happinessScore;
            uniquenessScoreText.text = "Uniqueness: " + uniquenessScore;
            scoreText.text = "Score: " + score.Value;
        }

        if (characteristic != null)
        {
            characteristicText.text = characteristic;
        }
    }

    private void LoadBackstory()
    {
        SceneManager.LoadScene(1);
    }

    private void OnContinueButton()
    {
        SceneManager.LoadScene(2);
    }

    private void OnNewButton()
    {
        // reset data manager variables
        DataManager.Instance.SetCivilizationDescription("The is the first step in the game and the user has not chosen to build anything yet. The civilization is currently a collection of people, with basic shelter structures set up, but nothing more.");
        DataManager.Instance.SetHappinessScore("None");
        DataManager.Instance.SetUniquenessScore("None");
        DataManager.Instance.SetPlanetDescription(null);
        DataManager.Instance.SetUserHasActed(false);
        DataManager.Instance.SetImage(null);
        DataManager.Instance.ResetCharacteristics();
        DataManager.Instance.ResetPastUserActions();

        SceneManager.LoadScene(2);
    }

    private void OnExitButton()
    {
        Application.Quit();
    }

    private int? CalculateScore(string score1, string score2)
    {
        // Try to parse the first score
        if (!float.TryParse(score1, out float parsedScore1))
        {
            return null; // Return null if parsing fails
        }

        // Try to parse the second score
        if (!float.TryParse(score2, out float parsedScore2))
        {
            return null; // Return null if parsing fails
        }

        // Calculate the average of the two scores
        float averageScore = (parsedScore1 + parsedScore2) / 2.0f;

        // round the score to nearest int
        int roundedScore = Mathf.RoundToInt(averageScore);

        return roundedScore;
    }
}

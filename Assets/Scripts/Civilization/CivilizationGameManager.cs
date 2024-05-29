using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CivilizationGameManager : MonoBehaviour
{
    private readonly float startDelay = 1.5f;
    private readonly float startTextMinimumTime = 17.0f;
    private readonly string initialStartText = "Your ships, which contain 100,000 people, are heading towards your assigned planet. Food, water, basic shelter, and security are guaranteed from the technology you possess. Happiness is not. Reminder: Unique civilizations are rewarded greatly. Good Luck! Your assigned planet is: Planet ";
    private readonly string openingQuestion = "\n\nYou will be landing momentarily. What is your first action for your civilization?";
    private readonly string nextRoundQuestion = "\n\nWhat will you do next?";

    // references
    [SerializeField] private CivilizationFeedback civilizationFeedback;
    [SerializeField] private PlanetGenerator planetGenerator;
    [SerializeField] private CivilizationUIManager uiManager;

    private void Start()
    {
        //StartCoroutine(RunIntro());
        StartCoroutine(TestFeedback());
    }

    private IEnumerator RunIntro()
    {
        // to give user a moment for eyes to adjust, and time for the api key to load
        yield return new WaitForSeconds(startDelay);

        // get new planet name
        string planetName = planetGenerator.GeneratePlanetName();
        // init start text with planet name
        StartCoroutine(uiManager.DisplayStartText(initialStartText + planetName));

        // set start time for ensuring start text is displayed for long enough
        float startTime = Time.time;

        // run planet generator and wait for results
        yield return StartCoroutine(planetGenerator.GeneratePlanetCoroutine(planetName));
        // get results and save to variables
        string planetDescription = planetGenerator.GetPlanetDescription();
        Texture2D planetImage = planetGenerator.GetPlanetImage();

        // find elapsed time and wait for additional time if necessary
        float elapsedTime = Time.time - startTime;
        
        if (elapsedTime < startTextMinimumTime)
        {
            float waitTime = startTextMinimumTime - elapsedTime;
            yield return new WaitForSeconds(waitTime);
        }

        // set results to UI and display main description text on screen
        uiManager.SetImage(planetImage);
        uiManager.DeactivateStartText();
        uiManager.ActivateImage();
        yield return StartCoroutine(uiManager.DisplayMainText(planetDescription + openingQuestion, true));
        
        // activate user input field
        uiManager.ActivateUserInput();
    }

    private IEnumerator TestFeedback()
    {
        yield return new WaitForSeconds(startDelay);
        yield return StartCoroutine(civilizationFeedback.GetFeedbackCoroutine("IQ-459712 is a diverse, uninhabited planet marked by striking topographical features. Enormous dunes sweep across its surface, while extensive, humid wetlands host a teeming ecosystem, scoring 8/10 for animal life. The terrain dramatically drops into profound gorges that slice through the landscape. Despite the abundant wetlands, the planet lacks overall water coverage, only scoring a mere 1/10. It's a remarkable world of contrast and biodiversity, sans signs of intelligent life, awaiting future exploration.", "I first throw a large party to celebrate the new civilization we have created.", "The is the first step in the game and the user has not chosen to build anything yet. The civilization is currently a collection of people, with basic shelter structures set up, but nothing more.", "None", "None"));

        if (civilizationFeedback.IsFeedbackSuccess())
        {
            (string feedbackDescription, string happinessScore, string uniquenessScore, string characteristic) = civilizationFeedback.GetFeedback();
            Debug.Log("Feedback Description: " + feedbackDescription);
            Debug.Log("Happiness Score: " + happinessScore);
            Debug.Log("Uniqueness Score: " + uniquenessScore);
            Debug.Log("Characteristic: " + characteristic);

            // set scores to UI
            uiManager.SetCharacteristic(characteristic);
            uiManager.SetHappinessScore(happinessScore);
            uiManager.SetUniquenessScore(uniquenessScore);

            // display feedback in main text slot
            StartCoroutine(uiManager.DisplayMainText(feedbackDescription + nextRoundQuestion));

            yield return StartCoroutine(civilizationFeedback.GetFeedbackImageCoroutine(feedbackDescription));

            if (civilizationFeedback.IsImageSuccess())
            {
                Texture2D feedbackImage = civilizationFeedback.GetFeedbackImage();
                uiManager.SetImage(feedbackImage);
            }
        }
        else
        {
            Debug.LogError("Failed to retrieve valid feedback.");
        }

        // activate back to menu button
        uiManager.ActivateBackToMenuButton();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}

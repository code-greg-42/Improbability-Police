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
        if (DataManager.Instance.HasUserActed())
        {
            StartCoroutine(LoadCurrent());
        }
        else
        {
            StartCoroutine(RunIntro());
        }
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

        // save planetDescription and image to DataManager
        DataManager.Instance.SetPlanetDescription(planetDescription);
        DataManager.Instance.SetImage(planetImage);

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
        
        // activate user input field and back to menu button
        uiManager.ActivateUserInput();
        uiManager.ActivateBackToMenuButton();
    }

    private IEnumerator LoadCurrent()
    {
        // give user a moment for eyes to adjust, and time for the api key to load
        yield return new WaitForSeconds(startDelay);

        // get existing values from data manager
        Texture2D image = DataManager.Instance.GetImage();
        string currentText = DataManager.Instance.GetCivilizationDescription();
        string happinessScore = DataManager.Instance.GetHappinessScore();
        string uniquenessScore = DataManager.Instance.GetUniquenessScore();
        string characteristic = DataManager.Instance.GetCharacteristic();

        // set values to UI
        uiManager.SetImage(image);
        uiManager.SetHappinessScore(happinessScore);
        uiManager.SetUniquenessScore(uniquenessScore);
        uiManager.SetCharacteristic(characteristic);

        // activate image component
        uiManager.ActivateImage();

        // display text in main text box
        yield return StartCoroutine(uiManager.DisplayMainText(currentText + nextRoundQuestion));

        // activate user input box and back to menu button
        uiManager.ActivateUserInput();
        uiManager.ActivateBackToMenuButton();
    }

    private IEnumerator RunFeedbackCycle(string userInput)
    {
        // get current data from data manager
        string currentPlanetDescription = DataManager.Instance.GetPlanetDescription();
        string currentCivilizationDescription = DataManager.Instance.GetCivilizationDescription();
        string currentHappinessScore = DataManager.Instance.GetHappinessScore();
        string currentUniquenessScore = DataManager.Instance.GetUniquenessScore();
        List<string> pastUserActions = DataManager.Instance.GetPastUserActions();

        // send data and with user input to gpt-4
        yield return StartCoroutine(civilizationFeedback.GetFeedbackCoroutine(currentPlanetDescription, userInput, currentCivilizationDescription, currentHappinessScore, currentUniquenessScore, pastUserActions));

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

            // save variables to data manager
            DataManager.Instance.SetCivilizationDescription(feedbackDescription);
            DataManager.Instance.SetHappinessScore(happinessScore);
            DataManager.Instance.SetUniquenessScore(uniquenessScore);
            DataManager.Instance.AddToCharacteristics(characteristic);

            // set data manager bool to true as player has acted
            DataManager.Instance.SetUserHasActed(true);

            // display feedback in main text slot
            StartCoroutine(uiManager.DisplayMainText(feedbackDescription + nextRoundQuestion));

            yield return StartCoroutine(civilizationFeedback.GetFeedbackImageCoroutine(feedbackDescription));

            if (civilizationFeedback.IsImageSuccess())
            {
                Texture2D feedbackImage = civilizationFeedback.GetFeedbackImage();

                // save to data manager and set to UI
                DataManager.Instance.SetImage(feedbackImage);
                uiManager.SetImage(feedbackImage);
            }
        }
        else
        {
            Debug.LogError("Failed to retrieve valid feedback.");
        }

        // activate user input and back to menu button
        uiManager.ActivateUserInput();
        uiManager.ActivateBackToMenuButton();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void OnSubmit()
    {
        // get user input and set to a variable
        string userInput = uiManager.GetUserInput();

        // save user action to data manager
        DataManager.Instance.AddToPastUserActions(userInput);

        // update ui
        uiManager.DeactivateUserInput();
        uiManager.DeactivateBackToMenuButton();
        StartCoroutine(uiManager.RunInformationWarning("Reply Sent! Waiting..."));

        // start feedback cycle
        StartCoroutine(RunFeedbackCycle(userInput));
    }
}

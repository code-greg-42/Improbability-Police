using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilizationGameManager : MonoBehaviour
{
    public static CivilizationGameManager Instance { get; private set; }

    private readonly float startDelay = 1.5f;
    private readonly float startTextMinimumTime = 17.0f;
    private readonly string initialStartText = "Your ships, which contain 100,000 people, are heading towards your assigned planet. Food, water, basic shelter, and security are guaranteed from the technology you possess. Happiness is not. Reminder: Unique civilizations are rewarded greatly. Good Luck! Your assigned planet is: Planet ";
    private readonly string openingQuestion = "\n\nYou will be landing momentarily. What is your first action for your civilization?";

    private void Awake()
    {
        Instance = this;
    }

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
        string planetName = PlanetGenerator.Instance.GeneratePlanetName();
        // init start text with planet name
        StartCoroutine(CivilizationUIManager.Instance.DisplayStartText(initialStartText + planetName));

        // set start time for ensuring start text is displayed for long enough
        float startTime = Time.time;

        // run planet generator and wait for results
        yield return StartCoroutine(PlanetGenerator.Instance.GeneratePlanetCoroutine(planetName));
        // get results and save to variables
        string planetDescription = PlanetGenerator.Instance.GetPlanetDescription();
        Texture2D planetImage = PlanetGenerator.Instance.GetPlanetImage();

        // find elapsed time and wait for additional time if necessary
        float elapsedTime = Time.time - startTime;
        
        if (elapsedTime < startTextMinimumTime)
        {
            float waitTime = startTextMinimumTime - elapsedTime;
            yield return new WaitForSeconds(waitTime);
        }

        // set results to UI and display main description text on screen
        CivilizationUIManager.Instance.SetImage(planetImage);
        CivilizationUIManager.Instance.DeactivateStartText();
        CivilizationUIManager.Instance.ActivateImage();
        yield return StartCoroutine(CivilizationUIManager.Instance.DisplayMainText(planetDescription + openingQuestion));
        
        // activate user input field
        CivilizationUIManager.Instance.ActivateUserInput();
    }

    private IEnumerator TestFeedback()
    {
        yield return new WaitForSeconds(startDelay);
        yield return StartCoroutine(CivilizationFeedback.Instance.GetFeedbackCoroutine("IQ-459712 is a diverse, uninhabited planet marked by striking topographical features. Enormous dunes sweep across its surface, while extensive, humid wetlands host a teeming ecosystem, scoring 8/10 for animal life. The terrain dramatically drops into profound gorges that slice through the landscape. Despite the abundant wetlands, the planet lacks overall water coverage, only scoring a mere 1/10. It's a remarkable world of contrast and biodiversity, sans signs of intelligent life, awaiting future exploration.", "I first throw a large party to celebrate the new civilization we have created."));
        string feedback = CivilizationFeedback.Instance.GetFeedback();
        Debug.Log(feedback);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilizationGameManager : MonoBehaviour
{
    public static CivilizationGameManager Instance { get; private set; }

    private readonly int startDelay = 2;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(RunIntro());
    }

    private IEnumerator RunIntro()
    {
        // to give user a moment for eyes to adjust, and time for the api key to load
        yield return new WaitForSeconds(startDelay);

        // get new planet name
        string planetName = PlanetGenerator.Instance.GeneratePlanetName();
        // init start text with planet name
        StartCoroutine(CivilizationUIManager.Instance.DisplayStartText("You Have Been Assigned: Planet " + planetName));

        // run planet generator and wait for results
        yield return StartCoroutine(PlanetGenerator.Instance.GeneratePlanetCoroutine(planetName));
        // get results and save to variables
        string planetDescription = PlanetGenerator.Instance.GetPlanetDescription();
        Texture2D planetImage = PlanetGenerator.Instance.GetPlanetImage();

        // set results to UI and display main description text on screen
        CivilizationUIManager.Instance.SetImage(planetImage);
        CivilizationUIManager.Instance.DeactivateStartText();
        StartCoroutine(CivilizationUIManager.Instance.DisplayMainText(planetDescription));
        CivilizationUIManager.Instance.ActivateImage();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuGameManager : MonoBehaviour
{
    [SerializeField] private Button backstoryButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button newCivilizationButton;

    void Start()
    {
        backstoryButton.onClick.AddListener(LoadBackstory);
        continueButton.onClick.AddListener(OnContinueButton);
        newCivilizationButton.onClick.AddListener(OnNewButton);
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

        SceneManager.LoadScene(2);
    }
}

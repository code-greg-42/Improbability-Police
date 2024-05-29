using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    private string planetDescription;
    private string civilizationDescription = "The is the first step in the game and the user has not chosen to build anything yet. The civilization is currently a collection of people, with basic shelter structures set up, but nothing more.";
    private string happinessScore = "None";
    private string uniquenessScore = "None";

    private List<string> pastUserActions = new List<string>();

    private Texture2D image;
    private bool userHasActed;

    private List<string> characteristics = new List<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddToCharacteristics(string characteristic)
    {
        characteristics.Add(characteristic);
    }

    public void AddToPastUserActions(string userAction)
    {
        pastUserActions.Add(userAction);
    }

    public string GetCharacteristic()
    {
        if (characteristics.Count > 0)
        {
            return characteristics[characteristics.Count - 1];
        }

        return null; // Return null if the list is empty
    }

    public Texture2D GetImage()
    {
        return image;
    }

    // Method to get the most common characteristic in the list
    public string GetMostCommonCharacteristic()
    {
        if (characteristics.Count == 0)
        {
            return null; // Return null if the list is empty
        }

        Dictionary<string, int> characteristicCounts = new Dictionary<string, int>();

        // Count the occurrences of each characteristic
        foreach (string characteristic in characteristics)
        {
            if (characteristicCounts.ContainsKey(characteristic))
            {
                characteristicCounts[characteristic]++;
            }
            else
            {
                characteristicCounts[characteristic] = 1;
            }
        }

        // Find the characteristic with the highest count
        string mostCommon = characteristics[0];
        int maxCount = 0;

        foreach (KeyValuePair<string, int> pair in characteristicCounts)
        {
            if (pair.Value > maxCount)
            {
                mostCommon = pair.Key;
                maxCount = pair.Value;
            }
            else if (pair.Value == maxCount)
            {
                // If there is a tie, return the most recently added characteristic
                mostCommon = characteristics.FindLast(c => c == pair.Key);
            }
        }

        return mostCommon;
    }

    public string GetCivilizationDescription()
    {
        return civilizationDescription;
    }

    public string GetHappinessScore()
    {
        return happinessScore;
    }

    public List<string> GetPastUserActions()
    {
        return pastUserActions;
    }

    public string GetPlanetDescription()
    {
        return planetDescription;
    }

    public string GetUniquenessScore()
    {
        return uniquenessScore;
    }

    public bool HasUserActed()
    {
        return userHasActed;
    }

    public void ResetCharacteristics()
    {
        characteristics = new List<string>();
    }

    public void ResetPastUserActions()
    {
        pastUserActions = new List<string>();
    }

    public void SetCivilizationDescription(string description)
    {
        civilizationDescription = description;
    }

    public void SetUserHasActed(bool hasActed)
    {
        userHasActed = hasActed;
    }

    public void SetHappinessScore(string score)
    {
        happinessScore = score;
    }

    public void SetImage(Texture2D texture)
    {
        image = texture;
    }

    public void SetPlanetDescription(string description)
    {
        planetDescription = description;
    }

    public void SetUniquenessScore(string score)
    {
        uniquenessScore = score;
    }
}

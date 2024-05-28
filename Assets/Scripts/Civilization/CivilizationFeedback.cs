using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilizationFeedback : MonoBehaviour
{
    public static CivilizationFeedback Instance { get; private set; }

    private string feedbackDescription;
    private float happinessScore;
    private float uniquenessScore;
    private string characteristic;

    private Texture2D feedbackImage;
    private bool feedbackSuccess;
    private bool imageSuccess;

    private readonly string instructions = "Let's play a game. I'm the user, I've been given control of a new civilization with advanced building technology. " +
            "I am landing on a new planet to build a new civilization. Food, water, and basic shelter are guaranteed, but happiness of the civilization is not. " +
            "I'm going to give you a description of the planet, a description of where the civilization is at, and then give you a chosen action for the next step for the civilization. " +
            "Then, I'd like you to give an updated description of where the civilization is at, a happiness score (1 to 100 with 100 the most happy) for the civilization, a uniqueness score (1 to 100 with 100 as the most unique) for how unique the resulting civilization is, and a characteristic that describes my personality, given the chosen action for the civilization.\n\n" +
            "For the updated description, keep the description to 120 words or less. Include details about the happiness of the civilization. Include any problems that you might see occurring, pertaining to the happiness of the civilization, due to the route for the civilization that the user chose.\n\n" +
            "For happiness, be as objective as possible. It's a game, and we don't want everyone winning. Use what you know about people to project a happiness score.\n\n" +
            "For uniqueness, use both what you know about present civilizations, but also people's dreams for perfect civilizations, to project what the 'average' person would choose in this situation, and create a score with that as the benchmark. Be objective.\n\n" +
            "For characteristic, use a word that describes the action that I took in the user action section\n\n" +
            "Formatting (VERY IMPORTANT):\n\n" +
            "It should include four sections and nothing else. Description, Happiness, Uniqueness, Characteristic. The description section should be 120 words or less of text only with no additional formatting, the happiness/uniqueness sections should be only numbers between 1 and 100, and the characteristic section should be a single word.\n\n" +
            "Example response:\n\n" +
            "*begin response*\n" +
            "Description: *120 word or less description goes here*\n\n" +
            "Happiness: 75\n\n" +
            "Uniqueness: 25\n" +
            "Characteristic: Resourceful" +
            "*end response*\n\n" +
            "Here is the information for the game:";

    private void Awake()
    {
        Instance = this;
    }

    public IEnumerator GetFeedbackCoroutine(string planetDescription, string userAction, string civilizationDescription = "The is the first step in the game and the user has not chosen to build anything yet. The civilization is currently a collection of people, with basic shelter structures set up, but nothing more.")
    {
        string formattedPrompt = FormatPrompt(planetDescription, userAction, civilizationDescription);

        // send formatted prompt to open ai api and wait for response
        yield return OpenAIManager.Instance.GetResponseCoroutine(formattedPrompt);

        // get result
        string feedback = OpenAIManager.Instance.GetResponse();

        // parse into feedback variables
        if (!string.IsNullOrEmpty(feedback))
        {
            feedbackSuccess = ParseFeedback(feedback);

            if (!feedbackSuccess)
            {
                Debug.LogError("Failed to parse feedback correctly.");
            }
        }
        else
        {
            Debug.LogError("Failed to receive feedback from OpenAI.");
        }
    }

    public IEnumerator GetFeedbackImageCoroutine(string civilizationDescription)
    {
        yield return OpenAIManager.Instance.GetImageCoroutine(civilizationDescription);
        feedbackImage = OpenAIManager.Instance.GetImage();

        if (feedbackImage != null)
        {
            imageSuccess = true;
            Debug.Log("Image generation successful.");
        }
        else
        {
            Debug.Log("Image generation unsuccessful.");
        }
    }

    public Texture2D GetFeedbackImage()
    {
        // reset imageSuccess bool and return image
        imageSuccess = false;
        return feedbackImage;
    }

    public (string, float, float, string) GetFeedback()
    {
        // Reset feedbackSuccess to false after the values are retrieved
        var result = (feedbackDescription, happinessScore, uniquenessScore, characteristic);
        feedbackSuccess = false;
        return result;
    }

    public bool IsFeedbackSuccess()
    {
        return feedbackSuccess;
    }

    public bool IsImageSuccess()
    {
        return imageSuccess;
    }

    // helper method
    private string FormatPrompt(string planetDescription, string userAction, string civilizationDescription)
    {
        string prompt = instructions + "\n\nPlanet Description:\n" + planetDescription + "\n\nCivilization Description:\n" + civilizationDescription + "\n\nUser Action:\n" + userAction;
        return prompt;
    }

    private bool ParseFeedback(string feedback)
    {
        try
        {
            string[] lines = feedback.Split(new[] { "\n" }, System.StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                if (line.StartsWith("Description: "))
                {
                    feedbackDescription = line.Substring("Description: ".Length);
                }
                else if (line.StartsWith("Happiness: "))
                {
                    if (float.TryParse(line.Substring("Happiness: ".Length), out float happiness))
                    {
                        happinessScore = happiness;
                    }
                    else
                    {
                        Debug.LogError("Failed to parse Happiness score.");
                        return false;
                    }
                }
                else if (line.StartsWith("Uniqueness: "))
                {
                    if (float.TryParse(line.Substring("Uniqueness: ".Length), out float uniqueness))
                    {
                        uniquenessScore = uniqueness;
                    }
                    else
                    {
                        Debug.LogError("Failed to parse Uniqueness score.");
                        return false;
                    }
                }
                else if (line.StartsWith("Characteristic: "))
                {
                    characteristic = line.Substring("Characteristic: ".Length);
                }
            }

            // Ensure all required fields are populated
            if (string.IsNullOrEmpty(feedbackDescription) || string.IsNullOrEmpty(characteristic) || happinessScore == 0 || uniquenessScore == 0)
            {
                Debug.LogError("Incomplete feedback data.");
                return false;
            }

            return true;
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Exception occurred while parsing feedback: " + ex.Message);
            return false;
        }
    }
}

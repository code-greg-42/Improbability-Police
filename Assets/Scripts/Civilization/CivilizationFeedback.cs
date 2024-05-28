using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilizationFeedback : MonoBehaviour
{
    public static CivilizationFeedback Instance { get; private set; }

    private string feedback;
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

    public string GetFeedback()
    {
        return feedback;
    }

    public IEnumerator GetFeedbackCoroutine(string planetDescription, string userAction, string civilizationDescription = "The is the first step in the game and the user has not chosen to build anything yet. The civilization is currently a collection of people, with basic shelter structures set up, but nothing more.")
    {
        string formattedPrompt = FormatPrompt(planetDescription, userAction, civilizationDescription);

        // send formatted prompt to open ai api and wait for response
        yield return OpenAIManager.Instance.GetResponseCoroutine(formattedPrompt);
        // get result and set to feedback variable
        feedback = OpenAIManager.Instance.GetResponse();
    }

    private string FormatPrompt(string planetDescription, string userAction, string civilizationDescription)
    {
        string prompt = instructions + "\n\nPlanet Description:\n" + planetDescription + "\n\nCivilization Description:\n" + civilizationDescription + "\n\nUser Action:\n" + userAction;
        return prompt;
    }
}

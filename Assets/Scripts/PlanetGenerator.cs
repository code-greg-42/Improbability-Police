using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGenerator : MonoBehaviour
{
    private int terrainsUsed = 3; // number of different terrains sent in the prompt

    // Array of uppercase alphabet letters for planet name
    private string[] alphabet = new string[]
    {
        "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
        "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
    };

    private string[] terrainTypes = new string[]
    {
        "flat", "hilly", "mountainous", "aquatic",
        "desert", "forest", "jungle", "swamp", "tundra", "urban",
        "cave", "volcanic", "plains", "canyon", "coastal",
        "marsh", "meadow", "savanna", "steppe", "badlands", "dunes",
        "gorge", "island", "peninsula", "plateau", "prairie", "valley",
        "wetlands", "glacier", "delta", "bayou", "rainforest", "moor",
        "mesa", "oasis", "foothills", "sinkhole"
    };

    // single random instance
    private System.Random random = new System.Random();

    void Start()
    {
        string randomPrompt = GeneratePlanetPrompt();
        Debug.Log(randomPrompt);
    }

    private string GetRandomTerrainType()
    {
        return terrainTypes[random.Next(terrainTypes.Length)];
    }

    private string[] GenerateTerrainTypes(int amount)
    {
        string[] selectedTerrains = new string[amount];

        // loop through and add each new random terrain
        for (int i = 0; i < amount; i++)
        {
            selectedTerrains[i] = GetRandomTerrainType();
        }

        return selectedTerrains;
    }

    private string GeneratePlanetName()
    {
        // randomize letters for planet name
        string firstLetter = alphabet[random.Next(alphabet.Length)];
        string secondLetter = alphabet[random.Next(alphabet.Length)];

        // randomize planet number
        int planetNumber = random.Next(0, 1000000);

        // combine random values into full planet name
        string planetName = firstLetter + secondLetter + "-" + planetNumber;

        return planetName;
    }

    private string GeneratePlanetPrompt()
    {
        // get new terrain types
        string[] selectedTerrains = GenerateTerrainTypes(terrainsUsed);
        // get new planet name
        string planetName = GeneratePlanetName();

        // randomize scores for water and animal life
        int waterFactor = random.Next(0, 11);
        int animalLifeFactor = random.Next(0, 11);

        // combine into prompt
        string prompt = "Describe the fictional planet " + planetName + "in 200 words or less. This planet has but is not limited to the following terrain types: " + selectedTerrains[0] + ", " + selectedTerrains[1] + ", " + selectedTerrains[2] + ". It has a water score of " + waterFactor + "/10" + " and an animal life score of " + animalLifeFactor + "/10. No intelligent life has been found.";
        return prompt;
    }
}

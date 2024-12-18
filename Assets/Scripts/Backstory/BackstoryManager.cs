using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackstoryManager : MonoBehaviour
{
    // affects speed of words across the screen
    private readonly float wordDisplayDelay = 0.1f; // delay between each word
    private readonly float pauseMultiplier = 0.25f; // used in conjuction with the length of each paragraph to determine pause between each paragraph

    // delay timers
    private readonly float startDelay = 1.0f;
    private readonly float endingDelay = 4.2f;

    // rate for fading music
    private readonly float musicFadeTimer = 0.35f; // fading 1% every X seconds

    [Header("References")]
    [SerializeField] private TMP_Text backstoryText;
    [SerializeField] private Button menuButton;
    [SerializeField] private BackstorySpaceshipSpawner spaceshipSpawner;
    [SerializeField] private AudioSource audioSource;

    // narrative writing separated into paragraphs
    private readonly List<string> paragraphs = new List<string>
    {
        "The year is 5,231,100,224 AD. Humanity has left the cradle of Earth and taken to the stars after the sun's collapse. Our mission: to find new homes among the countless stars.",
        "Throughout our journey, we have discovered many hospitable planets. Each one holds the promise of a new beginning, but the path to these new worlds is long and arduous. As we travel, we prepare for our future by running advanced simulations, aimed at finding new and unlikely leaders.",
        "In these simulations, participants are tasked with a crucial mission: to build and lead a new society. Armed with advanced molecular technology that constructs at lightning speeds, and accompanied by teams of capable individuals, players must decide the best course of action to establish and nurture a thriving civilization.",
        "Given the technology, survival is not a primary concern. Instead, success is measured by the overall happiness and fulfillment of the civilization. However, there is another critical factor that could land you the role of a leader: uniqueness. Humanity seeks to cultivate diverse societies across the stars, so out-of-the-box thinking is highly rewarded.",
        "As a player in these simulations, your decisions will shape the future. Will you lead your people to prosperity and happiness? Will your vision stand out among the stars? The fate of humanity's next chapter lies in your hands."
    };

    void Start()
    {
        StartCoroutine(RunScene());

        menuButton.onClick.AddListener(ReturnToMenu);
    }

    private IEnumerator RunScene()
    {
        // wait for start delay
        yield return new WaitForSeconds(startDelay);

        // display paragraphs
        yield return StartCoroutine(DisplayParagraphs());

        // wait additional time
        yield return new WaitForSeconds(endingDelay);

        // return to main menu
        SceneManager.LoadScene(0);
    }

    private IEnumerator DisplayParagraphs()
    {
        for (int i = 0; i < paragraphs.Count; i++)
        {
            backstoryText.text = ""; // Clear any existing text
            string[] words = paragraphs[i].Split(' '); // Split the paragraph into an array of words

            // If this is the last paragraph, stop the spaceship spawning
            if (i == paragraphs.Count - 1)
            {
                // stop spawning of additional ships
                spaceshipSpawner.StopSpawn();

                // start the music fade
                StartCoroutine(FadeMusic());
            }

            foreach (string word in words)
            {
                backstoryText.text += word + " "; // Add the next word to the text
                yield return new WaitForSeconds(wordDisplayDelay); // Wait for the specified delay
            }

            float paragraphPauseDuration = words.Length * pauseMultiplier; // Calculate pause duration based on the number of words
            yield return new WaitForSeconds(paragraphPauseDuration); // Wait for the specified pause duration
        }
    }

    private IEnumerator FadeMusic()
    {
        while (audioSource.volume > 0.01f)
        {
            audioSource.volume -= 0.01f;
            yield return new WaitForSeconds(musicFadeTimer);
        }

        audioSource.volume = 0f;
        audioSource.Stop();
    }

    private void ReturnToMenu()
    {
        // load main menu scene
        SceneManager.LoadScene(0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class OpenAIManager : MonoBehaviour
{
    private string apiKey;
    private readonly string apiUrl = "https://api.openai.com/v1/chat/completions";
    private readonly int maxTokens = 150;
    private readonly string systemInstructions = "You are a helpful assistant.";

    public TMP_InputField inputField;
    public TMP_Text responseText;
    public Button submitButton;

    void Start()
    {
        LoadApiKeyFromConfig();

        if (string.IsNullOrEmpty(apiKey))
        {
            Debug.LogError("API Key is missing. Please set the API key in the config.json file.");
            return;
        }

        submitButton.onClick.AddListener(OnSubmit);
    }

    private void LoadApiKeyFromConfig()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "config.json");

        if (File.Exists(path))
        {
            // Read the config file and deserialize the API key
            string json = File.ReadAllText(path);
            Config config = JsonUtility.FromJson<Config>(json);
            apiKey = config.apiKey;
        }
        else
        {
            Debug.LogError("Config file not found: " + path);
        }
    }

    public void OnSubmit()
    {
        // Get the user prompt from the input field and send the prompt
        string prompt = inputField.text;
        SendPrompt(prompt, OnResponseReceived);
    }

    public void SendPrompt(string prompt, System.Action<string> callback)
    {
        // Start the coroutine to send the prompt to the OpenAI API
        StartCoroutine(SendPromptCoroutine(prompt, callback));
    }

    private IEnumerator SendPromptCoroutine(string prompt, System.Action<string> callback)
    {
        // Create the messages list
        List<Message> messages = new List<Message>
        {
            new Message { role = "system", content = systemInstructions },
            new Message { role = "user", content = prompt }
        };

        // Create the request object
        OpenAIRequest requestObject = new OpenAIRequest
        {
            model = "gpt-4",
            messages = messages,
            max_tokens = maxTokens
        };

        // Convert the JSON object to a string
        string jsonData = JsonUtility.ToJson(requestObject);

        // Create a new UnityWebRequest for the POST request
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");

        // Set the request body to the JSON data
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);

        // Set up the response handler
        request.downloadHandler = new DownloadHandlerBuffer();

        // Set the request headers for content type and authorization
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);

        // Send the request and wait for the response
        yield return request.SendWebRequest();

        // Check if the request was successful
        if (request.result == UnityWebRequest.Result.Success)
        {
            // Parse the response JSON
            string responseText = request.downloadHandler.text;
            OpenAIChatResponse response = JsonUtility.FromJson<OpenAIChatResponse>(responseText);

            // Execute the callback with the received message content
            callback(response.choices[0].message.content.Trim());
        }
        else
        {
            // Log an error if the request failed
            Debug.LogError("Error: " + request.error);
            callback(null);
        }
    }

    private void OnResponseReceived(string response)
    {
        if (!string.IsNullOrEmpty(response))
        {
            responseText.text = response;
        }
        else
        {
            responseText.text = "Error receiving response.";
        }
    }

    [System.Serializable]
    private class Config
    {
        public string apiKey;
    }

    [System.Serializable]
    private class OpenAIChatResponse
    {
        public Choice[] choices;
    }

    [System.Serializable]
    private class Choice
    {
        public Message message;
    }

    [System.Serializable]
    private class Message
    {
        public string role;
        public string content;
    }

    [System.Serializable]
    private class OpenAIRequest
    {
        public string model;
        public List<Message> messages;
        public int max_tokens;
    }
}


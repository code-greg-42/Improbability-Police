using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class OpenAIManager : MonoBehaviour
{
    public static OpenAIManager Instance { get; private set; }

    private string apiKey;
    //private readonly string apiUrl = "https://api.openai.com/v1/chat/completions";
    //private readonly string imageApiUrl = "https://api.openai.com/v1/images/generations"; // URL for image generation
    private readonly string backendUrl = "https://ec2-3-15-237-163.us-east-2.compute.amazonaws.com";
    private readonly string responseEndpoint = "/get_response";
    private readonly string imageEndpoint = "/get_image";

    private string recentResponse;
    private Texture2D recentImage;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //LoadApiKeyFromConfig();

        //if (string.IsNullOrEmpty(apiKey))
        //{
        //    Debug.LogError("API Key is missing. Please set the API key in the config.json file.");
        //    return;
        //}
    }

    //private void LoadApiKeyFromConfig()
    //{
    //    string path = Path.Combine(Application.streamingAssetsPath, "config.json");

    //    if (File.Exists(path))
    //    {
    //        // Read the config file and deserialize the API key
    //        string json = File.ReadAllText(path);
    //        Config config = JsonUtility.FromJson<Config>(json);
    //        apiKey = config.apiKey;
    //    }
    //    else
    //    {
    //        Debug.LogError("Config file not found: " + path);
    //    }
    //}

    public string GetResponse()
    {
        return recentResponse;
    }

    public Texture2D GetImage()
    {
        return recentImage;
    }

    public IEnumerator GetResponseCoroutine(string prompt, string systemInstructions = "You are a helpful assistant.", int maxTokens = 200)
    {
        // Create the messages list
        //List<Message> messages = new List<Message>
        //{
        //    new Message { role = "system", content = systemInstructions },
        //    new Message { role = "user", content = prompt }
        //};

        //// Create the request object
        //OpenAIRequest requestObject = new OpenAIRequest
        //{
        //    model = "gpt-4",
        //    messages = messages,
        //    max_tokens = maxTokens
        //};

        OpenAIRequest requestObject = new OpenAIRequest
        {
            prompt = prompt,
            systemInstructions = systemInstructions,
            maxTokens = maxTokens
        };

        // Use the helper method to create the request
        UnityWebRequest request = CreatePostRequest(responseEndpoint, requestObject);

        // Disable SSL verification
        request.certificateHandler = new BypassCertificate();

        // Send the request and wait for the response
        yield return request.SendWebRequest();

        // Check if the request was successful
        if (request.result == UnityWebRequest.Result.Success)
        {
            // Parse the response JSON
            string responseText = request.downloadHandler.text;
            Debug.Log(responseText);
            OpenAIChatResponse response = JsonUtility.FromJson<OpenAIChatResponse>(responseText);

            Debug.Log(response);
            // set the recent response variable
            recentResponse = response.choices[0].message.content.Trim();
        }
        else
        {
            // Log an error if the request failed
            Debug.LogError("Error: " + request.error);
            recentResponse = null;
        }
    }

    public IEnumerator GetImageCoroutine(string description)
    {
        // Create the request object for image generation
        //OpenAIImageRequest requestObject = new OpenAIImageRequest
        //{
        //    prompt = description,
        //    n = 1,
        //    size = "1024x1024"
        //};

        OpenAIImageRequest requestObject = new OpenAIImageRequest
        {
            description = description
        };

        // Use the helper method to create the request
        UnityWebRequest request = CreatePostRequest(imageEndpoint, requestObject);

        // Disable SSL verification
        request.certificateHandler = new BypassCertificate();

        // Send the request and wait for the response
        yield return request.SendWebRequest();

        // Check if the request was successful
        if (request.result == UnityWebRequest.Result.Success)
        {
            // Parse the response JSON
            string responseText = request.downloadHandler.text;
            OpenAIImageResponse response = JsonUtility.FromJson<OpenAIImageResponse>(responseText);

            // Download the image
            UnityWebRequest imageRequest = UnityWebRequestTexture.GetTexture(response.data[0].url);
            yield return imageRequest.SendWebRequest();

            if (imageRequest.result == UnityWebRequest.Result.Success)
            {
                // Set the recent image variable
                recentImage = DownloadHandlerTexture.GetContent(imageRequest);
            }
            else
            {
                // Log an error if the image download failed
                Debug.LogError("Error downloading image: " + imageRequest.error);
                recentImage = null;
            }
        }
        else
        {
            // Log an error if the request failed
            Debug.LogError("Error: " + request.error);
            recentImage = null;
        }
    }

    private UnityWebRequest CreatePostRequest(string endpoint, object requestObject)
    {
        // Convert the JSON object to a string
        string jsonData = JsonUtility.ToJson(requestObject);

        Debug.Log(jsonData);

        // Create a new UnityWebRequest for the POST request
        UnityWebRequest request = new UnityWebRequest(backendUrl + endpoint, "POST");

        // Set the request body to the JSON data
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);

        // Set up the response handler
        request.downloadHandler = new DownloadHandlerBuffer();

        // Set the request headers for content type and authorization
        request.SetRequestHeader("Content-Type", "application/json");
        //request.SetRequestHeader("Authorization", "Bearer " + apiKey);

        return request;
    }

    private class BypassCertificate : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
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
        public string prompt;
        public string systemInstructions;
        public int maxTokens;
    }

    [System.Serializable]
    private class OpenAIImageRequest
    {
        public string description;
    }

    [System.Serializable]
    private class OpenAIImageResponse
    {
        public Data[] data;
    }

    [System.Serializable]
    private class Data
    {
        public string url;
    }
}


using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using TMPro;
using System.IO;
using System;

public class AINarrativeGenerator : MonoBehaviour
{
    public EnvironmentLoader environmentLoader;
    private ErdaTime erdaTime;
    private string openAiUrl = "https://api.openai.com/v1/engines/gpt-3.5-turbo/completions"; // Ensure this is the correct URL
    private string apiKey;
    public TextMeshProUGUI narrativeText;

    void Start()
    {
        string apiKeyFilePath = @"C:\Users\Main\.openai\ApiKey.txt";

        try
        {
            apiKey = File.ReadAllText(apiKeyFilePath);
        }
        catch (Exception e)
        {
            Debug.LogError("Error reading the API key file: " + e.Message);
        }

        erdaTime = GameObject.Find("WorldTime").GetComponent<ErdaTime>();

        if (environmentLoader == null)
        {
            environmentLoader = GetComponent<EnvironmentLoader>();
        }

        StartCoroutine(SendDataToAI());
    }

    IEnumerator SendDataToAI()
    {
        EnvironmentData environmentData = environmentLoader.GetCurrentEnvironment();
        string prompt = PreparePrompt(environmentData);  // Make sure this method just prepares a descriptive prompt based on environment data
        string chatEndpointUrl = "https://api.openai.com/v1/chat/completions";

        // Include max_tokens in the requestData and adjust the system message
        string requestData = "{\"model\": \"gpt-3.5-turbo\", \"max_tokens\": 150, \"messages\": [{\"role\": \"system\", \"content\": \"You are a helpful assistant. Your task is to describe the scene based on the environmental data provided. Do not take actions or make decisions for the player, and do not add any information that is not present in the environmental data.\"}, {\"role\": \"user\", \"content\": \"" + prompt + "\"}]}";
        byte[] bodyRaw = Encoding.UTF8.GetBytes(requestData);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(chatEndpointUrl, ""))
        {
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            Debug.Log("API Key: " + apiKey);
            webRequest.SetRequestHeader("Authorization", "Bearer " + apiKey);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
                Debug.Log("Response: " + webRequest.downloadHandler.text);
            }
            else
            {
                string responseText = ParseResponse(webRequest.downloadHandler.text);
                UpdateNarrativeUI(responseText);
            }
        }
    }




    private string PreparePrompt(EnvironmentData environmentData)
    {
        // Find the ErdaTime instance attached to the GameObject
        ErdaTime erdaTime = GameObject.Find("WorldTime").GetComponent<ErdaTime>();

        // Get the scene description
        string sceneDescription = erdaTime.GetSceneDescription();

        // Use the scene description in your narrative prompt
        return $"The scene is as follows: {sceneDescription}. " +
               $"Create a brief narrative in the first person, describing an environment with {environmentData.environment.vegetation} vegetation, " +
               $"{environmentData.environment.wildlife} wildlife, " +
               $"and situated in a {environmentData.environment.terrain} terrain. " +
               $"Keep it to 2-3 sentences, focusing on the immediate sensory experiences.";
    }




    private string ParseResponse(string jsonResponse)
    {
        var responseObject = JsonUtility.FromJson<ResponseObject>(jsonResponse);
        if (responseObject.choices != null && responseObject.choices.Length > 0)
        {
            return responseObject.choices[0].message.content;
        }
        else
        {
            return "No response received.";
        }
    }

    [System.Serializable]
    private class ResponseObject
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
        public string content;
    }

    private void UpdateNarrativeUI(string text)
    {
        if (narrativeText != null)
        {
            narrativeText.text = text;
        }
        else
        {
            Debug.LogError("Narrative TextMeshPro UI element is not assigned.");
        }
    }
}

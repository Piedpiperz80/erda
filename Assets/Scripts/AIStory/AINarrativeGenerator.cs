using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using TMPro;


public class AINarrativeGenerator : MonoBehaviour
{
    public EnvironmentLoader environmentLoader;
    private WorldTime worldTime;
    private string openAiUrl = "https://api.openai.com/v1/engines/gpt-3.5-turbo/completions"; // Ensure this is the correct URL
    private string apiKey = "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"; // Replace with your actual OpenAI API key
    public TextMeshProUGUI narrativeText;

    void Start()
    {
        worldTime = GameObject.Find("WorldTime").GetComponent<WorldTime>();

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

        string currentDayInfo = worldTime.GetCurrentDayInfo();

        // Instructing the AI to create a first-person narrative based on the environment data
        return $"Today is {currentDayInfo}. Create a brief narrative in the first person, describing an environment with {environmentData.environment.vegetation} vegetation," +
            $" {environmentData.environment.wildlife} wildlife," +
            $" and situated in a {environmentData.environment.terrain} terrain. Keep it to 2-3 sentences, focusing on the immediate sensory experiences.";
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

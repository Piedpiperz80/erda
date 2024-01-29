using UnityEngine;

public class EnvironmentLoader : MonoBehaviour
{
    public EnvironmentData currentEnvironment;

    void Start()
    {
        TextAsset jsonData = Resources.Load<TextAsset>("environment");
        if (jsonData != null)
        {
            // Assuming that the JSON contains a single EnvironmentData object, directly mapped
            currentEnvironment = JsonUtility.FromJson<EnvironmentData>(jsonData.text);
            Debug.Log("Environment loaded successfully.");
        }
        else
        {
            Debug.LogError("Failed to load environment data.");
        }
    }

    public EnvironmentData GetCurrentEnvironment()
    {
        return currentEnvironment;
    }
}
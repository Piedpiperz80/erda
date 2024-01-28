using UnityEngine;

public class EnvironmentLoader : MonoBehaviour
{
    public EnvironmentData currentEnvironment;

    void Start()
    {
        TextAsset jsonData = Resources.Load<TextAsset>("environment");
        currentEnvironment = JsonUtility.FromJson<EnvironmentData>(jsonData.text);
    }

    public EnvironmentData GetCurrentEnvironment()
    {
        return currentEnvironment;
    }
}

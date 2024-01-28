using UnityEngine;
using TMPro; // Add this namespace to work with TextMeshPro

public class StoryInputHandler : MonoBehaviour
{
    public TMP_InputField playerInputField; // Assign this in the inspector
    public TextMeshProUGUI storyText; // Assign this in the inspector

    // This will be called when the player submits their input
    public void HandleSubmit()
    {
        string inputText = playerInputField.text;
        // TODO: Send 'inputText' to the AI and get the response
        // For now, let's just clear the input field
        playerInputField.text = string.Empty;
    }

    // Call this function to update the story text with the AI's response
    public void UpdateStoryText(string aiResponse)
    {
        storyText.text = aiResponse;
    }
}

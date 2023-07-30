using UnityEngine;
using TMPro;
using System.Collections;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private TMP_Text gameText;
    [SerializeField] private TMP_InputField userInput;
    [SerializeField] private StoryBlock storyBlockAsset; // Reference to the ScriptableObject

    private Coroutine processingAnimation;

    private ChatbotManager chatbotManager = new ChatbotManager();
    private StoryManager storyManager;
    private UserInputManager userInputManager;

    private void Start()
    {
        storyManager = new StoryManager(storyBlockAsset);
        userInputManager = new UserInputManager(userInput);
        UpdateStory();
        userInput.onEndEdit.AddListener(OnUserInputEndEdit);
        userInputManager.ResetAndActivateInputField();
    }

    private void Update()
    {
        // If the current StoryBlock is a "press any key to continue" block and the user has pressed any key,
        // move to the next StoryBlock
        if (storyManager.GetCurrentStoryBlock().RequiresAIInteraction() && Input.anyKeyDown)
        {
            if (storyManager.GetNextStoryBlockCount() > 0)  // Added this check
            {
                storyManager.MoveToNextStoryBlock(0);
                UpdateStory();
            }
            else
            {
                // Handle the scenario when there's no next StoryBlock
            }
        }
    }

    private void UpdateStory()
    {
        string storyText = storyManager.GetCurrentStoryText();
        if (storyText == null)
        {
            Debug.LogError("Current story text is null. Please check your StoryBlock objects.");
            return;
        }

        gameText.text = storyText;

        if (storyManager.GetNextStoryBlockCount() == 0 || (storyManager.GetNextStoryBlockCount() == 1 && !storyManager.GetCurrentStoryBlock().RequiresAIInteraction()))
        {
            userInputManager.SetInputFieldPlaceholder("Press any key to continue...");
        }
        else if (storyManager.GetNextStoryBlockCount() > 1)
        {
            userInputManager.SetInputFieldPlaceholder("Please input your choice...");
        }
        else
        {
            userInputManager.SetInputFieldPlaceholder("Please input your next action...");
        }

        // New condition to check if the current block should trigger combat scene
        if (storyManager.GetCurrentStoryBlock().GetIsCombatTrigger())
        {
            SceneLoader.Instance.LoadScene("CombatScene");
            // Save game state here before transitioning to combat scene
        }
    }

    private async void OnUserInputEndEdit(string value)
    {
        userInputManager.SetUserInput(""); // Clear input field

        if (storyManager.GetNextStoryBlockCount() == 0 || (storyManager.GetNextStoryBlockCount() == 1 && !storyManager.GetCurrentStoryBlock().RequiresAIInteraction()))
        {
            // If the story manager has no next blocks, simply move to the next one without any user input processing
            if (storyManager.GetNextStoryBlockCount() > 0)
            {
                storyManager.MoveToNextStoryBlock(0);
                UpdateStory();
            }

            userInputManager.ResetAndActivateInputField();
        }
        else
        {
            if (storyManager.GetNextStoryBlockCount() > 1) // In a choice block
            {
                int choice;

                if (int.TryParse(value, out choice) && choice > 0 && choice <= storyManager.GetNextStoryBlockCount())
                {
                    // Subtract 1 from choice to make it 0-based
                    storyManager.MoveToNextStoryBlock(choice - 1);
                    UpdateStory();
                }
                else
                {
                    Debug.LogWarning("Invalid user input.");
                }

                userInputManager.ResetAndActivateInputField();
            }
            else // In an AI block
            {
                processingAnimation = StartCoroutine(ProcessingAnimation());

                string completion = await chatbotManager.SendRequestToChatbot(storyManager.GetCurrentStoryText(), value);
                StopCoroutine(processingAnimation);
                if (storyManager.GetNextStoryBlockCount() > 0)
                {
                    storyManager.MoveToNextStoryBlock(0);
                }
                else
                {
                    // Handle the scenario when there's no next StoryBlock
                }
                storyManager.SetCurrentStoryText(completion);
                UpdateStory();

                userInputManager.ResetAndActivateInputField();
            }
        }
    }

    private IEnumerator ProcessingAnimation()
    {
        while (true)
        {
            gameText.text += ".";
            yield return new WaitForSeconds(1.0f);
        }
    }
}

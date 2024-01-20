using UnityEngine;
using TMPro;
using System.Collections;
using System.Threading.Tasks;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private TMP_Text gameText;
    [SerializeField] private TMP_InputField userInput;
    [SerializeField] private StoryBlock storyBlockAsset; // Reference to the ScriptableObject

    private Coroutine processingAnimation;

    private ChatbotManager chatbotManager = new ChatbotManager();
    private StoryManager storyManager;
    private UserInputManager userInputManager;
    private bool readyForCombat = false;

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
        Debug.Log("[Update] Method called");

        // Check if the current block is a combat trigger and set readyForCombat accordingly
        if (storyManager.GetCurrentStoryBlock().GetIsCombatTrigger() && !readyForCombat)
        {
            Debug.Log("[Update] Combat trigger block identified, setting readyForCombat to true");
            readyForCombat = true;
            userInputManager.SetInputFieldPlaceholder("Press any key to continue..."); // Prompt the user to press a key
            return; // Return here to avoid immediately loading the combat scene
        }

        // Load combat scene only after a key press when readyForCombat is true
        if (readyForCombat && Input.anyKeyDown)
        {
            Debug.Log("[Update] Combat trigger detected, loading combat scene");
            SceneLoader.Instance.LoadScene("CombatScene");
            readyForCombat = false; // Reset the flag
            return; // Return here to prevent further processing in Update
        }

        // Handling other key presses for AI interaction and story progression
        if (Input.anyKeyDown)
        {
            Debug.Log("[Update] A key was pressed");

            // Check if the current story block requires AI interaction
            if (storyManager.GetCurrentStoryBlock().RequiresAIInteraction())
            {
                Debug.Log("[Update] Current block requires AI Interaction");
                if (storyManager.GetNextStoryBlockCount() > 0)
                {
                    Debug.Log("[Update] Moving to next story block");
                    storyManager.MoveToNextStoryBlock(0);
                    UpdateStory();
                }
                else
                {
                    Debug.Log("[Update] No next story block");
                }
            }
            else
            {
                Debug.Log("[Update] Current block is neither AI Interaction nor Combat Trigger");
            }
        }
    }


    private void UpdateStory()
    {
        Debug.Log("[UpdateStory] Method called");

        string storyText = storyManager.GetCurrentStoryText();
        if (storyText == null)
        {
            Debug.LogError("[UpdateStory] Current story text is null. Please check your StoryBlock objects.");
            return;
        }

        Debug.Log($"[UpdateStory] Current Story Text: {storyText}");
        Debug.Log($"[UpdateStory] Is Current Block a Combat Trigger? {storyManager.GetCurrentStoryBlock().GetIsCombatTrigger()}");

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

        readyForCombat = false; // Reset the flag whenever the story updates
    }

    private async void OnUserInputEndEdit(string value)
    {
        Debug.Log("[OnUserInputEndEdit] Method called");
        userInputManager.SetUserInput(""); // Clear input field

        // Check for combat trigger
        if (storyManager.GetCurrentStoryBlock().GetIsCombatTrigger())
        {
            Debug.Log("[OnUserInputEndEdit] Handling combat trigger");
            HandleCombatTrigger();
            return;
        }

        // Check for AI interaction
        if (storyManager.GetCurrentStoryBlock().RequiresAIInteraction())
        {
            Debug.Log("[OnUserInputEndEdit] Processing AI interaction");
            await HandleAIInteraction(value);
            return;
        }

        // Check for multiple choices
        if (storyManager.GetNextStoryBlockCount() > 1)
        {
            Debug.Log("[OnUserInputEndEdit] Handling multiple choice input");
            HandleMultipleChoice(value);
            return;
        }

        // Default action for moving to the next story block
        Debug.Log("[OnUserInputEndEdit] Moving to next story block");
        MoveToNextStoryBlock();
    }

    private void HandleCombatTrigger()
    {
        // Logic to handle combat trigger
        SceneLoader.Instance.LoadScene("CombatScene");
    }

    private async Task HandleAIInteraction(string userInput)
    {
        processingAnimation = StartCoroutine(ProcessingAnimation());
        string completion = await chatbotManager.SendRequestToChatbot(storyManager.GetCurrentStoryText(), userInput);
        StopCoroutine(processingAnimation);
        storyManager.SetCurrentStoryText(completion);
        UpdateStory();
        userInputManager.ResetAndActivateInputField();
    }

    private void HandleMultipleChoice(string userInput)
    {
        int choice;
        if (int.TryParse(userInput, out choice) && choice > 0 && choice <= storyManager.GetNextStoryBlockCount())
        {
            storyManager.MoveToNextStoryBlock(choice - 1);
            UpdateStory();
        }
        else
        {
            Debug.LogWarning("[HandleMultipleChoice] Invalid user input.");
        }
        userInputManager.ResetAndActivateInputField();
    }

    private void MoveToNextStoryBlock()
    {
        if (storyManager.GetNextStoryBlockCount() > 0)
        {
            storyManager.MoveToNextStoryBlock(0);
            UpdateStory();
        }
        userInputManager.ResetAndActivateInputField();
    }

    private IEnumerator ProcessingAnimation()
    {
        Debug.Log("[ProcessingAnimation] Coroutine started");
        while (true)
        {
            gameText.text += ".";
            yield return new WaitForSeconds(1.0f);
        }
    }

}

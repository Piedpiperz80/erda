using UnityEngine;

[CreateAssetMenu(menuName = "Story Block")]
public class StoryBlock : ScriptableObject
{
    [TextArea(14, 10)]
    [SerializeField] string storyText;
    [SerializeField] StoryBlock[] nextStoryBlocks;
    [SerializeField] bool requiresAIInteraction;  // New field
    [SerializeField] bool isCombatTrigger; // New variable

    public string GetStoryText()
    {
        return storyText;
    }

    public int GetNextStoryBlockCount()
    {
        return nextStoryBlocks.Length;
    }

    public StoryBlock GetNextStoryBlock(int index)
    {
        return nextStoryBlocks[index];
    }

    public StoryBlock[] GetAllNextStoryBlocks()
    {
        return nextStoryBlocks;
    }

    public bool RequiresAIInteraction()  // New method
    {
        return requiresAIInteraction;
    }

    public bool GetIsCombatTrigger() // New method
    {
        return isCombatTrigger;
    }
}

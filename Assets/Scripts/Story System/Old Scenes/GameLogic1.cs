using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameLogic1 : MonoBehaviour
{
    [SerializeField] TMP_Text storyText;
    [SerializeField] StoryBlock startingStoryBlock;

    private StoryBlock currentStory;

    // Start is called before the first frame update
    void Start()
    {
        currentStory = startingStoryBlock;
        storyText.text = startingStoryBlock.GetStoryText();
    }

    // Update is called once per frame
    void Update()
    {
        var nextStory = currentStory.GetNextStoryBlock();

        for (int i = 0; i < nextStory.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                currentStory = nextStory[i];
            }
        }

        storyText.text = currentStory.GetStoryText();
    }
}

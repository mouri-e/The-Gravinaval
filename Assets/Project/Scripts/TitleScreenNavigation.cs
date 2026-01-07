using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
public class TitleScreenNavigation : MonoBehaviour
{
    [Header("Opening Screen Data")]
    public TMP_Text gameTitle;
    public TMP_Text creators;
    public TMP_Text pressOneButtonInstruction;
    public TMP_Text enjoyGameMessage;

    [Header("Game Story Data")]
    public Image story;
    public TMP_Text pressTwoButtonInstruction;
    public AudioSource musicSource;
    private Animator storyAnimator;

    [Header("Game Controls Data")]
    public TMP_Text controls;
    public TMP_Text mechanics;
    public TMP_Text pressThreeButtonInstruction;
    public TMP_Text separatorLine;

    enum ScreenText
    {
        TITLESCREEN,
        STORYSCREEN,
        CONTROLSCREEN,
        GOODLUCKSCREEN
    };

    ScreenText textDirectionsState;
    //bool onStartScreen = true;
    //bool onStoryScreen = false;
    //bool onControlsScreen = false;

    void Awake()
    {
        musicSource.Stop();
        textDirectionsState = ScreenText.TITLESCREEN;
        //enable start screen text
        gameTitle.enabled = true;
        creators.enabled = true;
        pressOneButtonInstruction.enabled = true;

        //disable all other instructions/text
        storyAnimator = story.gameObject.GetComponent<Animator>();
        story.enabled = false;
        controls.enabled = false;
        mechanics.enabled = false;
        enjoyGameMessage.enabled = false;
        pressTwoButtonInstruction.enabled = false;
        pressThreeButtonInstruction.enabled = false;
        separatorLine.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (textDirectionsState)
        {
            case ScreenText.TITLESCREEN:
                SwitchToStoryTextOnAnyKeyPress();
                return;
            case ScreenText.STORYSCREEN:
                if (storyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Finished")) {
                    if(!musicSource.isPlaying) musicSource.Play();
                    pressTwoButtonInstruction.enabled = true;
                    SwitchToControlsTextOnAnyKeyPress();
                }
                return;
            case ScreenText.CONTROLSCREEN:
                StartGame();
                return;
            case ScreenText.GOODLUCKSCREEN:
                return;
        }
    }

    void SwitchToStoryTextOnAnyKeyPress()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            storyAnimator.SetTrigger("start");
            musicSource.Pause();
            textDirectionsState = ScreenText.STORYSCREEN;

            //turn off start screen text
            gameTitle.enabled = false;
            creators.enabled = false;
            pressOneButtonInstruction.enabled = false;

            //display the story image
            story.enabled = true;

        }
    }

    void SwitchToControlsTextOnAnyKeyPress()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            textDirectionsState = ScreenText.CONTROLSCREEN;

            //turn off the story text
            story.enabled = false;
            pressTwoButtonInstruction.enabled = false;

            //display the controls and mechanics text
            controls.enabled = true;
            separatorLine.enabled = true;
            mechanics.enabled = true;
            pressThreeButtonInstruction.enabled = true;

        }
    }

    void StartGame()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            textDirectionsState = ScreenText.GOODLUCKSCREEN;

            //turn off controls text
            controls.enabled = false;
            separatorLine.enabled = false;
            mechanics.enabled = false;
            pressThreeButtonInstruction.enabled = false;

            //display enjoy the game message and load the first level
            enjoyGameMessage.enabled = true;

            Invoke("LoadLevelMenu", 2);
            
        }
    }

    void LoadLevelMenu()
    {
        SceneManager.LoadScene("LevelMenu");
    }
}

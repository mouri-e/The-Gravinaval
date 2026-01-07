using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
public class EpilogueNavigation : MonoBehaviour
{
    [Header("Load Data")]
    public TMP_Text pressOneButtonInstruction;

    [Header("Game Story Data")]
    public Image story;
    public TMP_Text pressTwoButtonInstruction;
    private Animator storyAnimator;

    [Header("Data Data")]
    public TMP_Text gemsText;
    public Image gemImage;
    public TMP_Text pressThreeButtonInstruction;

    [Header("Gratitude Data")]
    public TMP_Text thankYouText;

    enum ScreenText
    {
        LOADSCREEN,
        STORYSCREEN,
        DATASCREEN,
        GRATITUDESCREEN
    };

    ScreenText textDirectionsState;

    void Awake()
    {
        MusicPersistence.Instance.gameObject.GetComponent<AudioSource>().Stop();
        textDirectionsState = ScreenText.LOADSCREEN;
        //enable start screen text
        pressOneButtonInstruction.enabled = true;

        //disable all other instructions/text
        storyAnimator = story.gameObject.GetComponent<Animator>();
        story.enabled = false;
        gemImage.enabled = false;
        gemsText.enabled = false;
        thankYouText.enabled = false;
        pressTwoButtonInstruction.enabled = false;
        pressThreeButtonInstruction.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (textDirectionsState)
        {
            case ScreenText.LOADSCREEN:
                SwitchToStoryTextOnAnyKeyPress();
                return;
            case ScreenText.STORYSCREEN:
                if (storyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Finished"))
                {
                    pressTwoButtonInstruction.enabled = true;
                    SwitchToDataTextOnAnyKeyPress();
                }
                return;
            case ScreenText.DATASCREEN:
                EndGame();
                return;
            case ScreenText.GRATITUDESCREEN:
                return;
        }
    }

    void SwitchToStoryTextOnAnyKeyPress()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            storyAnimator.SetTrigger("start");
            textDirectionsState = ScreenText.STORYSCREEN;

            //turn off start screen text
            pressOneButtonInstruction.enabled = false;

            //display the story image
            story.enabled = true;

        }
    }

    void SwitchToDataTextOnAnyKeyPress()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            textDirectionsState = ScreenText.DATASCREEN;

            //turn off the story text
            story.enabled = false;
            pressTwoButtonInstruction.enabled = false;

            //display the controls and mechanics text
            gemsText.enabled = true;
            GetGemsText();
            gemImage.enabled = true;
            pressThreeButtonInstruction.enabled = true;

        }
    }

    void EndGame()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            textDirectionsState = ScreenText.GRATITUDESCREEN;

            //turn off controls text
            gemsText.enabled = false;
            gemImage.enabled = false;
            pressThreeButtonInstruction.enabled = false;

            //display enjoy the game message and load the first level
            thankYouText.enabled = true;

            Invoke("End", 2);

        }
    }

    void End()
    {
        SceneManager.LoadScene("GameOpening");
    }

    void GetGemsText() {
        int gems = 0;
        for (int i = 0; i < 7; i++) {
            gems += PlayerPrefs.GetInt("Gem" + i, 0);
        }
        gemsText.text = "x" + gems + "/7";
        if (gems == 7) {
            gemsText.color = Color.yellow;
        }
    }
}

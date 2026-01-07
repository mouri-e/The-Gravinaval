using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenuBehavior : MonoBehaviour
{
    public Button[] DefaultLevelButtons;
    public Button[] FogOfWarLevelButtons;
    public GameObject DefaultLevelCanvasElement;
    public GameObject FogOfWarLevelCanvasElement;
    public TMP_Text gameModeText;
    public Button gameModeButton;
    public Sprite normalgameModeSprite;
    public Sprite FogOfWargameModeSprite;

    public enum GameMode { Default, FogOfWar }
    GameMode currentGameMode = GameMode.Default;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //Use to clear the level data before playing the game
        //PlayerPrefs.DeleteAll();
        if (gameModeText != null && gameModeButton != null && normalgameModeSprite != null && FogOfWargameModeSprite != null)
        {
            switch (currentGameMode)
            {
                case GameMode.Default:
                    gameModeText.text = "Default";
                    gameModeButton.image.sprite = normalgameModeSprite;
                    break;
                case GameMode.FogOfWar:
                    gameModeText.text = "Fog Of War";
                    gameModeButton.image.sprite = FogOfWargameModeSprite;
                    break;
            }
        }
        FogOfWarLevelCanvasElement.SetActive(false);

        int highestLevelUnlocked = PlayerPrefs.GetInt("HighestLevelUnlocked", 0);
        for (int i = 0; i < DefaultLevelButtons.Length; i++)
        {
            if (i <= highestLevelUnlocked)
            {
                DefaultLevelButtons[i].interactable = true;
                FogOfWarLevelButtons[i].interactable = true;
            }
            else
            {
                DefaultLevelButtons[i].interactable = false;
                FogOfWarLevelButtons[i].interactable = false;
            }
        }
    }

    public void SwitchGameMode()
    {
        switch (currentGameMode)
        {
            case GameMode.Default:
                gameModeText.text = "Fog Of War";
                gameModeButton.image.sprite = FogOfWargameModeSprite;
                DefaultLevelCanvasElement.SetActive(false);
                FogOfWarLevelCanvasElement.SetActive(true);
                currentGameMode = GameMode.FogOfWar;
                break;

            case GameMode.FogOfWar:
                gameModeText.text = "Default";
                gameModeButton.image.sprite = normalgameModeSprite;
                FogOfWarLevelCanvasElement.SetActive(false);
                DefaultLevelCanvasElement.SetActive(true);
                currentGameMode = GameMode.Default;
                break;
        }

    }

    public void LoadLevel(string LevelName)
    {
        //PauseMenuBehavior.isGamePaused = false;
        SceneManager.LoadScene(LevelName);
    }
}

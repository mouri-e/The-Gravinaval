using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class LevelCompleteBehavior : MonoBehaviour
{
    public TMP_Text winText;
    public TMP_Text nextLevelText;
    public TMP_Text replayLevelText;
    public ExitDoorBehavior exitDoor;
    public string NextLevel;
    private int saveGem = -1;

    void Awake()
    {
        if(!exitDoor) exitDoor = GameObject.FindGameObjectWithTag("ExitDoor").GetComponent<ExitDoorBehavior>();

        if (winText) winText.enabled = false;
        if (nextLevelText) nextLevelText.enabled = false;
        if (replayLevelText) replayLevelText.enabled = false;
    }

    void Update()
    {
        if (exitDoor.PlayerReachedDoor)
        {
            DisplayNavigationMenuOnWin();
            if (saveGem >= 0) PlayerPrefs.SetInt("Gem" + saveGem.ToString(), 1);
            if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
            {
                LoadLevel(NextLevel);
            }
            if (Input.GetKeyDown(KeyCode.Keypad0) || Input.GetKeyDown(KeyCode.Alpha0))
            {
                Scene scene = SceneManager.GetActiveScene();
                LoadLevel(scene.name);
            }
        }
    }

    void LoadLevel(string sceneName)
    {
        //Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(sceneName);
    }

    void DisplayNavigationMenuOnWin()
    {
        if (winText) winText.enabled = true;
        if (nextLevelText) nextLevelText.enabled = true;
        if (replayLevelText) replayLevelText.enabled = true;
    }

    /*public void CloseStartMenu()
    {
        directionsText.enabled = false;
        startLevelButton.SetActive(false);
    }*/

    public void SaveGem(int gem)
    {
        saveGem = gem;
    }
}

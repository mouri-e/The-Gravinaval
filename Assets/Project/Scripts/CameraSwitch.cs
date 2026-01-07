using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public GameObject playerVirtualCamera;
    public GameObject levelVirtualCamera;
    bool onLevelCam = true;
    bool inGame = true;
    void Awake()
    {
        inGame = true;
        //take the cursor off screen to disable mouse axis movement
        Cursor.visible = false; // Hide the cursor
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor

        if (levelVirtualCamera == null) levelVirtualCamera = GameObject.FindGameObjectWithTag("Level Camera");
        if (playerVirtualCamera == null) playerVirtualCamera = GameObject.FindGameObjectWithTag("Player Camera");
        onLevelCam = true;
        playerVirtualCamera.SetActive(false);
    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (onLevelCam)
            {
                playerVirtualCamera.SetActive(true);
                levelVirtualCamera.SetActive(false);
                onLevelCam = false;
            }
            //on the player cam
            else
            {
                levelVirtualCamera.SetActive(true);
                playerVirtualCamera.SetActive(false);
                onLevelCam = true;

            }

        }
    }

    public void SetInGame(bool tf)
    {
        inGame = tf;
        if (inGame)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}

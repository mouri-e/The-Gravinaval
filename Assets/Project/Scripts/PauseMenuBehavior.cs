using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuBehavior : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public Button resumeButton;
    public Button mainMenuButton;
    public Button quitButton;
    bool isGamePaused = false;
    public CameraSwitch cameraSwitch;

    void Awake()
    {
        if (resumeButton != null)
            resumeButton.onClick.AddListener(ResumeGame);
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(LoadMainMenu);
        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
        
        pauseMenuPanel.SetActive(false);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResumeGame();
        GameObject mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        if (mainCam != null)
        {
            cameraSwitch = mainCam.GetComponent<CameraSwitch>();
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        GameObject mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        if (mainCam != null)
        {
            cameraSwitch = mainCam.GetComponent<CameraSwitch>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "GameOpening")
        {
            Destroy(gameObject);
            return;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isGamePaused)
            {
                // resume the game
                ResumeGame();
            }
            else
            {
                // pause the game
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        isGamePaused = false;
        Time.timeScale = 1f;
        if (!pauseMenuPanel)
        {
            return;
        }
        pauseMenuPanel.SetActive(false);

        if (cameraSwitch != null)
            cameraSwitch.SetInGame(true);
    }

    public void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0f;
        if (!pauseMenuPanel)
        {
            return;
        }
        pauseMenuPanel.SetActive(true);

        if (cameraSwitch != null)
            cameraSwitch.SetInGame(false);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("LevelMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

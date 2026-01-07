using UnityEngine;


public class ExitDoorBehavior : MonoBehaviour
{
    AudioSource audioSource;
    PlayerController playerController;
    public bool PlayerReachedDoor { get; private set; }
    public int levelNumber;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        PlayerReachedDoor = false;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
        }
        else
        {
            Debug.LogWarning("Player not found....");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //if you are on the current highest level, unlock the next one
            UnlockNextLevel();
            Transform teleportVFX = other.transform.Find("TeleportVFX");
            if (teleportVFX != null)
            {
                Transform rings = teleportVFX.Find("TeleportVFXRings");
                Transform glow = teleportVFX.Find("TeleportVFXGlow");
                playerController.SetIsTeleporting(true);

                if (rings != null)
                    rings.gameObject.SetActive(true);

                if (glow != null)
                    glow.gameObject.SetActive(true);
            }
            audioSource.Play();
            PlayerReachedDoor = true;
        }
    }

    void UnlockNextLevel()
    {
        if (levelNumber >= PlayerPrefs.GetInt("HighestLevelUnlocked", 0)) 
        {
            PlayerPrefs.SetInt("HighestLevelUnlocked", levelNumber + 1);
            PlayerPrefs.Save();
        }
    }
}

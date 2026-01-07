using UnityEngine;

public class MenuGem : MonoBehaviour
{
    [SerializeField] private int Level;
    private void Awake()
    {
        if (PlayerPrefs.GetInt("Gem" + Level, 0) == 0) Destroy(gameObject);
    }
}

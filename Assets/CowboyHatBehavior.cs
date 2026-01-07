using UnityEngine;

public class CowboyHatBehavior : MonoBehaviour
{
    private void Awake()
    {
        int gems = 0;
        for (int i = 0; i < 7; i++)
        {
            gems += PlayerPrefs.GetInt("Gem" + i, 0);
        }
        if (gems != 7)
        {
            Destroy(gameObject);
        }
    }
}

using UnityEngine;

public class MusicPersistence : MonoBehaviour
{
    private static MusicPersistence instance = null;
    public static MusicPersistence Instance
    {
        get { return instance; }
    }
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
}

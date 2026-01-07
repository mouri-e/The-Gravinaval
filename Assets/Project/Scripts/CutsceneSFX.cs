using UnityEngine;

public class CutsceneSFX : MonoBehaviour
{
    public AudioClip[] soundEffects;

    public void PlaySound(int index) {
        if (index < soundEffects.Length && index >= 0) {
            AudioSource.PlayClipAtPoint(soundEffects[index], Camera.main.transform.position);
        }
    }
}

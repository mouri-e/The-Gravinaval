using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            PlayerController pc = other.gameObject.GetComponent<PlayerController>();
            if (pc)
            {
                pc.Kill();
            }
            else
            {
                Debug.Log("Player does not have PlayerController Script, add this to Player.");
            }
        }
    }
}

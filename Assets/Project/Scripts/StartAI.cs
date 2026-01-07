using UnityEngine;

public class StartAI : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {
            PlayerController pc = other.gameObject.GetComponent<PlayerController>();
            pc.DialogueSet();
        }
    }
}

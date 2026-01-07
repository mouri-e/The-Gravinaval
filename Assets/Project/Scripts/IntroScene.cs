using UnityEngine;
using UnityEngine.UI;

public class IntroScene : MonoBehaviour
{
    [SerializeField] private string[] dialogueText;
    [SerializeField] private GameObject AIBuddyNPC;
    private bool startedDialogue;
    private PlayerController pc;

    private void Update()
    {
        if (pc != null && startedDialogue) {
            if (!pc.InDialogue()) {
                Destroy(AIBuddyNPC);
                Destroy(gameObject);
            }
        }
    }
    void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player")) {
            pc = collision.gameObject.GetComponent<PlayerController>();
            pc.GiveAIBuddy();
            pc.PutDialogue(dialogueText);
            startedDialogue = true;
        }
    }
}

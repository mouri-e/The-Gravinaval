using UnityEngine;
using UnityEngine.SceneManagement;

public class ConsoleScene : MonoBehaviour
{
    [SerializeField] private string[] dialogueText;
    [SerializeField] private string EpilogueScene;
    private bool startedDialogue;
    private PlayerController pc;

    private void Update()
    {
        if (pc != null && startedDialogue) {
            if (!pc.InDialogue()) {
                SceneManager.LoadScene(EpilogueScene);
            }
        }
    }
    void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player")) {
            pc = collision.gameObject.GetComponent<PlayerController>();
            pc.PutDialogue(dialogueText);
            startedDialogue = true;
        }
    }
}

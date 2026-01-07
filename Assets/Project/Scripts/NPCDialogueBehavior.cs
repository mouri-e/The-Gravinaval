using UnityEngine;

public class NPCDialogueBehavior : MonoBehaviour
{
    private bool inDialogue;
    public bool getInDialogue()
    {
        return inDialogue;
    }

    public void setInDialogue(bool tf)
    {
        inDialogue = tf;
        Debug.Log("inDialogue is now" + tf);
    }
}

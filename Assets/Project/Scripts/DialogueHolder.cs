using UnityEngine;

public class DialogueHolder : MonoBehaviour
{
    [SerializeField] private string[] text;

    public string[] GetText() {
        return text;
    }
}

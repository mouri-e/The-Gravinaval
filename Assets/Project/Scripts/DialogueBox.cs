using UnityEngine;
using TMPro;

public class DialogueBox : MonoBehaviour
{
    string[] text;
    int index;
    bool on = false;
    [SerializeField] TextMeshProUGUI textbox;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = new string[]{};
        index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (index >= text.Length) on = false;
        if (!on) gameObject.SetActive(false);
        if(index < text.Length)textbox.text = text[index];
    }

    public void SetText(string[] text)
    {
        Toggle();
        index = 0;
        this.text = text;
    }

    public void TurnPage() {
        index++;
    }

    public void Toggle()
    {
        on = true;
    }
}

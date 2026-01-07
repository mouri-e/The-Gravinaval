using TMPro;
using UnityEngine;

public class FadeTextEffect : MonoBehaviour
{
    public TMP_Text text;
    Color lerpedColor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lerpedColor = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        //lerp between transparent and solid
        lerpedColor = Color.Lerp(new Color(255, 255, 255, 0), new Color(255, 255, 255, 1), Mathf.PingPong(Time.time, 1.3f));
        text.color = lerpedColor;

    }
}

using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UI/Effects/Extensions/TextGradient")]
public class TextGradient : MonoBehaviour {
    public Text textComponent;
    public Gradient colorGradient;

    void Start() {
        string originalText = "渐变文字效果";
        string richText = "";
        for (int i = 0; i < originalText.Length; i++) {
            Color color = colorGradient.Evaluate((float)i / originalText.Length);
            richText += $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{originalText[i]}</color>";
        }
        textComponent.text = richText;
    }
}
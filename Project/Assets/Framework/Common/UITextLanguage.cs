using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class UITextLanguage : MonoBehaviour
{
    [Tooltip("多语言key值")] public string langKey;

    [HideInInspector] public Text text;


    // Use this for initialization
    void Awake()
    {
        Refresh();
    }

    [Button]
    void Refresh()
    {
        if (text == null)
        {
            text = GetComponent<Text>();
        }
        if (text != null && !string.IsNullOrEmpty(text.text) && text.text.StartsWith("@"))
        {
            langKey = text.text.Substring(1);
        }
        if (!string.IsNullOrEmpty(langKey))
        {
            text.text = LanguageTool.Get(langKey);
        }
    }
}
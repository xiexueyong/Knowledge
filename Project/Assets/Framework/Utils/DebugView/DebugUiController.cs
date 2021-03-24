/*
 * @file DebugUiController
 * Debug
 * @author lu
 */

using UnityEngine;
using UnityEngine.UI;

public class DebugUiController : MonoBehaviour
{
    public Button ShowButton;
    public DebugView debugView;

    public void Start()
    {
        ShowButton = transform.Find("openDebugButton").GetComponent<Button>();
        debugView = transform.Find("DebugPanel").GetComponent<DebugView>();

        if (GameConfig.Inst.DebugEnable)
        {
            ShowButton.onClick.AddListener(OnOpenWindow);
        }
        else
        {
            debugView.gameObject.SetActive(false);
            ShowButton.gameObject.SetActive(false);
        }
    }

    // 打开界面时调用(每次打开都调用)    
    public void OnOpenWindow()
    {
        if (debugView.gameObject.activeSelf)
        {
            debugView.gameObject.SetActive(false);
            ShowButton.transform.Find("Text").GetComponent<Text>().text = "Debug";
        }
        else
        {
            debugView.gameObject.SetActive(true);
            ShowButton.transform.Find("Text").GetComponent<Text>().text = "Hide";
        }
    }
}
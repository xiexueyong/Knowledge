using Framework.Asset;
using Framework.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugView : MonoBehaviour
{
    public Text FPS { get; set; }

    public InputField InputText1 { get; set; }
    public InputField InputText2 { get; set; }

    public Transform ScrollView { get; set; }
    public Button SwitchButton { get; set; }
    //  public Text SwitchButtonTxt { get; set; }

    public Button CloseButton { get; set; }

    Transform Content { get; set; }


    private float _lastTap;
    private int _tapCount;
    public int RequiredTapCount = 3;
    public float ResetTime = 0.5f;


    private void Start()
    {
        ScrollView = transform.Find("Scroll View");
        Content = transform.Find("Scroll View/Viewport/Content");

        InputText1 = transform.Find("InputField1").GetComponent<InputField>();
        InputText2 = transform.Find("InputField2").GetComponent<InputField>();

        SwitchButton = transform.Find("SwitchBtn").GetComponent<Button>();
        CloseButton = transform.Find("Scroll View/CloseButton").GetComponent<Button>();
        //  SwitchButtonTxt = transform.Find("SwitchBtn/Text").GetComponent<Text>();

        FPS = transform.Find("TextFPS").GetComponent<Text>();
        Build();

        SwitchButton.onClick.AddListener(OpenShow);
        CloseButton.onClick.AddListener(() => Show(false));
    }

    private bool _show = false;

    /// <summary>
    /// 三击显示
    /// </summary>
    private void OpenShow()
    {
       
        if (Time.unscaledTime - _lastTap > ResetTime)
        {
            _tapCount = 0;
        }

        _lastTap = Time.unscaledTime;
        _tapCount++;

        if (_tapCount == RequiredTapCount)
        {
            Show(true);
            _tapCount = 0;
        }
    }

    private void Build()
    {
        List<DebugCfg> debugActions = DebugModel.GetCfg();
        foreach (DebugCfg cfg in debugActions)
        {
            GameObject buttonItem = Res.LoadResource<GameObject>("Prefab/Common/DebugActionItem");
            buttonItem.name = "DebugButton";
            Button button = buttonItem.transform.GetComponent<Button>();
            buttonItem.transform.Find("Text").GetComponent<Text>().text = cfg.TitleStr;
            buttonItem.transform.SetParent(Content);
            button.onClick.AddListener(
            () =>
            {
                cfg.ClickCallBack(InputText1.text, InputText2.text);
            }
            );
        }
        FPS.transform.gameObject.AddComponent<FPSComponet>();
        Show(false);
    }

    /// <summary>
    /// 显示调试面板
    /// </summary>
    /// <param name="value">显示 true  隐藏 false</param>
    private void Show(bool value)
    {
        InputText1.transform.gameObject.SetActive(value);
        InputText2.transform.gameObject.SetActive(value);
        ScrollView.transform.gameObject.SetActive(value);
        _show = value;
        //if (_show)
        //{
        //    SwitchButtonTxt.text = "Hide";
        //}
        //else
        //{
        //    SwitchButtonTxt.text = "Show";
        //}
    }
}
using EventUtil;
using Framework.Asset;
using Framework.Tables;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class UIAnnouncement : BaseUI
{
    //组件
    public Text txt_title;
    public Text txt_context;
    public Button btn_confirm;
    public Button btn_close;
    public bool isClosed;

    public override void OnAwake()
    {
        btn_confirm = this.transform.Find("root/ButtonComplete").GetComponent<Button>();
        btn_close = this.transform.Find("root/Btnclose").GetComponent<Button>();
        txt_title = this.transform.Find("root/Title").GetComponent<Text>();
        txt_context = this.transform.Find("root/Scroll View/Viewport/Content/Text").GetComponent<Text>();

        btn_close.onClick.AddListener(
                () =>
                {
                    Close();
                }
        );
    }
    public override void SetData(params object[] args)
    {
        string title = (string)args[0];//标题
        string context = (string)args[1];//提示内容
        string appUrl = (string)args[2];//更新地址
        bool showCloseBtn = (bool)args[3];//是否显示关闭按钮

        isClosed = false;
        txt_context.text = context;
        if (!string.IsNullOrEmpty(appUrl))
        {
            btn_confirm.transform.Find("Text").GetComponent<Text>().text = "UPDATE";
            btn_confirm.onClick.AddListener(
                () =>
                {
                    Application.OpenURL(appUrl);
                }
            );
        }
        else
        {
            btn_confirm.transform.Find("Text").GetComponent<Text>().text = "I KNOW";
            btn_confirm.onClick.AddListener(
                    () =>
                    {
                        Close();
                    }
            );
        }

        btn_close.gameObject.SetActive(showCloseBtn);
    }

    public override void OnClose()
    {
        isClosed = true;
    }
}
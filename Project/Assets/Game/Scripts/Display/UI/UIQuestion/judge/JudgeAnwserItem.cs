using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class JudgeAnwserItem : MonoBehaviour
{
    
    [SerializeField] private Sprite sprite_bg_default;
    [SerializeField] private Sprite sprite_bg_right;
    [SerializeField] private Sprite sprite_bg_wrong;
    
    [SerializeField] private Sprite sprite_sign_right;
    [SerializeField] private Sprite sprite_sign_wrong;
    
    [SerializeField] private Text contentText;
    [SerializeField] private Button btn;
    [SerializeField] private Image bg;
    [SerializeField] private Image status_sign;
    
    public Action<bool> OnSelectListener;
    
    private string key;
    private string content;
    private bool isRight;

    private AnwserStatus _status;
    
    public enum AnwserStatus
    {
        Default = 0,
        Right = 1,
        Wrong = 2,
    }
    public void Awake()
    {
        btn.onClick.AddListener(onSelect);
    }

    void onSelect()
    {
        if (LevelHelper.Inst.anwserRight)
        {
            UIManager.Inst.ShowMessage("已选择正确答案");
            return;
        }
        //状态判断
        if (_status != AnwserStatus.Default)
        {
            UIManager.Inst.ShowMessage("已选择此项");
            return;
        }
        //脑力
        if (DataManager.Inst.userInfo.Energy <= 0)
        {
            UIManager.Inst.ShowMessage("脑力不足");
            return;
        }
        
        if (isRight)
        {
            setStaus(AnwserStatus.Right);
        }
        else
        {
            setStaus(AnwserStatus.Wrong);
        }
        OnSelectListener.Invoke(isRight);
    }

    public void SetAnwser(string key,string content,bool isRight)
    {
        this.key = key;
        this.isRight = isRight;
        
        this.contentText.text = content;

        setStaus(AnwserStatus.Default);
    }

    private void setStaus(AnwserStatus status)
    {
        _status = status;
        switch (status)
        {
            case AnwserStatus.Right:
                bg.sprite = sprite_bg_right;
                status_sign.gameObject.SetActive(true);
                status_sign.sprite = sprite_sign_right;
                status_sign.SetNativeSize();
                break;
            case AnwserStatus.Wrong:
                bg.sprite = sprite_bg_wrong;
                status_sign.gameObject.SetActive(true);
                status_sign.sprite = sprite_sign_wrong;
                status_sign.SetNativeSize();
                break;
            default:
                bg.sprite = sprite_bg_default;
                status_sign.gameObject.SetActive(false);
                break;
        }
    }
}

using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class ChooseAnwserItem : MonoBehaviour
{
    
    [SerializeField] private Sprite sprite_bg_default;
    [SerializeField] private Sprite sprite_bg_right;
    [SerializeField] private Sprite sprite_bg_wrong;
    
    [SerializeField] private Sprite sprite_sign_right;
    [SerializeField] private Sprite sprite_sign_wrong;
    
    [SerializeField] private Text indexText;
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
        //状态判断
        if (_status != AnwserStatus.Default)
        {
            UIManager.Inst.ShowMessage("已选择此项");
            return;
        }
        if (DataManager.Inst.userInfo.Energy <= 0)
        {
            UIManager.Inst.ShowUI(UIName.UIEnergyPanel);
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
        this.content = content;
        this.isRight = isRight;
        
        this.indexText.text = key;
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
                break;
            case AnwserStatus.Wrong:
                bg.sprite = sprite_bg_wrong;
                status_sign.gameObject.SetActive(true);
                status_sign.sprite = sprite_sign_wrong;
                break;
            default:
                bg.sprite = sprite_bg_default;
                status_sign.gameObject.SetActive(false);
                break;
        }
    }
}

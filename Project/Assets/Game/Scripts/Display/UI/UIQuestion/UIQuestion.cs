
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Framework.Asset;
using Framework.Tables;

public class UIQuestion : BaseUI
{
    //题目、类型
    //插图
    //答案（选择和对错）
    //空间
    //解析、下一题按钮
    private Text txt_subject;
    private Text txt_question;

    [SerializeField] private Button _btn_explain;
    [SerializeField] private Button _btn_nextLevel;
    [SerializeField] private RectTransform _space;
    [SerializeField] private Image img_illustration;
    [SerializeField] private QuestionComponent _questionComponent;
    [SerializeField] private ChooseAnwsersComponent _chooseAnwsersComponent;
    [SerializeField] private JudgeAnwsersComponent _judgeAnwsersComponent;
    [SerializeField] private ExplainComponent _explainComponent;
    

    public bool anwserRight;
    private int _curLevel;
    private TableQuestion _question;
    public override void OnAwake()
    {
        _btn_explain.onClick.AddListener(OnExplainClick);
        _btn_nextLevel.onClick.AddListener(OnNextLevelClick);
        
        _chooseAnwsersComponent.Init();
        _judgeAnwsersComponent.Init();
        _explainComponent.Init();
        _chooseAnwsersComponent.OnSelectListener = OnSelectAnwser;
        _judgeAnwsersComponent.OnSelectListener = OnSelectAnwser;
    }

    void OnExplainClick()
    {
        if (DataManager.Inst.userInfo.Coins < Table.GameConst.cost_explain)
        {
            UIManager.Inst.ShowMessage("金币不足");
            UIManager.Inst.ShowUI(UIName.UICoinPanel);
        }
        else
        {
            DataManager.Inst.userInfo.ChangeGoodsCount(Table.GoodsId.Coin,-Table.GameConst.cost_explain);
            _explainComponent.Show(_question.subject,_question.explanation);    
        }
        
    }
    
    void OnNextLevelClick()
    {
        int nextLevel = ++_curLevel;
        if (nextLevel > Table.GameConst.levelMax)
        {
            UIManager.Inst.ShowMessage("敬请期待更新");
        }
        else
        {
            SetQuestion(nextLevel);    
        }
        
    }

    void Reset()
    {
        if (_explainComponent.gameObject.activeSelf)
        {
            _explainComponent.gameObject.SetActive(false);
        }

        anwserRight = false;
        _question = null;
    }

    public void SetQuestion(int questionId)
    {
        Reset();
        _question =Table.Question.Get(questionId);
        //题目
        _questionComponent.SetQuestion(_question.subject,_question.question);
        //插图
        if (string.IsNullOrEmpty(_question.Illustrations))
        {
            img_illustration.gameObject.SetActive(false);
            _space.gameObject.SetActive(true);
        }
        else
        {
            img_illustration.gameObject.SetActive(true);
            img_illustration.sprite = Res.LoadResource<Sprite>("Texture/illustrations/"+_question.Illustrations);
            _space.gameObject.SetActive(false);
        }
        //答案 "J" "C"
        if (_question.type == "J")
        {
            _judgeAnwsersComponent.gameObject.SetActive(true);
            _chooseAnwsersComponent.gameObject.SetActive(false);
            _judgeAnwsersComponent.SetAnwsers(_question.judgeAnwser);
        }
        else 
        {
            _judgeAnwsersComponent.gameObject.SetActive(false);
            _chooseAnwsersComponent.gameObject.SetActive(true);
            Dictionary<string,string> a = new Dictionary<string, string>();
            a["A"] = _question.anwserA;
            a["B"] = _question.anwserB;
            a["C"] = _question.anwserC;
            a["D"] = _question.anwserD;
            _chooseAnwsersComponent.SetAnwsers(a,_question.chooseAnwer);   
        }
        //解析、下一关
        _btn_nextLevel.gameObject.SetActive(false);

    }
    
    public void OnSelectAnwser(bool isRight)
    {
        anwserRight = isRight;
        if (isRight)
        {
            _btn_nextLevel.gameObject.SetActive(true);
            if (_curLevel >= DataManager.Inst.userInfo.Level)
            {
                DataManager.Inst.userInfo.Level = _curLevel + 1;    
            }
        }
        else
        {
            DataManager.Inst.userInfo.ChangeEnergy(-1);
            
        }
    }
    
    
    public override void OnStart()
    {
    }

    public override void SetData(params object[] args)
    {
        _curLevel = (int)args[0];
        SetQuestion(_curLevel);
        
    }
}

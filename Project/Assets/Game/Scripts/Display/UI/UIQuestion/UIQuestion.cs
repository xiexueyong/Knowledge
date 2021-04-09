
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Framework.Asset;

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
    

    public bool anwserRight;
    public override void OnAwake()
    {
        _chooseAnwsersComponent.OnSelectListener = OnSelectAnwser;
        _btn_explain.onClick.AddListener(OnExplainClick);
        _btn_nextLevel.onClick.AddListener(OnNextLevelClick);
    }

    void OnExplainClick()
    {
        
    }
    
    void OnNextLevelClick()
    {
        
    }

    public void SetQuestion(int questionId)
    {
        anwserRight = false;
        var q =Table.Question.Get(questionId);
        //题目
        _questionComponent.SetQuestion("类别",q.question);
        //插图
        if (string.IsNullOrEmpty(q.Illustrations))
        {
            img_illustration.gameObject.SetActive(false);
            _space.gameObject.SetActive(true);
        }
        else
        {
            img_illustration.gameObject.SetActive(true);
            img_illustration.sprite = Res.LoadResource<Sprite>("Texture/illustrations/"+q.Illustrations);
            _space.gameObject.SetActive(false);
        }
        //答案 "J" "C"
        if (q.type == "J")
        {
            _judgeAnwsersComponent.gameObject.SetActive(true);
            _chooseAnwsersComponent.gameObject.SetActive(false);
            _judgeAnwsersComponent.SetAnwsers(q.judgeAnwser);
        }
        else 
        {
            _judgeAnwsersComponent.gameObject.SetActive(false);
            _chooseAnwsersComponent.gameObject.SetActive(true);
            Dictionary<string,string> a = new Dictionary<string, string>();
            a["A"] = q.anwserA;
            a["B"] = q.anwserB;
            a["C"] = q.anwserC;
            a["D"] = q.anwserD;
            _chooseAnwsersComponent.SetAnwsers(a,q.chooseAnwer);   
        }
        //解析、下一关
        _btn_nextLevel.gameObject.SetActive(false);

    }
    
    public void OnSelectAnwser(bool isRight)
    {
        anwserRight = isRight;
        if (isRight)
        {
            
        }
        else
        {
            
        }
    }
    
    
    public override void OnStart()
    {
    }

    public override void SetData(params object[] args)
    {
        int level = (int)args[0];
        SetQuestion(level);
        
    }
}

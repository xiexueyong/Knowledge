
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [SerializeField] private Image img_bg;
    [SerializeField] private Button _btn_explain;
    [SerializeField] private Button _btn_nextLevel;
    [SerializeField] private Button _btn_illustration;
    [SerializeField] private RectTransform _space;
    [SerializeField] private Image img_illustration;
    [SerializeField] private QuestionComponent _questionComponent;
    [SerializeField] private ChooseAnwsersComponent _chooseAnwsersComponent;
    [SerializeField] private JudgeAnwsersComponent _judgeAnwsersComponent;
    [SerializeField] private ExplainComponent _explainComponent;
    [SerializeField] private DegreeComponent _degreeComponent;
    [SerializeField] private IllustrationComponent _illustrationComponent;
    
    [SerializeField] private PrizeComponent _prizeComponent1;
    [SerializeField] private PrizeComponent _prizeComponent2;
    [SerializeField] private PrizeComponent _prizeComponent3;
    [SerializeField] private PrizeComponent _prizeComponent_shengji;
    [SerializeField] private PrizeComponent _prizeComponent_biye;
    [SerializeField] private ExplainButton _explainButton;
    //Rects
    [SerializeField] private VerticalLayoutGroup _layout;
    [SerializeField] private RectTransform _layoutRect;
    [SerializeField] private RectTransform _degreeRect;
    [SerializeField] private RectTransform _questionRect;
    [SerializeField] private RectTransform _anwserRect;
    [SerializeField] private RectTransform _bottomRect;
    
    
    
    

    
    private int _curLevel;
    private TableQuestion _question;
    private TableDegree _degree;
    public override void OnAwake()
    {
        _btn_explain.onClick.AddListener(OnExplainClick);
        _btn_nextLevel.onClick.AddListener(OnNextLevelClick);
        _btn_illustration.onClick.AddListener(onIllustrationClick);
        
        _chooseAnwsersComponent.Init();
        _judgeAnwsersComponent.Init();
        _explainComponent.Init();
        _degreeComponent.Init();
        _illustrationComponent.Init();
        _chooseAnwsersComponent.OnSelectListener = OnSelectAnwser;
        _judgeAnwsersComponent.OnSelectListener = OnSelectAnwser;
    }

    void onIllustrationClick()
    {
        _illustrationComponent.Show();
    }

    void OnExplainClick()
    {
        if (LevelHelper.Inst.buyExplain)
        {
            SoundPlay.PlaySFX(Table.Sound.explain_show);
            _explainComponent.Show(_question.subject,_question.explanation);
            return;
        }
        if (DataManager.Inst.userInfo.Coins < Table.GameConst.cost_explain)
        {
            SoundPlay.PlaySFX(Table.Sound.click);
            UIManager.Inst.ShowMessage("金币不足");
            UIManager.Inst.ShowUI(UIName.UICoinPanel);
        }
        else
        {
            DataManager.Inst.userInfo.ChangeGoodsCount(Table.GoodsId.Coin,-Table.GameConst.cost_explain);
            TopBarManager.Inst.FlyFromBar(TopBarType.Coin,_explainButton.transform.position,
                () =>
                {
                    SoundPlay.PlaySFX(Table.Sound.explain_show);
                    LevelHelper.Inst.buyExplain = true;
                    _explainButton.setExplainCost(0);
                    _explainComponent.Show(_question.subject,_question.explanation);
                }
            );
         }
    }
    
    void OnNextLevelClick()
    {
        int nextLevel = ++_curLevel;
        if (nextLevel > Table.GameConst.levelMax)
        {
            SoundPlay.PlaySFX(Table.Sound.click);
            UIManager.Inst.ShowMessage("敬请期待更新");
        }
        else
        {
            SoundPlay.PlaySFX(Table.Sound.next_level);
            SetQuestion(nextLevel);    
        }
        
    }

    void Reset()
    {
        _explainComponent.Hide();
        _illustrationComponent.Hide();

        LevelHelper.Inst.anwserRight = false;
        LevelHelper.Inst.buyExplain = false;
        _explainButton.setExplainCost(Table.GameConst.cost_explain);
        _question = null;
    }

    public void SetQuestion(int questionId)
    {
        Reset();
        _question =Table.Question.Get(questionId);
        int levelBottom;
        var newDrgree = LevelHelper.getDegree(questionId,out levelBottom);
        //背景
        if (_degree == null || newDrgree.Id != _degree.Id)
        {
            _degree = newDrgree;
            // img_bg.sprite = Res.LoadResource<Sprite>("Texture/Scapes/"+_degree.bg);
            ScapeTool.Inst.SetScape(_degree.bg);
            SoundPlay.PlayMusic(_degree.music);
        }
        //学位进度
        _degreeComponent.SetDegree(newDrgree,levelBottom);
        _degreeComponent.SetLevel(questionId-1);
        //题目
        _questionComponent.SetQuestion(_question.subject,_question.question);
        //插图
        if (string.IsNullOrEmpty(_question.Illustrations))
        {
            noIllustration();
        }
        else
        {
            
            var s = Res.LoadResource<Sprite>("Texture/illustrations/"+_question.Illustrations);
            if (s != null)
            {
                img_illustration.gameObject.SetActive(true);
                // _space.gameObject.SetActive(false);

                float customHeight = 160f;//指定高度为215
                _illustrationComponent.sprite = s;
                var h = s.texture.height;
                var w = s.texture.width;
                var newWidth = w/(h / customHeight);
                img_illustration.sprite = s;
                (img_illustration.transform as RectTransform).sizeDelta = new Vector2(newWidth,customHeight);
            }
            else
            { 
                noIllustration();
                Debug.LogError("插图不存在："+_question.Illustrations);  
            }
          
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
        //布局适配
        StartCoroutine(setSpaceHeight());
    }

    IEnumerator setSpaceHeight()
    {
        yield return null;
        float sumH = _layoutRect.rect.height - _degreeRect.rect.height-_questionRect.rect.height-_anwserRect.rect.height-_bottomRect.rect.height;
        sumH -= _layout.spacing * 4;
        if (sumH <= 40)
        {
            _space.gameObject.SetActive(false); 
        }
        else
        {
            yield return null;
            _space.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, sumH*0.67f);
            LayoutRebuilder.ForceRebuildLayoutImmediate(_layoutRect);
        }
    }

    void noIllustration()
    {
        img_illustration.gameObject.SetActive(false);
        _illustrationComponent.sprite = null;
        // _space.gameObject.SetActive(true);
    }
    
    public void OnSelectAnwser(bool isRight)
    {
        LevelHelper.Inst.anwserRight = isRight;
        if (isRight)
        {
            LevelHelper.Inst.rightStreak++;
            _btn_nextLevel.gameObject.SetActive(true);
            bool upLevel = false;
            if (_curLevel >= DataManager.Inst.userInfo.Level)
            {
                DataManager.Inst.userInfo.Level = _curLevel + 1;
                _degreeComponent.SetLevel(_curLevel);
                showReward(_curLevel);
            }
            showPraise(LevelHelper.Inst.rightStreak,_curLevel);
        }
        else
        {
            SoundPlay.PlaySFX(Table.Sound.anwser_wrong);
            LevelHelper.Inst.rightStreak = 0;
            DataManager.Inst.userInfo.ChangeEnergy(-1);
        }
    }
    public override void OnStart()
    {
    }

    void showReward( int level)
    {
        int lb;
        var t = LevelHelper.getDegree(level,out lb);
        if (level == t.levelTop)
        {
            UIManager.Inst.ShowUI(UIName.UIGetRewardPanel,false, t.coin);
        }
    }
    void showPraise(int rightStreak,int level)
    {
        int lb;
        var t = LevelHelper.getDegree(level,out lb);
        if (t != null && t.levelTop == level)
        {
            if (string.IsNullOrEmpty(t.degreeRaise))
            {
                //升级
                SoundPlay.PlaySFX(Table.Sound.upgrade);
                _prizeComponent_shengji.Show();
                return;
            }
            else
            {
                //毕业
                SoundPlay.PlaySFX(Table.Sound.upgrade);
                _prizeComponent_biye.Show();    
                return;
            }
        }
        
        if (rightStreak >= 3)
        {
            _prizeComponent3.Show();
        }else if (rightStreak >= 2)
        {
            _prizeComponent2.Show();
        }else if (rightStreak >= 1)
        {
            _prizeComponent1.Show();
        }
        SoundPlay.PlaySFX(Table.Sound.anwser_right);
    }

    public override void SetData(params object[] args)
    {
        _curLevel = (int)args[0];
       
        
        SetQuestion(_curLevel);
        
    }

}

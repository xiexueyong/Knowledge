using System;
using System.Collections;
using System.Collections.Generic;
using Framework.Asset;
using Framework.Tables;
using UnityEngine;
using UnityEngine.UI;

public class UIWheel : BaseUI
{
    private const float raduis = 180;
    private const int GridCount = 8;//格子数量
    private const float MaxSpeed = 720f;//最大速度
    private const float Acceleration = -2000f;//启动加速度
    private const float StopTurn = 3;//转几圈停止

    float sumDistance = 0;
    private float curSpeed = 0;
    private float stopAcce = 0;
    private float stopDistance = 0;
    
    public GameObject awardItemTemplate;
    public List<wheelAwardItem> wheelItems;
    public RectTransform TurnRect;
    
    public Button ControlBtn;
    public Button CloseBtn;
    public Text ControlText;

    public RectTransform Arrow;
    

    //计算
    private bool spining;//正在启动
    private bool stoping;//正在停止
    private bool stop = true;//处于停止状态
    private string spinCommond = "start";// start stop

    private int target = 0;
    private int _coinCost = 0;
    List<TableWheels> wheelRewards;

    private WheelTools.WheelType wheelType;
    private int freeCount;
    private Action<Dictionary<int, int>> wheelCallback;
    public override void OnAwake()
    {
        ControlBtn.onClick.AddListener(onControlBtnClick);
        CloseBtn.onClick.AddListener(CloseClick);
    }
    Dictionary<int, int> result;
    public override void SetData(params object[] args)
    {
        TurnRect.rotation = Quaternion.identity;
        sumDistance = 0;
        wheelRewards = (List<TableWheels>) args[0];
        wheelType = (WheelTools.WheelType)args[1];
        freeCount = (int)args[2];

        while (wheelRewards.Count > GridCount)
        {
            wheelRewards.RemoveAt(wheelRewards.Count-1);
        }

        Reset();
        ControlText.text = "开始";
    }
    public void setWheelCallback(Action<Dictionary<int, int>> callback)
    {
        wheelCallback = callback;
    }
    public override void OnStart()
    {
       

    }
    
    void Reset()
    {
        target = 0;
        curSpeed = 0;
        spining = false;
        stoping = false;
        spinCommond = "start";
        sumDistance = sumDistance % 360;
        stopDistance = 0;
        stop = true;
    }

    void StartSpin()
    {
        target = 5;//WheelTools.getLuckyGrid(wheelRewards);
        result = new Dictionary<int, int>();
        result[wheelRewards[target].GoodsId] = wheelRewards[target].Num;
        
        spining = true;
        stoping = false;
        stop = false;
    }
    internal IEnumerator StopSpin()
    {
        // yield return new WaitForSeconds(1f);
        spining = false;
        stoping = true;

        float leftToOneTurn = -360 - sumDistance % 360;
        stopDistance = -360 * StopTurn + leftToOneTurn + target * 360 / GridCount;
        stopAcce = curSpeed * curSpeed / (-2 * stopDistance) ;
        yield break;
    }
    void OnWheelStop()
    {
        // UIGainGoodsPanel p =(UIGainGoodsPanel) UIManager.Inst.ShowUI(UIModuleEnum.UIGainGoodsPanel, false, result, UIModuleEnum.UIWheel);
        // p.setCollectCallback(OnCollectCallback);
    }

    private void onControlBtnClick()
    {
        if (spinCommond == "start" && stop)
        {
            ControlText.text = "停止";
            StartSpin();
            spinCommond = "stop";
        }
        else if(spinCommond == "stop")
        {
            ControlText.text = "开始";
            StopClick();
            spinCommond = "start";
        }
    }

    private void StopClick()
    {
        StartCoroutine(StopSpin());
    }

    private void CloseClick()
    {
        Reset();
        Close();
    }
    void Update()
    {
        if (stop)
        {
            return;
        }
        if (spining)
        {
            curSpeed += Acceleration * Time.deltaTime;
            curSpeed = Mathf.Clamp(curSpeed,-MaxSpeed, 0);

            float d = curSpeed * Time.deltaTime;
            TurnRect.Rotate(new Vector3(0, 0, d));
            sumDistance += d;
        }
        else if (stoping)
        {
            curSpeed += stopAcce * Time.deltaTime;
            curSpeed = Mathf.Clamp(curSpeed, -MaxSpeed,-4);

            float d = curSpeed * Time.deltaTime;
            stopDistance -= d;

            if (stopDistance>0)
            {
                d += stopDistance;
                stopDistance = 0;
            }
            sumDistance += d;
            TurnRect.Rotate(new Vector3(0, 0, d));
            if (stopDistance >= 0)
            {
                OnWheelStop();
                Reset();
            }
        }
    }
}

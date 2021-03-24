using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using EventUtil;
using FFF.Scripts.Framework.Data.DataFramework.Server;
using Framework.Storage;
using SimpleJSON;
using Framework.Tables;



public class ServerDataManager : D_MonoSingleton<ServerDataManager>
{
    private bool _inited;


    public void Init()
    {
        _inited = true;
        refresh();
    }
    public void refresh()
    {
        //首次心跳
        Inst.updateLogic();
    }


    private ActivityInfo _activityInfo;
    public ActivityInfo activityInfo
    {
        get
        {
            if (_activityInfo == null)
                _activityInfo = new ActivityInfo();
            return _activityInfo;
        }
    }

    private LevelRankInfo _levelRankInfo;

    public LevelRankInfo LevelRankInfo
    {
        get
        {
            if (_levelRankInfo == null)
            {
                _levelRankInfo = new LevelRankInfo();
            }

            return _levelRankInfo;
        }
    }

    /// <summary>
    /// 邮件
    /// </summary>
    private ServerMailInfo _serverMailInfo;
    public ServerMailInfo serverMailInfo
    {
        get
        {
            if (_serverMailInfo == null)
                _serverMailInfo = new ServerMailInfo();
            return _serverMailInfo;
        }
    }

    /// <summary>
    /// 签到
    /// </summary>
    private SigninIno _signinInfo;
    public SigninIno signinInfo
    {
        get
        {
            if (_signinInfo == null)
                _signinInfo = new SigninIno();
            return _signinInfo;
        }
    }


    /// <summary>
    /// 心跳
    /// </summary>
    private HeartBeatInfo _heartBeatInfo;
    public HeartBeatInfo heartBeatInfo
    {
        get
        {
            if (_heartBeatInfo == null)
                _heartBeatInfo = new HeartBeatInfo();
            return _heartBeatInfo;
        }
    }



    ///心跳
    float m_UpdateInterval = 60f;
    float m_LastUpdateTime = 0f;
    void Update()
    {
        if (_inited)
        {
            if (Time.realtimeSinceStartup - m_LastUpdateTime >= m_UpdateInterval)
            {
                m_LastUpdateTime = Time.realtimeSinceStartup;
                updateLogic();
            }
        }
    }
    private void updateLogic()
    {
        heartBeatInfo.HeartBeat();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressInfo
{
    //总数
    public int totalCount;
    //开始下载的index，从第几个开始下载
    public int startIndex;
    //当前进度的序号
    private int _curIndex;
    public int curIndex {
        get
        {
            return _curIndex;
        }
        set
        {
            if(_curIndex != value)
            {
                _curIndexDone = false;
                _curIndex = value;
            }
        }
    }
    //当前序号的进度是否完成
    private bool _curIndexDone;
    public bool curIndexDone {
        get
        {
            return _curIndexDone;
        }
        set
        {
            _curIndexDone = value;
        }
    }

    //当前序号的进度
    public float curProgress;


    public float progress
    {
        get
        {
            if (totalCount <= 0)
            {
                return 1f;
            }
            else
            {
                //float p = (1f / totalCount) * (curIndex + curProgress);
                //if (p > 1f)
                //{
                //    return 1;
                //}
                //return p<0.03f?0.03f:p;
                return curProgress<0.03f?0.03f:curProgress;
            }
        }
    }
    public void Reset()
    {
        start = false;
        sucess = false;
        fail = false;

        totalCount = 0;
        curIndex = 0;
        curProgress = 0;
    }

    public bool start;
    public bool sucess;
    public bool fail;
}

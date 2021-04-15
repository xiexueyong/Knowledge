using System;
using System.Collections;
using System.Collections.Generic;
using Framework.Asset;
using Framework.Tables;
using UnityEngine;
using UnityEngine.UI;

public class DegreeComponent : MonoBehaviour
{
    [SerializeField] private Image degree_icon;
    [SerializeField] private Image degree_progress;
    [SerializeField] private Text txt_level;
    [SerializeField] private Text txt_degree_name;

    private TableDegree _curDegree;
    private int levelBottom;
    private int curLevel;
    private int levelTop;
    public void Init()
    {
        
    }

    // public void SetData(TableDegree degree,int level,int levelBottom)
    // {
    //     if (degree != _curDegree)
    //     {
    //         _curDegree = degree;
    //         txt_degree_name.text = degree.degreeName;
    //         degree_icon.sprite = Res.LoadResource<Sprite>("Texture/Degree/"+degree.icon);
    //     }
    //
    //     txt_level.text = string.Format("{0}/{1}",level,degree.levelTop);
    //     //空：10，满：280
    //     float w = (level-levelBottom)*1f/(_curDegree.levelTop - levelBottom)*270;
    //     degree_progress.rectTransform.sizeDelta = new Vector2(10+w,18);
    // }
    public void SetDegree(TableDegree degree,int levelBottom)
    {
        if (degree != _curDegree)
        {
            _curDegree = degree;
            txt_degree_name.text = degree.degreeName;
            degree_icon.sprite = Res.LoadResource<Sprite>("Texture/Degree/"+degree.icon);
        }

        this.levelTop = _curDegree.levelTop;
        this.levelBottom = levelBottom;
    }
    public void SetLevel(int level)
    {
        if (_curDegree != null)
        {
            curLevel = level;
            txt_level.text = string.Format("{0}/{1}",level,_curDegree.levelTop);
            //空：10，满：280
            float w = (level-levelBottom)*1f/(_curDegree.levelTop - levelBottom)*270;
            degree_progress.rectTransform.sizeDelta = new Vector2(10+w,18);    
        }
        
    }
    
    
}

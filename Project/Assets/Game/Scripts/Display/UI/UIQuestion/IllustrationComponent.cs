using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class IllustrationComponent : MonoBehaviour
{
    [SerializeField] private RectTransform imgRectTransform;
    [SerializeField] private Button btn;
    [SerializeField] private Image image;
    [NonSerialized]public Sprite sprite;
    public void Awake()
    {
        btn.onClick.AddListener(onClickMask);
    }

    public void Init()
    {
        
    }

    void onClickMask()
    {
        Hide();
    }

    public void Show()
    {
        if (sprite != null)
        {
            gameObject.SetActive(true);
            image.sprite = sprite;

            Vector2? size = UIManager.Inst.GetUISize();
            var h = sprite.texture.height;
            var w = sprite.texture.width;

            var imgW = size.Value.x - 40;
            var imgH = h/(w/imgW);
            imgRectTransform.sizeDelta = new Vector2(imgW,imgH);
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
  
    
}

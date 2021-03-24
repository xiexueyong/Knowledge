using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSComponet : MonoBehaviour
{
    // FPS
    float m_UpdateShowDeltaTime = 0.5f;
    float m_LastUpdateShowTime = 0f;
    int m_FrameUpdate = 0;


    public Text FPSText;

    void Start()
    {
        FPSText = transform.GetComponent<Text>();
    }
    void Update()
    {
        m_FrameUpdate++;
        if (Time.realtimeSinceStartup - m_LastUpdateShowTime >= m_UpdateShowDeltaTime)
        {
            float m_FPS = m_FrameUpdate / (Time.realtimeSinceStartup - m_LastUpdateShowTime);
            FPSText.text = "" + Mathf.RoundToInt(m_FPS); ;
            m_FrameUpdate = 0;
            m_LastUpdateShowTime = Time.realtimeSinceStartup;
        }
    }
}





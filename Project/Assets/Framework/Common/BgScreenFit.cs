using UnityEngine;

public class BgScreenFit : MonoBehaviour
{
    private readonly Vector2 m_TargetResolution = new Vector2(720f,1280f);
    private RectTransform m_RectTrans;

    // Use this for initialization
    void Start()
    {
        m_RectTrans = gameObject.GetComponent<RectTransform>();

        float targetRadio = m_TargetResolution.y / m_TargetResolution.x;
        float bgWidth = m_RectTrans.rect.width;
        float bgHeight = m_RectTrans.rect.height;
        float screenRadio = (float) Screen.height / Screen.width;
        float bgRadio = bgHeight / bgWidth;
        if (screenRadio > bgRadio) {
            float scaleH = screenRadio * (1.0f / targetRadio);
            m_RectTrans.sizeDelta = new Vector2(bgWidth * scaleH, m_TargetResolution.y * scaleH);
        }
        else {
            float scaleW = 1.0f / screenRadio / (1.0f / targetRadio);
            float scaleH = m_TargetResolution.x * scaleW / bgWidth;
            m_RectTrans.sizeDelta = new Vector2(m_TargetResolution.x * scaleW, bgHeight * scaleH);
        }
    }
}
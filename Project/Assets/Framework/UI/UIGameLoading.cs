using UnityEngine;
using UnityEngine.UI;

public class UIGameLoading : BaseUI
{
    public Text progressText;
    public Image progressImage;
    public ProgressInfo progressInfo;
    public override void OnAwake()
    {
        progressText = transform.Find("root/Text").GetComponent<Text>();
        progressImage = transform.Find("root/ProgressSlot/Progress").GetComponent<Image>();
        progressImage.fillAmount = 0.5f;
    }

    public void setProgress(float progress)
    {
        progressImage.fillAmount = progress;
        progressText.text = (int)(progress*100)+"%";
    }

    public void Update()
    {
        if (progressInfo != null)
        {
            setProgress(progressInfo.progress);
        }
    }
}

using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UISkinShopItemLockTip : BaseUI
{
    public float moveY;
    public float durationMove;
    public float durationFade;
    public Text contentText;
    public Image bgImage;
    public Image lockImage;
    public GameObject parentObj;

    public override void SetData(params object[] objs)
    {
        base.SetData(objs);
        bool needLockImage = true;
        if (objs.Length > 0)
        {
            contentText.text = (string) objs[0];
        }

        if (objs.Length > 1)
        {
            needLockImage = (bool) objs[1];
        }

        lockImage.gameObject.SetActive(needLockImage);
        bgImage.color = Color.white;
        lockImage.color = Color.white;
    }

    //public override void OnShowBegin()
    //{
    //    base.OnShowBegin();
    //    parentObj.transform.localPosition = Vector3.zero;

    //    Sequence sequence = DOTween.Sequence();
    //    sequence.Append(parentObj.transform.DOLocalMoveY(parentObj.transform.localPosition.y + moveY, durationMove));
    //    sequence.Append(bgImage.DOFade(0f, durationFade));
    //    sequence.Join(contentText.DOFade(0f, durationFade));
    //    sequence.Join(lockImage.DOFade(0f, durationFade));
    //    sequence.AppendCallback(() => { UIManager.Inst.HideUI(UIModuleEnum.UISkinShopItemLockTip, true); });
    //    bgImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
    //        contentText.preferredWidth + 100);
    //}
}
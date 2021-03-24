using UnityEngine;
using UnityEngine.UI;

public class ChinScreenSimulate
{
    /// <summary>
    /// 开启模拟
    /// </summary>
    public static void OpenSimulate()
    {
        DataManager.Inst.SimulateChinScreen = true;
        if (UIManager.Inst == null) {
            return;
        }

        var tf = UIManager.Inst.gameObject.GetComponent<RectTransform>();
        if (tf == null) {
            return;
        }

        var pixelWidth = UIManager.Inst.UICamera.pixelWidth;
        var pixelHeight = UIManager.Inst.UICamera.pixelHeight;
        float radio = pixelWidth * 1.0f / pixelHeight * 1.0f;
        if (radio > 0.4762f) {
            // 说明不是刘海屏，强制改变分辨率
#if UNITY_EDITOR
            UnityEditor.EditorUtility.DisplayDialog("刘海屏测试", "Game 视图分辨率不是刘海屏比例比 2.1 至少！", "手动修改");
            return;
#endif
        }

        CreateSimulateUI();
    }

    private static void CreateSimulateUI()
    {
        string canvasName = "UISimulateNotch";
        if (GameObject.Find(canvasName)) {
            return;
        }

        var root = new GameObject(canvasName);
        Object.DontDestroyOnLoad(root);
        Canvas canvas = root.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        var scaler = root.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;

        GameObject child = new GameObject("Notch");
        Image img = child.AddComponent<Image>();
        img.color = Color.black;
        RectTransform rectTransform = child.transform as RectTransform;
        rectTransform.SetParent(root.transform, false);
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.sizeDelta = new Vector2(328f, 80f);
        rectTransform.anchorMin = new Vector2(0.5f, 1f);
        rectTransform.anchorMax = rectTransform.anchorMin;
        rectTransform.pivot = rectTransform.anchorMin;
    }
}
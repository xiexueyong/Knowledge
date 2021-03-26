using EventUtil;
using Framework.Storage;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class BackTopBarItem : TopBarItem
{
    public Button btn;
    public void Awake()
    {
        btn.onClick.AddListener(() =>
        {
            UIManager.Inst.Back();
        });
    }
}

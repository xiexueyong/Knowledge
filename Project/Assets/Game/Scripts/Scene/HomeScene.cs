using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeScene : MonoBehaviour
{
    void Start()
    {
        ScapeTool.Inst.SetScape("");
        UIManager.Inst.ShowUI(UIName.UITest);
        UIManager.Inst.ShowUI(UIName.UILobby);
        SoundPlay.PlayMusic("music_04");
        Debug.Log("Homescene ");
    }
    private void OnEnable()
    {
        //SoundPlay.PlayMusic("music_04");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
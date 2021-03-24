using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIManager.Inst.StartGameImage.gameObject.SetActive(false);
        UIManager.Inst.ShowUI(UIModuleEnum.UITest);
        UIManager.Inst.ShowUI(UIModuleEnum.UILobby);
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
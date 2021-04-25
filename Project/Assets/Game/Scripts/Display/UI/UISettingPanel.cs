
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using EnhancedScrollerDemos.CellEvents;

public class UISettingPanel : BaseUI
{
    
    [SerializeField] private Sprite sprite_music_on;
    [SerializeField] private Sprite sprite_music_off;
    [SerializeField] private Sprite sprite_sound_on;
    [SerializeField] private Sprite sprite_sound_off;
    
    public Button btn_sound;
    public Button btn_music;
    public Button btn_close;
    
    public override void OnAwake()
    {
        btn_sound.onClick.AddListener(onBtnSoundClick);
        btn_music.onClick.AddListener(onBtnMusicClick);
        btn_close.onClick.AddListener(() => { Close();});
    }
    void onBtnSoundClick()
    {
        SoundPlay.isSoundOpen = !SoundPlay.isSoundOpen;
        RefreshUI();
    }
    void onBtnMusicClick()
    {
        SoundPlay.isMusicOpen = !SoundPlay.isMusicOpen;
        RefreshUI();
    }

    void RefreshUI()
    {
        if (SoundPlay.isSoundOpen)
        {
            btn_sound.GetComponent<Image>().sprite = sprite_sound_on;
        }
        else
        {
            btn_sound.GetComponent<Image>().sprite = sprite_sound_off;
        }
        
        if (SoundPlay.isMusicOpen)
        {
            btn_music.GetComponent<Image>().sprite = sprite_music_on;
        }
        else
        {
            btn_music.GetComponent<Image>().sprite = sprite_music_off;
        }
        
    }

    public override void SetData(params object[] args)
    {
        base.SetData(args);
        RefreshUI();
    }
}

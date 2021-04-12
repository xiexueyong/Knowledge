using Framework.Asset;
using UnityEngine;
using System;

public static class SoundPlay
{
    private static SoundManager soundManager;

    private static bool _inited;

    public static void Init()
    {
        if (!_inited)
        {
            _inited = true;
            isMusicOpen = isMusicOpen;
            isSoundOpen = isSoundOpen;
        }
    }

    //1状态是开，0状态是关 
    public static bool isMusicOpen{
        get
        {
            return PlayerPrefs.GetInt("MusicOpen", 1) == 1;
        }
        set
        {
            PlayerPrefs.SetInt("MusicOpen", value ? 1 : 0);
            SoundManager.MuteMusic(!value);
        }
		
	}
	public static bool isSoundOpen{
        get
        {
            return PlayerPrefs.GetInt("SoundOpen", 1) == 1;
        }
        set
        {
            PlayerPrefs.SetInt("SoundOpen", value ? 1 : 0);
            SoundManager.MuteSFX(!value);
        }
	}
    
    public static void StopMusic()
    {
        SoundManager.StopMusic();
    }
    public static void PlayMusic(string clipName,  bool looping = true)
    {
        if (_inited)
        {
            AudioClip audioClip = Res.LoadResource<AudioClip>("Audio/Music/" + clipName);
            if (audioClip != null)
            {
                SoundManager.Play(audioClip,looping);
            }
            else
            {
                DebugUtil.LogError(string.Format("Music __{0}__ is not exist ",clipName));
            }
        }
    }

    public static void Stop()
    {
        if (_inited)
        {
            SoundManager.Stop();
        }
    }

    public static void StopSFX()
    {
        if (_inited)
        {
            SoundManager.StopSFX();
        }
    }
    [Obsolete("StopSFX(string) is obsolete,use StopSFX(AudioSource) to instead of")]
    public static void StopSFX(string clipName)
    {
        DebugUtil.LogError("StopSFX(clipName) is obsolete,use StopSFX(AudioSource) to instead of");
    }
    public static void StopSFX(AudioSource audioSource)
    {
        if (_inited && audioSource != null)
        {
            SoundManager.StopSFXObject(audioSource);
        }
    }

    public static void SetVolume(float volume)
    {
        SoundManager.SetVolume(volume);
    }

    public static AudioSource PlaySFX(int soundId, bool looping = false, float delay = 0f, float volume = float.MaxValue, float pitch = float.MaxValue, Vector3 location = default(Vector3))
    {
        string soundName = Table.Sounds.Get(soundId).Sound_Res;
        return PlaySFX(soundName,looping,delay,volume,pitch,location);
    }

    public static AudioSource PlaySFX(string clipName, bool looping = false, float delay = 0f, float volume = float.MaxValue, float pitch = float.MaxValue, Vector3 location = default(Vector3))
    {
        if (string.IsNullOrEmpty(clipName))
        {
            return null;
        }
        if (_inited)
        {
            AudioClip audioClip = Res.LoadResource<AudioClip>(clipName);
            if (audioClip != null)
            {
                return SoundManager.PlaySFX(audioClip, looping,delay);
            }
            else
            {
                DebugUtil.LogError(string.Format("Sound __{0}__ is not exist ", clipName));
            }
        }
        return null;
    }





    #region SoundHelper
    public static void PlayCommBtnClick()
    {
        SoundPlay.PlaySFX("");
    }



    #endregion
}

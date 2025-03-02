using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    AudioSource bgmAudioSource;

    [SerializeField]
    AudioSource seAudioSource;

    [SerializeField]
    List<BGMSoundData> bgmSoundDatas;

    [SerializeField]
    List<SESoundData> seSoundDatas;

    public static float masterVolume = 1;
    public static float bgmMasterVolume = 1;
    public static float seMasterVolume = 1;
    private float datavolume = 0f;

    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        bgmAudioSource.volume = datavolume * bgmMasterVolume * masterVolume;
    }

    public void PlayBGM(BGMSoundData.BGM bgm)
    {
        BGMSoundData data = bgmSoundDatas.Find(data => data.bgm == bgm);
        bgmAudioSource.clip = data.audioClip;
        datavolume = data.volume;
        bgmAudioSource.Play();
    }

    public void PlaySE(SESoundData.SE se)
    {
        SESoundData data = seSoundDatas.Find(data => data.se == se);
        seAudioSource.volume = data.volume * seMasterVolume * masterVolume;
        seAudioSource.PlayOneShot(data.audioClip);
    }
}

[System.Serializable]
public class BGMSoundData
{
    public enum BGM
    {
        All, //ラベル
    }

    public BGM bgm;
    public AudioClip audioClip;

    [Range(0, 1)]
    public float volume = 1;
}

[System.Serializable]
public class SESoundData
{
    public enum SE
    {
        Correct,
        InCorrect, // これがラベルになる
        Clear,
        GameOver,
    }

    public SE se;
    public AudioClip audioClip;

    [Range(0, 1)]
    public float volume = 1;
}

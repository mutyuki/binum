using UnityEngine;

public class BGM : MonoBehaviour
{
    void Start()
    {
        SoundManager.Instance.PlayBGM(BGMSoundData.BGM.All);
    }
}

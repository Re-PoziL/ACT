using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AudioSourceType
{
    Hit,
    GreatSword,
    Katana,
    Parry,
    Null,
}

[CreateAssetMenu()]
public class AudioSourceSO : ScriptableObject
{
    

    [System.Serializable]
    public struct AudioAsset
    {
        public AudioSourceType type;
        public List<AudioClip> audioClips;
    }


    public AudioAsset[] audioAssets;

    private Dictionary<AudioSourceType, AudioClip[]> audioDic = new Dictionary<AudioSourceType, AudioClip[]>();

    public void Init()
    {
        foreach (var item in audioAssets)
        {
            audioDic.Add(item.type, item.audioClips.ToArray());
        }
    }

    public AudioClip GetAudioClip(AudioSourceType audioSourceType)
    {
        return audioDic[audioSourceType][Random.Range(0,audioDic[audioSourceType].Length)];
    }


}

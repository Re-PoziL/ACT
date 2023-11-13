using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : SingletonBase<GameAssets>
{

    [SerializeField]private AudioSourceSO audioSourceSO;

    private void Awake()
    {
        audioSourceSO.Init();
    }

    
    private void Update()
    {
    }

    public void PlayAudio(AudioSource audioSource, AudioSourceType audioSourceType)
    {
        audioSource.clip = audioSourceSO.GetAudioClip(audioSourceType);
        audioSource.Play();
    }

}

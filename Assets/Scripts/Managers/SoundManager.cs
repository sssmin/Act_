using UnityEngine;
using UnityEngine.Audio;

public enum ESoundType
{
    Master,
    Background,
    Effect
}

public class SoundManager : MonoBehaviour
{
    public AudioMixer AudioMixer { get; set; }
    private string Master => "AudioMaster";
    private string Background => "AudioBackground";
    private string Effect => "AudioEffect";
    
    public void SFXPlay(string sfxName, ESoundType soundType = ESoundType.Effect)
    {
        AudioClip audioClip = GI.Inst.ResourceManager.GetAudioClip(sfxName);
        if (audioClip == null)
        {
            Debug.Log("오디오 클립 없음");
            return;
        }

        if (soundType == ESoundType.Effect)
        {
            GameObject go = GI.Inst.ResourceManager.Instantiate("AudioSourceObject");
            AudioSource audioSource = go.GetComponent<AudioSource>();
            audioSource.clip = audioClip;
            AudioMixerGroup[] groups = AudioMixer.FindMatchingGroups("Effect");
            if (groups.Length >= 0)
            {
                audioSource.outputAudioMixerGroup = groups[0];
                GI.Inst.ResourceManager.Destroy(go, audioClip.length);
            }
            else
                Debug.Log("믹스 그룹 없음");
            audioSource.Play();
        }
        else if (soundType == ESoundType.Background)
        {
            GameObject go = GI.Inst.ResourceManager.Instantiate("AudioSource_Background");
            AudioSource audioSource = go.GetComponent<AudioSource>();
            audioSource.clip = audioClip;
            AudioMixerGroup[] groups = AudioMixer.FindMatchingGroups("Background");
            if (groups.Length >= 0)
            {
                audioSource.outputAudioMixerGroup = groups[0];
                audioSource.loop = true;
            }
            else
                Debug.Log("믹스 그룹 없음");
            audioSource.Play();
        }
    }

    public void GetAudioVolume(ESoundType soundType, out float value)
    {
        value = 0f;
        switch (soundType)
        {
            case ESoundType.Master:
                AudioMixer.GetFloat(Master, out value);
                break;
            case ESoundType.Background:
                AudioMixer.GetFloat(Background, out value);
                break;
            case ESoundType.Effect:
                AudioMixer.GetFloat(Effect, out value);
                break;
        }
    }

    public void SetAudioVolume(ESoundType soundType, float value)
    {
        switch (soundType)
        {
            case ESoundType.Master:
                AudioMixer.SetFloat(Master, value);
                break;
            case ESoundType.Background:
                AudioMixer.SetFloat(Background, value);
                break;
            case ESoundType.Effect:
                AudioMixer.SetFloat(Effect, value);
                break;
        }
        
    }
    
    
}



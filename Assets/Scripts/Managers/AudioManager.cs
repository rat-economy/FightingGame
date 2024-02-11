using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager Instance;
    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        foreach (var sound in sounds)
        {
            sound.m_Source = gameObject.AddComponent<AudioSource>();
            sound.m_Source.clip = sound.m_Clip;
            sound.m_Source.volume = sound.m_Volume;
        }
    }

    public void PlaySoundOnce(Sound sound)
    {
        if(sound == null)
        {
            return;
        }
        sound.m_Source.loop = false;
        sound.m_Source.Play();
    }
    public void PlaySoundLooped(Sound sound)
    {
        if(sound == null)
        {
            return;
        }
        sound.m_Source.loop = true;
        sound.m_Source.Play();
    }

    public void StopSound(Sound sound)
    {
        if(sound == null)
        {
            return;
        }
        sound.m_Source.Stop();
    }

    /*
        OVERLOADED FUNCTIONS TO PLAY SOUND BY THEIR STRING NAME
    */
    public void PlaySoundOnce(string soundName)
    {
        var s = GetAudioSource(soundName);
        s.loop = false;
        s.Play();
    }
    public void PlaySoundLooped(string soundName)
    {
        var s = GetAudioSource(soundName);
        s.loop = true;
        s.Play();
    }

    public void StopSound(string soundName)
    {
        GetAudioSource(soundName).Stop();
    }

    private AudioSource GetAudioSource(string soundName)
    {
        foreach (Sound sound in sounds)
        {
            if(sound.name == soundName) return sound.m_Source;
        }
        Debug.LogError("Sound Name Not Found!");
        return null;
    }
}
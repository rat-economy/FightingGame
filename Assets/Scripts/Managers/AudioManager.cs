using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public Sound[] menuSongs;
    public Sound[] fightSongs;
    public static AudioManager Instance;

    private Sound currentFightSong;
    private void OnEnable()
    {
        foreach (var sound in sounds)
        {
            sound.m_Source = gameObject.AddComponent<AudioSource>();
            sound.m_Source.clip = sound.m_Clip;
            sound.m_Source.volume = sound.m_Volume;
        }
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);
        // SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /*void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("loading scene!");
        foreach (var sound in sounds)
        {
            sound.m_Source = gameObject.AddComponent<AudioSource>();
            sound.m_Source.clip = sound.m_Clip;
            sound.m_Source.volume = sound.m_Volume;
        }
    }*/

    private bool IsSoundNull(Sound sound)
    {
        if(sound == null)
        {
            Debug.LogError("Sound is null");
            return true;
        }
        else if (sound.m_Source == null)
        {
            Debug.LogError("Sound's audio source is null");
            return true;
        }
        return false;
    }

    public void PlaySoundOnce(Sound sound)
    {
        if( IsSoundNull(sound) )
        {
            return;
        }
        sound.m_Source.loop = false;
        sound.m_Source.Play();
    }

    public IEnumerator C_PlaySoundAndWait(Sound sound)
    {
        if( IsSoundNull(sound) )
        {
            yield return 0;
        }
        sound.m_Source.loop = false;
        sound.m_Source.Play();
        yield return new WaitForSeconds(sound.m_Source.clip.length);
    }

    public void PlaySoundLooped(Sound sound)
    {
        if( IsSoundNull(sound) )
        {
            return;
        }
        sound.m_Source.loop = true;
        sound.m_Source.Play();
    }

    public void StopSound(Sound sound)
    {
        if( IsSoundNull(sound) )
        {
            return;
        }
        sound.m_Source.Stop();
    }

    public void PickFightSong()//This is shit and i will not hardcode in next version but need to fly now
    {
        int index = Random.Range(0, fightSongs.Length);
        PlaySoundOnce(fightSongs[index]);
    }

    public void PlaySoundOnce(Sound[] sound)
    {

    }
    
    public void StopFightMusic() //Fuck you roosevelt
    {
        StopSound(fightSongs[0]);
        StopSound(fightSongs[1]);
    }
}
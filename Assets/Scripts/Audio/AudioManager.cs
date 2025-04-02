using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public SaveManager saveManager;
    public Sound[] sounds;
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        if (saveManager.state.isMuted)
            s.source.mute = true;
        s.source.Play();
    }
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Stop();
    }

    public void StopAll()
    {
        foreach (Sound s in sounds)
        {
            s.source.Stop();
        }
    }

    public void PauseAll()
    {
        foreach (Sound s in sounds)
        {
            s.source.Pause();
        }
    }
    public void UnPauseAll()
    {
        foreach (Sound s in sounds)
        {
            s.source.UnPause();
        }
    }
    public void ToggleMute()
    {
        saveManager.state.isMuted = !saveManager.state.isMuted;
        saveManager.Save();
        if (saveManager.state.isMuted)
        {
            MuteAll();
        }
        else
        {
            UnmuteAll();
        }
    }

    public void MuteAll()
    {
        foreach (Sound s in sounds)
        {
            s.source.mute = true;
        }
    }
    public void UnmuteAll()
    {
        foreach (Sound s in sounds)
        {
            s.source.mute = false;
        }
    }
}

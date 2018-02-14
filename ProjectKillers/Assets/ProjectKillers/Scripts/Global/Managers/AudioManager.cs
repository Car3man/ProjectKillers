using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : GlobalSingletonBehaviour<AudioManager> {
    private ComponentPool<AudioSource> audioSourcePool;
    private ComponentPool<AudioSource> AudioSourcePool { get { return audioSourcePool ?? (audioSourcePool = new ComponentPool<AudioSource>(gameObject, 1)); } }

    /// <summary>
    /// Similar to Play(), but creates a new AudioSource prior to playing, which can be cancelled.
    /// </summary>
    public AudioSource PlayAsAudioSource(AudioClip clip) {
        if (clip == null) throw new ArgumentNullException("clip");

        AudioSource audioSource = AudioSourcePool.Request;

        audioSource.clip = clip;
        audioSource.Play();
        audioSource.loop = false;

        audioSource.AddOnDoneCallback(AudioSourcePool.Free);

        return audioSource;
    }

    /// <summary>
    /// Similar to Play(), but creates a new AudioSource prior to playing, which can be cancelled. AudioClip taked from resoruces
    /// </summary>
    public AudioSource PlayAsAudioSource(string resourcePath) {
        if (string.IsNullOrEmpty("string")) throw new ArgumentNullException("resourcePath");

        AudioSource audioSource = AudioSourcePool.Request;

        audioSource.clip = Resources.Load<AudioClip>(resourcePath);
        audioSource.Play();
        audioSource.loop = false;

        audioSource.AddOnDoneCallback(AudioSourcePool.Free);

        return audioSource;
    }
}

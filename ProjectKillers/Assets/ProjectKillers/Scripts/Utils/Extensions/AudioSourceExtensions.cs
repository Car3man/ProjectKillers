using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

public static class AudioSourceExtensions {
    /// <summary>
    /// Executes the callback when the AudioSource is done playing.
    /// </summary>
    public static void AddOnDoneCallback([NotNull] this AudioSource audioSource, [NotNull] Action<AudioSource> callback) {
        if (audioSource == null) throw new ArgumentNullException("audioSource");
        if (callback == null) throw new ArgumentNullException("callback");

        IE_AudioSourceDoneCallback(audioSource, callback).RunCoroutine();
    }

    private static IEnumerator IE_AudioSourceDoneCallback([NotNull] AudioSource audioSource, [NotNull] Action<AudioSource> callback) {
        if (audioSource == null) throw new ArgumentNullException("audioSource");
        if (callback == null) throw new ArgumentNullException("callback");

        while (audioSource.isPlaying)
            yield return null;

        callback(audioSource);
    }
}
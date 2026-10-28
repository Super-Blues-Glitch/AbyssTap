using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;

    public void PlaySound(AudioSource source, AudioClip clip, bool loop = false)
    {
        source.Stop();
        source.clip = clip;
        source.loop = loop;
        source.Play();
        Debug.LogWarning("Se puso a sonar");
    }

    public void StopSound(AudioSource source)
    {
        source.Stop();
    }
}

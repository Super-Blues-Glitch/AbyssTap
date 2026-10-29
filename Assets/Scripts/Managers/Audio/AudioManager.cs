using PrimeTween;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private const string MASTERVOL = "MasterVol";
    private const string MUSICVOL = "MusicVol";
    private const string SFXVOL = "SfxVol";
    private const string VOICESVOL = "VoicesVol";

    public float MterVl { get => masterVol; set => masterVol = value; }
    private float masterVol;

    public float MuVl { get => musicVol; set => musicVol = value; }
    private float musicVol;

    public float SFVl { get => sfxVol; set => sfxVol = value; }
    private float sfxVol;

    public float VOIVl { get => voicesVol; set => voicesVol = value; }
    private float voicesVol;

    [SerializeField] private Ease decreasementEase;

    [SerializeField] AudioMixer mixer;
    private void Start()
    {
        SetVolume(GameEnums.Audio.AudioChannel.Master, PlayerPrefs.HasKey(MASTERVOL) ? PlayerPrefs.GetFloat(MASTERVOL) : 1);
        SetVolume(GameEnums.Audio.AudioChannel.Music, PlayerPrefs.HasKey(MUSICVOL) ? PlayerPrefs.GetFloat(MUSICVOL) : 1);
        SetVolume(GameEnums.Audio.AudioChannel.SFX, PlayerPrefs.HasKey(SFXVOL) ? PlayerPrefs.GetFloat(SFXVOL) : 1);
        SetVolume(GameEnums.Audio.AudioChannel.Voices, PlayerPrefs.HasKey(VOICESVOL) ? PlayerPrefs.GetFloat(VOICESVOL) : 1);
    }

    public void SetVolume(GameEnums.Audio.AudioChannel channel, float newVolume)
    {
        switch (channel)
        {
            case GameEnums.Audio.AudioChannel.Master:
                masterVol = newVolume;
                PlayerPrefs.SetFloat(MASTERVOL, newVolume);
                mixer.SetFloat(MASTERVOL, ToDecibels(newVolume));
                break;

            case GameEnums.Audio.AudioChannel.Music:
                musicVol = newVolume;
                PlayerPrefs.SetFloat(MUSICVOL, newVolume);
                mixer.SetFloat(MUSICVOL, ToDecibels(newVolume));
                break;

            case GameEnums.Audio.AudioChannel.SFX:
                sfxVol = newVolume;
                PlayerPrefs.SetFloat(SFXVOL, newVolume);
                mixer.SetFloat(SFXVOL, ToDecibels(newVolume));
                break;

            case GameEnums.Audio.AudioChannel.Voices:
                voicesVol = newVolume;
                PlayerPrefs.SetFloat(VOICESVOL, newVolume);
                mixer.SetFloat(VOICESVOL, ToDecibels(newVolume));
                break;
        }

    }

    public void PlaySound(AudioSource source, AudioClip sound, bool loop = false)
    {
        source.Stop();
        source.clip = sound;
        source.volume = 1;
        source.loop = loop;
        source.Play();
    }

    public void StopSound(AudioSource source, float volumeDecreasementSpeed, Ease volumeDecreasementEase)
    {
        Tween.Custom(1, 0, volumeDecreasementSpeed, (float volume) => source.volume = volume, volumeDecreasementEase);
    }

    private float ToDecibels(float volume) => Mathf.Log10(volume) * 20f;
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    // Variables
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioMixerGroup musicMixer;
    [SerializeField] private AudioMixerGroup sfxMixer;

    [SerializeField] private SFX defaultMusic;
    private AudioSource _musicSource;

    private List<AudioSource> _soundEffectSources = new List<AudioSource>();


    // Functions
    private void Start()
    {
        LoadVolume();
        PlayMusic(defaultMusic);
    }

    #region Volume
        private void LoadVolume()
        {
            SetMixerVolume(AudioMixerKeys.MasterVolumeKey, PlayerPrefs.GetFloat(AudioMixerKeys.MasterVolumeKey));
            SetMixerVolume(AudioMixerKeys.MusicVolumeKey, PlayerPrefs.GetFloat(AudioMixerKeys.MusicVolumeKey));
            SetMixerVolume(AudioMixerKeys.SFXVolumeKey, PlayerPrefs.GetFloat(AudioMixerKeys.SFXVolumeKey));
        }

        public void SetMixerVolume(string key, float volume)
        {
            audioMixer.SetFloat(key, Mathf.Log10(volume) * 20);
        }
    #endregion

    #region Plays
        private void PlayMusic(SFX music)
        {
            _musicSource = gameObject.AddComponent<AudioSource>();
            _musicSource.outputAudioMixerGroup = musicMixer;

            SetupSource(ref _musicSource, music);

            _musicSource.loop = true;
            _musicSource.Play();
        }

        public void PlaySound(SFX sound)
        {
            AudioSource soundSource = GetAvailableSFXSource();

            SetupSource(ref soundSource, sound);

            soundSource.Play();
        }

        public void PlayRandomSound(List<SFX> listOfSFX)
        {
            PlaySound(listOfSFX[Random.Range(0, listOfSFX.Count)]);
        }
    #endregion

    private void SetupSource(ref AudioSource source, SFX sfx)
    {
        source.clip = sfx.Clip;
        source.volume = sfx.Volume;
        source.pitch = sfx.Pitch;
    }

    private AudioSource GetAvailableSFXSource()
    {
        foreach (AudioSource soundEffectSource in _soundEffectSources)
        {
            if (!soundEffectSource.isPlaying)
            {
                return soundEffectSource;
            }
        }

        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
        newAudioSource.outputAudioMixerGroup = sfxMixer;
        _soundEffectSources.Add(newAudioSource);
        return newAudioSource;
    }
}
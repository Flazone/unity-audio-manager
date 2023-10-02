using FLZ.Pooling;
using FLZ.Services;
using UnityEngine;
using UnityEngine.Audio;

namespace FLZ.Audio
{
    public class AudioManager : IService
    {
        private bool _initialized;

        private static AudioSourcePool<PoolableAudioSource> AudioSourcePool;

        public static AudioMixer MasterMixer => _settings.MasterMixer;
        public static AudioMixer MusicMixer => _settings.MusicMixer;
        public static AudioMixer SFXMixer => _settings.SfxMixer;
        
        private AnimationCurve _volumeCurve => _settings.VolumeCurve;
        private float _minVolume => _settings.MinVolume;
        private float _maxVolume => _settings.MaxVolume;
        
        
        private const string GLOBAL_VOLUME = "GlobalMasterVolume";
        private const string MUSIC_VOLUME = "GlobalMusicVolume";
        private const string SFX_VOLUME = "GlobalSFXVolume";
            
        private static AudioManagerSettings _settings => AudioManagerSettings.Instance;
        public static AudioManagerSettings Settings => _settings;
        
        
        public void OnAllServicesReady()
        {
            var parent = new GameObject("AudioManager").transform;
            parent.gameObject.AddComponent<AudioListener>();
            
            AudioSourcePool = new AudioSourcePool<PoolableAudioSource>(_settings.AudioSourcePoolSize, new ComponentFactory<PoolableAudioSource>(), parent);
                
            InitializeVolume(MasterMixer, GLOBAL_VOLUME);
            InitializeVolume(MusicMixer, MUSIC_VOLUME);
            InitializeVolume(SFXMixer, SFX_VOLUME);

            _initialized = true;
        }

        public bool IsReady() => _initialized;


        /// <summary>
        /// Initialize mixer parameters, pull the data from PlayerPrefs and feed the mixers 
        /// </summary>
        private void InitializeVolume(AudioMixer mixer, string paramName)
        {
            float value = PlayerPrefs.GetFloat(paramName, 1);
            SetVolumeParameter(mixer, paramName, value);
        }
        
        public void SetParameter(AudioMixer mixer, string paramName, float value)
        {
            mixer.SetFloat(paramName, value);
            PlayerPrefs.SetFloat(paramName, value);
        }
        
        private float GetParameter(string paramName, AudioMixer mixer = null)
        {
            if (mixer == null)
                mixer = MasterMixer;
            
            if (!mixer.GetFloat(paramName, out var value))
            {
                value = PlayerPrefs.GetFloat(paramName, 1);
            }

            return value;
        }
        
        
        private void SetVolumeParameter(AudioMixer mixer, string paramName, float value)
        {
            float volume = Mathf.Lerp(_minVolume, _maxVolume, _volumeCurve?.Evaluate(value) ?? value);
            SetParameter(mixer, paramName, volume);
        }

        private float GetVolumeParameter(AudioMixer mixer, string paramName)
        {
            var rawValue = GetParameter(paramName, mixer);
            var lerp = Mathf.InverseLerp(_minVolume, _maxVolume, rawValue);

            return _volumeCurve.EvaluateInverse(lerp);
        }
        
        
        public void SetGlobalVolume(float value)
        {
            SetVolumeParameter(MasterMixer, GLOBAL_VOLUME, value);
        }

        public void SetMusicVolume(float volume)
        {
            SetVolumeParameter(MusicMixer, MUSIC_VOLUME, volume);
        }

        public void SetSFXVolume(float volume)
        {
            SetVolumeParameter(SFXMixer, SFX_VOLUME, volume);
        }
        
        
        
        public float GetGlobalVolume() => GetVolumeParameter(MasterMixer, GLOBAL_VOLUME);

        public float GetMusicVolume() => GetVolumeParameter(MusicMixer, MUSIC_VOLUME);
        
        public float GetSFXVolume() => GetVolumeParameter(SFXMixer, SFX_VOLUME);

        
        public static PoolableAudioSource Play(ISound sound, AudioMixerGroup mixer = null)
        {
            if (mixer == null)
            {
                mixer = MasterMixer.outputAudioMixerGroup;
            }
            
            var pooledAudioSource = AudioSourcePool.Spawn();
            pooledAudioSource.SetupAndPlay(AudioSourcePool, sound, mixer);
            
            return pooledAudioSource;
        }

        public static void Stop(ISound sound)
        {
            PoolableAudioSource playingSource = null;
            foreach (var audioSource in AudioSourcePool.ActiveObjects)
            {
                if (audioSource.Sound == sound)
                {
                    playingSource = audioSource;
                    break;
                }
            }

            if (playingSource != null)
            {
                AudioSourcePool.DeSpawn(playingSource);
            }
        }

        public static int GetCurrentPoolSize()
        {
            return AudioSourcePool.Capacity;
        }
    }
    
    public struct SoundSettings
    {
        public AudioClip Clip;
        public float Volume;
        public float Pitch;
        public float PanStereo;
        public float Delay;
        public bool Loop;

        public SoundSettings(AudioClip clip, float volume, float pitch, float panStereo, float delay, bool loop)
        {
            Clip = clip;
            Volume = volume;
            Pitch = pitch;
            PanStereo = panStereo;
            Delay = delay;
            Loop = loop;
        }
    }
    
    public static class AudioSourceExtensions
    {
        public static void ApplySettings(this AudioSource source, SoundSettings sourceSettings)
        {
            source.clip = sourceSettings.Clip;
            source.volume = sourceSettings.Volume;
            source.pitch = sourceSettings.Pitch;
            source.panStereo = sourceSettings.PanStereo;
            source.loop = sourceSettings.Loop;
        }
    }
}
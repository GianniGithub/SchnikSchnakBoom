using GrandDevs.ExtremeScooling.Common;
using System.Collections.Generic;
using UnityEngine;

namespace GrandDevs.ExtremeScooling
{
    public class SoundManager : IService, ISoundManager
    {
        public bool SoundOn { get; set; }

        private ILoadObjectsManager _loadObjectsManager;

        private List<SoundSource> _soundSources;

        private Transform _soundContainer;

        public void Dispose()
        {
        }

        public void Init()
        {
            _loadObjectsManager = GameClient.Get<ILoadObjectsManager>();

            _soundSources = new List<SoundSource>();
            SoundOn = true;
            _soundContainer = new GameObject("[Sound Container]").transform;
            MonoBehaviour.DontDestroyOnLoad(_soundContainer);
        }

        public void Update()
        {
            for (int i = 0; i < _soundSources.Count; i++)
            {
                if (_soundSources[i].IsSoundEnded())
                {
                    _soundSources[i].Dispose();
                    _soundSources.RemoveAt(i--);
                }

                if (SoundOn)
                    _soundSources[i].AudioSource.volume = 1f;
                else
                    _soundSources[i].AudioSource.volume = 0f;
            }

            
        }

        public void PlaySound(Enumerators.SoundType soundType, bool random = false, SoundParameters parameters = null)
        {
            string path = $"Sounds/{soundType}";
            AudioClip sound;

            if (random)
            {
                // todo improve
                sound = _loadObjectsManager.GetObjectByPath<AudioClip>($"{path}/{soundType}");
            }
            else
            {
                sound = _loadObjectsManager.GetObjectByPath<AudioClip>(path);
            }

            SoundSource foundSameSource = _soundSources.Find(soundSource => soundSource.SoundType == soundType);

            if (foundSameSource != null)
            {
                foundSameSource.Dispose();
                _soundSources.Remove(foundSameSource);
            }

            _soundSources.Add(new SoundSource(_soundContainer, sound, soundType, parameters));
        }

        public void StopSound(Enumerators.SoundType soundType)
        {
            for (int i = 0; i < _soundSources.Count; i++)
            {
                if (_soundSources[i].SoundType == soundType)
                {
                    _soundSources[i].Dispose();
                    _soundSources.RemoveAt(i--);
                }
            }
        }

        class SoundSource
        {
            public GameObject SoundSourceObject { get; }
            public AudioClip Sound { get; }
            public AudioSource AudioSource { get; }
            public Enumerators.SoundType SoundType { get; }

            public SoundSource(Transform parent, AudioClip sound, Enumerators.SoundType soundType, SoundParameters parameters)
            {
                Sound = sound;
                SoundType = soundType;

                SoundSourceObject = new GameObject($"[Sound] - {SoundType} - {Time.time}");
                SoundSourceObject.transform.SetParent(parent);
                AudioSource = SoundSourceObject.AddComponent<AudioSource>();
                AudioSource.clip = Sound;

                if (parameters != null)
                {
                    AudioSource.volume = parameters.Volume;
                    AudioSource.loop = parameters.Loop;
                }

                AudioSource.Play();
            }

            public bool IsSoundEnded()
            {
                return !AudioSource.loop && !AudioSource.isPlaying;
            }

            public void Dispose()
            {
                AudioSource.Stop();
                MonoBehaviour.Destroy(SoundSourceObject);
            }
        }

        public class SoundParameters
        {
            public bool Loop { get; set; } = false;
            public float Volume { get; set; } = 1f;
        }
    }
}
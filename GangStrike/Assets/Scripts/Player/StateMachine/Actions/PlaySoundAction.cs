using System.Threading.Tasks;
using System.Xml.Serialization;
using StateMachine.Attributes;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace StateMachine.Actions
{
    [XmlTag("PlaySoundAction")]
    public sealed class PlaySoundAction : ActionBase
    {
        [XmlAttribute("address")] public string Address { get; set; }
        [XmlAttribute("volume")] public float Volume { get; set; } = 1f;
        
        private AudioClip _clip;
        private AudioSource _audioSource;
        
        public override async Task Initialize(PlayerRoot owner)
        {
            _audioSource = owner.characterRoot.audioSource;
            Debug.Log(Address);
            _clip = await Addressables.LoadAssetAsync<AudioClip>(Address).Task;
            if (!_clip) Debug.LogError($"AudioClip not found: Resources/{Address}");
        }
        
        public override void Execute(PlayerRoot owner)
        {
            if (!_clip) return;
            _audioSource.PlayOneShot(_clip, Volume);
        }
    }
}
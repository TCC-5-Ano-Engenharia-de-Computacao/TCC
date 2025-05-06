using System.Xml.Serialization;
using StateMachine.Attributes;
using UnityEngine;

namespace StateMachine.Actions
{
    [XmlTag("MakeSoundAction")]
    public sealed class MakeSoundAction : ActionBase
    {
        [XmlAttribute("path")]   public string AudioPath { get; set; }
        [XmlAttribute("volume")] public float Volume { get; set; } = 1f;
        [XmlAttribute("loop")]   public bool Loop   { get; set; }
        private AudioClip _clip;
        public override void Initialize(PlayerRoot owner)
        {
            _clip = Resources.Load<AudioClip>(AudioPath);
            if (!_clip) Debug.LogError($"AudioClip not found: Resources/{AudioPath}");
        }
        public override void Execute(PlayerRoot owner)
        {
            if (!_clip) return;
            var src = owner.GetComponent<AudioSource>() ?? owner.gameObject.AddComponent<AudioSource>();
            src.clip = _clip;
            src.volume = Volume;
            src.loop = Loop;
            src.Play();
        }
    }
}
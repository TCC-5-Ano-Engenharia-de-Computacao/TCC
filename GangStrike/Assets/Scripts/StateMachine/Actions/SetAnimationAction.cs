using System.Threading.Tasks;
using System.Xml.Serialization;
using StateMachine.Attributes;
using UnityEngine;

namespace StateMachine.Actions
{
    [XmlTag("SetAnimationAction")]
    public sealed class SetAnimationAction : ActionBase
    {
        [XmlAttribute("path")]   public string SpriteSheetPath { get; set; }
        [XmlAttribute("frames")] public int FrameCount { get; set; }

        private Sprite[] _frames;
        public override async Task Initialize(PlayerRoot owner)
        {
            var texture = Resources.Load<Texture2D>(SpriteSheetPath);
            if (!texture)
            {
                Debug.LogError($"SpriteSheet not found: Resources/{SpriteSheetPath}");
                return;
            }
            int w = texture.width / FrameCount;
            int h = texture.height;
            _frames = new Sprite[FrameCount];
            for (int i = 0; i < FrameCount; i++)
            {
                var rect = new Rect(i * w, 0, w, h);
                _frames[i] = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f), 100f);
            }
        }
        public override void Execute(PlayerRoot owner)
        {
            if (_frames == null || _frames.Length == 0) return;
            var sr = owner.GetComponent<SpriteRenderer>();
            if (sr) sr.sprite = _frames[0];
        }
    }
}
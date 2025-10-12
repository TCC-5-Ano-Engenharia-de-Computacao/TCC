using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Xml.Linq;
using StateMachine;
using UnityEngine;
using UnityEngine.AddressableAssets;
// sua ActionBase (Execute sem parâmetros)

namespace Player.NewStateMachine.Actions
{
    public sealed class PlayAnimationClipAction : ActionBase
    {
        [SerializeField] private string animationClipAddress;
        [SerializeField] private Animator animator;
        [SerializeField] private AnimatorOverrideController overrideController;
        [SerializeField] private AnimationClip animationClip;
        [SerializeField] private float speed = 1f;
        
        public override void Execute()
        {
            animator.runtimeAnimatorController = overrideController;
            animator.speed = speed;
            animator.Play("CurrentState", 0, 0f);
            
        }

        public static async Task<ActionBase> ConstructFromXmlAsync(XElement node, Transform parent, PlayerRoot player)
        {
            var go = new GameObject(nameof(PlayAnimationClipAction));
            go.transform.SetParent(parent, false);

            var a = go.AddComponent<PlayAnimationClipAction>();
            a.animationClipAddress     = (string)node.Attribute("animationClipAddress") ?? string.Empty;
        

            a.animator = player.characterRoot.animator;
            
            a.speed =
                float.TryParse((string)node.Attribute("speed"),
                    NumberStyles.Float,
                    CultureInfo.InvariantCulture,
                    out var spd)
                    ? spd : a.speed;
        
            var baseController = a.animator.runtimeAnimatorController;
        
            var animationClip = await Addressables.LoadAssetAsync<AnimationClip>(a.animationClipAddress).Task;
        
            var overrideController = new AnimatorOverrideController(baseController);
       
            overrideController["Idle"] = animationClip;
            a.overrideController = overrideController;
            a.animationClip = animationClip;
        
            return a;
        }

    

        [RuntimeInitializeOnLoadMethod]
        private static void Register() =>
            ActionFactory.Register(nameof(PlayAnimationClipAction), ConstructFromXmlAsync);

        private void OnDestroy()
        {
            if (animationClip != null)
            {
                Addressables.Release(animationClip);
            }
        }
    }
}

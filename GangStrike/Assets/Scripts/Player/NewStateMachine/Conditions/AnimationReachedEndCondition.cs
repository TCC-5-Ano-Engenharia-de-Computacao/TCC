using System.Threading.Tasks;
using System.Xml.Linq;
using Player.NewStateMachine.Conditions;
using StateMachine;
using UnityEngine;

public sealed class AnimationReachedEndCondition : ConditionBase
{
    [SerializeField] private Animator animator;
    [SerializeField] private string   clipName = string.Empty; // opcional

    public override bool Evaluate()
    {
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // 1) pega o primeiro clip atual (o de maior peso)
        var clips = animator.GetCurrentAnimatorClipInfo(0);
        if (clips.Length == 0) return false;

        var currentClip = clips[0].clip;   // AnimationClip
        bool correctClip =
            string.IsNullOrEmpty(clipName) ||
            currentClip.name == clipName;   // compara pelo nome do clip real

        // 2) tempo transcorrido do state em segundos
        float elapsedSec = stateInfo.normalizedTime * currentClip.length;

        // chegou no final do clip?
        bool ended = elapsedSec >= currentClip.length;

        // debug opcional
        Debug.Log($"clip:{currentClip.name} t={elapsedSec:F3}/{currentClip.length:F3}");

        return correctClip && ended;
    }

    public static Task<ConditionBase> ConstructFromXmlAsync(
        XElement node, Transform parent, PlayerRoot player)
    {
        var go = new GameObject(nameof(AnimationReachedEndCondition));
        go.transform.SetParent(parent, false);

        var c       = go.AddComponent<AnimationReachedEndCondition>();
        c.animator  = player.characterRoot.animator;
        c.clipName  = (string?)node.Attribute("clipName") ?? string.Empty;
        return Task.FromResult<ConditionBase>(c);
    }

    [RuntimeInitializeOnLoadMethod]
    private static void Register() =>
        ConditionFactory.Register(nameof(AnimationReachedEndCondition), ConstructFromXmlAsync);
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Player.NewStateMachine.Actions; // sua ActionBase (Execute sem parâmetros)
using StateMachine;

public sealed class SetCurrentStateByAddressAction : ActionBase
{
    [SerializeField] private string address;
    [SerializeField] private string placeholder = "Idle"; // nome do CLIP placeholder no controller base

    [SerializeField] private Animator animator;
    private RuntimeAnimatorController baseController;

    private AsyncOperationHandle<AnimationClip> clipHandle;
    private AnimationClip loadedClip;

    private AnimatorOverrideController preparedOverride;
    private bool ready;

    public override void Execute()
    {
        if (!ready)
        {
            Debug.LogWarning($"[{nameof(SetCurrentStateByAddressAction)}] Ainda não preparado (address={address}).");
            return;
        }
        animator.runtimeAnimatorController = preparedOverride;
    }

    public static async Task<ActionBase> ConstructFromXmlAsync(XElement node, Transform parent, PlayerRoot player)
    {
        var go = new GameObject(nameof(SetCurrentStateByAddressAction));
        go.transform.SetParent(parent, false);

        var a = go.AddComponent<SetCurrentStateByAddressAction>();
        a.address     = (string)node.Attribute("address") ?? string.Empty;
        a.placeholder = (string)node.Attribute("placeholder") ?? "CurrentState";

        // Resolve Animator (ajuste se você expõe um Animator específico no PlayerRoot)
        a.animator = player != null ? player.GetComponentInChildren<Animator>() : null;
        if (a.animator == null)
        {
            Debug.LogError($"[{nameof(SetCurrentStateByAddressAction)}] Animator não encontrado no PlayerRoot.");
            return a;
        }

        a.baseController = a.animator.runtimeAnimatorController;
        if (a.baseController == null)
        {
            Debug.LogError($"[{nameof(SetCurrentStateByAddressAction)}] Animator sem RuntimeAnimatorController.");
            return a;
        }

        if (string.IsNullOrEmpty(a.address))
        {
            Debug.LogError($"[{nameof(SetCurrentStateByAddressAction)}] 'address' vazio no XML.");
            return a;
        }

        // Carrega o clip via Addressables
        a.clipHandle = Addressables.LoadAssetAsync<AnimationClip>(a.address);
        await a.clipHandle.Task;
        if (a.clipHandle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError($"[{nameof(SetCurrentStateByAddressAction)}] Falha ao carregar AnimationClip em '{a.address}'.");
            return a;
        }
        a.loadedClip = a.clipHandle.Result;

        // Prepara o override trocando APENAS o placeholder
        var ovr   = new AnimatorOverrideController(a.baseController);
       
        ovr["Idle"] = a.loadedClip;
        a.preparedOverride = ovr;
        a.ready = true;

        return a;
    }

    private void OnDisable()
    {
        // volta pro controller base se quiser desfazer ao sair do estado
        if (animator != null && baseController != null)
            animator.runtimeAnimatorController = baseController;
    }

    private void OnDestroy()
    {
        if (clipHandle.IsValid())
            Addressables.Release(clipHandle);
    }

    [RuntimeInitializeOnLoadMethod]
    private static void Register() =>
        ActionFactory.Register(nameof(SetCurrentStateByAddressAction), ConstructFromXmlAsync);
}

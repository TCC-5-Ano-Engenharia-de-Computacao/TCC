using System;
using System.Collections.Generic;
using System.Linq;
using Input;
using UnityEngine;

/// <summary>
/// Input Buffer sem corrotinas nem dicionários.
/// Um “gerente” externo deve chamar:
///   • <see cref="ProcessFrame"/>  – logo no começo do frame (Update do GameManager)  
///   • <see cref="EndFrame"/>      – logo no fim do frame (LateUpdate do GameManager)
/// 
/// Funcionamento:
/// 1. <see cref="RegisterInput"/> marca <c>pendingStart = true</c>.
/// 2. <see cref="ProcessFrame"/>:
///    • Se <c>pendingStart</c> → inicia o primeiro frame (<c>isFrameStart = true</c>).
///    • Se <c>isFrameStart</c> → converte para lingering e começa contagem.
///    • Caso contrário → desconta <c>timeRemaining</c>.
/// 3. Métodos de consulta (<see cref="StartedThisFrame"/> e <see cref="IsLingering"/>) 
///    marcam <c>triedConsume = true</c>.
/// 4. <see cref="EndFrame"/> remove inputs que foram consumidos no mesmo frame.
/// </summary>
public sealed class NewInputBuffer : MonoBehaviour
{
    // -------------------------------------------------------------- CONFIG
    [SerializeField] private float inputLingerDuration = 0.5f;

    // ---------------------------------------------------------------- DATA
    [Serializable]
    public class InputEntry
    {
        public string inputName;

        [Tooltip("True apenas no frame em que o botão foi pressionado.")]
        public bool isFrameStart;

        [Tooltip("Tempo restante em segundos. > 0 significa que o input está bufferizado.")]
        public float timeRemaining;

        // ---- flags internas ----
        [Tooltip("Registrado neste frame; aguarda ProcessFrame para virar isFrameStart.")]
        public bool pendingStart;

        [Tooltip("Algum método de consulta observou este input neste frame.")]
        public bool triedConsume;
    }

    [SerializeField] private List<InputEntry> _entries = new();

    // ---------------------------------------------------------------- REF
    private InputController _inputController;

    // ============================================================ UNITY
    private void Awake()
    {
        _inputController = FindFirstObjectByType<InputController>();
    }

    private void OnEnable()
    {
        if (_inputController != null)
            _inputController.inputPerformedEvent.AddListener(RegisterInput);
    }

    private void OnDisable()
    {
        if (_inputController != null)
            _inputController.inputPerformedEvent.RemoveListener(RegisterInput);
    }

    private void Update()
    {
        ProcessFrame(Time.deltaTime); // começo do frame
        EndFrame();                  // fim do frame
    }

    // ====================================================== PUBLIC API
    public void RegisterInput(string inputName)
    {
        var e = FindEntry(inputName) ?? AddEntry(inputName);

        e.pendingStart  = true;  // aguardará ProcessFrame
        e.isFrameStart  = false; // ainda não iniciou
        e.timeRemaining = 0f;    // contagem começa depois
        e.triedConsume  = false; // ainda não foi consultado
    }

    /// <summary>Processa transição de estados e contagem-regressiva.  
    /// Deve ser chamado no início de cada frame.</summary>
    public void ProcessFrame(float deltaTime)
    {
        foreach (var e in _entries)
        {
            if (e.pendingStart)
            {
                // 1º frame após registro
                e.pendingStart = false;
                e.isFrameStart = true;  // true durante TODO o frame
            }
            else if (e.isFrameStart)
            {
                // 2º frame: converte para lingering
                e.isFrameStart  = false;
                e.timeRemaining = inputLingerDuration;
            }
            else if (e.timeRemaining > 0f)
            {
                // Frames subsequentes: desconta lingering
                e.timeRemaining -= deltaTime;
                if (e.timeRemaining < 0f) e.timeRemaining = 0f;
            }
        }
    }

    /// <summary>Finaliza o frame. Remove entradas que foram consultadas.</summary>
    public void EndFrame()
    {
        foreach (var e in _entries.Where(x => x.triedConsume))
        {
            e.isFrameStart  = false;
            e.timeRemaining = 0f;
            e.triedConsume  = false;
        }
    }

    // ----------------------------- CONSULTAS (com marcação de consumo)
    public bool StartedThisFrame(string name)
    {
        var e = FindEntry(name);
        if (e is { isFrameStart: true })
        {
            e.triedConsume = true;
            return true;
        }
        return false;
    }

    public bool IsLingering(string name)
    {
        var e = FindEntry(name);
        if (e is { timeRemaining: > 0f })
        {
            e.triedConsume = true;
            return true;
        }
        return false;
    }

    // ----------------------------- UTILIDADES
    public void Clear()
    {
        foreach (var e in _entries)
        {
            e.pendingStart  = false;
            e.isFrameStart  = false;
            e.timeRemaining = 0f;
            e.triedConsume  = false;
        }
    }

    // ====================================================== INTERNOS
    private InputEntry? FindEntry(string name) => _entries.Find(x => x.inputName == name);

    private InputEntry AddEntry(string name)
    {
        var e = new InputEntry { inputName = name };
        _entries.Add(e);
        return e;
    }
}

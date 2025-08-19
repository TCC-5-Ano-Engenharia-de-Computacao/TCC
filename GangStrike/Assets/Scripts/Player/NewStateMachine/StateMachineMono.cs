using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Player.NewStateMachine.Runtime;
using StateMachine;
using UnityEngine;

namespace Player.NewStateMachine
{
    public class StateMachineMono : MonoBehaviour
    {
        [SerializeField] private string initialStateId;

        [SerializeField] private StateMono _current;
        private PlayerRoot _player;

        private void Update()
        {
            if (_current == null || _player == null) return;

            _current.Tick();
            var next = _current.TryGetValidTransition();
            if (next != null) SetState(next);
        }

        private void SetState(string id)
        {
            var next = FindChildState(id);
            if (next == null)
            {
                Debug.LogError($"State '{id}' não encontrado entre os filhos de '{name}'. Certifique-se que o GameObject filho se chama exatamente '{id}'.");
                return;
            }

            _current?.OnLeave();
            _current = next;

            var jump = _current.OnBeforeEnterAndCheckImmediate();
            if (jump != null) { SetState(jump); return; }

            _current.OnEnter();
        }

        /// <summary>
        /// Procura SOMENTE nos filhos diretos por um GameObject cujo nome == id e retorna o StateMono.
        /// </summary>
        private StateMono FindChildState(string id)
        {
            // Se o estado for filho direto e o nome bater, dá pra usar Transform.Find(id)
            var t = transform.Find(id);
            if (t == null) return null;

            var state = t.GetComponent<StateMono>();
            if (state == null)
            {
                Debug.LogError($"GameObject '{id}' encontrado, mas sem componente StateMono.");
            }
            return state;
        }

        public static async Task<StateMachineMono> ConstructFromXmlAsync(XElement node, Transform parent, PlayerRoot player)
        {
            // <StateMachine initialState="Idle">...</StateMachine>
            var go = new GameObject("StateMachine");
            go.transform.SetParent(parent, false);

            var sm = go.AddComponent<StateMachineMono>();
            sm.initialStateId = (string)node.Attribute("initialState");
            sm._player = player;

            // Cria cada <State> como FILHO do GO da state machine
            // GARANTA que StateMono.ConstructFromXmlAsync nomeie o GO do estado com o ID do estado.
            var tasks = node.Elements("State")
                            .Select(sn => StateMono.ConstructFromXmlAsync(sn, go.transform, player));
            var created = await Task.WhenAll(tasks);

            // Se por algum motivo StateMono não nomear, forçamos o nome do GO = Id
            foreach (var s in created)
            {
                if (s == null) continue;
                if (s.gameObject.name != s.Id) s.gameObject.name = s.Id;
            }

            if (!string.IsNullOrEmpty(sm.initialStateId))
                sm.SetState(sm.initialStateId);
            else
                Debug.LogError("initialState não definido no XML.");

            return sm;
        }
    }
}

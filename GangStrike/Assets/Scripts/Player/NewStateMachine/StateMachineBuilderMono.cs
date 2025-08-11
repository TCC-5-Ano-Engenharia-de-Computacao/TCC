using System.Xml.Linq;
using StateMachine;
using UnityEngine;

namespace Player.NewStateMachine
{
    public class StateMachineBuilderMono : MonoBehaviour
    {
        [Header("Fonte do XML")]
        [SerializeField] private TextAsset xml;

        [Header("Dependências")]
        [SerializeField] private PlayerRoot player;

        [Header("Saída (instância criada)")]
        public StateMachineMono instance;

        private async void Start()
        {
            if (xml == null)
            {
                Debug.LogError("[StateMachineBuilderMono] TextAsset XML não atribuído.");
                return;
            }
            if (player == null)
            {
                player = GetComponentInParent<PlayerRoot>();
                if (player == null)
                {
                    Debug.LogError("[StateMachineBuilderMono] PlayerRoot não encontrado.");
                    return;
                }
            }

            var xdoc = XDocument.Parse(xml.text);
            var root = xdoc.Root; // <StateMachine/>
            instance = await StateMachineMono.ConstructFromXmlAsync(root, transform, player);
            StateMachineMono temp = instance;
        }
    
    }
}
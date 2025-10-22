using System.Xml.Linq;
using StateMachine;
using UnityEngine;
using System.IO;

namespace Player.NewStateMachine
{
    public class StateMachineBuilderMono : MonoBehaviour
    {
        //[Header("Fonte do XML")]
        //[SerializeField] private TextAsset xml;

        [Header("Dependências")]
        [SerializeField] private PlayerRoot player;

        [Header("Saída (instância criada)")]
        public StateMachineMono instance;

        private async void Start()
        {
            /*if (xml == null)
            {
                Debug.LogError("[StateMachineBuilderMono] TextAsset XML não atribuído.");
                return;
            }*/
            if (player == null)
            {
                player = GetComponentInParent<PlayerRoot>();
                if (player == null)
                {
                    Debug.LogError("[StateMachineBuilderMono] PlayerRoot não encontrado.");
                    return;
                }
            }
            
            string buildFolder = Directory.GetParent(Application.dataPath).FullName;
            string filePath = Path.Combine(buildFolder, "statemachine.xml");

            if (File.Exists(filePath))
            {
                string conteudo = File.ReadAllText(filePath);
                var xdoc = XDocument.Parse(conteudo);
                var root = xdoc.Root; // <StateMachine/>
                instance = await StateMachineMono.ConstructFromXmlAsync(root, transform, player);
                StateMachineMono temp = instance;
            }
            else
            {
                Debug.LogWarning("Arquivo não encontrado em: " + filePath);
            }
        }
    
    }
}
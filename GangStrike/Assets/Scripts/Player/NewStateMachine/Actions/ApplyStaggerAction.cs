using System.Globalization;
using System.Threading.Tasks;
using System.Xml.Linq;
using StateMachine;
using UnityEngine;

namespace Player.NewStateMachine.Actions
{
    public class ApplyStaggerAction : ActionBase
    {

        public override void Execute()
        {
            Debug.Log("ApplyStagger");
        }

        public static async Task<ActionBase> ConstructFromXmlAsync(XElement node, Transform parent, PlayerRoot player)
        {
            var go = new GameObject(nameof(ApplyStaggerAction));
            go.transform.SetParent(parent, false);

            var a = go.AddComponent<ApplyStaggerAction>();
           
            return a;
        }
        
        [RuntimeInitializeOnLoadMethod]
        private static void Register() =>
            ActionFactory.Register(nameof(ApplyStaggerAction), ConstructFromXmlAsync);
    }
}
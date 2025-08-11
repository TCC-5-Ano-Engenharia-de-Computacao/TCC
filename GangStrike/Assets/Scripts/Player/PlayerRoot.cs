using System.Threading.Tasks;
using Input;
using Player.NewStateMachine;
using UnityEngine;

namespace StateMachine
{
    public class PlayerRoot : MonoBehaviour
    {
        public  CharacterRoot characterRoot;
        public StateMachineMono stateMachine;
        public InputRoot inputRoot;

        public async Task Initialize()
        {
            throw new System.NotImplementedException();
        }

        public void ProcessInputs()
        {
            throw new System.NotImplementedException();
        }

        public void ProcessConditions()
        {
            throw new System.NotImplementedException();
        }

        public void ProcessActions()
        {
            throw new System.NotImplementedException();
        }

        public void ProcessCleaning()
        {
            throw new System.NotImplementedException();
        }
    }
}
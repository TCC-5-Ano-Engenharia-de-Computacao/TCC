using System;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    public class PlayerManager : MonoBehaviour
    {
        public List<PlayerRoot> players = new List<PlayerRoot>();

        //async start method to initialize all players
        private async void Start()
        {
            foreach (var player in players)
            {
                try
                {
                    await player.Initialize();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error initializing player {player.name}: {e.Message}");
                }
            }
        }
        
        private void Update()
        {
            foreach (var player in players)
            {
                player.ProcessInputs();
            }

            foreach (var player in players)
            {
                player.ProcessConditions();
            }
            
            foreach (var player in players)
            {
                player.ProcessActions();
            }

            foreach (var player in players)
            {
                player.ProcessCleaning();
            }
        }
    }
}
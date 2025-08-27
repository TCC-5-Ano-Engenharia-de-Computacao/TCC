using System;
using StateMachine;
using UnityEngine;
using UnityEngine.Serialization;

public class SideSwapper : MonoBehaviour
{
    [SerializeField] private Transform characterRoot;
    [SerializeField] private bool startSwapped;
    [HideInInspector] public bool swapped;

    private Transform enemyPlayer;

    private void Start()
    {
        FindEnemy();
        
        if (startSwapped)
        {
            SwapSide();
        }
    }

    private void FindEnemy()
    {
        var playerRootList = Transform.FindObjectsByType<CharacterRoot>(FindObjectsSortMode.None);
        foreach (var cr in playerRootList)
        {
            if (cr.transform != characterRoot)
            {
                enemyPlayer = cr.transform;
            }
        }
        if (enemyPlayer == null)
        {
            Debug.LogWarning("Transform do inimigo nao encontrado");
        }
    }
    
    public void UpdateSideSwap()
    {
        if ((!swapped && enemyPlayer.position.x < characterRoot.position.x) || (swapped && enemyPlayer.position.x > characterRoot.position.x))
        {
            SwapSide();
        }
    }

    private void SwapSide()
    {
        Debug.Log("SWAP");
        swapped = !swapped;
        var localScale = characterRoot.transform.localScale;
        localScale.x *= -1;
        characterRoot.transform.localScale = localScale;
    }
}

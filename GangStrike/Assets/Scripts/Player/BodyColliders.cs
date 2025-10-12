using System;
using System.Collections.Generic;
using UnityEngine;

public class BodyColliders : MonoBehaviour
{
    private Dictionary<string, Collider2D> attackCollidersDict = new();
    private Collider2D activeCollider;

    public Collider2D GetActiveCollider()
    {
        return activeCollider;
    }
    
    public void SetActiveCollider(string colliderName)
    {
        var col = GetAttackColliderByName(colliderName);
        
        if (activeCollider != col)
        {
            if (activeCollider is not null)
                activeCollider.gameObject.SetActive(false);
            activeCollider = col;
            col.gameObject.SetActive(true);
        }
    }

    public Collider2D GetAttackColliderByName(string colliderName)
    {
        return attackCollidersDict.GetValueOrDefault(colliderName)
            ?? throw new Exception($"No Collider2D found with the name '{colliderName}'.");
    }
    
    private void Awake()
    {
        foreach (Transform child in transform)
        {
            var col = child.GetComponent<Collider2D>();
            if (col != null)
            {
                attackCollidersDict[child.gameObject.name] = col;
            }
            else
            {
                Debug.LogError($"GameObject '{child.gameObject.name}' does not have a Collider2D component.");
            }
        }
    }
}

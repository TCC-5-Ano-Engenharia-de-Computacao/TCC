using System;
using System.Collections.Generic;
using UnityEngine;

public class AttackTriggers : MonoBehaviour
{
    private Dictionary<string, Collider2D> attackCollidersDict = new();

    public Collider2D GetAttackColliderByName(string hitTag)
    {
        return attackCollidersDict.GetValueOrDefault(hitTag)
            ?? throw new Exception($"No Collider2D found with the name '{hitTag}'.");
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

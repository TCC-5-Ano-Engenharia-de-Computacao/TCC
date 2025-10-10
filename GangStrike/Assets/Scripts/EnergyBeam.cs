using Player;
using StateMachine;
using UnityEngine;

public class EnergyBeam : MonoBehaviour
{
    [SerializeField] private float speed = 4f;
    [SerializeField] private CharacterRoot attackerCharacterRoot;
    [SerializeField] private IncomingHitBuffer.Hit hit;

    public void InitializeEnergyBeam(CharacterRoot pAttackerCharacterRoot, IncomingHitBuffer.Hit pHit)
    {
        this.attackerCharacterRoot = pAttackerCharacterRoot;
        this.hit = pHit;
    }
    
    private void Update()
    {
        transform.Translate(Vector3.right * (speed * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var enemyCharacterRoot = other.GetComponent<CharacterRoot>();
        if(enemyCharacterRoot != null)
        {
            if (enemyCharacterRoot != attackerCharacterRoot)
            {
                enemyCharacterRoot.incomingHitBuffer.AddHitToBuffer(hit);
                Destroy(gameObject);
            }
        }
    }
}

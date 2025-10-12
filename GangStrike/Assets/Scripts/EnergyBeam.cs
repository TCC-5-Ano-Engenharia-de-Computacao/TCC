using Player;
using StateMachine;
using UnityEngine;

public class EnergyBeam : MonoBehaviour
{
    [SerializeField] private float speed = 4f;
    [SerializeField] private CharacterRoot attackerCharacterRoot;
    [SerializeField] private IncomingHitBuffer.Hit hit;
    [SerializeField] private Animator anim;
    [SerializeField] private float lifetime = 5f;
    
    private bool flagStopMoving;
    private float lifeTimer;

    public void InitializeEnergyBeam(CharacterRoot pAttackerCharacterRoot, IncomingHitBuffer.Hit pHit, float pSpeed)
    {
        this.attackerCharacterRoot = pAttackerCharacterRoot;
        this.hit = pHit;
        this.speed = pSpeed;
    }
    
    private void Update()
    {
        if (!flagStopMoving)
        {
            transform.Translate(Vector3.right * (speed * Time.deltaTime));
        }
        lifeTimer += Time.deltaTime;
        if (lifeTimer >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var enemyCharacterRoot = other.GetComponent<CharacterRoot>();
        if(enemyCharacterRoot != null)
        {
            if (enemyCharacterRoot != attackerCharacterRoot)
            {
                enemyCharacterRoot.incomingHitBuffer.AddHitToBuffer(hit);
                anim.SetTrigger("Explode");
                flagStopMoving = true;
            }
        }
    }

    private void DestroyEnergyBeam() // Chamado no fim da animação exploding (por Animation Event)
    {
        Destroy(gameObject);
    }
}

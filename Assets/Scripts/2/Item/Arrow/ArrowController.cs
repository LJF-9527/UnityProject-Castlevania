using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private string targetLayerName = "Player";

    [SerializeField] private float  xVelocity;
    private Rigidbody2D rb;

    [SerializeField] private bool canMove=true;
    [SerializeField] private bool flipped;

    private CharacterStats myStats;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void SetupArrow(float _speed,CharacterStats _myStats)
    {
        xVelocity = _speed;
        if(xVelocity<0) transform.Rotate(0, 180, 0);
        myStats = _myStats;
    }
    private void Update()
    {
        if (canMove)
            rb.velocity=new Vector2(xVelocity,rb.velocity.y);
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(targetLayerName))
        {
            //collision.GetComponent<CharacterStats>()?.TakeDamage(damage);
            
            myStats.DoDamage(collision.GetComponent<CharacterStats>());

            StuckInto(collision);
        }
        else if(collision.gameObject.layer==LayerMask.NameToLayer("Ground"))
        {
            StuckInto(collision); 
        }
        
    }

    private void StuckInto(Collider2D collision)
    {
        canMove = false;
        GetComponentInChildren<ParticleSystem>().Stop();
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = collision.transform;
        int dir;
        if(flipped) { dir =- 1; } else { dir=1;}
        transform.position = new Vector2(transform.position.x + (0.25f*dir), transform.position.y);
        GetComponent<Collider2D>().enabled = false;

        Destroy(gameObject, Random.Range(5,7));
    }

    public void FlipArrow()
    {
        if(flipped) { return; }

        xVelocity = xVelocity * -1;
        flipped = true;
        transform.Rotate(0, 180, 0);
        targetLayerName = "Enemy";
    }
}

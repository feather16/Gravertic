using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Entity
{
    [SerializeField] private float SPEED; 
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity *= SPEED / rigidbody.velocity.magnitude;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject obj = collision.collider.gameObject;

        // Entity‚©‚ÂPlayer‚Å–³‚¯‚ê‚Î‚È‚ç‚Î
        Entity entity;
        if(obj.name != "Player" && obj.TryGetComponent(out entity))
        {
            entity.Damaged(1);
        }

        Damaged();
    }
}

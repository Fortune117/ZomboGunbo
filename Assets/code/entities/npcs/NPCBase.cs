using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBase : Entity {


    protected float turnRate { get; set; }
   
    protected override void Initialise()
    {

        canDie = true;
        mass = 80;
        friction = 0.1F;
        turnRate = 0.05F;

    }

    protected override void ThinkInternal()
    {

    }

    protected override void Think()
    {
        Transform plyTransform = FindObjectOfType<Player>().transform;
        AimAtPoint(plyTransform.position);
        velocity = (plyTransform.position - transform.position).normalized*15;
    }

    protected void AimAtPoint( Vector2 point )
    {
        Vector2 dir = point - (Vector2)transform.position;

        float ang = -Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg + 95;

        transform.rotation = Quaternion.Euler(0, 0, Mathf.LerpAngle(transform.eulerAngles.z, ang, turnRate));
    }

    public override void Die()
    {
        DoOnDeath();
        Destroy(gameObject);
    }
}

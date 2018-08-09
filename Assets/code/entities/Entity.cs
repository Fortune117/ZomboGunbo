using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))] //All entities will be required to have a collider. We may not use it, but it should be there.
public abstract class Entity : MonoBehaviour {

    public Vector2 velocity { get; set; }

    public float friction { get; set; }//This is the coefficient of friction. For the purpose of simplicity, we're only going to use one value and not distinguish between static and kinetic friction.
    
    public float mass { get; set; } //This is mass in kg for the purpose of maths.

    public float angularVelocity { get; set; } //This is in radians per second. +ve is anticlockwise, -ve is clockwise. Since the game is top down, we don't need to worry about any other kinds of rotation.

    public float angularFriction { get; set; } //Basically, a value of 1 means that it doesn't slow down. A value of 0.99 means it slows down eventually. A value of 0 means it doesn't rotate.

    protected float gravity = 10;//This isn't used for calculating fall speed or anything, it's used for calculating forces for things like friction. Since this value doesn't make much sense in 2D topdown tbh, we'll make it private
    protected float velFrictionLimit = 0.04F;
     
    public float gravityMul { get; set; } //This multiplier effects the gravity this entity will experience. A higher multiplier means higher gravity.

    public float startHealth; //I'm not really sure if we need this one, but we're going to use it anyway. When the class is created, its health and maxHealth will be assigned this value.

    public float health { get; set; } //How much health the entity has.

    public float maxHealth { get; set; } //The maximum amount of health the entity can have.

    public bool canDie { get; set; } //Entities can't die by default. Yay? We'll probably overwrite this in just about everything.

    new public BoxCollider2D collider; //The collider we will be using for our entities. 

    public void AddVelocity(Vector2 vel) //Add to the objects velocity, without overwriting it.
    {
        velocity += vel;
    }

    public void SetVelocityX(float x)
    {
        velocity = new Vector2(x, velocity.y);
    }

    public void SetVelocityY(float y)
    {
        velocity = new Vector2(velocity.x, y);
    }

    public float GetSpeed()
    {
        return velocity.magnitude;
    }

    public void SetGravity(float n) //This actually sets the graviuty multiplier. 1 is normal, 0.5 is 50%, 1.5 is 150% etc.
    {
        gravityMul = n;
    }

    public float GetGravity() //Since the gravity const is the same for every entity, we're going to use this.
    {
        return gravityMul;
    }

    public int GetDisplayHealth() //This is what we will be using when we want to display health. Might as well let every entity have this, incase we want to use it.
    {
        return Mathf.FloorToInt(health);
    }

    public int GetDisplayMaxHealth()
    {
        return Mathf.FloorToInt(maxHealth);
    }

    protected virtual void OnDamageReceived()
    {

    }

    public virtual void TakeDamage() //This is going to be used when entities take damage. We're going to need to make a DamageInfo class, or similar, so that we can handle it properly.
    {
        OnDamageReceived(); //We call this before we apply the effects of taking damage (e.g. health decrease). Doing so lets us use it for resistances and similar, e.g. bullet vest, explosive vest, etc.
        ApplyDamage(); //Actually deal the damage to the entity.
    }

    protected virtual void ApplyDamage()
    {
        if (health <= 0)
        {
            if (canDie)
            {
                Die(); //One can argue it doesn't really make sense for all entities to 'die' but fuck it.
            }
        }
    }

    public virtual void DoOnDeath() // This is called when the entity is able to die and it drops to 0 hit points. This is called before it is removed, so you can still use it here.
    {

    }

    public virtual void Die() //Called when we want the entity to die. Will call DoOnDeath() first, then probably remove it. Need to add that.
    {
        DoOnDeath();
    }

    public virtual void DieNoCall() //Kill the entity without calling DoOnDeath(). Not really sure what this is useful for yet.
    {
        
    }

    public void Remove() //Remove the entity object. TODO :P
    {

    }

	protected void Start () {
        collider = GetComponent<BoxCollider2D>(); //All entities will need a collider, and we can make some useful functions for when entities load.

        health = maxHealth = startHealth;
        mass = 1;
        friction = 0;
        angularFriction = 1;
        angularVelocity = 0;
        gravityMul = 1;
        canDie = false;

        InitialiseInternal(); //We can use these so we don't need to be overwritng the start function.
        Initialise();
	}

    protected virtual void InitialiseInternal() {}
    protected virtual void Initialise() {}
	
	protected void Update () {
        ThinkInternal();    //These two methods will be used for every entity. 
        Think();            //We will need them for some base stuff that all entities will need.

        CheckCollisions(); //See if we're colliding with something.
        FrictionLogic(); //Apply friction to velocity.
        ApplyVelocity(); //Translate the doodido.
	}

    //We can leave these empty here though, as the base entity doesn't need them.
    protected virtual void ThinkInternal() { }
    protected virtual void Think() { }

    public virtual void CheckCollisions()
    {

    }

    protected virtual void FrictionLogic() //Apply the effects of friction onto the velocity of the entity.
    {

        if (velocity.magnitude > velFrictionLimit)
        {
            float frAccel = (gravity * gravityMul) * friction;
            velocity -= velocity.normalized * (frAccel) * Time.deltaTime; //Lose velocity in the direction they're moving.
        }
        else
        {
            velocity = Vector2.zero;
        }

        angularVelocity = angularVelocity * angularFriction; //Might change this later.
    }

    protected virtual void ApplyVelocity()
    {
        float rotSpeed = angularVelocity * Mathf.Rad2Deg * Time.deltaTime;
        transform.Rotate(new Vector3( 0, 0, rotSpeed));
        transform.Translate(velocity*Time.deltaTime, Space.World);
    }

}

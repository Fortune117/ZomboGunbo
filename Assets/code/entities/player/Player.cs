﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{

    public float moveSpeed { get; set; }
    public float accelTime { get; set; }
    public float sprintMul { get; set; }
    public float sprintAccelTime { get; set; }

    public float turnRate { get; set; }
    public float headTurnRate { get; set; }
    public float hipsTurnRate { get; set; }

    private bool hasInputs = false;
    private float moveAcceleration;
    private float sprintAcceleration;

    private float _trauma;
    public float trauma //This is our 'trauma' value, which represents how much stress our character is experiencing. Can use it for camera shake. Value [0,1].
    {
        get
        {
            return _trauma;
        }
        set
        {
            _trauma = Mathf.Clamp01(value);
        }
    }
    public float traumaShrinkTime = 1F; //How long it takes to lose all of our trauma in seconds.

    public GunBase gun;
    public float aimGap;

    public PlayerInventory inv;

    public PlayerHUD hud;

    protected override void Initialise()
    {

        canDie = true;
        mass = 80;
        friction = 5;
        moveSpeed = 12;
        turnRate = 0.079F; //2*Mathf.PI;
        headTurnRate = 0.25F;
        hipsTurnRate = 0.09F;
        accelTime = 0.3F;
        sprintMul = 1.5F;
        sprintAccelTime = 0.4F;
        trauma = 0;

        inv.invData.width = 10;
        inv.invData.height = 10;
        inv.invData.weightLimit = 10;

        moveAcceleration = (moveSpeed / accelTime);

    }

    protected override void ThinkInternal()
    {
        UpdateTrauma();
       // AimAtCursor(); //We're going to change this to be handled by the players animation class.
        MoveInputs();
        WeaponInputs();
        UIInputs();
    }

    protected void UpdateTrauma()
    {
        trauma -= traumaShrinkTime * Time.deltaTime;
    }

    public Vector2 GetAimDir() //This returns a normalized 2D vector in the direction of where the player is looking.
    {
        Vector3 mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 pos2D = new Vector2(mPos.x, mPos.y);

        Vector2 dir = (pos2D - (Vector2)transform.position).normalized;

        return dir;
    }

    public float GetAimDirAngles()
    {
        Vector2 dir = GetAimDir();
        return -Mathf.Atan2(dir.x, dir.y);
    }

    protected void AimAtCursor()
    { 
        Vector2 dir = GetAimDir();
        float ang = -Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg + 95;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.LerpAngle(transform.eulerAngles.z, ang, turnRate));
    }

    protected void MoveInputs()
    {

        Vector2 deltaVelocity = new Vector2(0, 0);

        hasInputs = false;
        if (Input.GetKey(KeyCode.A))
        {
            deltaVelocity += -Vector2.right * moveAcceleration * Time.deltaTime;
            Vector2 frAccel = (gravity * gravityMul) * friction * velocity.normalized;
            deltaVelocity += new Vector2(frAccel.x, 0) * Time.deltaTime;
            hasInputs = true;
        }
        
        if (Input.GetKey(KeyCode.D))
        {
            deltaVelocity += Vector2.right * moveAcceleration * Time.deltaTime;
            Vector2 frAccel = (gravity * gravityMul) * friction * velocity.normalized;
            deltaVelocity += new Vector2(frAccel.x, 0) * Time.deltaTime;
            hasInputs = true;
        }

        if (Input.GetKey(KeyCode.W))
        {
            deltaVelocity += Vector2.up * moveAcceleration * Time.deltaTime;
            Vector2 frAccel = (gravity * gravityMul) * friction * velocity.normalized;
            deltaVelocity += new Vector2(0, frAccel.y) * Time.deltaTime;
            hasInputs = true;
        }

        if (Input.GetKey(KeyCode.S))
        {
            deltaVelocity += -Vector2.up * moveAcceleration * Time.deltaTime;
            Vector2 frAccel = (gravity * gravityMul) * friction * velocity.normalized;
            deltaVelocity += new Vector2(0, frAccel.y) * Time.deltaTime;
            hasInputs = true;
        }

        if ((velocity + deltaVelocity).magnitude > moveSpeed)
        {
            float n = Mathf.Max(moveSpeed - velocity.magnitude, 0);
            deltaVelocity = deltaVelocity.normalized * n;
            velocity += deltaVelocity;
        }
        else
        {
            velocity += deltaVelocity;
        }

    }

    protected void WeaponInputs()
    {

        if (Input.GetMouseButton( 1 ))
        {
            if (!gun.isAiming)
            {
                gun.StartAiming();
            }
            else
            {
                if (gun.automatic && Input.GetMouseButton(0))
                {
                    gun.Shoot();
                }
                else if (Input.GetMouseButtonDown(0))
                {
                    gun.Shoot();
                }
            }
        }
        else if (gun.isAiming)
        {
            gun.StopAiming();
        }

        if (Input.GetKeyDown( KeyCode.R ) )
        {
            if (gun.CanAttemptFastReload())
            {
                gun.AttemptFastReload();
            }
            else if (gun.CanReload())
            {
                gun.StartReload();
            }
        }
    }

    protected void UIInputs()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inv.isOpen)
            {
                inv.Close();
            }
            else
            {
                inv.Open();
            }
        }
    }

    protected override void ApplyVelocity()
    {
        float rotSpeed = angularVelocity * Mathf.Rad2Deg * Time.deltaTime;
        transform.Rotate(new Vector3(0, 0, rotSpeed));
        transform.Translate(velocity * Time.deltaTime, Space.World);
    }
}
 
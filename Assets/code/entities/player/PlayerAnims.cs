using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnims : MonoBehaviour {

    public Player ply;
    public Transform head;
    public Transform torso;
    public Transform hips;

    public float maxHeadOffset; //Maximum difference between our heads rotation and our bodies rotation.
    public float maxHipsOffset; //Maximum difference between hip rotation and body rotation.

    public bool walkingBackwards = false;

    protected void Update()
    {
        UpdatePlayerTorso();
        UpdatePlayerHead();
        UpdatePlayerHips();
    }

    protected void UpdatePlayerTorso()
    {
        Vector2 dir = ply.GetAimDir();
        float ang = -Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg + 95;
        torso.rotation = Quaternion.Euler(0, 0, Mathf.LerpAngle(torso.eulerAngles.z, ang, ply.turnRate));
    }

    protected void UpdatePlayerHead()
    {
        Vector2 dir = ply.GetAimDir();
        float ang = -Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg + 95; //This is our target angle, but we're going to want to limit how far our head can rotate.
        float dAngle = Mathf.DeltaAngle(ang, torso.eulerAngles.z);

        if (Mathf.Abs(dAngle) > maxHeadOffset)
        {
            ang = torso.eulerAngles.z - Mathf.Sign(dAngle) * maxHeadOffset;
        }

        head.rotation = Quaternion.Euler(0, 0, Mathf.LerpAngle(head.eulerAngles.z, ang, ply.headTurnRate));
    }

    protected void UpdatePlayerHips() //We're going to use two animations for walking, one for walking forwards, one for walking backwards.
    {
        Vector2 vel = ply.velocity;
        Vector2 dir;
        if (vel.magnitude < 0.2)
        {
            dir = ply.GetAimDir();
        }
        else
        {
            dir = vel.normalized;
        }

        float ang = -Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg + 95; //This is our target angle, but we're going to want to limit how far our head can rotate.
        float dAngle = Mathf.DeltaAngle(ang, torso.eulerAngles.z);

        if (Mathf.Abs(dAngle) > maxHipsOffset)
        {
            //ang = torso.eulerAngles.z - Mathf.Sign(dAngle) * maxHipsOffset;
            ang = -ang;
            walkingBackwards = true;
        }
        else
        {
            walkingBackwards = false;
        }

        hips.rotation = Quaternion.Euler(0, 0, Mathf.LerpAngle(hips.eulerAngles.z, ang, ply.hipsTurnRate));
    }
}

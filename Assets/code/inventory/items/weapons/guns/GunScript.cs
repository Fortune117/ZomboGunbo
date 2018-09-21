using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Gun")]
public class GunScript : ScriptableObject {

    [Header("Firing and Damage")]
    public float damage;
    public bool automatic;
    public float roundsPerSecond;

    [Space]
    [Header("Reloading")]
    public int maxMagazineSize;
    public float reloadTime;
    public float fastReloadSuccessFraction;
    public bool canFastReload;
    public float fastReloadFailMultiplier;

    [Space]
    [Header("Cone of Fire")]
    public float fireConeStartAngle;
    public float fireConeEndAngle;
    public float aimTime;
    public float fireConeAnglePunchOnShot;
    public float fireConeDelayOnShot;

    [Space]
    [Header("Misc")]
    [Range(0,1)]
    public float traumaValueOnFire;

    [Space]
    [Header("Inventory Data")]
    public Vector2 inventoryDimensions;

    [Space]
    [Header("Prefabs")]
    public GameObject BulletTrailPrefab;

    [HideInInspector]
    public bool valid = false;

    public void OnValidate()
    {
        valid = false;
    }

}

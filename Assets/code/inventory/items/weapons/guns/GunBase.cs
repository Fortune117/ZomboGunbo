using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBase : ItemBase {
    
    public float damage { get; set; } //The amount of damage each bullet does.

    public float roundsPerSecond { get; set; } //How many times the gun fires per second.
    public bool automatic { get; set; }
    protected float fireDelay { get; set; }
    protected float nextShotTime; //We're going to use this to figure out how much time is between each shot. The rounds per second value is the readable/meaningful value, this one is just what we see.

    public string[] fireSounds;

    public int magazineSize { get; set; } //How many bullets are in the weapons magazine.
    public int maxMagazineSize { get; set; } //The maximum number of bullets in the weapons magazine.

    public float reloadTime { get; set; } //How long it takes to reload the weapon in seconds.
    public bool isReloading { get; set; }
    public float fastReloadSuccessFraction { get; set; }
    [HideInInspector]
    public float reloadFraction;
    [HideInInspector]
    public bool attemptedFastReload;
    [HideInInspector]
    public bool failedFastReload;
    protected float reloadStartTime;
    protected bool canFastReload;
    protected float fastReloadFailTimeMultiplier; //Add this fraction of the default reload time to the reload duration left on a failed fast reload.

    public float fireConeStartAngle { get; set; } //The angle of the starting cone of fire in degrees.
    public float fireConeEndAngle { get; set; } //The angle of the cone of fire after aiming for a little bit.
    public float aimTime { get; set; } //How long it takes to go from the Start Angle to the End Angle for firing the weapon.

    public float fireConeAnglePunchOnShot { get; set; } //How much our cone of fire angle should increase when we fire the weapon.
    public float traumaValueOnFire { get; set; } //How much trauma we add to the player when we fire the gun.

    public bool isAiming { get; set; } //This is whether or not the player is aiming the weapon.
    public float fireConeDelayOnShot { get; set; } //How long in seconds it should be before the cone of fire begins shrinking again after we fire a weapon.
    protected float fireConeDelayStartTime;
    protected float fireConeAngle; //The internal angle for when we are going to be firing the weapon.
    protected float fireConeAngleEasingValue;
    protected float aimStartTime;

    public float bulletEffectGap;
    public Transform BulletTrailPrefab;

    public Vector2 firePos;
    public Player ply;

    public GunScript gunData;

    protected override void Start()
    {

        base.Start();

        isAiming = false;
        isReloading = false;
        nextShotTime = 0;
        reloadStartTime = 0;
        attemptedFastReload = false;

        InitialiseGunData(); //Load our gun data.

    }

    public void OnValidate()
    {   
        if (gunData != null)
        {
            InitialiseGunData(); //Reload our gun data from our script.
            print("uggle wuggle");
        }

    }

    public void InitialiseGunData()
    {
        if(gunData != null)
        {
            damage = gunData.damage;
            automatic = gunData.automatic;
            roundsPerSecond = gunData.roundsPerSecond;
            fireDelay = 1 / roundsPerSecond;
            maxMagazineSize = gunData.maxMagazineSize;
            magazineSize = maxMagazineSize;
            reloadTime = gunData.reloadTime;
            fastReloadSuccessFraction = gunData.fastReloadSuccessFraction;
            canFastReload = gunData.canFastReload;
            fastReloadFailTimeMultiplier = gunData.fastReloadFailMultiplier;
            fireConeStartAngle = gunData.fireConeStartAngle;
            fireConeEndAngle = gunData.fireConeEndAngle;
            fireConeAngle = fireConeStartAngle;
            aimTime = gunData.aimTime;
            fireConeAnglePunchOnShot = gunData.fireConeAnglePunchOnShot;
            traumaValueOnFire = gunData.traumaValueOnFire;
            fireConeDelayOnShot = gunData.fireConeDelayOnShot;
            inventoryDimensions = gunData.inventoryDimensions;
            gunData.valid = true;
        }
    }

    //Aiming is what happens when the player holds down mouse 2, by default. This will allow us to do some fancy shit.
    public void StartAiming() //This function is called when the player starts aiming.
    {
        isAiming = true;
        fireConeAngle = fireConeStartAngle;
        aimStartTime = Time.time;
    }

    public void StopAiming() //This function is called when the player stops aiming.
    {
        isAiming = false;
    }

    //public float EaseFunc(float p)
    //{
    //    return Mathf.Sqrt((2 - p) * p);
    //}

    public float EaseFunc(float p) //We could use different ease functions for different weapons... Could be interesting.
    {
        float f = (p - 1); //This is the cubic ease out. I, uh, don't really know how it works.¯\_(ツ)_/¯
        return f * f * f + 1;
    }

    float CubeRoot(float d)
    {
        if (d < 0.0f)
        {
            return -Mathf.Pow(-d, 1f / 3f);
        }
        else
        {
            return Mathf.Pow(d, 1f / 3f);
        }
    }

    public float AntiEase(float p)
    {
       return CubeRoot( p - 1 ) + 1; //Nasty floating root hack. Oh well.
    }

    protected virtual void UpdateFireCone()
    {
        fireConeAngleEasingValue = EaseFunc((Mathf.Min((Time.time - aimStartTime) / aimTime, 1)));
        fireConeAngle = fireConeStartAngle - (fireConeStartAngle - fireConeEndAngle) * fireConeAngleEasingValue; //Slap in an easing function.
    }

    protected override void ItemThink()
    {
        DataThink();
        AimThink();
        ReloadThink();

    }

    protected void DataThink()
    {
        if (gunData != null && gunData.valid == false)
            InitialiseGunData();
    }

    protected void AimThink()
    {
        if (isAiming)
        {
            if (Time.time >= fireConeDelayStartTime + fireConeDelayOnShot)
            {
                UpdateFireCone();
            }

        }
    }

    protected void ReloadThink()
    {
        if (isReloading)
        {
            if (attemptedFastReload && failedFastReload)
            {
                if (Time.time > reloadStartTime + (reloadTime * fastReloadFailTimeMultiplier))
                {
                    FinishReload();
                }
                reloadFraction = (Time.time - reloadStartTime) / (reloadTime * fastReloadFailTimeMultiplier);
            }
            else
            {
                if (Time.time > reloadStartTime + reloadTime)
                {
                    FinishReload();
                }
                reloadFraction = (Time.time - reloadStartTime) / reloadTime;
            }
        }
    }

    protected void DoRecoil()
    {
        float desiredAng = Mathf.Min(fireConeAngle + fireConeAnglePunchOnShot, fireConeStartAngle);
        aimStartTime = Time.time - AntiEase((fireConeStartAngle - desiredAng) / (fireConeStartAngle - fireConeEndAngle)) * aimTime; //We're calculating the recoil like this so as to avoid having to directly change the fireConeAngle variable.
        fireConeDelayStartTime = Time.time;                                                                                         //In doing so, we can still get the effect of directly changing the angle value, but also get the effect of easing functions.
        UpdateFireCone(); //Do the recoil here because of the fire cone delay.
        aimStartTime += +fireConeDelayOnShot; //Add the delay after we update the recoil.

        ply.trauma += traumaValueOnFire;
    }


    public float GetFireConeAngle()
    {
        return fireConeAngle;
    }

    public virtual bool CanShoot()
    {
        return (Time.time > nextShotTime) && !isReloading && magazineSize > 0;
    }

    public void Shoot()
    {
        if (CanShoot())
        {
            float ang = ply.GetAimDirAngles() * Mathf.Rad2Deg + 90 + Random.Range(-0.5F, 0.5F) * fireConeAngle; //Pick a random angle somewhere in the fire cone.
            Effects( ang );
            FireBullet(ang);
            DoRecoil(); //Do this after we fire the bullet. It shouldn't technically matter, since the cone wont change until the next frame, but we'll put this here in case we change it.
            nextShotTime = Time.time + fireDelay;
            magazineSize -= 1; //Remove one bullet from the magazine.
        }
    }

    void Effects ( float angle )
    {
        GameObject.Instantiate(BulletTrailPrefab, ply.transform.position + (Vector3)ply.GetAimDir()*(ply.aimGap + bulletEffectGap), Quaternion.Euler(0, 0, angle));
        GameObject.FindObjectOfType<AudioManager>().Play(fireSounds[Random.Range(0, fireSounds.Length)]);
    }

    public void FireBullet( float angle )
    {

    }

    public virtual bool CanReload()
    {
        return (Time.time > nextShotTime) && (magazineSize < maxMagazineSize) && !isReloading;
    }

    public virtual void OnStartReload()
    {
    }

    public virtual void OnReloadFinished()
    {
    }

    public void FinishReload()
    {
        magazineSize = maxMagazineSize;
        isReloading = false;
        OnReloadFinished();
    }

    public void StartReload()
    {
        reloadFraction = 0;
        reloadStartTime = Time.time;
        isReloading = true;
        attemptedFastReload = false;
        OnStartReload();
        ply.hud.OnStartReloading();
    }

    public bool CanAttemptFastReload()
    {
        return canFastReload && !attemptedFastReload && isReloading;
    }

    public void AttemptFastReload()
    {
        if (Mathf.Abs( 0.5F-reloadFraction) <= fastReloadSuccessFraction/2)
        {
            ply.hud.OnFastReloadSuccess();
            FinishReload();
        }
        else
        {
            failedFastReload = true;
            ply.hud.OnFastReloadFail();
        }
        attemptedFastReload = true;
    }
}

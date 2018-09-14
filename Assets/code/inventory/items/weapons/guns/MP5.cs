using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MP5 : GunBase {

    protected override void ItemInitialiseInternal()
    {

        damage = 1F;
        roundsPerSecond = 30;
        magazineSize = maxMagazineSize = 30;
        reloadTime = 2;
        canFastReload = true;
        fastReloadSuccessFraction = 0.08F; //The fractional cutoff for our fast reload. i.e. 10% of the reload time allows for fast reload.
        fastReloadFailTimeMultiplier = 1.5F;
        fireConeStartAngle = 50; //60 degrees start angle.
        fireConeEndAngle = 10; //20 degrees end angle.
        fireConeAnglePunchOnShot = 1.0F;
        traumaValueOnFire = 0.18F;
        fireConeDelayOnShot = 0.08F;
        aimTime = 1.8F; //How long it takes to go from the start angle to end angle.
        isAiming = false;
        isReloading = false;
        automatic = true;

        invetoryDimensions = new Vector2(2, 2);

    }

}

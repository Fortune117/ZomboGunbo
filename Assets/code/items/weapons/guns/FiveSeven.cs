using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiveSeven : GunBase {

    protected override void ItemInitialiseInternal()
    {

        damage = 1F;
        roundsPerSecond = 8;
        magazineSize = maxMagazineSize = 20;
        reloadTime = 2;
        canFastReload = true;
        fastReloadSuccessFraction = 0.1F; //The fractional cutoff for our fast reload. i.e. 10% of the reload time allows for fast reload.
        fireConeStartAngle = 40; //60 degrees start angle.
        fireConeEndAngle = 8; //20 degrees end angle.
        fireConeAnglePunchOnShot = 2.5F;
        traumaValueOnFire = 0.15F;
        fireConeDelayOnShot = 0.12F;
        aimTime = 1.5F; //How long it takes to go from the start angle to end angle.
        isAiming = false;
        isReloading = false;
        automatic = true;

    }

}

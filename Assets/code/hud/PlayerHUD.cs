using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour {

    public Player ply;

    public Image aimBar1;
    private float bar1Length;

    public Image aimBar2;
    private float bar2Length;

    public Text ammoText;
    public Image reloadBarOutline; //The outline for the bar.
    public Image reloadBar; //The reload bar.
    public Image fastReloadThreshold; //The image we use for the fast reload success zone.
    public Image reloadProgress; //The little marker we use for progress along our reload.
    public float reloadBarShake; //Shake for the reload bar.
    public float reloadBarShakeOnFail; //How much shake we get when the bar fails.
    public float reloadBarMaxOffset; //The maximum offset for our shaking.

    public float colorFadeTime; //How long it takes to fade between colors.
    public Color defaultColor;
    public Color fastReloadSuccessColor;
    public Color fastReloadFailColor;
    public Color fastReloadThresholdDefaultColor;
    public Color fastReloadThresholdFailColor;
    public Color fastReloadProgressDefaultColor;
    public Color fastReloadProgressFailColor;

	// Use this for initialization
	void Start () {
        bar1Length = (aimBar1.sprite.bounds.size.x);
        bar2Length = (aimBar2.sprite.bounds.size.x);
    }

    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    public static Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }

    // Update is called once per frame
    void Update () {
        UpdateAimBars();
        UpdateAmmoText();
        UpdateReloadBar();

        UpdateInventory();
	}

    public void UpdateAimBars()
    {
        if (ply.gun.isAiming)
        {
            aimBar1.enabled = true;
            aimBar2.enabled = true;

            float bar1Angle = ply.GetAimDirAngles() * Mathf.Rad2Deg + 90 + ply.gun.GetFireConeAngle() / 2;
            aimBar1.transform.position = (Vector2)ply.transform.position + ply.GetAimDir()*ply.aimGap + DegreeToVector2(bar1Angle) * bar1Length / 4;
            aimBar1.transform.rotation = Quaternion.Euler(0, 0, bar1Angle);

            float bar2Angle = ply.GetAimDirAngles() * Mathf.Rad2Deg + 90 - ply.gun.GetFireConeAngle() / 2;
            aimBar2.transform.position = (Vector2)ply.transform.position + ply.GetAimDir()*ply.aimGap + DegreeToVector2(bar2Angle) * bar2Length / 4;
            aimBar2.transform.rotation = Quaternion.Euler(0, 0, bar2Angle);
        }
        else
        {
            aimBar1.enabled = false;
            aimBar2.enabled = false;
        }
    }

    public void UpdateAmmoText()
    {
        ammoText.text = ply.gun.magazineSize + "/" + ply.gun.maxMagazineSize;
    }

    public void OnStartReloading()
    {
        reloadBarOutline.color = defaultColor;
        reloadProgress.color = fastReloadProgressDefaultColor;
        fastReloadThreshold.color = fastReloadThresholdDefaultColor;
        reloadBarShake = 0;
    }

    public void OnFastReloadFail()
    {
        reloadBarOutline.color = fastReloadFailColor;
        reloadProgress.color = fastReloadProgressFailColor;
        fastReloadThreshold.color = fastReloadThresholdFailColor;
        reloadBarShake = reloadBarShakeOnFail;
    }

    public void OnFastReloadSuccess()
    {
        reloadBarOutline.color = fastReloadSuccessColor;
    }

    public void UpdateReloadBar()
    {
        if (ply.gun.isReloading)
        {
            reloadBarOutline.enabled = true;
            reloadBar.enabled = true;
            fastReloadThreshold.enabled = true;
            reloadProgress.enabled = true;

            reloadBarOutline.transform.localPosition = new Vector2( 0, -70) + Vector2.Lerp(reloadBarOutline.transform.position, new Vector2(Random.Range(-reloadBarMaxOffset, reloadBarMaxOffset), Random.Range(-reloadBarMaxOffset, reloadBarMaxOffset)) * (reloadBarShake * reloadBarShake), 0.08F); //This needs fixing, but it's fine for now.
            reloadProgress.transform.localPosition = new Vector2(reloadBar.rectTransform.rect.width * ply.gun.reloadFraction - reloadBar.rectTransform.rect.width/2, 0);
            fastReloadThreshold.rectTransform.sizeDelta = new Vector2(reloadBar.rectTransform.sizeDelta.x * ply.gun.fastReloadSuccessFraction, fastReloadThreshold.rectTransform.sizeDelta.y);
        }
        else
        {
            reloadBarOutline.enabled = false;
            reloadBar.enabled = false;
            fastReloadThreshold.enabled = false;
            reloadProgress.enabled = false;
        }

        reloadBarShake -= reloadBarShake * Time.deltaTime;
    }

    public void UpdateInventory()
    {

    }
}

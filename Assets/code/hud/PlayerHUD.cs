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
		if (ply.gun.isAiming)
        {
            aimBar1.CrossFadeAlpha(1, 0, true);
            aimBar2.CrossFadeAlpha(1, 0, true);

            float bar1Angle = ply.GetAimDirAngles() * Mathf.Rad2Deg + 90 + ply.gun.GetFireConeAngle() / 2;
            aimBar1.transform.position = (Vector2)ply.transform.position + DegreeToVector2(bar1Angle)*bar1Length/4;
            aimBar1.transform.rotation = Quaternion.Euler(0, 0, bar1Angle);

            float bar2Angle = ply.GetAimDirAngles() * Mathf.Rad2Deg + 90 - ply.gun.GetFireConeAngle() / 2;
            aimBar2.transform.position = (Vector2)ply.transform.position + DegreeToVector2(bar2Angle) * bar2Length / 4;
            aimBar2.transform.rotation = Quaternion.Euler(0, 0, bar2Angle);
        }
        else
        {
            aimBar1.CrossFadeAlpha(0, 0, true);
            aimBar2.CrossFadeAlpha(0, 0, true);
        }
	}
}

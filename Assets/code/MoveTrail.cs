using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTrail : MonoBehaviour {
    public float bulletSpeed = 230;
    public float liveTime = 1;
	void Update () {
        transform.Translate(Vector2.right * Time.deltaTime * bulletSpeed);
        Destroy(this.gameObject, liveTime);
	}
}

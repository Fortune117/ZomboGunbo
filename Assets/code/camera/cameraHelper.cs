using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraHelper : MonoBehaviour {

    public Player ply;
    public float cameraShake { get; set; }

    public float maxAngle; //Max angle in degrees. Good value for this perspective is just 0.
    public float maxOffset; //Max offest in units. Good value for this is 0.3;

    private Quaternion startRotation = Quaternion.Euler(new Vector3(0, 0, 0));

	// Use this for initialization
	void Update () {

        Transform targ = ply.transform;

        cameraShake = Mathf.Pow(ply.trauma, 1.5F);

        float x = targ.position.x + maxOffset * cameraShake * Random.Range(-1F, 1F);
        float y = targ.position.y + maxOffset * cameraShake * Random.Range(-1F, 1F);
        transform.rotation = Quaternion.Euler(new Vector3(startRotation.x, startRotation.y, startRotation.z + maxAngle * cameraShake * Random.Range(-1F, 1F)));
        transform.position = Vector3.Lerp(transform.position, new Vector3(x, y, transform.position.z), 0.2F);
	}
	
}

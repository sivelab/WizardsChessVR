using UnityEngine;
using System.Collections;

public class CameraFly : MonoBehaviour {
	public float speed = 0.5f;
	void Update () {
		transform.position += Input.GetAxis("Vertical") * transform.forward * speed
			+ Input.GetAxis("Horizontal") * transform.right * speed;
	}
}

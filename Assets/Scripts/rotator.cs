using UnityEngine;
using System.Collections;

public class rotator : MonoBehaviour {
	
	void Update () {
		if(Input.GetKey(KeyCode.Q)){
			transform.Rotate(new Vector3(0,1,0), -1f);
		}
		if(Input.GetKey(KeyCode.E)){
			transform.Rotate(new Vector3(0,1,0), 1f);
		}
	}
}

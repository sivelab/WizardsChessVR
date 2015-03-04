using UnityEngine;
using System.Collections;

public class joystickDebug : MonoBehaviour {
	void Start () {
		Debug.Log("Joysticks connected:");
		foreach (string name in Input.GetJoystickNames()) {
			Debug.Log(name);
		}
	}

	void Update() {

	}
}

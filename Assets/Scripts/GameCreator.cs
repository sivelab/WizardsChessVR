using UnityEngine;
using System.Collections;

public class GameCreator : MonoBehaviour
{

	private static KeyCode[] trackButtons = {
                KeyCode.Alpha1,
                KeyCode.Alpha2,
                KeyCode.Alpha3,
                KeyCode.Alpha4,
                KeyCode.Alpha5,
                KeyCode.Alpha6,
                KeyCode.Alpha7,
                KeyCode.Alpha8,
        };
	public Object[] gamePrefabs;

	void Update ()
	{
		for (int i = 0; i < trackButtons.Length && i < gamePrefabs.Length; i++) {
			if (Input.GetKeyDown (trackButtons [i])) {
				Instantiate (gamePrefabs [i], new Vector3 (0, 0, 0), new Quaternion ());
				Destroy (this);
			}
		}
	}
}

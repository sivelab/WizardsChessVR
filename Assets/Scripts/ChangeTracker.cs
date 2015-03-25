using UnityEngine;
using System.Collections;

[RequireComponent (typeof(TrackPosition))]
public class ChangeTracker : MonoBehaviour
{
	TrackPosition trackPosition;

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

	void Start ()
	{
		trackPosition = (TrackPosition)GetComponent<TrackPosition> ();
	}

	void Update ()
	{
		for (int i = 0; i < 8; i++) {
			if (Input.GetKeyDown (trackButtons [i])) {
				trackPosition.channel = i;
				break;
			}
		}
	}
}
